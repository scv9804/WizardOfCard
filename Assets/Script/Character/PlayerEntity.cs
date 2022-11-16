
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using Unity.Jobs;
using Unity.Collections;

public class PlayerEntity : MonoBehaviour
{
    public static PlayerEntity Inst { get; private set; }

    private void Awake()
    {
        Inst = this;

        //DontDestroyOnLoad(this);
    }

    [HideInInspector] public PlayerChar playerChar;
    [SerializeField] SpriteRenderer charaterSprite;
    [SerializeField] GameObject spineObeject;
    [SerializeField] TMP_Text healthTMP;
    [SerializeField] TMP_Text ShieldTMP;
    [SerializeField] Image healthImage_Bar;
    [SerializeField] GameObject AttackEffect;
    [SerializeField] SpriteRenderer AttackEffectSpriteRenderer;
    [SerializeField] SpriteRenderer damagedEffectSpriteRenderer;
    Image healthImage_UI;

    Vector3 originScale;
    Vector3 originPos;

    public bool attackable;

    //bool is_attackAble; // 미사용 더미
    bool is_die = false; // 사용은 되는데 의미는 없음
    //bool is_canUseSelf; // 미사용 더미

    public int karma = 0;
    int i_enhacneVal = 1;
    int playerMoney = 0;
    //int i_calcDamage;

    #region Job System

    ProtectionJob myProtectionJob = new ProtectionJob();
    ShieldJob myShieldJob = new ShieldJob();
    BurningJob myBurningJob = new BurningJob();

    #endregion

    private void Start()
	{
        healthImage_UI = GameObject.Find("UI_Left_Health").GetComponent<Image>();
        SetDefultPS();
    }

    // <<22-10-21 장형용 :: 추가>>
    void OnEnable()
    {
        #region 액션 등록

        Utility.onBattleStart += ResetMagicAffinity_Battle;
        Utility.onBattleStart += ResetProtection;

        TurnManager.onStartTurn += ResetValue_Shield;
        TurnManager.onStartTurn += ResetMagicAffinity_Turn;
        TurnManager.onStartTurn += ReduceProtection;
        TurnManager.onStartTurn += ResetCannotDrawCard;

        #endregion
    }

    // <<22-10-21 장형용 :: 추가>>
    void OnDisable()
    {
        #region 액션 등록 해제

        Utility.onBattleStart -= ResetMagicAffinity_Battle;
        Utility.onBattleStart -= ResetProtection;

        TurnManager.onStartTurn -= ResetValue_Shield;
        TurnManager.onStartTurn -= ResetMagicAffinity_Turn;
        TurnManager.onStartTurn -= ReduceProtection;
        TurnManager.onStartTurn -= ResetCannotDrawCard;

        #endregion
    }

    #region 플레이어 능력치

    // Ex) Health, Aether, Shield, ...
    #region Status

    int maxAether = 5; // 최대 코스트
    // <<22-11-12 장형용 :: 전투 간 추가되는 임시 최대 코스트 추가>>
    int maxAether_battle;
    int i_aether; // 코스트

    float i_health; // <<장형용 :: i가 아니자나 ㅠㅠㅠㅠ>>
    float maxHealth;
    int i_shield = 0;

    #endregion

    // Ex) Magic Affinity, Magic Resistance, ...
    #region Buffs

    // 마법 친화성  타입별 친화성으로 방어, 힐 효과 증폭... defult = 0
    // <<22-11-09 장형용 :: 제거>>
    //int i_magicAffinity_fire = 0;
    //int i_magicAffinity_earth = 0;
    //int i_magicAffinity_water = 0;
    //int i_magicAffinity_air = 0;

    int i_magicResistance = 0;

    // <<22-10-21 장형용 :: 추가>>
    int i_magicAffinity_permanent = 0; // 장비(나 이벤트) 등으로 얻는 계속 유지되는 마나 친화성
    int i_magicAffinity_battle = 0; // 1번의 전투 동안 유지되는 마나 친화성
    int i_magicAffinity_turn = 0; // 1턴간 유지되는 마나 친화성

    // <<22-11-05 장형용 :: 추가>>
    int i_protection = 0; // 수치 이하의 데미지를 방어 (수치 이상의 피해는 방어 못함, 쉴드 상위호환으로 겹쳐서 살짝 바꿔봄)

    // <<22-11-12 장형용 :: 상태이상을 막아주는 면역 효과 추가>>
    int i_emmune;

    #endregion

    // Ex) Burning, Cannot Draw Card, ...
    #region Debuffs

    // <<22-11-09 장형용 :: 제거>>
    //int i_decrease_magicAffinity;
    //int i_status_;

    int i_burning = 0;

    bool b_cannotDrawCard = false;

