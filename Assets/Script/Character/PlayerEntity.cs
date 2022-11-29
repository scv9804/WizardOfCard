
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
    [HideInInspector] public int money;
    [HideInInspector] public int manaInchentValue = 0;
    [SerializeField] SpriteRenderer charaterSprite;
    [SerializeField] GameObject spineObeject;
    [SerializeField] TMP_Text healthTMP;
    [SerializeField] TMP_Text ShieldTMP;
    [SerializeField] GameObject shieldEffectObject;
    [SerializeField] SpriteRenderer shieldEffectSpriteRenderer;
    [HideInInspector] public Vector3 originShieldEffectScale;
    [SerializeField] Image healthImage_Bar;
    [SerializeField] GameObject AttackEffect;
    [SerializeField] SpriteRenderer AttackEffectSpriteRenderer;
    [SerializeField] SpriteRenderer damagedEffectSpriteRenderer;

    [Header("������")]
    [SerializeField] GameObject buffImageSlot;
    [SerializeField] GameObject buffPrefab;
    [SerializeField] TMP_Text skillNameTmp;
    [SerializeField] List<GameObject> buffImageList;

    Image healthImage_UI;

    Vector3 originSkillNamePos;
    Vector3 originScale;
    Vector3 originPos;

    public bool attackable;

    bool isTextMove = false; // ���� �ؽ�Ʈ �����̴°� üũ��
    bool isShieldAnim = false;
    bool is_die = false; // ����� �Ǵµ� �ǹ̴� ����


    public int karma = 0;

    int popupSpeed = 10;
    int i_enhacneVal = 1;
    //int i_calcDamage;

    #region Job System

    ProtectionJob myProtectionJob = new ProtectionJob();
    ShieldJob myShieldJob = new ShieldJob();
    BurningJob myBurningJob = new BurningJob();

    #endregion

    private void Start()
	{
        healthImage_UI = GameObject.Find("UI_Left_Health").GetComponent<Image>();
        originShieldEffectScale = shieldEffectObject.transform.localScale;
        SetDefultPS();
    }

    private void FixedUpdate()
    {
        if (isTextMove)
        {
            skillNameTmp.rectTransform.anchoredPosition3D += popupSpeed * Vector3.up;
        }
    }
    // <<22-10-21 ������ :: �߰�>>
    void OnEnable()
    {
        #region �׼� ���

        Utility.onBattleStart += ResetMagicAffinity_Battle;
        Utility.onBattleStart += ResetProtection;

        TurnManager.onStartTurn += ResetValue_Shield;
        TurnManager.onStartTurn += ResetMagicAffinity_Turn;
        TurnManager.onStartTurn += ReduceProtection;
        TurnManager.onStartTurn += ResetCannotDrawCard;

        #endregion
    }

    // <<22-10-21 ������ :: �߰�>>
    void OnDisable()
    {
        #region �׼� ��� ����

        Utility.onBattleStart -= ResetMagicAffinity_Battle;
        Utility.onBattleStart -= ResetProtection;

        TurnManager.onStartTurn -= ResetValue_Shield;
        TurnManager.onStartTurn -= ResetMagicAffinity_Turn;
        TurnManager.onStartTurn -= ReduceProtection;
        TurnManager.onStartTurn -= ResetCannotDrawCard;

        #endregion
    }

    #region Player's Stat

    // Ex) Health, Aether, Shield, ...
    #region Status

    int maxAether = 5; // �ִ� �ڽ�Ʈ
    // <<22-11-12 ������ :: ���� �� �߰��Ǵ� �ӽ� �ִ� �ڽ�Ʈ �߰�>>
    int maxAether_battle;
    int i_aether; // �ڽ�Ʈ

    float i_health; // <<������ :: i�� �ƴ��ڳ� �ФФФ�>>
    float maxHealth;
    int i_shield = 0;

    #endregion

    // Ex) Magic Affinity, Magic Resistance, ...
    #region Buffs

    // ���� ģȭ��  Ÿ�Ժ� ģȭ������ ���, �� ȿ�� ����... defult = 0
    // <<22-11-09 ������ :: ����>>
    //int i_magicAffinity_fire = 0;
    //int i_magicAffinity_earth = 0;
    //int i_magicAffinity_water = 0;
    //int i_magicAffinity_air = 0;

    int i_magicResistance = 0;

    // <<22-10-21 ������ :: �߰�>>
    int i_magicAffinity_permanent = 0; // ���(�� �̺�Ʈ) ������ ��� ��� �����Ǵ� ���� ģȭ��
    int i_magicAffinity_stage = 0; // 1���� ���������� �����Ǵ� ���� ģȭ��
    int i_magicAffinity_battle = 0; // 1���� ������ �����Ǵ� ���� ģȭ��
    int i_magicAffinity_turn = 0; // 1�ϰ� �����Ǵ� ���� ģȭ��

    // <<22-11-05 ������ :: �߰�>>
    int i_protection = 0; // ��ġ ������ �������� ��� (��ġ �̻��� ���ش� ��� ����, ���� ����ȣȯ���� ���ļ� ��¦ �ٲ㺽)

    // <<22-11-12 ������ :: �����̻��� �����ִ� �鿪 ȿ�� �߰�>>
    int i_emmune;

    #endregion

    // Ex) Burning, Cannot Draw Card, ...
    #region Debuffs

    // <<22-11-09 ������ :: ����>>
    //int i_decrease_magicAffinity;
    //int i_status_;

    int i_burning = 0;

    bool b_cannotDrawCard = false;

    #endregion

    #endregion

    #region Properties

    // === �������ͽ� ����/���� ====
    // �ִ°��� +- �� �����ϱ�
    #region Status

    public float Status_Health
    {
        get
        {
            return i_health;
        }
        set
        {
            // <<22-10-26 ������ :: ������ �ʰ�ȸ�� ó���� ���� �ʾƵ� �ǵ��� ����>>
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

    // <<22-11-12 ������ :: �߰�>>
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

		set
		{
			maxAether = value;
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

    // <<22-11-12 ������ :: �ش� Properties���� Buff �Ǵ� Debuff�� ����>>
    //public int Status_EnchaneValue
    //public int Status_MagicAffinity_Permanent
    //public int Status_MagicAffinity_Battle
    //public int Status_MagicAffinity_Turn
    //public int Status_MagicResistance
    //public int Status_Protection
    //public int Status_MagicAffinity

    #endregion

    // <<22-11-12 ������ :: Status => Buff�� ���� �� ����>>
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

    // <<22-10-21 ������ :: �߰�>>
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

    // <<22-11-24 ������ :: �߰�>>
    public int Buff_MagicAffinity_Stage
    {
        get
        {
            return i_magicAffinity_stage;
        }

        set
        {
            i_magicAffinity_stage = value;
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

    // ��ü ���� ģȭ���� ����
    // <<22-11-07 ������ :: ���ü��� ���� �߰�>>
    // <<22-11-24 ������ :: �������� ���� ģȭ�� �߰�>>
    public int Buff_MagicAffinity
    {
        get
        {
            return i_magicAffinity_permanent
                + i_magicAffinity_stage
                + i_magicAffinity_battle
                + i_magicAffinity_turn;
        }
    }

    #endregion

    // <<22-10-21 ������ :: �߰�>>
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

    // <<22-11-05 ������ :: �߰�>>
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

    // <<22-11-12 ������ :: �߰�>>
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

    // <<22-11-12 ������ :: ����>>
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

    public IEnumerator SkillNamePopup(string _skillName)
    {
        skillNameTmp.text = _skillName;
        skillNameTmp.rectTransform.anchoredPosition3D = originSkillNamePos;
        skillNameTmp.gameObject.SetActive(true);
        isTextMove = true;

        Sequence sequence = DOTween.Sequence()
       .Append(skillNameTmp.DOFade(1, 0.0f))
       .Append(skillNameTmp.DOFade(0, 1f));
        yield return new WaitForSeconds(1f);

        isTextMove = false;
        skillNameTmp.gameObject.SetActive(false);
    }

    #region ���� �̹��� �˾�
    public void AddBuffImage(Sprite _sprite, string _buffDebuffName, int _code, int _value)
    {
        var temt = Instantiate(buffPrefab);
        temt.GetComponent<BuffDebuffImageSpawn>().Setup(_sprite, _buffDebuffName, _value, _code);
        temt.transform.SetParent(buffImageSlot.transform, false);
        buffImageList.Add(temt);
    }

    public bool CompareBuffImage(int _code, int _value)
    {
        foreach (var buff in buffImageList)
        {
            var temt = buff.GetComponent<BuffDebuffImageSpawn>();

            if (temt.BuffDebuffCode == _code)
            {
                temt.value += _value;
                temt.SetValue();
                return true;
            }
        }
        return false;
    }

    #endregion


    // �÷��̾� �⺻ ���� ����
    public void SetupPlayerChar(PlayerChar _playerChar)
    {
        playerChar = _playerChar;
        i_health = _playerChar.i_health;
        maxHealth = i_health;
        money = 0;
		if (i_shield == 0)
        {
            ShieldTMP.gameObject.SetActive(false);
		}
		else
		{
            ShieldTMP.text = i_shield.ToString();
            ShieldTMP.gameObject.SetActive(true);
        }

        originSkillNamePos = skillNameTmp.rectTransform.anchoredPosition3D;
        UIManager.Inst.HealthTMP_UI.text = i_health.ToString();
        charaterSprite.sprite = _playerChar.sp_sprite;
        healthTMP.text = i_health.ToString();
        UIManager.Inst.money_TMP.text = money.ToString("D3");
    }

    // <<22-10-21 ������ :: ����>>
    // <<22-11-12 ������ :: ���� ����, �ִ����� ������� ������ ���װ� ���� �� ����>>
    // <<22-11-24 ������ :: ��ȯ Ÿ�� Int�� ����>>
    public int Damaged(int _damage, Card _card = null)
    {
        #region Status_Health -= _damage;

        NativeArray<int> values = new NativeArray<int>(5, Allocator.TempJob);
        values[0] = _damage;
        values[1] = Buff_Protection;
        values[2] = Status_Shiled;
        values[3] = (int)Status_Health;
        values[4] = Debuff_Burning;

        #region �Է� �� �����

        if (DebugManager.instance.isPrintDamageCalculating)
        {
            Debug.Log("-------------------------------------");
            Debug.Log("�Է� - ������ : " + values[0]);
            Debug.Log("�Է� - ��ȣ : " + values[1]);
            Debug.Log("�Է� - ���� : " + values[2]);
            Debug.Log("�Է� - ü�� : " + values[3]);
            Debug.Log("�Է� - ȭ�� : " + values[4]);
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

        #region ��� �� �����

        if (DebugManager.instance.isPrintDamageCalculating)
        {
            Debug.Log("��� - ������ : " + values[0]);
            Debug.Log("��� - ��ȣ : " + values[1]);
            Debug.Log("��� - ���� : " + values[2]);
            Debug.Log("��� - ü�� : " + values[3]);
            Debug.Log("��� - ȭ�� : " + values[4]);
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

        return _damage;
    }

    //  <<22-10-21 ������ :: ȭ�� �߰�>>
    //  <<22-11-12 ������ :: �� ������ ���Ŀ� ���� (������ �����ϰ� ī��� ���� ���������� ���� X)>>
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
    public void ShieldEffect()
    {
		if (!isShieldAnim)
		{
            Sequence sequence1 = DOTween.Sequence()
            .Append(this.shieldEffectObject.transform.DOScale(originShieldEffectScale * 2, 0f))
            .Append(this.shieldEffectObject.transform.DOScale(originShieldEffectScale, 0.5f));
            Sequence sequence2 = DOTween.Sequence()
           .Append(shieldEffectSpriteRenderer.DOFade(0, 0.0f))
           .Append(shieldEffectSpriteRenderer.DOFade(1, 0.3f));
        }

    }

    void Set_ShieldActivate()
	{
        if (0 < i_shield)
        {
            ShieldTMP.gameObject.SetActive(true);
            ShieldEffect();
            isShieldAnim = true;
        }
        else
        {
            ShieldTMP.gameObject.SetActive(false);
            isShieldAnim = false;
        }
    }

    #region StatusValueChange

    #region ���� ģȭ��

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

    #region ��ȣ

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

    //���ݽ�ų �ƴ� ��� ����Ʈ������ �� �����
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