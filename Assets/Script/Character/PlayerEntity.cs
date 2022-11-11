
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
    [SerializeField] TMP_Text healthTMP;
    [SerializeField] TMP_Text ShieldTMP;
    [SerializeField] Image healthImage_Bar;
    [SerializeField] GameObject AttackEffect;
    [SerializeField] SpriteRenderer AttackEffectSpriteRenderer;
    [SerializeField] SpriteRenderer damagedEffectSpriteRenderer;
    [SerializeField] Animator animatior;
    Image healthImage_UI;


    Vector3 originScale;
    Vector3 originPos;

    public bool attackable;
    
    bool is_attackAble;
    bool is_die = false;
    bool is_canUseSelf;

    public int karma = 0;
    int i_enhacneVal = 1;
    int i_calcDamage;


	private void Start()
	{
        healthImage_UI = GameObject.Find("UI_Left_Health").GetComponent<Image>();
        animatior = GetComponent<Animator>();
        SetDefultPS();
    }

    // <<22-10-21 장형용 :: 추가>>
    void OnEnable()
    {
        Utility.onBattleStart += ResetMagicAffinity_Battle;
        Utility.onBattleStart += ResetProtection;

        TurnManager.onStartTurn += ResetValue_Shield;
        TurnManager.onStartTurn += ResetMagicAffinity_Turn;
        TurnManager.onStartTurn += ReduceProtection;
    }

    // <<22-10-21 장형용 :: 추가>>
    void OnDisable()
    {
        Utility.onBattleStart -= ResetMagicAffinity_Battle;
        Utility.onBattleStart -= ResetProtection;

        TurnManager.onStartTurn -= ResetValue_Shield;
        TurnManager.onStartTurn -= ResetMagicAffinity_Turn;
        TurnManager.onStartTurn -= ReduceProtection;
    }


    #region status
    int maxAether = 5; // 최대 코스트
    int i_aether; // 코스트


    float i_health; // <<장형용 :: i가 아니자나 ㅡㅡ>>
    float maxHealth;
    int i_shield = 0;

    //마법 친화성  타입별 친화성으로 방어, 힐 효과 증폭... defult = 0
    // <<22-11-09 장형용 :: 미사용 제거>>
    //[HideInInspector] int i_magicAffinity_fire = 0;
    //[HideInInspector] int i_magicAffinity_earth = 0;
    //[HideInInspector] int i_magicAffinity_water = 0;
    //[HideInInspector] int i_magicAffinity_air = 0;

    [HideInInspector] int i_magicResistance = 0;

    // <<22-10-21 장형용 :: 추가>>
    [HideInInspector] int i_magicAffinity_permanent = 0; // 장비(나 이벤트) 등으로 얻는 계속 유지되는 마나 친화성
    [HideInInspector] int i_magicAffinity_battle = 0; // 1번의 전투 동안 유지되는 마나 친화성
    [HideInInspector] int i_magicAffinity_turn = 0; // 1턴간 유지되는 마나 친화성

    // <<22-11-05 장형용 :: 추가>>
    [HideInInspector] int i_protection = 0; // 수치 이하의 데미지를 방어 (수치 이상의 피해는 방어 못함)

    #endregion


    #region debuff

    // <<22-11-09 장형용 :: 미사용 제거>>
    //[HideInInspector] int i_decrease_magicAffinity;
    //[HideInInspector] int i_status_;

    [HideInInspector] public int i_burning = 0;

    #endregion


    // === 스테이터스 증가/감소 ====
    // 넣는값을 +- 로 조절하기
    #region property_status

    // <<22-11-09 장형용 :: 더미 제거>>
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
    public int Status_MaxAether
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
    public int Status_EnchaneValue
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
    public int Status_MagicAffinity_Permanent
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

    public int Status_MagicAffinity_Battle
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

    public int Status_MagicAffinity_Turn
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

    public int Status_MagicResistance
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
    public int Status_Protection
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

    // <<22-11-07 장형용 :: 가시성을 위해 추가>>
    public int Status_MagicAffinity
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

    public int Debuff_Burning
    {
        get
        {
            return i_burning;
        }

        set
        {
            if(value > 0)
            {
                i_burning = value;
            }
            else
            {
                i_burning = 0;
            }
        }
    }



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
        //if (i_protection >= _damage) // 보호 => 보호 수치 이하의 데미지 무효
        //{
        //    _damage = 0;
        //    return;
        //}

        //// <<22-11-09 장형용 :: 오류 수정>>
        //if (i_protection > 0)
        //{
        //    i_protection--;
        //}

        //int totalDamage = _damage - i_shield;

        //if (totalDamage < 0)
        //{
        //    totalDamage = 0;
        //}

        //if (i_shield > _damage) // 쉴드 => 쉴드 수치만큼 데미지 차감
        //{
        //    i_shield -= _damage;
        //}
        //else
        //{
        //    i_health -= totalDamage;
        //    i_shield = 0;
        //}

        #region Status_Health -= _damage;

        NativeArray<int> values = new NativeArray<int>(5, Allocator.TempJob);
        values[0] = _damage;
        values[1] = Status_Protection;
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

        JobHandle firstJob = new ProtectionJob
        {
            values = values,

            isPrint = DebugManager.instance.isPrintDamageCalculating
        }
        .Schedule();

        JobHandle secondJob = new ShieldJob
        {
            values = values,

            isPrint = DebugManager.instance.isPrintDamageCalculating
        }
        .Schedule(firstJob);

        JobHandle thirdJob = new BurningJob
        {
            values = values,

            isPrint = DebugManager.instance.isPrintDamageCalculating
        }
        .Schedule(secondJob);

        thirdJob.Complete();

        _damage = values[0];
        Status_Protection = values[1];
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

        //  <<22-10-21 장형용 :: 화상 추가>>
        //  <<22-11-12 장형용 :: 위 데미지 공식에 통합 (기존과 동일하게 카드로 가한 데미지에는 포함 X)>>
        //if (Debuff_Burning > 0)
        //{
        //    Burning();
        //}

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
    //{
    //    if (i_shield > Debuff_Burning)
    //    {
    //        i_shield -= Debuff_Burning;
    //    }
    //    else
    //    {
    //        Status_Health -= (i_burning - i_shield);
    //        i_shield = 0;
    //    }

    //    Debuff_Burning--;

    //    if (Status_Health <= 0)
    //    {
    //        Status_Health = 0;
    //        is_die = true;
    //    }

    //    return;
    //}

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

    void ResetValue_Shield(bool isMyTurn)
    {
        if(isMyTurn)
        {
            i_shield = 0;

            RefreshPlayer();
        }
    }

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

    void ResetProtection()
    {
        Status_Protection = 0;

        RefreshPlayer();
    }

    void ReduceProtection(bool isMyTurn)
    {
        if (isMyTurn)
        {
            Status_Protection--;

            CardManager.Inst.RefreshMyHands();
        }
    }

    public void SpellEnchaneReset()
    {
        Status_EnchaneValue = 1;

        CardManager.Inst.RefreshMyHands();
    }


    #region DoTween


    public IEnumerator AttackSprite(Sprite _character, Sprite _effect)
	{
        this.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f) , 0);
        //   charaterSprite.sprite = _character;
        animatior.SetBool("Attack", true);
        AttackEffectSpriteRenderer.sprite = _effect;
        AttackWandEffect();
        yield return new WaitForSeconds(0.25f);
        animatior.SetBool("Attack", false);
        DoOrigin();
	}

    public void SetDamagedSprite(Sprite _damagedEffedt)
	{
        StartCoroutine(DamagedSprite(_damagedEffedt));
	}

    IEnumerator DamagedSprite(Sprite _damagedEffet)
    {
        this.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0);
        charaterSprite.sprite = playerChar.damagedSprite;
        damagedEffectSpriteRenderer.sprite = _damagedEffet;
        DamagedEffect();
        this.transform.DOMove(this.originPos + new Vector3(- 0.15f, 0, 0), 0.15f);
        yield return new WaitForSeconds(0.15f);
        this.transform.DOMove(this.originPos, 0.05f);
        yield return new WaitForSeconds(0.2f);
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