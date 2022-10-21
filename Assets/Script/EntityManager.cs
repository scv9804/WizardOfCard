using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EntityManager : MonoBehaviour
{
    public static EntityManager Inst { get; private set; }
    // 인스턴스화 및 로드시 Destroy 안시키기.
    private void Awake()
    {
        Inst = this;
    }

    [Header("DefultSettings")]
    [SerializeField] float enemyAttackDelay;
    [SerializeField] EnemySO enemySO;
    [SerializeField] EnemyBossSO enemyBossSO;
    [SerializeField] CharacterSO characterSO;
    [SerializeField] GameObject entitiyPrefab;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject damagePrefab;
    [SerializeField] GameObject targetSelector;



    //[SerializeField] List<Entity> myEntities;
    [SerializeField] public List<Entity> enemyEntities; // << 22-10-21 장형용 :: 접근 제한 public으로 변경>>
    [SerializeField] Entity bossEntity;

    [SerializeField] Transform spawnPlayerChar_Tf;
    [SerializeField] Transform spawnEnemy_Tf;
    [SerializeField] Transform sortEnemyPos_Tf;
    [SerializeField] Card myUseCard;


    [SerializeField] EnemyAttackList enemyAttackList;
    [SerializeField] List<Enemy> enemyBuffer;
    [SerializeField] short Length;
    [SerializeField] private float f_targetSelectorUpPos;

    [HideInInspector] public PlayerEntity playerEntity;


    PlayerChar playerChar;


    LevelGeneration levelGeneration;


    const int MAX_ENEMY_COUNT = 3;





    bool is_canMouseInput => TurnManager.Inst.myTurn && !TurnManager.Inst.isLoding;


    [SerializeField] Entity selectEntity;
    [SerializeField] Entity targetPickEntity;

    [SerializeField] PlayerEntity selectPlayerEntity;
    [SerializeField] PlayerEntity targetPickPlayerEntity;

    WaitForSeconds delay10 = new WaitForSeconds(1.0f);

    private void Start()
    {
      
        levelGeneration = GameObject.Find("LevelGenerator").GetComponent<LevelGeneration>();
        TurnManager.onStartTurn += OnTurnStarted;
    }


    void EnemyEntityAlignment()
    {
        for (int i = 0; i < enemyEntities.Count; i++)
        {
            float f_Target_X =  sortEnemyPos_Tf.position.x  + i*2.5f;

            var targetEntity = enemyEntities[i];
            targetEntity.originPos = new Vector3(f_Target_X, targetEntity.transform.position.y ,0);
            targetEntity.MoveTransForm(targetEntity.originPos, true, 0.5f);
            targetEntity.GetComponent<OrderLayer>().SetOriginOrder(i);
        }
    }


    public IEnumerator EnemyEntityAttack()
	{
		foreach (var enemy in enemyEntities)
		{
            enemy.Attack(playerEntity);
            yield return new WaitForSeconds(enemyAttackDelay);
        }
        LevelGeneration.Inst.EndTurn();
    }




	#region enemySpaw
	public void SetEnemyEntity(Enemy _enemy)
    {
        var entityObject = Instantiate(entitiyPrefab, spawnEnemy_Tf.position, Quaternion.identity);
        var entity = entityObject.GetComponent<Entity>();
        entity.attackable = true;

        enemyEntities.Insert(UnityEngine.Random.Range(0, enemyEntities.Count), entity);
        entity.SetupEnemy(_enemy);
        EnemyEntityAlignment();
    }

    // 이거 호출하면 다 호출
    public void SpawnEnemyEntity()
    {
        int rand = UnityEngine.Random.Range(0, MAX_ENEMY_COUNT)+1;

        if (enemyBuffer.Count == 0)
            SetupEnemyBuffer();

        for (int i =0; i<rand; i++)
        {
			if (enemyBuffer.Count != 0)
			{
                SetEnemyEntity(enemyBuffer[i]);
			}
			else
			{
                SetupEnemyBuffer();
			}
        }

    }

    public void SpawnEnemyBossEntity()
	{
        SetEnemyBossEntity();
	}

    public void SetEnemyBossEntity()
    {
        var Boss = enemyBossSO.enemyBoss[0];
        var entityObject = Instantiate(entitiyPrefab, spawnEnemy_Tf.position, Quaternion.identity);
        var entity = entityObject.GetComponent<Entity>();
        entity.attackable = true;

        enemyEntities.Insert(0, entity);
        entity.SetupEnemy(Boss);
        EnemyEntityAlignment();
    }

    void SetupEnemyBuffer()
    {
        enemyBuffer = new List<Enemy>(100);

        // 아이템 버퍼에 추가
        for (int i = 0; i < enemySO.enemy.Length; i++)
        {
            Enemy enemy = enemySO.enemy[i];
            for (int j = 0; j < enemy.f_percentage; j++)
            {
                enemyBuffer.Add(enemy);
            }
        }

        for (int i = 0; i < enemyBuffer.Count; i++)
        {
            int rand = UnityEngine.Random.Range(i, enemyBuffer.Count);
            Enemy temp = enemyBuffer[i];
            enemyBuffer[i] = enemyBuffer[rand];
            enemyBuffer[rand] = temp;
        }

    }



	#endregion

	#region PlayerSpawn

	public void SetPlayerEntity(PlayerChar _playerChar)
    {
        var entityObject = Instantiate(playerPrefab, spawnPlayerChar_Tf.position, Quaternion.identity);
        var playerEntityTemp = entityObject.GetComponent<PlayerEntity>();

        playerEntityTemp.SetupPlayerChar(_playerChar);
        playerEntity = playerEntityTemp;
    }


    public void SpawnPlayerEntity()
    {
        ChosePlayer();
        SetPlayerEntity(playerChar);
    }



    //캐릭터 셀렉트 창 만들거임 차후 수정 부탁.... 누가 할지는 모르겠지만 고생해라 ㅎㅎ 코드자체는 대부분 깔끔하게 만들었는데 너무 귀찮아서 변수명 조금 이상하긴하네 ㅎㅎ;; 미안합니다!
    void ChosePlayer()
    {
        playerChar = characterSO.playrChar[0];
    }




    #endregion


    #region TurnManger
    void OnTurnStarted(bool _myTurn)
    {
        SetAttackable(_myTurn);
    }

    public void CheckDieEveryEnemy()
	{
		try 
        {
            foreach (var enemy in enemyEntities)
            {
                if (enemy.is_die)
                {
                    enemy.DestroyTest();
                    enemyEntities.Remove(enemy);
                    Debug.Log("하나죽었다.");
                }
                if (enemyEntities.Count == 0)
                {
                    Debug.Log("다음방으로");
                    UIManager.Inst.ButtonActivate();
                //    RewordManager.Inst.GameClear();
                    CardManager.Inst.SetCardStateCannot();

                    TurnManager.Inst.myTurn = false; // <<22-10-21 장형용 :: UI 못 쓰게 myTurn으로 걸어버림 다른 방법이 있나>>
                }
            }

        }
        catch
		{
            Debug.Log("이미 Destroy됨");
		}
		
    }






	#endregion




	#region PlayerEntity

	public void EntityMouseOverPlayer(PlayerEntity _playerEntity)
    {
        if (!is_canMouseInput)
            return;

        targetPickPlayerEntity = _playerEntity;

        // 이거는 약간 수정요소있음. 공격불린값을 어떻게 수정해야할지 약간 고민필요,
        if (targetPickPlayerEntity.attackable && CardManager.Inst.is_cardUsing)
        {
            targetSelector.SetActive(true);
            targetSelector.transform.position = _playerEntity.transform.position + Vector3.up * f_targetSelectorUpPos;
        }
    }

    public void PlayerEntityMouseExit()
    {
        if (!is_canMouseInput)
        {
            return;
        }
        else
        {
            targetSelector.SetActive(false);
            selectPlayerEntity = null;
            targetPickPlayerEntity = null;
        }
    }

    public void PlayerEntityMouseDown()
    {
        if (CardManager.Inst.is_cardUsing)
        {
            selectPlayerEntity = targetPickPlayerEntity;
        }
    }

    public void PlayerEntityMouseUp()
    {
        UseCard_Singel();
    }


    #endregion


    #region Entity

    public void EntityMouseOver(Entity _entity)
    {
        if (!is_canMouseInput)
            return;

        targetPickEntity = _entity;

        if (targetPickEntity.attackable && CardManager.Inst.is_cardUsing )
        {
            targetSelector.SetActive(true);
            targetSelector.transform.position = _entity.transform.position + Vector3.up * f_targetSelectorUpPos;
        }
    }

 

    public void EntityMouseExit()
    {
        if (!is_canMouseInput)
        {
            return;
        }
        else 
        {
            targetSelector.SetActive(false);
            selectEntity = null;
            targetPickEntity = null;
        }
    }

    public void EntityMouseDown()
    {
        if (CardManager.Inst.is_cardUsing)
        {
            selectEntity = targetPickEntity;
        }
    }

    public void EntityMouseUp()
    {
        UseCard_Singel();
    }
    #endregion


    #region CardUseSet
    //적 클릭후 마우스 업 시 적용됨.


    void UseCard_Singel()
    {
        if (!is_canMouseInput || (selectEntity == null && selectPlayerEntity == null))
		{
            return;
        }

        if (CardManager.Inst.is_cardUsing)
        {
            CardManager.Inst.is_cardUsing = false;
        }

        targetSelector.SetActive(false);
        Debug.Log("Single");

        //카드 사용 직후 SelectCard를 비워줌 으로써 이문구가 실행 되지 않도록 함.
        try
        {
            CardManager.Inst.UseCardSetmyCemetery();
            if (selectPlayerEntity == null)
            {
                //BattleCalculater.Inst.BattleCalc(CardManager.Inst.selectCard, selectEntity);
                CardManager.Inst.selectCard.UseCard(selectEntity); // <<22-10-21 장형용 :: 변경>>
            }
            if (selectEntity == null)
            {
                //BattleCalculater.Inst.BattleCalc(CardManager.Inst.selectCard, selectPlayerEntity);
                CardManager.Inst.selectCard.UseCard(null, selectPlayerEntity); // <<22-10-21 장형용 :: 변경>>
            }
        }
        catch
        {
            Debug.Log("SelectCard가 비었습니다.");
        }
        SetSelectedCardNull();
    }


    void SetSelectedCardNull()
    {
        CardManager.Inst.selectCard = null;
    }


    public void UseCard_AllEnemy()
    {
        if (!is_canMouseInput)
        {
            return;
        }


        Debug.Log("AllEnemy");

        try
        {
            CardManager.Inst.UseCardSetmyCemetery();
            //for (int i = 0; i < enemyEntities.Count; i++)
            //         {
            //             BattleCalculater.Inst.BattleCalc(CardManager.Inst.selectCard, enemyEntities[i]);
            //         }
            CardManager.Inst.selectCard.UseCard(null); // <<22-10-21 장형용 :: 변경>>
        }
        catch
        {
            Debug.Log("SelectCard가 비었습니다.");
        }
    }

    public void UseCard_Self()
    {
        if (!is_canMouseInput)
        {
            return;
        }

        Debug.Log("self");

        try
        {
            CardManager.Inst.UseCardSetmyCemetery();
            //BattleCalculater.Inst.BattleCalc(CardManager.Inst.selectCard, playerEntity);
            CardManager.Inst.selectCard.UseCard(null, playerEntity); // <<22-10-21 장형용 :: 변경>>

        }
        catch
        {
            Debug.Log("SelectCard가 비었습니다.");
        }
    }





    //턴 넘어갈때 공격 가능 여부 변경.
    public void SetAttackable(bool _isMine)
    {
        if (_isMine)
        {
            enemyEntities.ForEach(x => x.attackable = true);
            playerEntity.attackable = true;
        }
        else
        {
            enemyEntities.ForEach(x => x.attackable = false);
            playerEntity.attackable = false;
        }
        // 턴 넘어갈 때 공격인터페이스 끄기 //버그 수정 코드임. 
        try
        {
            targetSelector.SetActive(false);
        }
        catch
        {
            Debug.Log("이미 꺼져있습니다.");
        }
      

    }
	#endregion


	#region Card

	public void SetUseCard(Card _card)
    {
        myUseCard = _card;
    }

    public void DelectUseCard()
    {
        myUseCard = null;
    }

    #endregion





}
