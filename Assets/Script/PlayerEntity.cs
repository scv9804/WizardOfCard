using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerEntity : MonoBehaviour
{
    [SerializeField] PlayerChar playerChar;
    [SerializeField] SpriteRenderer charater;
    [SerializeField] TMP_Text healthTMP;
    [SerializeField] TMP_Text ShieldTMP;
    [SerializeField] Image healthImage;
    
    


    [HideInInspector] public bool is_mine;
    [HideInInspector] public bool attackable;
    [HideInInspector] public bool is_attackAble;


    [HideInInspector] public bool is_die = false;


    #region status
    public int MAXAETHER = 5; // 최대 코스트
    public int i_aether; // 코스트


    [HideInInspector] public float i_health;
    [HideInInspector] public float HEALTHMAX;
    [HideInInspector] public int i_shield = 0;

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
	#region set_status

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
    public void Add_Status_Health(int _addStatus)
	{
        i_health += _addStatus;
        RefreshPlayer();
    }
    public void Add_status_MaxHealth(int _addStatus)
    {
        HEALTHMAX += _addStatus;
        RefreshPlayer();
    }
    public void Add_Status_Aether(int _addStatus) 
    {
        i_aether += _addStatus;
        RefreshPlayer();
    }
    public void Add_Status_MaxAether(int _addStatus)
	{
        MAXAETHER += _addStatus;
        RefreshPlayer();
    }
    public void Add_Status_Shiled(int _addStatus)
	{
        i_shield += _addStatus;
        RefreshPlayer();
    }



    #endregion





    // === 디버프 셋팅 ===
    #region set_debuff


    #endregion




    // 플레이어 기본 정보 설정
    public void SetupPlayerChar(PlayerChar playerChar)
    {
        i_health = playerChar.i_health;
        HEALTHMAX = i_health;
        ShieldTMP.gameObject.SetActive(false);


        UIManager.Inst.HealthTMP_UI.text = i_health + " / " + HEALTHMAX;
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
        if (0 < i_shield)
        {
            ShieldTMP.gameObject.SetActive(true);
        }
        else
        {
            ShieldTMP.gameObject.SetActive(false);
        }
        healthImage.fillAmount = i_health / HEALTHMAX;
        UIManager.Inst.HealthTMP_UI.text = i_health + " / " + HEALTHMAX;
        healthTMP.text = i_health.ToString();
        ShieldTMP.text = i_shield.ToString();
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
