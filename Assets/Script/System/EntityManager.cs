using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using XSSLG;
using UnityEngine.SceneManagement;

public class EntityManager : MonoBehaviour
{
    public static EntityManager Inst { get; private set; }
    // �ν��Ͻ�ȭ �� �ε�� Destroy �Ƚ�Ű��.
    private void Awake()
    {
        Inst = this;
        DontDestroyOnLoad(this);
    }

    #region �������
    [Header("DefultSettings")]
    [SerializeField] float enemyAttackDelay;
    [SerializeField] EnemySO enemySO;
    [SerializeField] EnemySpawnPatternSO enemySpawnPatternSO;
    [SerializeField] EnemySpawnPatternSO enemyBossSO;
    [SerializeField] CharacterSO characterSO;
    [SerializeField] GameObject entitiyPrefab;
    [SerializeField] GameObject playerPrefab;
    //[SerializeField] GameObject damagePrefab; // �̻��
    [SerializeField] GameObject targetSelector;



    //[SerializeField] List<Entity> myEntities;
    [SerializeField] public List<Entity> enemyEntities; // << 22-10-21 ������ :: ���� ���� public���� ����>>
    [SerializeField] public GameObject[] enemyEntitiesObjcet; // << 22-10-21 ������ :: ���� ���� public���� ����>>
    [SerializeField] Entity bossEntity;

    [SerializeField] Transform spawnPlayerChar_Tf;
    [SerializeField] Transform spawnEnemy_Tf;
    [SerializeField] Transform sortEnemyPos_Tf;
    //[SerializeField] Card myUseCard; // �̻��


    //[SerializeField] EnemyAttackList enemyAttackList; // �̻��
    [SerializeField] List<Enemy> enemyBuffer;
    //[SerializeField] short Length; // �̻��
    [SerializeField] private float f_targetSelectorUpPos;

    [HideInInspector] public PlayerEntity playerEntity;


    PlayerChar playerChar;


    //  LevelGeneration levelGeneration;


    const int MAX_ENEMY_COUNT = 3;

    public static int i_checkingEntitiesCount = 0; // <<22-10-30 ������ :: ���� ���� ��ü �ǰ� ��� ��>>



    bool is_canMouseInput => TurnManager.Inst.myTurn && !TurnManager.Inst.isLoding;


    [SerializeField] Entity selectEntity;
    [SerializeField] Entity targetPickEntity;

    [SerializeField] PlayerEntity selectPlayerEntity;
    [SerializeField] PlayerEntity targetPickPlayerEntity;

    //WaitForSeconds delay10 = new WaitForSeconds(1.0f); // �̻��

    #endregion

    private void Start()
    {
        //levelGeneration = GameObject.Find("LevelGenerator").GetComponent<LevelGeneration>();
        TurnManager.onStartTurn += OnTurnStarted;
        //�̰� ������
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
        //foreach ������ :: ���� �� ���� �߰� ������ �����ɸ�
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

    // <<22-10-28 ������ :: �߰�>>
    #region Ÿ�� ���� ���� / ����ִ� ��ü Ȯ��

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

        #region �����ڵ�
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

    #region �ҿ�Ÿ���� �� �����ϱ�
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



    #region �������ϱ�
    //void SetReward(SpawnPattern _pattern)
    //{
    //       RewardManager.Inst.AddReward(_pattern);
    //}
    #endregion

    //�� ID������ �߰� ��ȯ �ӽø��Ƴ���
    public void SelectSpawnEnemyEntity(int ID)
    {
        var entityObject = Instantiate(entitiyPrefab, spawnEnemy_Tf.position, Quaternion.identity);
        var entity = entityObject.GetComponent<Entity>();
        entity.attackable = false;

        enemyEntities.Add(entity);
        //entity.SetupEnemy(enemySO.enemy[ID]);
        EnemyEntityAlignment();
    }

    // �̰� ȣ���ϸ� �� ȣ��
    public void SpawnEnemyEntity()
    {
        #region �����ڵ� 
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
                    Debug.Log("���������� : " + entity.transform.position); 
                }*/
    }