    #endregion

    #endregion

    #region Properties

    // === 스테이터스 증가/감소 ====
    // 넣는값을 +- 로 조절하기
    #region Status

    // <<22-11-09 장형용 :: 제거>> 일단더미로 두자
    //public void Add_Status_MagicAffinity_Fire(int _addStatus)
    //public void Add_Status_MagicAffinity_Earth(int _addStatus)
    //public void Add_Status_MagicAffinity_Water(int _addStatus)
    //public void Add_Status_MagicAffinity_Air(int _addStatus)

    public float Status_Health
    {
        get
        {
            return i_health;
        }
        set
        {
            // <<22-10-26 장형용 :: 별도의 초과회복 처리를 하지 않아도 되도록 수정>>
            if (value > maxHealth)
            {
                i_health = maxHealth;
            }
            else
            {
                i_health = value;
            }

            RefreshPlayer();
        }
    }

    public float Status_MaxHealth
    {
        get
        {
            return maxHealth;
        }
        set
        {
            maxHealth = value;

            RefreshPlayer();
        }
    }
    public int Status_Aether
    {
        get
        {
            return i_aether;
        }
        set
        {
            i_aether = value;

            RefreshPlayer();
        }

    }

    public int Status_MaxAether_Permanent
    {
        get
        {
            return maxAether;
        }
        set
        {
            maxAether = value;

            RefreshPlayer();
        }

    }

    // <<22-11-12 장형용 :: 추가>>
    public int Status_MaxAether_Battle
    {
        get
        {
            return maxAether_battle;
        }
        set
        {
            maxAether_battle = value;

            RefreshPlayer();
        }
    }

    public int Status_MaxAether
    {
        get
        {
            return maxAether_battle + maxAether;
        }

        //set
        //{
        //    Status_MaxAether = value;
        //}
    }

    public int Status_Shiled
    {
        get
        {
            return i_shield;
        }
        set
        {
            if (value > 0)
            {
                i_shield = value;
            }
            else
            {
                i_shield = 0;
            }

            //i_shield = value;
            RefreshPlayer();
        }
    }

    public int money
	{
        get
        {
            return playerMoney;
        }

		set
		{
            playerMoney += value;
            UIManager.Inst.money_TMP.text = playerMoney.ToString();
		}
	}

    // <<22-11-12 장형용 :: 해당 Properties들을 Buff 또는 Debuff로 변경>>
    //public int Status_EnchaneValue
    //public int Status_MagicAffinity_Permanent
    //public int Status_MagicAffinity_Battle
    //public int Status_MagicAffinity_Turn
    //public int Status_MagicResistance
    //public int Status_Protection
    //public int Status_MagicAffinity

    #endregion

    // <<22-11-12 장형용 :: Status => Buff로 변경 및 정리>>
    #region Buffs

    public int Buff_EnchaneValue
    {
        get
        {
            return i_enhacneVal;
        }
        set
        {
            i_enhacneVal = value;
        }
    }

    // <<22-10-21 장형용 :: 추가>>
    #region ManaAffinities

    public int Buff_MagicAffinity_Permanent
    {
        get
        {
            return i_magicAffinity_permanent;
        }

        set
        {
            i_magicAffinity_permanent = value;
        }
    }

    public int Buff_MagicAffinity_Battle
    {
        get
        {
            return i_magicAffinity_battle;
        }

        set
        {
            i_magicAffinity_battle = value;
        }
    }

    public int Buff_MagicAffinity_Turn
    {
        get
        {
            return i_magicAffinity_turn;
        }

        set
        {
            i_magicAffinity_turn = value;
        }
    }

    // <<22-11-07 장형용 :: 가시성을 위해 추가>>
    public int Buff_MagicAffinity // 전체 마나 친화성의 총합
    {
        get
        {
            return i_magicAffinity_permanent
                + i_magicAffinity_battle
                + i_magicAffinity_turn;
        }

        //set
        //{
        //    Status_MagicAffinity = value;
        //}
    }

    #endregion

    // <<22-10-21 장형용 :: 추가>>
    public int Buff_MagicResistance
    {
        get
        {
            return i_magicResistance;
        }

        set
        {
            i_magicResistance = value;
        }
    }

    // <<22-11-05 장형용 :: 추가>>
    public int Buff_Protection
    {
        get
        {
            return i_protection;
        }

        set
        {
            if (value > 0)
            {
                i_protection = value;
            }
            else
            {
                i_protection = 0;
            }
        }
    }

    // <<22-11-12 장형용 :: 추가>>
    public int Buff_Emmune
    {
        get
        {
            return i_emmune;
        }

        set
        {
            i_emmune = value;
        }
    }

    #endregion

