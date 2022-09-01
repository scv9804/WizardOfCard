using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class Entity : MonoBehaviour
{
    [SerializeField] Item item;
    [SerializeField] Enemy enemy;
    [SerializeField] EnemyBoss enemyBoss;
    [SerializeField] SpriteRenderer charater;
    [SerializeField] SpriteRenderer DamagedSpriteRenederer;
    [SerializeField] TMP_Text healthTMP;
    [SerializeField] TMP_Text ShieldTMP;
    [SerializeField] Image healthImage;

    [HideInInspector] public float i_health;
    [HideInInspector] public float HEALTHMAX;
    [HideInInspector] public int i_shield = 0;
    [HideInInspector] public int i_attackCount ;
    [HideInInspector] public int i_damage;

    public bool is_mine;
    public bool attackable = true;


    [HideInInspector] public Vector3 originPos;
    

    public bool is_die = false;


    public void SetupEnemy(EnemyBoss _enemy)
    {
        i_health = _enemy.i_health;
        i_attackCount = _enemy.i_attackCount;
        i_damage = _enemy.i_damage;

        HEALTHMAX = i_health;
        healthImage.fillAmount = i_health / HEALTHMAX;
        ShieldTMP.gameObject.SetActive(false);

        charater.sprite = _enemy.sp_sprite;
        healthTMP.text = i_health.ToString();
    }

    public void SetupEnemy(Enemy _enemy)
    {
        i_health = _enemy.i_health;
        i_attackCount = _enemy.i_attackCount;
        i_damage = _enemy.i_damage;

        HEALTHMAX = i_health;
        healthImage.fillAmount = i_health / HEALTHMAX;
        ShieldTMP.gameObject.SetActive(false);

        charater.sprite = _enemy.sp_sprite;
        healthTMP.text = i_health.ToString();
    }

    public void MoveTransForm(Vector3 _pos, bool _isUseDotween, float _DotweenTime = 0)
    {
        if (_isUseDotween)
        {
            transform.DOMove(_pos, _DotweenTime);
        }
        else
        {
            transform.position = _pos;
        }
    }

    public bool Damaged(int _damage) 
    {
        if (0 <i_shield )
        {
            i_shield -= _damage;
            if (0 >= i_shield )
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
            RefreshEntity();
            return true;
        }
        RefreshEntity();
        return false;
    }

    public void RefreshEntity()
    {
        if (0< i_shield)
        {
            ShieldTMP.gameObject.SetActive(true);
        }
        else
        {
            ShieldTMP.gameObject.SetActive(false);
        }
        healthImage.fillAmount = i_health / HEALTHMAX;
        healthTMP.text = i_health.ToString();
        ShieldTMP.text = i_shield.ToString();
    }



    public IEnumerator Attack(PlayerEntity _player , Entity _enemy)
	{
		for (int i =0; i < _enemy.i_attackCount; i++)
        {
            _player.Damaged(_enemy.i_damage);
            AttackDOTween(_enemy);
            PlayerEntity.Inst.ChangeSprite(_player.playerChar.damagedSprite);
            yield return new WaitForSeconds(0.1f);
        }

	}

    public IEnumerator ChangeSprite(Sprite _sprite)
    {
        DamagedSpriteRenederer.sprite = _sprite;
        
    
        yield return new WaitForSeconds(0.15f);
 
        yield return new WaitForSeconds(0.05f);
    }

    public void Damaged()
	{

	} 


    void AttackDOTween(Entity _enemy)
	{
        _enemy.transform.DOLocalMoveX(_enemy.transform.localPosition.x - 0.5f , 0.2f ).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
	
    }





    //좋지않은 코드. 오브젝트 풀링 나중에 할거임 임시코드
    public void DestroyTest()
	{
        Destroy(this.gameObject);
	}

    private void OnMouseOver()
    {
        EntityManager.Inst.EntityMouseOver(this);
    }

    private void OnMouseExit()
    {
        EntityManager.Inst.EntityMouseExit();
    }

    private void OnMouseUp()
    {
        EntityManager.Inst.EntityMouseUp();
    }

    private void OnMouseDown()
    {
        EntityManager.Inst.EntityMouseDown();
    }

}
