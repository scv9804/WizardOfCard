
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Spine.Unity;
using UnityEngine.UI;
using UnityEngine.VFX;
using DG.Tweening;
using Unity.Jobs;
using Unity.Collections;
using XSSLG;

public class PlayerEntity : XSUnitNode
{
    public static PlayerEntity Inst { get; private set; }

    private void Awake()
    {
        Inst = this;
    }

    [HideInInspector] public PlayerChar playerChar;
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

    [SerializeField] public VisualEffect buffEffect;
    [SerializeField] public VisualEffect debuffEffect;

    [Header("������")]
    [SerializeField] SkeletonAnimation skeletonAnimation;

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
    DamageReduceJob myDamageReduceJob = new DamageReduceJob();
    ShieldJob myShieldJob = new ShieldJob();
    BurningJob myBurningJob = new BurningJob();

    #endregion

    private void Start()
	{
        healthImage_UI = GameObject.Find("UI_Left_Health").GetComponent<Image>();
        SetDefultPS();
        debuffEffect.Stop();
        buffEffect.Stop();
    }

    private void FixedUpdate()
    {
        if (isTextMove)
        {
            skillNameTmp.rectTransform.anchoredPosition3D += popupSpeed * Vector3.up;
        }
    }

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

    int maxAether; // �ִ� �ڽ�Ʈ
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

	#region ����

	//[HideInInspector] public int DecreaseDamage_Turn = 1;
	//[HideInInspector] public int DecreaseDamage_Battle = 1;
	//[HideInInspector] public int DecreaseDamage_Stage = 1;
	//[HideInInspector] public int DecreaseDamage_Permanent = 1;

	//[HideInInspector] public int damageUpBuff_Turn = 0;
	//[HideInInspector] public int damageUpBuff_Battle = 0;
	//[HideInInspector] public int damageUpBuff_Stage = 0;
	//[HideInInspector] public int damageUpBuff_Permanent = 0;  // ���߿� �̰ɷ� �ٲ��� �ް�����

	#endregion

	// <<22-11-05 ������ :: �߰�>>
	int i_protection = 0; // ��ġ ������ �������� ��� (��ġ �̻��� ���ش� ��� ����, ���� ����ȣȯ���� ���ļ� ��¦ �ٲ㺽)

    // <<22-11-12 ������ :: �����̻��� �����ִ� �鿪 ȿ�� �߰�>>
    int i_emmune;

    int i_reduce; // ������ ����

    int i_heal; // ȸ���� ����

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
    // <<22-12-3 �̵�ȭ :: ����ģȭ�� ���� �Կ� �Ⱥپ �׳� ����������� �ٲ� ����
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
    
    // <<22-12-03 ������ :: �߰�>>
    public int Buff_Reduce
    {
        get
        {
            return i_reduce;
        }

        set
        {
            i_reduce = value;
        }
    }

