using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.VFX;
using DG.Tweening;
using System.Text;

public class Entity : MonoBehaviour
{
  

    [SerializeField] EntityPattern entitiyPattern;
    [SerializeField] EnemyBoss enemyBoss;
    [SerializeField] public SpriteRenderer charater;
    [SerializeField] SpriteRenderer DamagedSpriteRenederer;
    [SerializeField] TMP_Text healthTMP;
    [SerializeField] TMP_Text ShieldTMP;
    [SerializeField] GameObject ShieldObject;
    [SerializeField] GameObject ShieldObjectBase;
    [SerializeField] SpriteRenderer ShieldSpriteRenderer;
    [SerializeField] GameObject StateOff;
    [SerializeField] Image healthImage;
    [SerializeField] Material dissolveMaterial;


    [HideInInspector] Sprite playerDamagedEffect;
    [HideInInspector] public Enemy enemy;
    [HideInInspector] public float i_health;
    [HideInInspector] public float HEALTHMAX;
    [HideInInspector] public int i_shield = 0;
    [HideInInspector] public int i_attackCount ;
    [HideInInspector] public int i_damage;
    [HideInInspector] public int attackTime = 0;
    [HideInInspector] public int nextPattorn = 0;

    [HideInInspector] public int i_burning = 0;

    public bool is_mine;
    public bool attackable = true;
    public bool is_die = false;

    [HideInInspector] public Vector3 originPos;
    [HideInInspector] public Vector3 originShieldScale = new Vector3(60,60,0);

    [Header("Grapics")]
    float fade = 1f;
    public bool isDissolving = false;

    public VisualEffect dissolveEffect;

    public int i_entityMotionRunning;

    StringBuilder sb = new StringBuilder();

    private void Start()
    {
        dissolveMaterial = GetComponent<SpriteRenderer>().material;
        entitiyPattern.Pattern(this);
        dissolveEffect.Stop();
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

	#region Entity Base
	public void SetupEnemy(EnemyBoss _enemy)
    {
        enemyBoss = _enemy;
        i_health = _enemy.i_health;
        i_attackCount = _enemy.i_attackCount;
        i_damage = _enemy.i_damage;

        HEALTHMAX = i_health;
        healthImage.fillAmount = i_health / HEALTHMAX;
        
        charater.sprite = _enemy.sp_sprite;
        healthTMP.text = i_health.ToString();
        RefreshEntity();
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
        RefreshEntity();

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

    public bool Damaged(int _damage, Card _card = null) 
    {
        int totalDamage = _damage - i_shield;

        if (i_shield > _damage)
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
            RefreshEntity();
            return true;
        }

        if (i_burning > 0) //  <<22-10-21 ������ :: ȭ�� �߰�>>
        {
            Burning();
        }

        RefreshEntity();
        return false;
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
            RefreshEntity();
            return;
        }

        RefreshEntity();
        return;
    }

    public void Attack(PlayerEntity _player)
    {
        entitiyPattern.Pattern(this);
    }

    public void ShieldEffect()
	{
        Sequence sequence1 = DOTween.Sequence()
       .Append(this.ShieldObject.transform.DOScale(originShieldScale * 2, 0f))
       .Append(this.ShieldObject.transform.DOScale(originShieldScale, 0.5f));
        Sequence sequence2 = DOTween.Sequence()
       .Append(ShieldSpriteRenderer.DOFade(0, 0.0f))
       .Append(ShieldSpriteRenderer.DOFade(1, 0.3f));
    }

    public void RefreshEntity()
    {
        if (0< i_shield)
        {
            ShieldTMP.gameObject.SetActive(true);
            ShieldObject.SetActive(true);
            ShieldObjectBase.SetActive(true);
            ShieldEffect();
        }
        else
        {
            ShieldTMP.gameObject.SetActive(false);
            ShieldObject.SetActive(false);
            ShieldObjectBase.SetActive(false);
        }
        healthImage.fillAmount = i_health / HEALTHMAX;
        healthTMP.text = i_health.ToString();
        ShieldTMP.text = i_shield.ToString();
    }

    public void ShowNextActionPattern(Sprite _NextActionSprite)
	{
        
	}

	#endregion

	#region Damage



    public IEnumerator DamagedEffectCorutin(Sprite _sprite)
    {
        EntityManager.i_entityMotionRunning++;  // <<22-10-30 ������ :: �߰�>>
        i_entityMotionRunning++;

        sb.Clear();
        sb.Append("ī��Ʈ ���� :: ");
        sb.Append(EntityManager.i_entityMotionRunning);
        Debug.Log(sb.ToString());

        DamagedSpriteRenederer.sprite = _sprite; 
        Sequence sequence1 = DOTween.Sequence()
        .Append(DamagedSpriteRenederer.DOFade(1, 0.15f))
        .Append(DamagedSpriteRenederer.DOFade(0, 0.05f));
        //SetDamagedOpacityTrue(); // <<22-11-03 ������ :: �ǰ� ����Ʈ�� 1ȸ���� �Ǿ������ ����>>
        this.transform.DOMove(this.originPos + new Vector3(0.15f, 0, 0), 0.1f);
        charater.sprite = enemy.EnemyDamagedSprite;
        yield return new WaitForSeconds(0.15f);
        this.transform.DOMove(this.originPos, 0.2f);

        if (i_health <= 0)
        {
            StartCoroutine(DestroyEffectCoroutine());

            EndCheckingEntity();
        }
        else
        {
            EndCheckingEntity(); // <<22-10-29 ������ :: ������ ������ �� �ڿ� �۾��� �ʿ� ������ else �η� ����>>
            i_entityMotionRunning--;

            yield return new WaitForSeconds(0.15f);
            yield return new WaitForAllMotionDone(this); // <<22-10-30 ������ :: ��� �� ��� �� �״�� �ڷ�ƾ ����>>

            charater.sprite = enemy.sp_sprite;
        }
    }

    IEnumerator DestroyEffectCoroutine() // <<22-10-26 ������ :: �ذ��ߴ�>>
    {
        dissolveEffect.Play();
        dissolveEffect.playRate = 2.5f;

        StateOff.SetActive(false);
        isDissolving = true;
        //�������� �ʿ���

        yield return new WaitForSeconds(0.4f);
        dissolveEffect.Stop();
        yield return new WaitForSeconds(0.4f);

        //EntityManager.Inst.CheckDieEveryEnemy(); // <<22-10-29 ������ :: ���������� �̰� ��������>>
        EntityManager.Inst.CheckDieEnemy(this);
    }

    public void EndCheckingEntity()
    {
        EntityManager.i_entityMotionRunning--;  // <<22-10-27 ������ :: �߰�>>

        sb.Clear();
        sb.Append("ī��Ʈ ���� :: ");
        sb.Append(EntityManager.i_entityMotionRunning);
        Debug.Log(sb.ToString());
    }

    //IEnumerator PlayDestroyEffect() // <<22-11-03 ������ :: ����>>
    //{
    //    //VFX
    //    dissolveEffect.Play();
    //    dissolveEffect.playRate = 2.5f;

    //    StateOff.SetActive(false);
    //    isDissolving = true;
    //    //�������� �ʿ���

    //    yield return new WaitForSeconds(0.4f);
    //    dissolveEffect.Stop();

    //    yield return null;
    //}

    //void SetDamagedOpacityTrue() // <<22-11-03 ������ :: �ǰ� ����Ʈ�� 1ȸ���� �Ǿ������ ����>>
    //{
    //    Color tempt = Color.white;
    //    tempt.a = 1f;
    //    DamagedSpriteRenederer.color = tempt;
    //} 

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