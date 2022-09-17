using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class Entity : MonoBehaviour
{
  

    [SerializeField] EntityPattern entitiyPattern;
    [SerializeField] EnemyBoss enemyBoss;
    [SerializeField] public SpriteRenderer charater;
    [SerializeField] SpriteRenderer DamagedSpriteRenederer;
    [SerializeField] Sprite playerDamagedEffect;
    [SerializeField] ParticleSystem particle;
    [SerializeField] TMP_Text healthTMP;
    [SerializeField] TMP_Text ShieldTMP;
    [SerializeField] Image healthImage;
    [SerializeField] Material dissolveMaterial;
    [SerializeField] GameObject StateOff;

    Item item;
    [HideInInspector] public Enemy enemy;
    [HideInInspector] public float i_health;
    [HideInInspector] public float HEALTHMAX;
    [HideInInspector] public int i_shield = 0;
    [HideInInspector] public int i_attackCount ;
    [HideInInspector] public int i_damage;
    [HideInInspector] public int attackTime = 0;

    public bool is_mine;
    public bool attackable = true;
    public bool is_die = false;

    [HideInInspector] public Vector3 originPos;


    float fade = 1f;
    public bool isDissolving = false;

    private void Start()
    {
        dissolveMaterial = GetComponent<SpriteRenderer>().material;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isDissolving = true;
        }
        if (isDissolving)
        {
            fade -= 2*Time.deltaTime;
            dissolveMaterial.SetFloat("_Fade", fade);
        }
    }

    public void SetupEnemy(EnemyBoss _enemy)
    {
        enemyBoss = _enemy;
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
        enemy = _enemy;
        i_health = _enemy.i_health;
        i_attackCount = _enemy.i_attackCount;
        i_damage = _enemy.i_damage;

        entitiyPattern = _enemy.entityPattern;
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



	#region Damage

	public void Attack(PlayerEntity _player )
	{
        entitiyPattern.ExcuteRole(this);
    }




    public IEnumerator Damaged(Sprite _sprite)
    {
        DamagedSpriteRenederer.sprite = _sprite;
        SetDamagedOpacityTrue();
        this.transform.DOMove(this.originPos + new Vector3(0.15f, 0, 0), 0.1f);
        charater.sprite = enemy.EnemyDamagedSprite;
        yield return new WaitForSeconds(0.15f);
        this.transform.DOMove(this.originPos, 0.2f);
        Sequence sequence1 = DOTween.Sequence()
        .Append(DamagedSpriteRenederer.DOFade(0, 0.2f));
        if (i_health <= 0)
        {
            particle.Play();
            StateOff.SetActive(false);
            isDissolving = true;
            yield return new WaitForSeconds(particle.main.duration + 0.4f);
            EntityManager.Inst.CheckDieEveryEnemy();
        }
        yield return new WaitForSeconds(0.15f);
        charater.sprite = enemy.sp_sprite;
    }


    void SetDamagedOpacityTrue()
    {
        Color tempt = Color.white;
        tempt.a = 1f;
        DamagedSpriteRenederer.color = tempt;
    } 





	#endregion

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