    // <<22-11-12 장형용 :: 정리>>
    #region Debuffs

    public int Debuff_Burning
    {
        get
        {
            return i_burning;
        }

        set
        {
            if (value > 0)
            {
                i_burning = value;
            }
            else
            {
                i_burning = 0;
            }
        }
    }

    public bool Debuff_CannotDrawCard
    {
        get
        {
            return b_cannotDrawCard;
        }

        set
        {
            b_cannotDrawCard = value;
        }
    }

    #endregion

    #endregion

    // 플레이어 기본 정보 설정
    public void SetupPlayerChar(PlayerChar _playerChar)
    {
        playerChar = _playerChar;
        i_health = _playerChar.i_health;
        maxHealth = i_health;
		if (i_shield == 0)
        {
            ShieldTMP.gameObject.SetActive(false);
		}
		else
		{
            ShieldTMP.text = i_shield.ToString();
            ShieldTMP.gameObject.SetActive(true);
        }


        UIManager.Inst.HealthTMP_UI.text = i_health.ToString();
        charaterSprite.sprite = _playerChar.sp_sprite;
        healthTMP.text = i_health.ToString();
    }

    // <<22-10-21 장형용 :: 수정>>
    // <<22-11-12 장형용 :: 대폭 수정, 최대한의 디버깅을 했으나 버그가 있을 수 있음>>
    public void Damaged(int _damage, Card _card = null)
    {
        if (_damage < 0)
            _damage = 0;

        #region Status_Health -= _damage;

        NativeArray<int> values = new NativeArray<int>(5, Allocator.TempJob);
        values[0] = _damage;
        values[1] = Buff_Protection;
        values[2] = Status_Shiled;
        values[3] = (int)Status_Health;
        values[4] = Debuff_Burning;

        #region 입력 값 디버그

        if (DebugManager.instance.isPrintDamageCalculating)
        {
            Debug.Log("-------------------------------------");
            Debug.Log("입력 - 데미지 : " + values[0]);
            Debug.Log("입력 - 보호 : " + values[1]);
            Debug.Log("입력 - 쉴드 : " + values[2]);
            Debug.Log("입력 - 체력 : " + values[3]);
            Debug.Log("입력 - 화상 : " + values[4]);
            Debug.Log("-------------------------------------");
        }

        #endregion

        myProtectionJob.values = values;
        myProtectionJob.isPrint = DebugManager.instance.isPrintDamageCalculating;

        JobHandle firstJob = myProtectionJob.Schedule();

        myShieldJob.values = values;
        myShieldJob.isPrint = DebugManager.instance.isPrintDamageCalculating;

        JobHandle secondJob = myShieldJob.Schedule(firstJob);

        myBurningJob.values = values;
        myBurningJob.isPrint = DebugManager.instance.isPrintDamageCalculating;

        JobHandle thirdJob = myBurningJob.Schedule(secondJob);

        thirdJob.Complete();

        _damage = values[0];
        Buff_Protection = values[1];
        Status_Shiled = values[2];
        Status_Health = values[3];
        Debuff_Burning = values[4];

        #region 결과 값 디버그

        if (DebugManager.instance.isPrintDamageCalculating)
        {
            Debug.Log("출력 - 데미지 : " + values[0]);
            Debug.Log("출력 - 보호 : " + values[1]);
            Debug.Log("출력 - 쉴드 : " + values[2]);
            Debug.Log("출력 - 체력 : " + values[3]);
            Debug.Log("출력 - 화상 : " + values[4]);
            Debug.Log("-------------------------------------");
        }

        #endregion

        values.Dispose();

        #endregion

        if (_damage > 0)
            Utility.onDamaged?.Invoke(_card, _damage);

        if (Status_Health <= 0)
        {
            Status_Health = 0;
            is_die = true;

            StartCoroutine(GameManager.Inst.GameOverScene());
        }

        RefreshPlayer();
    }

    //  <<22-10-21 장형용 :: 화상 추가>>
    //  <<22-11-12 장형용 :: 위 데미지 공식에 통합 (기존과 동일하게 카드로 가한 데미지에는 포함 X)>>
    //public void Burning()

    public void RefreshPlayer()
    {
        Set_ShieldActivate();
        healthImage_Bar.fillAmount = i_health / maxHealth;
        healthImage_UI.fillAmount = i_health / maxHealth;
        UIManager.Inst.HealthTMP_UI.text = i_health.ToString();
        healthTMP.text = i_health.ToString();
        ShieldTMP.text = i_shield.ToString();     
    }

    void Set_ShieldActivate()
	{
        if (0 < i_shield)
        {
            ShieldTMP.gameObject.SetActive(true);
        }
        else
        {
            ShieldTMP.gameObject.SetActive(false);
        }
    }

