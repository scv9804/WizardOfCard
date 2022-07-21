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


	[SerializeField] PlayerChar playerChar;
    [SerializeField] SpriteRenderer charater;
    [SerializeField] TMP_Text healthTMP;
    [SerializeField] TMP_Text ShieldTMP;
    [SerializeField] Image healthImage;



    public bool attackable;
    bool is_attackAble;


    bool is_die = false;


    bool is_canUseSelf;
    int i_enhacneVal = 1;
    int i_calcDamage;
    int i_everlasting = 0;  //고정 마법 증폭 (수정여지있음)




    #region status
    int maxAether = 5; // 최대 코스트
    int i_aether; // 코스트




    float i_health;
    float maxHealth;
     int i_shield = 0;

    //마법 친화성  타입별 친화성으로 방어, 힐 효과 증폭... defult = 0
    [HideInInspector] int i_magicAffinity_fire = 0;
    [HideInInspector] int i_magicAffinity_earth = 0;
    [HideInInspector] int i_magicAffinity_water = 0;
    [HideInInspector] int i_magicAffinity_air = 0;

    [HideInInspector] int i_magicResistance = 0; //대략 4에 1감소

    #endregion





    #region debuff
    [HideInInspector] int i_decrease_magicAffinity;
    [HideInInspector] int i_status_;


    #endregion


    // === 스테이터스 증가/감소 ====
    // 넣는값을 +- 로 조절하기
    #region property_status

    //보류
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
            i_health = value;
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


    #endregion





    // === 디버프 셋팅 ===
    #region set_debuff


    #endregion




    // 플레이어 기본 정보 설정
    public void SetupPlayerChar(PlayerChar playerChar)
    {
        i_health = playerChar.i_health;
        maxHealth = i_health;
        ShieldTMP.gameObject.SetActive(false);


        UIManager.Inst.HealthTMP_UI.text = i_health + " / " + maxHealth;
        charater.sprite = playerChar.sp_sprite;
        healthTMP.text = i_health.ToString();
    }


    public void Damaged(int _damage)
    {
        if (0 < i_shield)
        {
            i_shield -= _damage;
            if (0 >= i_shield)
            {
                i_health -= _damage;
                i_shield = 0;

            }

        }
        else
        {
            i_health -= _damage;

        }
        if (i_health <= 0)
        {
            i_health = 0;
            is_die = true;
            RefreshPlayer();
            
            StartCoroutine(GameManager.Inst.GameOverScene());
            return ;
        }
        RefreshPlayer();
        return ;
    }

    public void RefreshPlayer()
    {
        Set_ShieldActivate();
        healthImage.fillAmount = i_health / maxHealth;
        UIManager.Inst.HealthTMP_UI.text = i_health + " / " + maxHealth;
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
