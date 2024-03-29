using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EntityManager : MonoBehaviour
{
    public static EntityManager Inst { get; private set; }
    // 昔什渡什鉢 貢 稽球獣 Destroy 照獣徹奄.
    private void Awake()
    {
        Inst = this;
    }

    [SerializeField] EnemySO enemySO;
    [SerializeField] EnemyBossSO enemyBossSO;
    [SerializeField] CharacterSO characterSO;
    [SerializeField] GameObject entitiyPrefab;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject damagePrefab;
    [SerializeField] GameObject targetSelector;



    //[SerializeField] List<Entity> myEntities;
    [SerializeField] List<Entity> enemyEntities;
    [SerializeField] Entity bossEntity;

    [SerializeField] Transform spawnPlayerChar_Tf;
    [SerializeField] Transform spawnEnemy_Tf;
    [SerializeField] Transform sortEnemyPos_Tf;
    [SerializeField] Card myUseCard;



    [SerializeField] List<Enemy> enemyBuffer;
    [SerializeField] short Length;

    [HideInInspector] public PlayerEntity playerEntity;


    PlayerChar playerChar;





    LevelGeneration levelGeneration;

    [SerializeField] private float f_targetSelectorUpPos;
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


    //鎧亜 訊 戚享縦生稽 足澗走 蟹亀乞硯 せせせせせ 陥獣壱徴
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
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(enemy.Attack(playerEntity, enemy));
        }

        Debug.Log("Call from EnemyEntityAttack");
        yield return new WaitForSeconds(0.1f);
        TurnManager.Inst.EndTurn();
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

    // 戚暗 硲窒馬檎 陥 硲窒
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

        // 焼戚奴 獄遁拭 蓄亜
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



    //蝶遣斗 漆刑闘 但 幻級暗績 託板 呪舛 採店.... 刊亜 拝走澗 乞牽畏走幻 壱持背虞 ぞぞ 坪球切端澗 企採歳 薗懐馬惟 幻級醸澗汽 格巷 瑛諾焼辞 痕呪誤 繕榎 戚雌馬延馬革 ぞぞ;; 耕照杯艦陥!
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
		foreach (var enemy in enemyEntities)
		{
			if (enemy.is_die)
			{
                enemy.DestroyTest();
                enemyEntities.Remove(enemy);
                Debug.Log("馬蟹宋醸陥.");
            }
            if (enemyEntities.Count == 0)
            {
                Debug.Log("陥製号生稽");
                UIManager.Inst.ButtonActivate();  
            }
        }

    }



	#endregion




	#region PlayerEntity

	public void EntityMouseOverPlayer(PlayerEntity _playerEntity)
    {
        if (!is_canMouseInput)
            return;

        targetPickPlayerEntity = _playerEntity;

        // 戚暗澗 鉦娃 呪舛推社赤製. 因維災鍵葵聖 嬢胸惟 呪舛背醤拝走 鉦娃 壱肯琶推,
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
        UseCard();
    }


    #endregion


    #region Entity

    public void EntityMouseOver(Entity _entity)
    {
        if (!is_canMouseInput)
            return;

        targetPickEntity = _entity;

        if (targetPickEntity.attackable && CardManager.Inst.is_cardUsing)
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
        UseCard();
    }
    #endregion


    #region CardUseSet
    //旋 適遣板 原酔什 穣 獣 旋遂喫.


    void UseCard()
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


        //朝球 紫遂 送板 SelectCard研 搾趨捜 生稽潤 戚庚姥亜 叔楳 鞠走 省亀系 敗.
        try
        {
            CardManager.Inst.UseCardSetmyCemetery();
			if (selectPlayerEntity == null)
			{
                BattleCalculater.Inst.BattleCalc(CardManager.Inst.selectCard, selectEntity);
            }
            if (selectEntity == null)
            {
                BattleCalculater.Inst.BattleCalc(CardManager.Inst.selectCard, selectPlayerEntity);
            }
        }
        catch
        {
            Debug.Log("SelectCard亜 搾醸柔艦陥.");
        }
        SetSelectedCardNull();
    }

    void SetSelectedCardNull()
    {
        CardManager.Inst.selectCard = null;
    }

    //渡 角嬢哀凶 因維 亜管 食採 痕井.
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
        // 渡 角嬢哀 凶 因維昔斗凪戚什 塊奄 //獄益 呪舛 坪球績. 
        try
        {
            targetSelector.SetActive(false);
        }
        catch
        {
            Debug.Log("戚耕 襖閃赤柔艦陥.");
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
