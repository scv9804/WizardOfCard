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
    [SerializeField] TMP_Text skillNameTmp;
    [SerializeField] TMP_Text damagedValueTMP;


    [HideInInspector] Sprite playerDamagedEffect;
    [HideInInspector] public Enemy enemy;
    [HideInInspector] public float i_health;
    [HideInInspector] public float HEALTHMAX;
    [HideInInspector] public int increaseShield;
    [HideInInspector] public int i_shield = 0;
    [HideInInspector] public int i_attackCount ;
    [HideInInspector] public int i_damage;
    [HideInInspector] public int attackTime = 0;
    [HideInInspector] public int nextPattorn = 0;
    [HideInInspector] public int debuffValue = 1;
    [HideInInspector] public int buffValue = 1;
    [HideInInspector] public int i_burning = 0;


    public bool is_mine;
    public bool attackable = true;
    public bool is_die = false;
    bool isTextMove = false;
    int popupSpeed = 25;

    [HideInInspector] public Vector3 originPos;
    [HideInInspector] public Vector3 originSkillNamePos;
    [HideInInspector] public Vector3 originShieldScale = new Vector3(60,60,0);

    [Header("Grapics")]
    float fade = 1f;
    public bool isDissolving = false;

    public VisualEffect dissolveEffect;

    public int i_entityMotionRunning;

    StringBuilder sb = new StringBuilder();

    private void Start()
    {
        entitiyPattern.ShowNextPattern(this);
        dissolveMaterial = GetComponent<SpriteRenderer>().material;
        dissolveEffect.Stop();

        Debug.Log(skillNameTmp.rectTransform.position + "포지션");
        Debug.Log(skillNameTmp.rectTransform.anchoredPosition3D + "3D");
        Debug.Log(skillNameTmp.rectTransform.anchoredPosition + "2D");
        Debug.Log(skillNameTmp.transform.position + "트포");
        Debug.Log(skillNameTmp.transform.localPosition + "트로포");
    }

    private void Update()
    {
		//if (Input.GetKeyDown(KeyCode.Space))
		//{
		//	isDissolving = true;
		//}
		if (isDissolving)
		{
			fade -= 2 * Time.deltaTime;
			dissolveMaterial.SetFloat("_Fade", fade);
		}
    }

	private void FixedUpdate()
	{
        if (isTextMove)
        {
            skillNameTmp.rectTransform.anchoredPosition3D += popupSpeed * Vector3.up;
            Debug.Log("tlqkf dhodkseoa");
        }
    }

	public int IncreaseDamage 
    {
		set
		{
            i_damage = value;
		}      
    }

    public IEnumerator SkillNamePopup(string _skillName)
	{
        skillNameTmp.text = _skillName;
        skillNameTmp.gameObject.SetActive(true);
        isTextMove = true;

         Sequence sequence = DOTween.Sequence()
        .Append(skillNameTmp.DOFade(1, 0.0f))
        .Append(skillNameTmp.DOFade(0, 1f));
        yield return new WaitForSeconds(1f);

        isTextMove = false;
        skillNameTmp.gameObject.SetActive(false);
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
        increaseShield = _enemy.increaseShield;
        debuffValue = _enemy.debuffValue;
        buffValue = _enemy.buffValue;

        entitiyPattern = _enemy.entityPattern;
        HEALTHMAX = i_health ;
        healthImage.fillAmount = i_health / HEALTHMAX;
        originSkillNamePos = skillNameTmp.rectTransform.anchoredPosition3D;

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

        if (i_burning > 0 && _card != null) //  <<22-10-21 장형용 :: 화상 추가>>
        {
            Burning();
        }

        RefreshEntity();
        return false;
    }

    public void Burning() //  <<22-10-21 장형용 :: 화상 추가>>
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

    public void Attack()
    {
        entitiyPattern.Pattern(this);
        entitiyPattern.ShowNextPattern(this);
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
        EntityManager.i_entityMotionRunning++;  // <<22-10-30 장형용 :: 추가>>
        i_entityMotionRunning++;

        sb.Clear();
        sb.Append("카운트 증가 :: ");
        sb.Append(EntityManager.i_entityMotionRunning);
        Debug.Log(sb.ToString());

        DamagedSpriteRenederer.sprite = _sprite; 
        Sequence sequence1 = DOTween.Sequence()
        .Append(DamagedSpriteRenederer.DOFade(1, 0.15f))
        .Append(DamagedSpriteRenederer.DOFade(0, 0.05f));
        //SetDamagedOpacityTrue(); // <<22-11-03 장형용 :: 피격 이펙트가 1회성이 되어버려서 제거>>
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
            EndCheckingEntity(); // <<22-10-29 장형용 :: 어차피 뒤지면 이 뒤에 작업은 필요 없으니 else 부로 넣음>>
            i_entityMotionRunning--;

            yield return new WaitForSeconds(0.15f);
            yield return new WaitForAllMotionDone(this); // <<22-10-30 장형용 :: 대기 중 사망 시 그대로 코루틴 정지>>

            charater.sprite = enemy.sp_sprite;
        }
    }

    IEnumerator DestroyEffectCoroutine() // <<22-10-26 장형용 :: 해결했다>>
    {
        dissolveEffect.Play();
        dissolveEffect.playRate = 2.5f;

        StateOff.SetActive(false);
        isDissolving = true;
        //수동조정 필요함

        yield return new WaitForSeconds(0.4f);
        dissolveEffect.Stop();
        yield return new WaitForSeconds(0.4f);

        //EntityManager.Inst.CheckDieEveryEnemy(); // <<22-10-29 장형용 :: 최종적으로 이게 문제였음>>
        EntityManager.Inst.CheckDieEnemy(this);
    }

    public void EndCheckingEntity()
    {
        EntityManager.i_entityMotionRunning--;  // <<22-10-27 장형용 :: 추가>>

        sb.Clear();
        sb.Append("카운트 감소 :: ");
        sb.Append(EntityManager.i_entityMotionRunning);
        Debug.Log(sb.ToString());
    }

    //IEnumerator PlayDestroyEffect() // <<22-11-03 장형용 :: 제거>>
    //{
    //    //VFX
    //    dissolveEffect.Play();
    //    dissolveEffect.playRate = 2.5f;

    //    StateOff.SetActive(false);
    //    isDissolving = true;
    //    //수동조정 필요함

    //    yield return new WaitForSeconds(0.4f);
    //    dissolveEffect.Stop();

    //    yield return null;
    //}

    //void SetDamagedOpacityTrue() // <<22-11-03 장형용 :: 피격 이펙트가 1회성이 되어버려서 제거>>
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