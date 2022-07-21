using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour {

	Vector2 worldSize = new Vector2(4, 4);

	Room[,] rooms;

	MapSpriteSelector[] DrawMaps;

	List<Vector2> takenPositions = new List<Vector2>();

	int gridSizeX, gridSizeY, numberOfRooms = 20;
	int inPosX, inPosY;
	public bool i_Room_L, i_Room_R, i_Room_U, i_Room_D;


	public GameObject BossRoomIcon;
	public GameObject roomWhiteObj;



	// TYPE 4 없애고 IS_Bossroom으로 대체하기 (이와같은 방법으로 상점같은것들도 만들기 쉬울듯함.



	private void Start()
	{
		if (numberOfRooms >= (worldSize.x * 2) * (worldSize.y * 2))
		{
			numberOfRooms = Mathf.RoundToInt((worldSize.x * 2) * (worldSize.y * 2));
		}
		gridSizeX = Mathf.RoundToInt(worldSize.x); //그리드 절반
		gridSizeY = Mathf.RoundToInt(worldSize.y);
		inPosX = gridSizeX;
		inPosY = gridSizeY;
		CreateRooms(); 
		SetRoomDoors(); 
		DrawMap();
		CreateBossRoom();
		StartCoroutine(RefreshTest()); //다른 스크립트 로드 할 때 까지 호출 대기.(endframe)
	}


	void CreateRooms(){
		//setup
		rooms = new Room[gridSizeX * 2,gridSizeY * 2];
		rooms[gridSizeX,gridSizeY] = new Room(Vector2.zero, 1);
		rooms[gridSizeX, gridSizeY].Checked = true;
		takenPositions.Insert(0,Vector2.zero);

		//test Setup DrawObject
		DrawMaps = new MapSpriteSelector[30];

		Vector2 checkPos = Vector2.zero;
		//magic numbers
		float randomCompare = 0.2f, randomCompareStart = 0.2f, randomCompareEnd = 0.01f;
		//add rooms
		for (int i =0; i < numberOfRooms -1; i++){
			float randomPerc = ((float) i) / (((float)numberOfRooms - 1));
			randomCompare = Mathf.Lerp(randomCompareStart, randomCompareEnd, randomPerc);
			//grab new position
			checkPos = NewPosition();
			//test new position
			if (NumberOfNeighbors(checkPos, takenPositions) > 1 && Random.value > randomCompare){
				int iterations = 0;
				do
				{
					checkPos = SelectiveNewPosition();
					iterations++;
				}while(NumberOfNeighbors(checkPos, takenPositions) > 1 && iterations < 100);
				if (iterations >= 50)
					print("error: could not create with fewer neighbors than : " + NumberOfNeighbors(checkPos, takenPositions));
			}
			//finalize position
			rooms[(int) checkPos.x + gridSizeX, (int) checkPos.y + gridSizeY] = new Room(checkPos, 2);
			takenPositions.Insert(0,checkPos);
		}	
	}

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
			drawPos.x *= 15;//aspect ratio of map sprite
			drawPos.y *= 8;
			//create map obj and assign its variables
			MapSpriteSelector mapper = Object.Instantiate(roomWhiteObj, drawPos, Quaternion.identity).GetComponent<MapSpriteSelector>();
			mapper.type = room.type;
			mapper.up = room.doorTop;
			mapper.down = room.doorBot;
			mapper.right = room.doorRight;
			mapper.left = room.doorLeft;
			DrawMaps[add] = mapper;
			add++;
		}
	}

	//문 설정하기..
	void SetRoomDoors(){
		for (int x = 0; x < ((gridSizeX * 2)); x++){
			for (int y = 0; y < ((gridSizeY * 2)); y++){
				if (rooms[x,y] == null){
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


	// 보스룸 만들기. (모두고려하기)
	void CreateBossRoom()
	{
		foreach (var room in rooms)
		{
			if (room == null)
				continue;
			if (room.doorBot == true && room.doorLeft == false && room.doorTop == false && room.doorRight == false)
			{
				room.type = 4;
				room.isBossRoom = true;
				break;
			}
			if (room.doorBot == false && room.doorLeft == true && room.doorTop == false && room.doorRight == false)
			{
				room.type = 4;
				room.isBossRoom = true;
				break;
			}
			if (room.doorBot == false && room.doorLeft == false && room.doorTop == true && room.doorRight == false)
			{
				room.type = 4;
				room.isBossRoom = true;
				break;
			}
			if (room.doorBot == false && room.doorLeft == false && room.doorTop == false && room.doorRight == true)
			{	
				room.type = 4;
				room.isBossRoom = true;
				break;
			}
		}
	}

	void RoomRefersh()
	{
		//세상에서 가장 미친 문구 ㅋㅋㅋㅋㅋㅋㅋ 나름 깔끔하게 고침.
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

	//방이동하기
	public void MoveRoom(int _moveDir)
	{

		try
		{
			ChoiseRoom(_moveDir);
		}
		catch
		{
			Debug.Log("방이없습니다.");
		}
		RoomRefersh();
	}

	//0 --> left
	//1 --> right
	//2 --> above
	//3 --> bellow
	void ChoiseRoom(int _moveDir)
	{
		LevelGeneration level = GameObject.Find("LevelGenerator").GetComponent<LevelGeneration>();

		switch (_moveDir)
		{
			case 0:
				{
					if (rooms[inPosX - 1, inPosY] != null)
					{
						if (rooms[inPosX - 1, inPosY].Checked != true && rooms[inPosX - 1, inPosY].isBossRoom == false)
						{
							EntityManager.Inst.SpawnEnemyEntity();
						}
						else if (rooms[inPosX - 1, inPosY].Checked != true && rooms[inPosX - 1, inPosY].isBossRoom == true)
						{
							EntityManager.Inst.SpawnEnemyBossEntity();
						}

						rooms[inPosX - 1, inPosY].type = 1;
						rooms[inPosX, inPosY].type = 0;

						inPosX--;
					}
					if (rooms[inPosX, inPosY].Checked != true)
					{
						UIManager.Inst.ButtonDeActivate();
						TurnManager.Inst.SetMyTurn();
						rooms[inPosX, inPosY].Checked = true;
					}
					else
					{
						level.existRoomCheck();
						UIManager.Inst.ButtonDeActivate();
						UIManager.Inst.ButtonActivate();
					}
					break;
				}
			case 1:
				{
					if (rooms[inPosX + 1, inPosY] != null)
					{
						if (rooms[inPosX + 1, inPosY].Checked != true && rooms[inPosX + 1, inPosY].isBossRoom == false)
						{
							EntityManager.Inst.SpawnEnemyEntity();
						}
						else if (rooms[inPosX + 1, inPosY].Checked != true && rooms[inPosX + 1, inPosY].isBossRoom == true)
						{
							Debug.Log("보스소환시도");
							EntityManager.Inst.SpawnEnemyBossEntity();
						}

						rooms[inPosX + 1, inPosY].type = 1;
						rooms[inPosX, inPosY].type = 0;

						inPosX++;
					}
					if (rooms[inPosX, inPosY].Checked != true)
					{
						UIManager.Inst.ButtonDeActivate();
						TurnManager.Inst.SetMyTurn();
						rooms[inPosX, inPosY].Checked = true;
					}
					else
					{
						level.existRoomCheck();
						UIManager.Inst.ButtonDeActivate();
						UIManager.Inst.ButtonActivate();
					}
					break;
				}
			case 2:
				{
					if (rooms[inPosX, inPosY + 1] != null)
					{
						if (rooms[inPosX, inPosY + 1].Checked != true && rooms[inPosX, inPosY + 1].isBossRoom == false)
						{
							EntityManager.Inst.SpawnEnemyEntity();
						}
						else if (rooms[inPosX, inPosY + 1].Checked != true && rooms[inPosX, inPosY + 1].isBossRoom == true)
						{
							Debug.Log("보스소환시도");
							EntityManager.Inst.SpawnEnemyBossEntity();
						}

						rooms[inPosX, inPosY + 1].type = 1;
						rooms[inPosX, inPosY].type = 0;

						inPosY++;

					}
					if (rooms[inPosX, inPosY].Checked != true)
					{
						UIManager.Inst.ButtonDeActivate();
						TurnManager.Inst.SetMyTurn();
						rooms[inPosX, inPosY].Checked = true;
					}
					else
					{
						level.existRoomCheck();
						UIManager.Inst.ButtonDeActivate();
						UIManager.Inst.ButtonActivate();
					}
					break;
				}
			case 3:
				{
					if (rooms[inPosX, inPosY - 1] != null)
					{
						if (rooms[inPosX, inPosY - 1].Checked != true && rooms[inPosX, inPosY - 1].isBossRoom == false)
						{
							EntityManager.Inst.SpawnEnemyEntity();
						}
						else if (rooms[inPosX, inPosY - 1].Checked != true && rooms[inPosX, inPosY - 1].isBossRoom == true)
						{
							Debug.Log("보스소환시도");
							EntityManager.Inst.SpawnEnemyBossEntity();
						}

						rooms[inPosX, inPosY - 1].type = 1;
						rooms[inPosX, inPosY].type = 0;

						inPosY--;
					}
					if (rooms[inPosX, inPosY].Checked != true)
					{
						UIManager.Inst.ButtonDeActivate();
						TurnManager.Inst.SetMyTurn();
						rooms[inPosX, inPosY].Checked = true;
					}
					else
					{
						level.existRoomCheck();
						UIManager.Inst.ButtonDeActivate();
						UIManager.Inst.ButtonActivate();
					}
					break;
				}
		}
	}

	public void existRoomCheck()
	{
		if (rooms[inPosX - 1, inPosY] != null)
		{
			i_Room_L = true;
		}
		else
		{
			i_Room_L = false;
		}
		if (rooms[inPosX + 1, inPosY] != null)
		{
			i_Room_R = true;
		}
		else
		{
			i_Room_R = false;
		}
		if (rooms[inPosX, inPosY + 1] != null)
		{
			i_Room_U = true;
		}
		else
		{
			i_Room_U = false;
		}
		if (rooms[inPosX, inPosY - 1] != null)
		{
			i_Room_D = true;
		}
		else
		{
			i_Room_D = false;
		}
	}


}
