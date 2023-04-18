using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using XSSLG;
using UnityEngine.SceneManagement;

public class EntityManager : MonoBehaviour
{
    public static EntityManager Inst { get; private set; }
    // 인스턴스화 및 로드시 Destroy 안시키기.
    private void Awake()
    {
        Inst = this;
        DontDestroyOnLoad(this);
    }

    #region 변수등등
    [Header("DefultSettings")]
    [SerializeField] float enemyAttackDelay;
    [SerializeField] EnemySO enemySO;
    [SerializeField] EnemySpawnPatternSO enemySpawnPatternSO;
    [SerializeField] EnemySpawnPatternSO enemyBossSO;
    [SerializeField] CharacterSO characterSO;
    [SerializeField] GameObject entitiyPrefab;
    [SerializeField] GameObject playerPrefab;
    //[SerializeField] GameObject damagePrefab; // 미사용
    [SerializeField] GameObject targetSelector;



    //[SerializeField] List<Entity> myEntities;
    [SerializeField] public List<Entity> enemyEntities; // << 22-10-21 장형용 :: 접근 제한 public으로 변경>>
    [SerializeField] public GameObject[] enemyEntitiesObjcet; // << 22-10-21 장형용 :: 접근 제한 public으로 변경>>
    [SerializeField] Entity bossEntity;

    [SerializeField] Transform spawnPlayerChar_Tf;
    [SerializeField] Transform spawnEnemy_Tf;
    [SerializeField] Transform sortEnemyPos_Tf;
    //[SerializeField] Card myUseCard; // 미사용


    //[SerializeField] EnemyAttackList enemyAttackList; // 미사용
    [SerializeField] List<Enemy> enemyBuffer;
    //[SerializeField] short Length; // 미사용
    [SerializeField] private float f_targetSelectorUpPos;

    [HideInInspector] public PlayerEntity playerEntity;


    PlayerChar playerChar;


    //  LevelGeneration levelGeneration;


    const int MAX_ENEMY_COUNT = 3;

    public static int i_checkingEntitiesCount = 0; // <<22-10-30 장형용 :: 실행 중인 전체 피격 모션 수>>



    bool is_canMouseInput => TurnManager.Inst.myTurn && !TurnManager.Inst.isLoding;


    [SerializeField] Entity selectEntity;
    [SerializeField] Entity targetPickEntity;

    [SerializeField] PlayerEntity selectPlayerEntity;
    [SerializeField] PlayerEntity targetPickPlayerEntity;

    //WaitForSeconds delay10 = new WaitForSeconds(1.0f); // 미사용

    #endregion

    private void Start()
    {
        //levelGeneration = GameObject.Find("LevelGenerator").GetComponent<LevelGeneration>();
        TurnManager.onStartTurn += OnTurnStarted;
        //이거 꼭수정
    }

    void EnemyEntityAlignment()
    {
        for (int i = 0; i < enemyEntities.Count; i++)
        {
            float f_Target_X = sortEnemyPos_Tf.position.x + i * 2.5f;

            var targetEntity = enemyEntities[i];
            targetEntity.originPos = new Vector3(f_Target_X, targetEntity.transform.position.y, -i);
            targetEntity.MoveTransForm(targetEntity.originPos, true, 0.5f);
            targetEntity.GetComponent<OrderLayer>().SetOriginOrder(i);
        }
    }

    public IEnumerator EnemyEntityAttack()
    {
        //foreach 사용금지 :: 전투 중 몬스터 추가 스폰시 오류걸림
        for (int i = 0; i < enemyEntities.Count; i++)
        {
            if (enemyEntities[i].attackable)
            {
                enemyEntities[i].Attack();
                yield return new WaitForSeconds(enemyAttackDelay);
            }
        }
        LevelGeneration.Inst.EndTurn();
    }

    // <<22-10-28 장형용 :: 추가>>
    #region 타겟 랜덤 지정 / 살아있는 개체 확인

    public bool IsAlreadyAllDead()
    {
        for (int i = 0; i < enemyEntities.Count; i++)
        {
            if (!enemyEntities[i].is_die)
                return false;
        }

        return true;
    }

    public Entity SelectRandomTarget()
    {
        if (IsAlreadyAllDead())
        {
            return null;
        }

        #region 옛날코드
        //int _index = Random.Range(0, enemyEntities.Count);

        //while(enemyEntities[_index].is_die)
        //{
        //    _index = Random.Range(0, enemyEntities.Count);
        //}

        //if(enemyEntities[_index].is_die)
        //{
        //    return SelectRandomTarget();
        //}
        //else
        //{
        //    return enemyEntities[_index];
        //}
        #endregion

        int _index;

        do
        {
            _index = Random.Range(0, enemyEntities.Count);
        }
        while (enemyEntities[_index].is_die);

        return enemyEntities[_index];
    }

