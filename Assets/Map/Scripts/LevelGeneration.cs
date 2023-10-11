using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA;
using BETA.Singleton;

using Sirenix.OdinInspector;

using TacticsToolkit;

using UnityEngine.SceneManagement;

public class LevelGeneration : SingletonMonoBehaviour<LevelGeneration>
{
	#region 변수 등등
	Vector2 worldSize = new Vector2(4, 4);

	#region [SerializeField, TableMatrix(DrawElementMethod = "DrawMapMatrix")]
#if UNITY_EDITOR
	[SerializeField, TableMatrix(DrawElementMethod = "DrawMapMatrix")]
#else
    [SerializeField]
#endif 
    #endregion
    Room[,] rooms;
	[SerializeField]List<Room> eventRoom;
	List<Room> EdgeRooms;

	[SerializeField]MapSpriteSelector[] DrawMaps;

	List<Vector2> takenPositions = new List<Vector2>();

	int gridSizeX, gridSizeY;

	[SerializeField]int numberOfRooms = 20;

	int inPosX, inPosY;
	int loop = 0;//임시루프
	[SerializeField]bool create = false;

	//public bool i_Room_L, i_Room_R, i_Room_U, i_Room_D;

	public Dictionary<int, bool> IsMovable = new Dictionary<int, bool>()
	{
		{ 0, false },
		{ 1, false },
		{ 2, false },
		{ 3, false }
	};

	public GameObject BossRoomIcon;
	public GameObject roomWhiteObj;
	[SerializeField] bool tutorial = true;

	LevelGeneration level;

	GameObject mustDisableObject;


	//[SerializeField]ShopScirpt shopRoomScript;
	[SerializeField] List<RoomEventListScript> eventRoomScript;
	[SerializeField] RoomEventListScript tutorialRoomScript;
	[SerializeField] int eventRoomValue;

	int eventNumber;
	bool eventOn;
	bool shopOn;

	public int Stage = 1;

	[Header("기본몹")]
	[SerializeField]SceneSO sceneSO;
	[Header("보스")]
	[SerializeField]SceneSO bossSceneSO;
	[Header("레벨")]
	[SerializeField] SceneSO levelSceneSO;

	[SerializeField, TitleGroup("레벨제너레이션 이벤트")]
	private LevelGenerationEvent _events;

	// <<22-12-04 장형용 :: 편의성>>
	public Room CurrentRoom
    {
		get { return rooms[inPosX, inPosY]; }
	}

	// 장형용 :: 20231008 :: 추가
	#region private static Room DrawMapMatrix(Rect rect, Room room);
#if UNITY_EDITOR
	private static Room DrawMapMatrix(Rect rect, Room room)
	{
		if (room == null)
		{
			return null;
		}

		Color color;

		switch (room.RoomEventType)
		{
			case 0:
				color = new Color(1, 1, 1);
				break;

			case 1:
				color = new Color(1, 0, 0);
				break;

			case 2:
				color = new Color(1, 1, 0);
				break;

			case 3:
				color = new Color(0, 1, 0);
				break;

			default:
				color = new Color(0, 0, 0);
				break;
		}

		UnityEditor.EditorGUI.DrawRect(rect, color);

		return room;
	}
#endif
    #endregion

    #endregion

    private void OnEnable()
    {
		_events.OnGameEnd.Listener += OnGameEnd;

		_events.OnStageStart.Listener += OnStageStart;
	}

    private void OnDisable()
    {
		_events.OnGameEnd.Listener -= OnGameEnd;

		_events.OnStageStart.Listener -= OnStageStart;
	}

    // 장형용 :: 20231009 :: 좀 바꿀게요~
    protected override bool Initialize()
    {
		var isEmpty = base.Initialize();

		if (isEmpty)
		{
			DontDestroyOnLoad(gameObject);
		}

		return isEmpty;
	}

	private void OnGameEnd()
    {
		foreach (var selector in DrawMaps)
		{
			Destroy(selector?.gameObject);
		}

		DrawMaps = null;

		inPosX = gridSizeX;
		inPosY = gridSizeY;

		rooms = null;

		eventOn = false;
		shopOn = false;

		Stage = 1;
	}