    public void SpawnEnemyBossEntity()
    {
        SetEnemyBossEntity();
    }

    //��ǻ���°�
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


    #region ���� ���̰� �Ǿ���� �� ���ۿ� �߰��ϱ�.....
    //void SetupEnemyBuffer()
    //{
    //    enemyBuffer = new List<Enemy>(100);
    //    int rand = UnityEngine.Random.Range(0, enemySpawnPatternSO.spawnPattern.Length);

    //    // ������ ���ۿ� �߰�
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

    //ĳ���� �����Ҷ� ȣ���Ұ�.
    public void SpawnPlayerEntity()
    {
        ChosePlayer();
        SetPlayerEntity(playerChar);
    }


    //���� �˾Ƽ��ϼ�
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

            Debug.Log("��������");
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

    // <<22-11-09 ������ :: �Լ� �� ��ġ�� ���� ����>>
    public void CheckDieEnemy(Entity _entity)
    {
        if (_entity.is_die)
        {
            Destroy(_entity.gameObject);
            enemyEntities.Remove(_entity);
            Debug.Log("�ϳ��׾���.");
        }

        if (enemyEntities.Count == 0)
        {
            Debug.Log("����������");
            UIManager.Inst.ButtonActivate();
            CardManager.Inst.SetCardStateCannot();
            playerEntity.ResetMagicAffinity_Battle();
            playerEntity.ResetMagicAffinity_Turn(false);

            // << 22-12-04 ������ :: �߰�>>
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

        // �̰Ŵ� �ణ �����������. ���ݺҸ����� ��� �����ؾ����� �ణ ����ʿ�,
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
        // <<22-11-01 ������ :: ��� ���� ���� ���ۿ� �����ߴ� �ǵ� �����ϴ� �������� ������ �� ����;; ����;;;>>
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
    //�� Ŭ���� ���콺 �� �� �����.


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

        //ī�� ��� ���� SelectCard�� ����� ���ν� �̹����� ���� ���� �ʵ��� ��.
        try
        {
            CardManager.Inst.UseCardSetmyCemetery();
            if (selectPlayerEntity == null)
            { 
                // <<22-10-21 ������ :: ����>>
                //CardManager.Inst.selectCard.UseCard(selectEntity);
                StartCoroutine(CardManager.Inst.selectCard.UseCard(selectEntity, null));
            }
            if (selectEntity == null)
            {
                // <<22-10-21 ������ :: ����>>  ��
                //CardManager.Inst.selectCard.UseCard(null, selectPlayerEntity);
                StartCoroutine(CardManager.Inst.selectCard.UseCard(null, selectPlayerEntity));
            }
        }
        catch
        {
            Debug.Log("SelectCard�� ������ϴ�.");
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
            // <<22-10-21 ������ :: ����>>
            //CardManager.Inst.selectCard.UseCard(null);
            StartCoroutine(CardManager.Inst.selectCard.UseCard(null, null));
        }
        catch
        {
            Debug.Log("SelectCard�� ������ϴ�.");
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
            //CardManager.Inst.selectCard.UseCard(null, playerEntity); // <<22-10-21 ������ :: ����>>
            StartCoroutine(CardManager.Inst.selectCard.UseCard(null, playerEntity));
        }
        catch
        {
            Debug.Log("SelectCard�� ������ϴ�.");
        }
    }





    //�� �Ѿ�� ���� ���� ���� ����.
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
        // �� �Ѿ �� �����������̽� ���� //���� ���� �ڵ���. 
        try
        {
            targetSelector.SetActive(false);
        }
        catch
        {
            Debug.Log("�̹� �����ֽ��ϴ�.");
        }
      

    }
	#endregion

	#region Card

    // �̰� ���ϴ� �ֵ��̾�

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
