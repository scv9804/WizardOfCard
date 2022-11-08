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
    [SerializeField] GameObject inPlayerCanvas;
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

    [HideInInspector] public int i_burning = 0;


	#region 버프 스텟
	[HideInInspector] public int debuffValue = 1;
    [HideInInspector] public int buffValue = 1;

    [HideInInspector] public int DecreaseDamage_Turn = 1;
    [HideInInspector] public int DecreaseDamage_Battle = 1;

    [HideInInspector] public int damageUpBuff_Turn = 0;
    [HideInInspector] public int damageUpBuff_Battle = 0;
	#endregion

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

    [SerializeField] VisualEffect dissolveEffect;
    [SerializeField] VisualEffect buffEffect;
    [SerializeField] VisualEffect debuffEffect;

    public int i_entityMotionRunning;

    StringBuilder sb = new StringBuilder();



	#region 시작 생성 종료 업데이트
	private void Start()
    {
        entitiyPattern.ShowNextPattern(this);
        dissolveMaterial = GetComponent<SpriteRenderer>().material;
        Debug.Log(charater.sprite.bounds.size.y);
        //켄버스 위치 스프라이트 사이즈에 따라 조절.
        inPlayerCanvas.transform.localPosition = new Vector3(0, charater.sprite.bounds.size.y / 2 + 2f) ;
        AllEffectOff();
    }
    private void OnEnable()
    {
        TurnManager.onStartTurn += BuffOff_Turn;
        TurnManager.onStartTurn += DebuffOff_Turn;
    }

    private void OnDisable()
    {
        TurnManager.onStartTurn -= BuffOff_Turn;
        TurnManager.onStartTurn -= DebuffOff_Turn;
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
        }
    }
    #endregion


    #region 버프 디버프 및 이펙트 관련
    public void AllEffectOff()
    {
        dissolveEffect.Stop();
        debuffEffect.Stop();
        buffEffect.Stop();
    }

    //턴 디버프 종료 
    public void DebuffOff_Turn(bool isMyTurn)
	{
		if (!isMyTurn)
		{
            DecreaseDamage_Turn = 0;
			if (DecreaseDamage_Battle > 0)
			{
                debuffEffect.Stop();
			}
		}
	}

    //턴 버프 종료
    void BuffOff_Turn(bool isMyTurn)
	{
		if (!isMyTurn)
		{
            damageUpBuff_Turn = 0;
			if (damageUpBuff_Battle > 0)
			{
                buffEffect.Stop();
            }
		}
	}

    //데미지 계산
    public int FinalAttackValue()
	{
        return damageUpBuff_Turn + damageUpBuff_Battle + i_damage ;
	}

    // 데미지증가
	public int IncreaseDamage 
    {
		set
		{
            damageUpBuff_Battle = value;
            buffEffect.Play();
		}      
    }

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

	#endregion

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

        inPlayerCanvas.SetActive(false);
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