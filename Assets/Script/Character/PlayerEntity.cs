
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

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

    // <<22-10-21 ������ :: �߰�>>
    void OnEnable()
    {
        Utility.onBattleStart += ResetMagicAffinity_Battle;
        Utility.onBattleStart += ResetProtection;

        TurnManager.onStartTurn += ResetValue_Shield;
        TurnManager.onStartTurn += ResetMagicAffinity_Turn;
        TurnManager.onStartTurn += ReduceProtection;
    }

    // <<22-10-21 ������ :: �߰�>>
    void OnDisable()
    {
        Utility.onBattleStart -= ResetMagicAffinity_Battle;
        Utility.onBattleStart -= ResetProtection;

        TurnManager.onStartTurn -= ResetValue_Shield;
        TurnManager.onStartTurn -= ResetMagicAffinity_Turn;
        TurnManager.onStartTurn -= ReduceProtection;
    }


    #region status
    int maxAether = 5; // �ִ� �ڽ�Ʈ
    int i_aether; // �ڽ�Ʈ


    float i_health;
    float maxHealth;
     int i_shield = 0;

    [HideInInspector] public int i_burning = 0;

    //���� ģȭ��  Ÿ�Ժ� ģȭ������ ���, �� ȿ�� ����... defult = 0
    [HideInInspector] int i_magicAffinity_fire = 0;
    [HideInInspector] int i_magicAffinity_earth = 0;
    [HideInInspector] int i_magicAffinity_water = 0;
    [HideInInspector] int i_magicAffinity_air = 0;

    [HideInInspector] int i_magicResistance = 0;

    // <<22-10-21 ������ :: �߰�>>
    [HideInInspector] int i_magicAffinity_permanent = 0; // ���(�� �̺�Ʈ) ������ ��� ��� �����Ǵ� ���� ģȭ��
    [HideInInspector] int i_magicAffinity_battle = 0; // 1���� ���� ���� �����Ǵ� ���� ģȭ��
    [HideInInspector] int i_magicAffinity_turn = 0; // 1�ϰ� �����Ǵ� ���� ģȭ��

    // <<22-11-05 ������ :: �߰�>>
    [HideInInspector] int i_protection = 0;

    #endregion


    #region debuff
    [HideInInspector] int i_decrease_magicAffinity;
    [HideInInspector] int i_status_;


    #endregion


    // === �������ͽ� ����/���� ====
    // �ִ°��� +- �� �����ϱ�
    #region property_status

    //����
    public void Add_Status_MagicAffinity_Fire(int _addStatus)
	{
        i_magicAffinity_fire += _addStatus;
        RefreshPlayer();
    }
    public void Add_Status_MagicAffinity_Earth(int _addStatus)
    {
        i_magicAffinity_earth += _addStatus;
        RefreshPlayer();
    }
    public void Add_Status_MagicAffinity_Water(int _addStatus)
    {
        i_magicAffinity_water += _addStatus;
        RefreshPlayer();
    }
    public void Add_Status_MagicAffinity_Air(int _addStatus)
    {
        i_magicAffinity_air += _addStatus;
        RefreshPlayer();
    }


    public float Status_Health
	{
		get
		{
            return i_health;
		}
		set
		{
            if(value > maxHealth) // <<22-10-26 ������ :: ������ �ʰ�ȸ�� ó���� ���� �ʾƵ� �ǵ��� ����>>
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
            i_shield = value;
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

    // <<22-10-21 ������ :: �߰�>>
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

    // <<22-11-05 ������ :: �߰�>>

    public int Status_Protection
    {
        get
        {
            return i_protection;
        }

        set
        {
            if(value > 0)
            {
                i_protection = value;
            }
            else
            {
                i_protection = 0;
            }
        }
    }

    #endregion



    // === ����� ���� ===
    #region set_debuff


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


        UIManager.Inst.HealthTMP_UI.text = i_health.ToString();
        charaterSprite.sprite = _playerChar.sp_sprite;
        healthTMP.text = i_health.ToString();
    }


    public void Damaged(int _damage, Card _card = null)
    {
        if (i_protection >= _damage) // ��ȣ => ��ȣ ��ġ ������ ������ ��ȿ
        {
            _damage = 0;
            return;
        }

        i_protection--;

        int totalDamage = _damage - i_shield;

        if(totalDamage < 0)
        {
            totalDamage = 0;
        }

        if (i_shield > _damage) // ���� => ���� ��ġ��ŭ ������ ����
        {
            i_shield -= _damage;
        }
        else
        {
            i_health -= totalDamage;
            i_shield = 0;
        }

        if (totalDamage > 0)
        {
            Utility.onDamaged?.Invoke(_card, totalDamage);
        }

        if (i_health <= 0)
        {
            i_health = 0;
            is_die = true;
            RefreshPlayer();
            
            StartCoroutine(GameManager.Inst.GameOverScene());
            return ;
        }

        if(i_burning > 0) //  <<22-10-21 ������ :: ȭ�� �߰�>>
        {
            Burning();
        }

        RefreshPlayer();
        return ;
    }

    public void Burning() //  <<22-10-21 ������ :: ȭ�� �߰�>>
    {
        if (i_shield > i_burning)
        {
            i_shield -= i_burning;
        }
        else
        {
            i_health -= (i_burning - i_shield);
            i_shield = 0;
        }

        i_burning--;

        if (i_health <= 0)
        {
            i_health = 0;
            is_die = true;
            RefreshPlayer();

            StartCoroutine(GameManager.Inst.GameOverScene());
            return;
        }

        RefreshPlayer();
        return;
    }

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
        i_protection = 0;

        RefreshPlayer();
    }

    void ReduceProtection(bool isMyTurn)
    {
        if (isMyTurn)
        {
            i_protection = 0;

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