	// 장형용 :: 20231009 :: 좀 바꿀게요~
	private void OnStageStart()
    {
		DrawMaps.Require(() =>
		{
			foreach (var selector in DrawMaps)
			{
				Destroy(selector?.gameObject);
			}
		});

		if (numberOfRooms >= (worldSize.x * 2) * (worldSize.y * 2))
        {
            numberOfRooms = Mathf.RoundToInt((worldSize.x * 2) * (worldSize.y * 2));
        }
        gridSizeX = Mathf.RoundToInt(worldSize.x); //그리드 절반
        gridSizeY = Mathf.RoundToInt(worldSize.y);
        inPosX = gridSizeX;
        inPosY = gridSizeY;

		//level = GameObject.Find("LevelGenerator").GetComponent<LevelGeneration>();

		CreateRooms();

        //보스방 먼저 설정. 그 후 뒤에는 이벤트맵 추가.

        SetRoomDoors();


        SetEventChangeRoom();
        SetEdgeRooms();

        CreateBossRoom();
        CreateEventRoom();
        CreateShopRoom();

        DrawMap();

        StartCoroutine(RefreshTest()); //다른 스크립트 로드 할 때 까지 호출 대기.(endframe)

        if (tutorial && rooms[inPosX, inPosY].isStartRoom == true)
        {
            tutorialRoomScript.Event();

            tutorial = false;
        }

        eventOn = false;
        shopOn = false;

		UIManager.Instance.RefreshMoveButtons();
	}

	public void OnBattleEnd()
    {
		var character = GameObject.Find("Character 4(Clone)");
		var entity = character.GetComponent<Entity>();

		"일단 한 쪽은 다 죽음".Log();

		if (!entity.isAlive)
        {
			SceneManager.LoadScene("GameOverScene");
		}
        else
        {
			var room = CurrentRoom;

			if (room.RoomEventType == 1)
			{
				Stage += 1;

				LevelClear(Stage, 5);
			}
			else
			{
				//BETA.GameManager.Instance.Loading("Stage 1-1 Load");

				LoadMainStageScene();
			}

			UIManager.Instance.RefreshMoveButtons();
		}
    }