    #region StatusValueChange

    #region 마나 친화성

    void ResetMagicAffinity_Battle()
    {
        i_magicAffinity_battle = 0;

        CardManager.Inst.RefreshMyHands();
    }

    void ResetMagicAffinity_Turn(bool isMyTurn)
    {
        if (isMyTurn)
        {
            i_magicAffinity_turn = 0;

            CardManager.Inst.RefreshMyHands();
        }
    }

    #endregion

    #region 보호

    void ResetProtection()
    {
        Buff_Protection = 0;

        RefreshPlayer();
    }

    void ReduceProtection(bool isMyTurn)
    {
        if (isMyTurn)
        {
            Buff_Protection--;

            CardManager.Inst.RefreshMyHands();
        }
    }

    #endregion

    void ResetValue_Shield(bool isMyTurn)
    {
        if (isMyTurn)
        {
            i_shield = 0;

            RefreshPlayer();
        }
    }

    public void ResetEnhanceValue()
    {
        Buff_EnchaneValue = 1;

        CardManager.Inst.RefreshMyHands();
    } 

    public void ResetCannotDrawCard(bool isMyTurn)
    {
        if (isMyTurn)
        {
            Debuff_CannotDrawCard = false;
        }
    }

    #endregion

    #region DoTween


    public IEnumerator AttackSprite(Sprite _character, Sprite _effect)
	{
        this.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f) , 0);
        charaterSprite.sprite = _character;
        charaterSprite.enabled = true;
        spineObeject.SetActive(false);
        AttackEffectSpriteRenderer.sprite = _effect;
        AttackWandEffect();
        this.transform.DOMove(this.originPos + new Vector3(0.15f, 0, 0), 0.15f);
        yield return new WaitForSeconds(0.15f);
        this.transform.DOMove(this.originPos, 0.05f);
        yield return new WaitForSeconds(0.20f);
        charaterSprite.enabled = false;
        spineObeject.SetActive(true);
        DoOrigin();
	}

    //공격스킬 아닌 이펙트같은건 다 여기로
    public IEnumerator SpecialSkillSprite(Sprite _character , Sprite _effect)
	{
        this.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0);
        charaterSprite.sprite = _character;
        charaterSprite.enabled = true;
        spineObeject.SetActive(false);
        AttackEffectSpriteRenderer.sprite = _effect;
        AttackWandEffect();
        yield return new WaitForSeconds(0.35f);
        charaterSprite.enabled = false;
        spineObeject.SetActive(true);
        DoOrigin();
    }

    public void SetDamagedSprite(Sprite _damagedEffedt)
	{
        StartCoroutine(DamagedSprite(_damagedEffedt));
	}

    IEnumerator DamagedSprite(Sprite _damagedEffet)
    {
        this.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0);
        charaterSprite.enabled = true;
        spineObeject.SetActive(false);
        charaterSprite.sprite = playerChar.damagedSprite;
        damagedEffectSpriteRenderer.sprite = _damagedEffet;
        DamagedEffect();
        this.transform.DOMove(this.originPos + new Vector3(- 0.15f, 0, 0), 0.15f);
        yield return new WaitForSeconds(0.15f);
        this.transform.DOMove(this.originPos, 0.05f);
        yield return new WaitForSeconds(0.2f);
        charaterSprite.enabled = false;
        spineObeject.SetActive(true);
        DoOrigin();
    }


    public void AttackWandEffect()
	{
        Sequence sequence1 = DOTween.Sequence()
        .Append(AttackEffectSpriteRenderer.DOFade(1, 0.15f))
        .Append(AttackEffectSpriteRenderer.DOFade(0, 0.05f));
    }

    public void DamagedEffect()
    {
        Sequence sequence1 = DOTween.Sequence()
        .Append(damagedEffectSpriteRenderer.DOFade(1, 0.15f))
        .Append(damagedEffectSpriteRenderer.DOFade(0, 0.05f));
    }


    public void DoOrigin()
	{
        this.transform.DOScale(originScale, 0);
        this.transform.position = originPos;
        charaterSprite.sprite = playerChar.sp_sprite;
	}




    public void SetDefultPS()
	{
        originScale = this.transform.localScale;
        originPos = this.transform.position;
	}


	#endregion

	#region MouseControlle
	private void OnMouseOver()
    {
       EntityManager.Inst.EntityMouseOverPlayer(this);
    }

    private void OnMouseExit()
    {
        EntityManager.Inst.PlayerEntityMouseExit();
    }

    private void OnMouseUp()
    {
        EntityManager.Inst.PlayerEntityMouseUp();
    }

    private void OnMouseDown()
    {
        EntityManager.Inst.PlayerEntityMouseDown();
    }

	#endregion
}