    public int Buff_Heal
    {
        get
        {
            return i_heal;
        }

        set
        {
            i_heal = value;
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
    public void AddBuffImage(Sprite _sprite, string _buffDebuffName, int _code, int _value ,int _type , bool _isBuff)
    {
        var temt = Instantiate(buffPrefab);
        temt.GetComponent<BuffDebuffImageSpawn>().Setup(_sprite, _buffDebuffName, _value, _code , _type , _isBuff);
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
		if (i_shield == 0)
        {
            ShieldTMP.gameObject.SetActive(false);
		}
		else
		{
            ShieldTMP.text = i_shield.ToString();
            ShieldTMP.gameObject.SetActive(true);
        }

        Status_MaxAether = CharacterStateStorage.Inst.aether;
        Status_Aether = CharacterStateStorage.Inst.aether;

        originSkillNamePos = skillNameTmp.rectTransform.anchoredPosition3D;
        UIManager.Inst.HealthTMP_UI.text = i_health.ToString();
        charaterSprite.sprite = _playerChar.sp_sprite;
        healthTMP.text = i_health.ToString();
    }

    // <<22-10-21 ������ :: ����>>
    // <<22-11-12 ������ :: ���� ����, �ִ����� ������� ������ ���װ� ���� �� ����>>
    // <<22-11-24 ������ :: ��ȯ Ÿ�� Int�� ����>>
    public int Damaged(int _damage, Card _card = null)
    {
        #region Status_Health -= _damage;

        NativeArray<int> values = new NativeArray<int>(6, Allocator.TempJob);
        values[0] = _damage;
        values[1] = Buff_Protection;
        values[2] = Status_Shiled;
        values[3] = (int)Status_Health;
        values[4] = Debuff_Burning;
        values[5] = Buff_Reduce;

        #region �Է� �� �����

        if (DebugManager.instance.isPrintDamageCalculating)
        {
            Debug.Log("-------------------------------------");
            Debug.Log("�Է� - ������ : " + values[0]);
            Debug.Log("�Է� - ��ȣ : " + values[1]);
            Debug.Log("�Է� - ���� : " + values[2]);
            Debug.Log("�Է� - ü�� : " + values[3]);
            Debug.Log("�Է� - ȭ�� : " + values[4]);
            Debug.Log("�Է� - ������ ���� : " + values[5]);
            Debug.Log("-------------------------------------");
        }

        #endregion

        myProtectionJob.values = values;
        myProtectionJob.isPrint = DebugManager.instance.isPrintDamageCalculating;

        JobHandle firstJob = myProtectionJob.Schedule();

        myDamageReduceJob.values = values;
        myDamageReduceJob.isPrint = DebugManager.instance.isPrintDamageCalculating;

        JobHandle secondJob = myDamageReduceJob.Schedule(firstJob);

        myShieldJob.values = values;
        myShieldJob.isPrint = DebugManager.instance.isPrintDamageCalculating;

        JobHandle thirdJob = myShieldJob.Schedule(secondJob);

        myBurningJob.values = values;
        myBurningJob.isPrint = DebugManager.instance.isPrintDamageCalculating;

        JobHandle forthJob = myBurningJob.Schedule(thirdJob);

        forthJob.Complete();

        _damage = values[0];
        Buff_Protection = values[1];
        Status_Shiled = values[2];
        Status_Health = values[3];
        Debuff_Burning = values[4];
        //Buff_Reduce = values[5]; // �� ���� X

        #region ��� �� �����

        if (DebugManager.instance.isPrintDamageCalculating)
        {
            Debug.Log("��� - ������ : " + values[0]);
            Debug.Log("��� - ��ȣ : " + values[1]);
            Debug.Log("��� - ���� : " + values[2]);
            Debug.Log("��� - ü�� : " + values[3]);
            Debug.Log("��� - ������ ���� : " + values[5]);
            Debug.Log("-------------------------------------");
        }

        #endregion

        values.Dispose();

        #endregion

        //if (_damage > 0)
        //    Utility.onDamaged?.Invoke(_card, _damage);

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

    #region StatusValueChange And VFX

    #region ���� ģȭ�� �����ʿ�

    public void CheckBuffEffect()
    {
        if (buffImageList.Count == 0)
        {
            buffEffect.Stop();
            debuffEffect.Stop();
        }
        else
        {
            foreach (var buff in buffImageList)
            {
                if (buff.GetComponent<BuffDebuffImageSpawn>().isbuff)
                {
                    if (buffEffect.pause)
                        buffEffect.Play();
                }
                else
                {
                    if (debuffEffect.pause)
                        debuffEffect.Play();
                }

            }

        }
    }

    public void ResetMagicAffinity_Battle()
    {
        i_magicAffinity_battle = 0;

        for (int i = buffImageList.Count - 1; i >= 0; i--)
        {
            if ((buffImageList[i].GetComponent<BuffDebuffImageSpawn>().type == 1 || buffImageList[i].GetComponent<BuffDebuffImageSpawn>().type == 0) && !buffImageList[i].GetComponent<BuffDebuffImageSpawn>().isbuff)
            {
                Destroy(buffImageList[i]);
                buffImageList.RemoveAt(i);
            }
        }
  
        debuffEffect.Stop();

        CardManager.Inst.RefreshMyHands();
    }

    public void ResetMagicAffinity_Turn(bool isMyTurn)
    {
        if (!isMyTurn)
        {
            i_magicAffinity_turn = 0;
            for (int i = buffImageList.Count - 1; i >= 0; i--)
            {
                if (buffImageList[i].GetComponent<BuffDebuffImageSpawn>().type == 0)
                {
                    Destroy(buffImageList[i]);
                    buffImageList.RemoveAt(i);
                }
            }

            if (i_magicAffinity_battle <= 0)
            {
                debuffEffect.Stop();
            }
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
    /*	private void OnMouseOver()
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
        }*/

    #endregion

    #region ������ �ִϸ��̼�

    void SetSkeletonAnimation(SkeletonDataAsset dataAsset)
    {
        skeletonAnimation.ClearState();
        skeletonAnimation.skeletonDataAsset = dataAsset;
        skeletonAnimation.timeScale = 0.5f;
        skeletonAnimation.Initialize(true);
    }

    public void StartSkeletonAnimation()
    {
#if UNITY_EDITOR
        skeletonAnimation.Start();
#endif
    }

    #endregion

}