    #endregion

    #region enemySpaw

    #region 불에타버린 적 설정하기
    /*    public void SetEnemyEntity(Enemy _enemy)
        {
            var entityObject = Instantiate(entitiyPrefab, spawnEnemy_Tf.position, Quaternion.identity);
            var entity = entityObject.GetComponent<Entity>();
            entity.attackable = false;

            enemyEntities.Insert(UnityEngine.Random.Range(0, enemyEntities.Count), entity);
            entity.SetupEnemy(_enemy);
            EnemyEntityAlignment();
        }*/
    #endregion



    #region 보상설정하기
    //void SetReward(SpawnPattern _pattern)
    //{
    //       RewardManager.Inst.AddReward(_pattern);
    //}
    #endregion

    //적 ID값으로 추가 소환 임시막아놓음
    public void SelectSpawnEnemyEntity(int ID)
    {
        var entityObject = Instantiate(entitiyPrefab, spawnEnemy_Tf.position, Quaternion.identity);
        var entity = entityObject.GetComponent<Entity>();
        entity.attackable = false;

        enemyEntities.Add(entity);
        //entity.SetupEnemy(enemySO.enemy[ID]);
        EnemyEntityAlignment();
    }

    // 이거 호출하면 다 호출
    public void SpawnEnemyEntity()
    {
        #region 옛날코드 
        /*
		int rand = UnityEngine.Random.Range(0, MAX_ENEMY_COUNT) + 1;

		if (enemyBuffer.Count == 0)
			SetupEnemyBuffer();


		if (enemyBuffer.Count != 0)
		{
			SetEnemyEntity(enemyBuffer[0]);
			enemyBuffer.RemoveAt(0);
		}*/

        /*
		int randomPattern = UnityEngine.Random.Range(0, enemySpawnPatternSO.spawnPattern.Length);

		for (int i = 0; enemySpawnPatternSO.spawnPattern[randomPattern].enemy.Length > i; i++)
		{
            SetEnemyEntity(enemySpawnPatternSO.spawnPattern[randomPattern].enemy[i]);
		}
        SetReward(enemySpawnPatternSO.spawnPattern[randomPattern]);
        */
        #endregion

        for (int i = 0; i < enemyEntitiesObjcet.Length; i++)
        {
            if (enemyEntitiesObjcet[i].GetComponent<Entity>() == null)
            {

            }
            SetEnemyEntity(enemyEntitiesObjcet[i].GetComponent<Entity>());
        }
    }
    public void SetEnemyEntity(Entity _entity)
    {
        _entity.attackable = false;

        _entity.SetupEnemy();

        SetEnemyList();
    }

    void SetEnemyList()
    {
        foreach (var enemy in enemyEntitiesObjcet)
        {
            enemyEntities.Add(enemy.GetComponent<Entity>());
        }

    }

    void EnemyEntitiesSet()
    {
        enemyEntitiesObjcet = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log(enemyEntitiesObjcet.Length);
        /*        foreach (var entity in enemyEntitiesObjcet)
                {            
                    enemyEntities.Add(entity.GetComponent<Entity>());
                    Debug.Log("뭐가문제임 : " + entity.transform.position); 
                }*/
    }


    public void SpawnEnemyBossEntity()
    {
        SetEnemyBossEntity();
    }

    //사실상없는거
    public void SetEnemyBossEntity()
    {
        int rand = UnityEngine.Random.Range(0, enemyBossSO.spawnPattern.Length);
        foreach (var item in enemyBossSO.spawnPattern[rand].enemy)
        {
            var Boss = item;
            var entityObject = Instantiate(entitiyPrefab, spawnEnemy_Tf.position, Quaternion.identity);
            var entity = entityObject.GetComponent<Entity>();
            entity.attackable = true;
            enemyEntities.Insert(0, entity);
            //    entity.SetupEnemy(Boss);
        }

        EnemyEntityAlignment();
    }


    #region 이젠 더미가 되어버린 적 버퍼에 추가하기.....
    //void SetupEnemyBuffer()
    //{
    //    enemyBuffer = new List<Enemy>(100);
    //    int rand = UnityEngine.Random.Range(0, enemySpawnPatternSO.spawnPattern.Length);

    //    // 아이템 버퍼에 추가
    //    for (int i = 0; i < enemySpawnPatternSO.spawnPattern[rand].enemy.Length; i++)
    //    {
    //        Enemy enemy = enemySpawnPatternSO.spawnPattern[rand].enemy[i];
    //        for (int j = 0; j < enemy.f_percentage; j++)
    //        {
    //            enemyBuffer.Add(enemy);
    //        }
    //    }

