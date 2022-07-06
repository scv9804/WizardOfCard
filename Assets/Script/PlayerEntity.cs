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
   
    
    [HideInInspector] public float i_health;
    [HideInInspector] public float HEALTHMAX;
    [HideInInspector] public int i_shield = 0;

    [HideInInspector] public bool is_mine;
    [HideInInspector] public bool attackable;
    [HideInInspector] public bool is_attackAble;


    [HideInInspector] public bool is_die = false;

    public int MAXMANA = 5;
    public int i_manaCost;


	public void SetupPlayerChar(PlayerChar playerChar)
    {
        i_health = playerChar.i_health;
        HEALTHMAX = i_health;
        ShieldTMP.gameObject.SetActive(false);


        UIManager.Inst.HealthTMP_UI.text = i_health + " / " + HEALTHMAX;
        charater.sprite = playerChar.sp_sprite;
        healthTMP.text = i_health.ToString();
    }


    public bool Damaged(int _damage)
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
            return true;
        }
        RefreshPlayer();
        return false;
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



}