	#region 절 대 건 들 지 마
	Vector2 NewPosition(){
		int x = 0, y = 0;
		Vector2 checkingPos = Vector2.zero;
		do{
			int index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1)); // pick a random room
			x = (int) takenPositions[index].x;//capture its x, y position
			y = (int) takenPositions[index].y;
			bool UpDown = (Random.value < 0.5f);//randomly pick wether to look on hor or vert axis
			bool positive = (Random.value < 0.5f);//pick whether to be positive or negative on that axis
			if (UpDown){ //find the position bnased on the above bools
				if (positive){
					y += 1;
				}else{
					y -= 1;
				}
			}else{
				if (positive){
					x += 1;
				}else{
					x -= 1;
				}
			}
			checkingPos = new Vector2(x,y);
		}while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY); //make sure the position is valid
		return checkingPos;
	}

	Vector2 SelectiveNewPosition(){ // method differs from the above in the two commented ways
		int index = 0, inc = 0;
		int x =0, y =0;
		Vector2 checkingPos = Vector2.zero;
		do{
			inc = 0;
			do{ 
				//instead of getting a room to find an adject empty space, we start with one that only 
				//as one neighbor. This will make it more likely that it returns a room that branches out
				index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
				inc ++;
			}while (NumberOfNeighbors(takenPositions[index], takenPositions) > 1 && inc < 100);
			x = (int) takenPositions[index].x;
			y = (int) takenPositions[index].y;
			bool UpDown = (Random.value < 0.5f);
			bool positive = (Random.value < 0.5f);
			if (UpDown){
				if (positive){
					y += 1;
				}else{
					y -= 1;
				}
			}else{
				if (positive){
					x += 1;
				}else{
					x -= 1;
				}
			}
			checkingPos = new Vector2(x,y);
		}while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);
		if (inc >= 100){ // break loop if it takes too long: this loop isnt garuanteed to find solution, which is fine for this
			print("Error: could not find position with only one neighbor");
		}
		return checkingPos;
	}

	int NumberOfNeighbors(Vector2 checkingPos, List<Vector2> usedPositions){
		int ret = 0; // start at zero, add 1 for each side there is already a room
		if (usedPositions.Contains(checkingPos + Vector2.right)){ //using Vector.[direction] as short hands, for simplicity
			ret++;
		}
		if (usedPositions.Contains(checkingPos + Vector2.left)){
			ret++;
		}
		if (usedPositions.Contains(checkingPos + Vector2.up)){
			ret++;
		}
		if (usedPositions.Contains(checkingPos + Vector2.down)){
			ret++;
		}
		return ret;
	}

	// 맵 그리기. 여기서 맵의 스프라이트 색상 및 문이 열린 위치를 확인가능.
	void DrawMap(){
		int add=0;
		foreach (Room room in rooms)
		{
			if (room == null)
			{
				continue; //skip where there is no room
			}
			
			Vector2 drawPos = room.gridPos;
			drawPos.x *= 2.7f;//aspect ratio of map sprite
			drawPos.y *= 1.5f;
			//create map obj and assign its variables
			MapSpriteSelector mapper = Object.Instantiate(roomWhiteObj, drawPos, Quaternion.identity).GetComponent<MapSpriteSelector>();
			mapper.type = room.type;
			mapper.up = room.doorTop;
			mapper.down = room.doorBot;
			mapper.right = room.doorRight;
			mapper.left = room.doorLeft;
			mapper.RoomEventType = room.RoomEventType;
			mapper.roomNumX = room.roomNumX;
			mapper.roomNumY = room.roomNumY;
			DrawMaps[add] = mapper;
			add++;
		}
	}

	//문 설정하기..
	void SetRoomDoors(){
		for (int x = 0; x < ((gridSizeX * 2)); x++){
			for (int y = 0; y < ((gridSizeY * 2)); y++){
				if (rooms[x,y] == null)
				{
					continue;
				}
				Vector2 gridPosition = new Vector2(x,y);

				if (y - 1 < 0){ //윗문 확인
					rooms[x,y].doorBot = false;
				}else{
					rooms[x,y].doorBot = (rooms[x,y-1] != null);
				}
				if (y + 1 >= gridSizeY * 2){ //check bellow
					rooms[x,y].doorTop = false;
				}else{
					rooms[x,y].doorTop = (rooms[x,y+1] != null);
				}
				if (x - 1 < 0){ //check left
					rooms[x,y].doorLeft = false;
				}else{
					rooms[x,y].doorLeft = (rooms[x - 1,y] != null);
				}
				if (x + 1 >= gridSizeX * 2){ //check right
					rooms[x,y].doorRight = false;
				}else{
					rooms[x,y].doorRight = (rooms[x+1,y] != null);
				}
			}
		}
	}

	#endregion

	#region 추가 기능
	void RoomNumberring(int _x, int _y)
	{
		rooms[_x + gridSizeX , _y + gridSizeY].roomNumX = _x;
		rooms[_x + gridSizeX, _y + gridSizeY].roomNumY = _y;
	}


	//전체를 리프레쉬해주는 방식으로 함. 방 한칸식 해주고 싶은데 안떠오름 ㅋㅋㅋㅋ;;;;
	void RefreshSpriteColor()
	{
		int add = 0;
		
		foreach (Room room in rooms)
		{
			if (room == null)
			{
				continue; //skip where there is no room
			}

		
			DrawMaps[add].type = room.type;

			add++;
		}

		foreach (MapSpriteSelector te in DrawMaps)
		{
			if (te == null)
			{
				continue;
			}
			te.PickColor();
		}

	}

	 IEnumerator RefreshTest()
	{
		yield return new WaitForEndOfFrame();
		RoomRefersh();
	}
	#endregion

	#region RoomsType Set
	// type 
	// 0: normal, 1: enter 2: SetActiveFalse 3: SetActiveTrue And NotSerchedYet
	// 0: normal 1: Boss 2: Shop 3: Event

	// 보스룸 만들기.
	void CreateBossRoom()
	{
		create = false;
		int i = 0;
		do
		{
			i++;
			try
			{
				if (EdgeRooms.Count == 0) break;
				int randomRoom;
				randomRoom = UnityEngine.Random.Range(0, EdgeRooms.Count);

				if (EdgeRooms[randomRoom].type != 1)
				{
					EdgeRooms[randomRoom].type = 4;
					EdgeRooms[randomRoom].RoomEventType = 1;
					EdgeRooms.RemoveAt(randomRoom);
					create = true;
					break;
				}
			}
			catch
			{
				Debug.LogError("Error_Edge_Empty");
			}
		} while (i < 100);

		while(!create)
		{
			try
			{
				int randomRoom;
				randomRoom = UnityEngine.Random.Range(0, eventRoom.Count);

				if (eventRoom[randomRoom].type != 1)
				{
					int randomPos;
					randomPos = UnityEngine.Random.Range(0, 4);
					//0 --> left
					//1 --> right
					//2 --> above
					//3 --> bellow
					if (randomPos == 0)
						if (RoomExpansion_EventRoom(-1, 0, randomRoom, 1)) { create = true; break; }
					if (randomPos == 1)
						if (RoomExpansion_EventRoom(1, 0, randomRoom, 1)) { create = true; break; }
					if (randomPos == 2)
						if (RoomExpansion_EventRoom(0, 1, randomRoom, 1)) { create = true; break; }
					if (randomPos == 3)
						if (RoomExpansion_EventRoom(0, -1, randomRoom, 1)) { create = true; break; }
				}
			}
			catch
			{
				Debug.LogError("Error_Edge_Empty");
			}
		}
	}


	//이벤트 룸 일단 하나만 만들도록 해놓음
	void CreateEventRoom()
	{
		int eventRoomCount = 0;
		do
		{
			int randomRoom;
			if (eventRoom.Count == 0) break;
			randomRoom = UnityEngine.Random.Range(0, eventRoom.Count);
			if (eventRoom[randomRoom].type != 1)
			{
				eventRoom[randomRoom].RoomEventType = 3;
				eventRoom.RemoveAt(randomRoom);
				eventRoomCount++;
				if (eventRoomCount == eventRoomValue)
				{
					break;
				}
			}

		} while (true);

	}


	//상점은 남은방 중 
	void CreateShopRoom() // <<22-11-01 장형용 :: 프리즈 최적화 버전, 상점이 가끔 나오지 않는 버그는 일단 고치지 않음>>
	{
		// <<22. 11. 07 이동화 :: 프리즈 더 깔끔하게 최적화 완료 만약 엣지룸 없으면 무작위 방에서 추가로 생성하기로 함.>>
		create = false;
		do
		{
			int randomRoom;
			if (EdgeRooms.Count == 0) break; // 엣지룸 없으면 브레이크
			randomRoom = UnityEngine.Random.Range(0, EdgeRooms.Count);
			try
			{
				if (EdgeRooms[randomRoom].type != 1 && EdgeRooms[randomRoom].RoomEventType != 1)
				{
					int randomPos;
					randomPos = UnityEngine.Random.Range(0, 4);
					//0 --> left
					//1 --> right
					//2 --> above
					//3 --> bellow
					if (randomPos == 0)
						if (RoomExpansion_EdgeRoom(-1, 0, randomRoom, 2)) { create = true; break; }
					if (randomPos == 1)
						if (RoomExpansion_EdgeRoom(1, 0, randomRoom, 2)) { create = true; break; }
					if (randomPos == 2)
						if (RoomExpansion_EdgeRoom(0, 1, randomRoom, 2)) { create = true; break; }
					if (randomPos == 3)
						if (RoomExpansion_EdgeRoom(0, -1, randomRoom, 2)) { create = true; break; }
				}
			}
			catch
			{
				
			}
		} while (true);

		while (!create) // 만약 안됐으면 다른방에 추가로 만들기
		{
			int randomRoom;
			randomRoom = UnityEngine.Random.Range(0, eventRoom.Count);
			Debug.Log(":");
			try
			{
				if (eventRoom[randomRoom].type != 1)
				{
					int randomPos;
					randomPos = UnityEngine.Random.Range(0, 4);
					//0 --> left
					//1 --> right
					//2 --> above
					//3 --> bellow
					if (randomPos == 0)
						if (RoomExpansion_EventRoom(-1, 0, randomRoom, 2)) { create = true; break; }
					if (randomPos == 1)
						if (RoomExpansion_EventRoom(1, 0, randomRoom, 2)) { create = true; break; }
					if (randomPos == 2)
						if (RoomExpansion_EventRoom(0, 1, randomRoom, 2)) { create = true; break; }
					if (randomPos == 3)
						if (RoomExpansion_EventRoom(0, -1, randomRoom,2)) { create = true; break; }
				}
			}
			catch
			{
				Debug.LogError("상점생성 에러");
			}
		}
		SetRoomDoors();
	}

	#region 방 확장
	bool RoomExpansion_EdgeRoom(int _x, int _y, int _random, int roomEventType)
	{
		if (rooms[EdgeRooms[_random].roomNumX + _x + gridSizeX, EdgeRooms[_random].roomNumY + _y + gridSizeY] == null)
		{
			rooms[EdgeRooms[_random].roomNumX + _x + gridSizeX, 
				EdgeRooms[_random].roomNumY + _y + gridSizeY] = new Room(new Vector2(EdgeRooms[_random].roomNumX + _x, 
				EdgeRooms[_random].roomNumY + _y), 2, roomEventType);
			EdgeRooms.RemoveAt(_random);
			return true;
		}
		return false;
	}
	bool RoomExpansion_EventRoom(int _x, int _y, int _random, int roomEventType)
	{
		if (rooms[eventRoom[_random].roomNumX + _x + gridSizeX, eventRoom[_random].roomNumY + _y + gridSizeY] == null)
		{
			rooms[eventRoom[_random].roomNumX + _x + gridSizeX,
				eventRoom[_random].roomNumY + _y + gridSizeY] = new Room(new Vector2(eventRoom[_random].roomNumX + _x,
				eventRoom[_random].roomNumY + _y), 2, roomEventType);
			eventRoom.RemoveAt(_random);
			return true;
		}
		return false;
	}
	#endregion



	void SetEdgeRooms()
	{
		EdgeRooms = new List<Room>();
		int temt = 0;
		int temt2 = eventRoom.Count;
		for (int i = eventRoom.Count - 1; 0 <= i; i--)
		{
			if (eventRoom[i].doorTop)
				temt++;
			if (eventRoom[i].doorRight)
				temt++;
			if (eventRoom[i].doorLeft)
				temt++;
			if (eventRoom[i].doorBot)
				temt++;

			if (temt == 1)
			{
				EdgeRooms.Add(eventRoom[i]);
				eventRoom.RemoveAt(i);
			}
			temt = 0;
		}

		if (eventRoom.Count == temt2)
		{
			Debug.Log("error_Room_NoEdge");
		}
	}

	void SetEventChangeRoom()
	{
		eventRoom = new List<Room>();
		int temp = 0;
		foreach (var room in rooms)
		{
			if (room == null)
				continue;
			if (room.RoomEventType == 0)
			{
				eventRoom.Add(room);
				temp++;
			}
		}
	}

	void CreateRooms()
	{
		takenPositions.Clear();
		//setup
		rooms = new Room[gridSizeX * 2, gridSizeY * 2];
		rooms[gridSizeX, gridSizeY] = new Room(Vector2.zero, 1, 0);
		rooms[gridSizeX, gridSizeY].Checked = true;
		rooms[gridSizeX, gridSizeY].isStartRoom = true;
		takenPositions.Insert(0, Vector2.zero);

		//그릴 맵 총개수 테스트
		DrawMaps = new MapSpriteSelector[40];

		Vector2 checkPos = Vector2.zero;
		//magic numbers
		float randomCompare = 0.2f, randomCompareStart = 0.2f, randomCompareEnd = 0.01f;

		//add rooms
		for (int i = 0; i < numberOfRooms - 1; i++)
		{
			float randomPerc = ((float)i) / (((float)numberOfRooms - 1));
			randomCompare = Mathf.Lerp(randomCompareStart, randomCompareEnd, randomPerc);
			//grab new position
			checkPos = NewPosition();
			//test new position
			if (NumberOfNeighbors(checkPos, takenPositions) > 1 && Random.value > randomCompare)
			{
				int iterations = 0;
				do
				{
					checkPos = SelectiveNewPosition();
					iterations++;
				} while (NumberOfNeighbors(checkPos, takenPositions) > 1 && iterations < 100);

				if (iterations >= 50)
					print("error: could not create with fewer neighbors than : " + NumberOfNeighbors(checkPos, takenPositions));
			}

			//finalize position
			rooms[(int)checkPos.x + gridSizeX, (int)checkPos.y + gridSizeY] = new Room(checkPos, 2, 0);
			//방 번호 넘버링.
			RoomNumberring((int)checkPos.x , (int)checkPos.y );
			
			takenPositions.Insert(0, checkPos);
		}
	}




	#endregion

	#region RoomCode
	void RoomRefersh()
	{
		try
		{
			if (rooms[inPosX, inPosY] != null)
			{
				if (rooms[inPosX - 1, inPosY] != null && rooms[inPosX - 1, inPosY].Checked != true )
				{
					rooms[inPosX - 1, inPosY].type = 3;

				}
			}
		}
		catch
		{
			Debug.Log("LeftRoomIsOutOfIndex");
		}
		try
		{
			if (rooms[inPosX, inPosY] != null)
			{
				if (rooms[inPosX + 1, inPosY] != null && rooms[inPosX + 1, inPosY].Checked != true)
				{
					rooms[inPosX + 1, inPosY].type = 3;

				}

			}
		}
		catch
		{
			Debug.Log("RightRoomIsOutOfIndex");
		}
		try
		{
			if (rooms[inPosX, inPosY] != null)
			{

				if (rooms[inPosX, inPosY + 1] != null && rooms[inPosX, inPosY + 1].Checked != true)
				{
					rooms[inPosX, inPosY + 1].type = 3;

				}

			}
		}
		catch
		{
			Debug.Log("AboveRoomIsOutOfIndex");
		}
		try
		{
			if (rooms[inPosX, inPosY] != null)
			{

				if (rooms[inPosX, inPosY - 1] != null && rooms[inPosX, inPosY - 1].Checked != true)
				{
					rooms[inPosX, inPosY - 1].type = 3;
				}
			}
		}
		catch
		{
			Debug.Log("UnderRoomIsOutOfIndex");
		}
		RefreshSpriteColor();
	}

    public void Move(int direction)
    {
		existRoomCheck();

		ChoiseRoom(direction);

		RoomRefersh();
	}

    //방이동하기
    public void MoveRoom(int _moveDir)
	{
		ChoiseRoom(_moveDir);
	/*	try
		{
			
		}
		catch
		{
			Debug.Log("방이없습니다.");
		}*/
		RoomRefersh();
	}

	//0 --> left
	//1 --> right
	//2 --> above
	//3 --> bellow

	void ChoiseRoom(int _moveDir)
	{
		switch (_moveDir)
		{
			case 0:
				{
					if (rooms[inPosX - 1, inPosY] != null)
					{
						SerchRoomEvent(-1, 0);
					}
					SetMoveRoomButton();
					break;
				}
			case 1:
				{
					if (rooms[inPosX + 1, inPosY] != null)
					{
						SerchRoomEvent(1, 0);
					}
					SetMoveRoomButton();
					break;
				}
			case 2:
				{
					if (rooms[inPosX, inPosY + 1] != null)
					{
						SerchRoomEvent(0, 1);
					}
					SetMoveRoomButton();
					break;
				}
			case 3:
				{
					if (rooms[inPosX, inPosY - 1] != null)
					{
						SerchRoomEvent(0 , -1);
					}
					SetMoveRoomButton();
					break;
				}
		}
	}


	//방 체크 최종임. 여기서 진입.
	void SerchRoomEvent(int _x, int _y)
	{
		if (eventOn)
		{
			eventRoomScript[eventNumber].ExitRoom();
			eventRoomScript.RemoveAt(eventNumber); ;
			eventOn = false;
		}
		
		if(shopOn)
		{
			//shopRoomScript.ExitShop();
			_events.OnShopEnter.Launch(false);
			shopOn = false;
		}

		if (rooms[inPosX + _x, inPosY + _y].Checked != true)
		{
			switch (rooms[inPosX + _x, inPosY + _y].RoomEventType)
			{
				case 0:
					sceneSO.CallBattleScene(Stage);
					break;
				case 1:
					bossSceneSO.CallBattleScene(Stage);
					Debug.Log("보스소환시도");
					break;
				case 2:
					//LoadSceneManager.LoadScene("CopyScene");

					//shopRoomScript.EnterShop();
					_events.OnShopEnter.Launch(true);
					Debug.Log("상점이벤트");
					shopOn = true;
					break;
				case 3:
					//LoadSceneManager.LoadScene("CopyScene");

					Debug.Log("그냥 이벤트");
					break;
			}
		}
		else if (rooms[inPosX + _x, inPosY + _y].RoomEventType == 2)
		{
			//shopRoomScript.EnterShop();
			_events.OnShopEnter.Launch(true);
			Debug.Log("상점이벤트");
			shopOn = true;
		}


		rooms[inPosX + _x, inPosY + _y].type = 1;
		rooms[inPosX, inPosY].type = 0;

		inPosX += _x;
		inPosY += _y;
	}

	void SetMoveRoomButton()
	{
		if (rooms[inPosX, inPosY].Checked != true)
		{
			if (rooms[inPosX, inPosY].RoomEventType == 2 ||
				rooms[inPosX, inPosY].RoomEventType == 3)
			{
				UIManager.Instance.RefreshMoveButtons();
			}
			else
			{
				UIManager.Inst.DisableMoveButtons();
			}

			rooms[inPosX, inPosY].Checked = true;
		}
		else
		{
			//level.existRoomCheck
			existRoomCheck();
			UIManager.Instance.RefreshMoveButtons();
		}
	}

	public void existRoomCheck()
	{
		if (rooms == null)
        {
			return;
        }

		if (inPosX > 0 && rooms[inPosX - 1, inPosY] != null)
		{
			IsMovable[0] = true;
		}
		else
		{
			IsMovable[0] = false;
		}

		if (inPosX < rooms.GetLength(0) - 1 && rooms[inPosX + 1, inPosY] != null)
		{
			IsMovable[1] = true;
		}
		else
		{
			IsMovable[1] = false;
		}

		if (inPosY < rooms.GetLength(1) - 1 && rooms[inPosX, inPosY + 1] != null)
		{
			IsMovable[2] = true;
		}
		else
		{
			IsMovable[2] = false;
		}

		if (inPosY > 0 && rooms[inPosX, inPosY - 1] != null)
		{
			IsMovable[3] = true;
		}
		else
		{
			IsMovable[3] = false;
		}
	}

	#endregion

	#region 추가사항

	public void LevelClear(int stage, int _rommCount)//스테이지 입력
	{
		//SetLevelRoomsCount(_rommCount);
		//ReCreateRoom();
		numberOfRooms = _rommCount;

		levelSceneSO.CallLevel(Stage);
	}

	public void LoadMainStageScene()
    {
		levelSceneSO.CallMainStage(Stage);

	}

	//public void ReCreateRoom()
	//{
	//	foreach (var selector in DrawMaps)
	//	{
	//		Destroy(selector?.gameObject);
	//	}
	//	eventRoom.Clear();


	//	CreateRooms();

	//	SetRoomDoors();


	//	SetEventChangeRoom();
	//	SetEdgeRooms();

	//	CreateBossRoom();
	//	CreateEventRoom();
	//	CreateShopRoom();

	//	DrawMap();

	//	StartCoroutine(RefreshTest()); //다른 스크립트 로드 할 때 까지 호출 대기.(endframe)



	//	if (tutorial && rooms[inPosX, inPosY].isStartRoom == true)
	//	{
	//		tutorialRoomScript.Event();


	//		tutorial = false;

	//	}

	//	eventOn = false;
	//	shopOn = false;

	//	UIManager.Inst.ButtonActivate();
	//}

	//private void SetLevelRoomsCount(int _count)
	//{
	//	numberOfRooms = _count;
	//}



	#endregion


}