    //    for (int i = 0; i < enemyBuffer.Count; i++)
    //    {
    //        rand = UnityEngine.Random.Range(i, enemyBuffer.Count);
    //        Enemy temp = enemyBuffer[i];
    //        enemyBuffer[i] = enemyBuffer[rand];
    //        enemyBuffer[rand] = temp;
    //    }

    //}
    #endregion


    #endregion

    #region PlayerSpawn

    public void SetPlayerEntity(PlayerChar _playerChar)
    {
        /*	var entityObject = Instantiate(playerPrefab, spawnPlayerChar_Tf.position, Quaternion.identity);
			var playerEntityTemp = entityObject.GetComponent<PlayerEntity>(); ;*/

        //var entityObject = Instantiate(playerPrefab, spawnPlayerChar_Tf.position, Quaternion.identity);
        var playerEntityTemp = GameObject.FindWithTag("Player").GetComponent<PlayerEntity>();

        playerEntityTemp.SetupPlayerChar(_playerChar);
        playerEntity = playerEntityTemp;
    }

    //캐릭터 스폰할때 호출할거.
    public void SpawnPlayerEntity()
    {
        ChosePlayer();
        SetPlayerEntity(playerChar);
    }


    //몰라 알아서하셈
    void ChosePlayer()
    {
        playerChar = characterSO.playrChar[0];
    }




    #endregion

    #region TurnManger

    public void SetEnemyObjectArray()
    {
        if (enemyEntitiesObjcet.Length == 0)
        {
            EnemyEntitiesSet();

            Debug.Log("설정나옴");
        }
        if (enemyEntitiesObjcet.Length != 0)
        {
            SpawnEnemyEntity();
        }
    }

    void OnTurnStarted(bool _myTurn)
    {
		if (playerEntity != null)
        {
            SetAttackable(_myTurn);
        }
		else
		{
            playerEntity = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEntity>();            
            SpawnPlayerEntity();
        }

       // Debug.Log(enemyEntitiesObjcet.Length);
    }

    // <<22-11-09 장형용 :: 함수 안 거치고 직접 제거>>
    public void CheckDieEnemy(Entity _entity)
    {
        if (_entity.is_die)
        {
            Destroy(_entity.gameObject);
            enemyEntities.Remove(_entity);
            Debug.Log("하나죽었다.");
        }

        if (enemyEntities.Count == 0)
        {
            Debug.Log("다음방으로");
            UIManager.Inst.ButtonActivate();
            CardManager.Inst.SetCardStateCannot();
            playerEntity.ResetMagicAffinity_Battle();
            playerEntity.ResetMagicAffinity_Turn(false);

            // << 22-12-04 장형용 :: 추가>>
            if (LevelGeneration.Inst.CurrentRoom.RoomEventType == 1)
                SceneManager.LoadScene("SorryScene");
            else
                RewardManager.Inst.GameClear();
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
        // <<22-11-01 장형용 :: 사망 판정 원래 진작에 수정했던 건데 리셋하는 과정에서 누락된 듯 ㅎㅎ;; ㅈㅅ;;;>>
        if (!is_canMouseInput || _entity.is_die)
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
                // <<22-10-21 장형용 :: 변경>>
                //CardManager.Inst.selectCard.UseCard(selectEntity);
                StartCoroutine(CardManager.Inst.selectCard.UseCard(selectEntity, null));
            }
            if (selectEntity == null)
            {
                // <<22-10-21 장형용 :: 변경>>  굳
                //CardManager.Inst.selectCard.UseCard(null, selectPlayerEntity);
                StartCoroutine(CardManager.Inst.selectCard.UseCard(null, selectPlayerEntity));
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
            // <<22-10-21 장형용 :: 변경>>
            //CardManager.Inst.selectCard.UseCard(null);
            StartCoroutine(CardManager.Inst.selectCard.UseCard(null, null));
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
            //CardManager.Inst.selectCard.UseCard(null, playerEntity); // <<22-10-21 장형용 :: 변경>>
            StartCoroutine(CardManager.Inst.selectCard.UseCard(null, playerEntity));
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
            enemyEntities.ForEach(x => x.attackable = false);
            playerEntity.attackable = true;
        }
        else
        {
            enemyEntities.ForEach(x => x.attackable = true);
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

    // 이거 뭐하는 애들이야

	//public void SetUseCard(Card _card)
 //   {
 //       myUseCard = _card;
 //   }

 //   public void DelectUseCard()
 //   {
 //       myUseCard = null;
 //   }

    #endregion





}
