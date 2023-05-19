using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using TMPro;
using UnityEngine.UI;
using UnityEngine.VFX;
using DG.Tweening;
using System.Text;
using System;
using Unity.Jobs;
using Unity.Collections;
using XSSLG;

public class Entity : XSUnitNode
{
    [Header("�ʼ� ���")]
    public Enemy enemy;
    public AttackPatternSO attack;

    [Header("�⺻ ����")]
    public int AttackPattern = 0;
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
    [SerializeField] TMP_Text damagedValueTMP;
    [SerializeField] GameObject spineGameObject;
    [SerializeField] List<GameObject> buffImageList;

    [Header("������")]
    [SerializeField] SkeletonAnimation entitySkeletonAnimation;
    public GameObject entitySkeletonGameObject;
        
    [Header("������")]
    [SerializeField] GameObject buffPrefab;
    [SerializeField] GameObject buffImageSlot;
    [SerializeField] TMP_Text skillNameTmp;

    //�ɷ�ġ ��
    [HideInInspector] Sprite playerDamagedEffect;
    [HideInInspector] public int patternCount = -1;
    [HideInInspector] public float i_health; // <<������ :: i�� �ƴ϶�� �Ѥ�>>
    [HideInInspector] public float HEALTHMAX;
    [HideInInspector] public int increaseShield;
    [HideInInspector] public int i_shield = 0;
    [HideInInspector] public int i_attackCount ;
    [HideInInspector] public int i_damage;
    [HideInInspector] public int attackTime = 0;
    [HideInInspector] public int nextPattorn = 0;
    [HideInInspector] public float spriteSize_X = 0;
    [HideInInspector] public float spriteSize_Y = 0;
    [HideInInspector] public Sprite[] specialSkillSprite;
    [HideInInspector] public int i_burning = 0;


	#region ���� ����
	[HideInInspector] public int debuffValue = 1;
    [HideInInspector] public int buffValue = 1;

    [HideInInspector] public int DecreaseDamage_Turn = 1;
    [HideInInspector] public int DecreaseDamage_Battle = 1;

    [HideInInspector] public int damageUpBuff_Turn = 0;
    [HideInInspector] public int damageUpBuff_Battle = 0;

    bool []isSkillOn;
    #endregion

	public bool is_mine;
    public bool attackable = true;
    public bool is_die = false;
    bool isTextMove = false;
    bool shieldAnim = false;
    int popupSpeed = 25;


	[HideInInspector] public Vector3 originPos;
    [HideInInspector] public Vector3 originSkillNamePos;
    [HideInInspector] public Vector3 originShieldScale = new Vector3(60,60,0);
    
    [Header("�׷���")]
    float fade = 1f;
    public bool isDissolving = false;

    [SerializeField] VisualEffect dissolveEffect;
    [SerializeField] VisualEffect buffEffect;
    [SerializeField] VisualEffect debuffEffect;

    // << 22-11-09 ������ :: ����>>
    //public int i_entityMotionRunning;

    StringBuilder sb = new StringBuilder();

    public static Action buffAction;

    #region Job �ý���

    ShieldJob myShieldJob = new ShieldJob();
    BurningJob myBurningJob = new BurningJob();

    #endregion


    #region ���� ���� ���� ������Ʈ
    private void Start()
    {
       //entitiyPattern.ShowNextPattern(this);
        dissolveMaterial = GetComponentInChildren<SpriteRenderer>().material;  
        AllEffectOff();
    }
    private void OnEnable()
    {
        TurnManager.onStartTurn += BuffOff_Turn;
        TurnManager.onStartTurn += DebuffOff_Turn;

        TurnManager.onStartTurn += ResetValue_Shield;
    }

    private void OnDisable()
    {
        TurnManager.onStartTurn -= BuffOff_Turn;
        TurnManager.onStartTurn -= DebuffOff_Turn;

        TurnManager.onStartTurn -= ResetValue_Shield;
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

    #region ���� ����� �� ����Ʈ ����
    public void AllEffectOff()
    {
        dissolveEffect.Stop();
        debuffEffect.Stop();
        buffEffect.Stop();
    }

/// <summary>
/// �̰� vfx���� �������� ã���ߴµ�.... ��....
/// </summary>
    public void CheckBuffEffect()
	{
		if (buffImageList.Count == 0)
		{
            buffEffect.Stop();
            debuffEffect.Stop();
		}
		else
		{
			foreach (var buff in buffImageList )
			{
				if (buff.GetComponent<BuffDebuffImageSpawn>().isbuff)
				{
                    if(buffEffect.pause)
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


    //�� ����� ���� 
    public void DebuffOff_Turn(bool isMyTurn)
	{
		if (!isMyTurn)
		{
            DecreaseDamage_Turn = 0;
            for (int i = buffImageList.Count-1; i >= 0; i--)
            {
                if (buffImageList[i].GetComponent<BuffDebuffImageSpawn>().type == 0 && !buffImageList[i].GetComponent<BuffDebuffImageSpawn>().isbuff)
                {
                    Destroy(buffImageList[i]);
                    buffImageList.RemoveAt(i);
                }
            }
            if (DecreaseDamage_Battle > 0)
			{
                debuffEffect.Stop();
			}
		}
	}

    //�� ���� ����
    void BuffOff_Turn(bool isMyTurn)
	{
		if (!isMyTurn)
		{
            damageUpBuff_Turn = 0;
            for (int i = buffImageList.Count-1; i >= 0; i--)
            {
                if (buffImageList[i].GetComponent<BuffDebuffImageSpawn>().type == 0 && buffImageList[i].GetComponent<BuffDebuffImageSpawn>().isbuff)
                {
                    Destroy(buffImageList[i]);
                    buffImageList.RemoveAt(i);
                }
            }
            if (damageUpBuff_Battle > 0)
			{
                buffEffect.Stop();
            }
		}
	}

    //������ ���
    public int FinalAttackValue()
	{
        return damageUpBuff_Turn + damageUpBuff_Battle + i_damage ;
	}

    // ����������
	public int IncreaseDamage 
    {
		set
		{
            damageUpBuff_Battle += value;
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

    public void AddBuffImage(Sprite _sprite, string _buffDebuffName , int _code , int _value, int _type, bool _isBuff)
	{
        var temt = Instantiate(buffPrefab);
        temt.GetComponent<BuffDebuffImageSpawn>().Setup(_sprite, _buffDebuffName, _value , _code, _type, _isBuff);
        temt.transform.SetParent(buffImageSlot.transform, false);
        buffImageList.Add(temt);
	}

    public bool CompareBuffImage(int _code ,int _value)
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

    #region Entity Base

    //���̰� �Ǿ��׿� ����
    /*public void SetupEnemyBoss(Enemy _enemy)
    {
        enemy = _enemy;
        i_health = _enemy.i_health;
        i_attackCount = _enemy.i_attackCount;
        i_damage = _enemy.i_damage;
        increaseShield = _enemy.increaseShield;
        debuffValue = _enemy.debuffValue;
        buffValue = _enemy.buffValue;
        SetSkeletonAnimation(_enemy.skeletonDataAsset);

        entitiyPattern = _enemy.entityPattern;
        HEALTHMAX = i_health;
        healthImage.fillAmount = i_health / HEALTHMAX;
        originSkillNamePos = skillNameTmp.rectTransform.anchoredPosition3D;

        RefreshEntity();

        charater.sprite = _enemy.sp_sprite;
        healthTMP.text = i_health.ToString();
        spriteSize_X = charater.sprite.bounds.size.x;
        spriteSize_Y = charater.sprite.bounds.size.y;

        charater.enabled = false;
        entitySkeletonGameObject.transform.localPosition = new Vector3(0, 0, 0);
        //�˹��� ��ġ ��������Ʈ ����� ���� ����. (ü�¹� ��ġ)
        inPlayerCanvas.transform.localPosition = new Vector3(0, charater.sprite.bounds.size.y + 2.0f);
    }*/

    public void SetupEnemy()
    {
        Debug.Log("����Ǿ���");
        i_health = enemy.i_health;
        i_attackCount = enemy.i_attackCount;
        i_damage = enemy.i_damage;
        increaseShield = enemy.increaseShield;
        debuffValue = enemy.debuffValue;
        buffValue = enemy.buffValue;

        entitiyPattern = enemy.entityPattern;
        HEALTHMAX = i_health ;
        healthImage.fillAmount = i_health / HEALTHMAX;
        originSkillNamePos = skillNameTmp.rectTransform.anchoredPosition3D;

        RefreshEntity();

        charater.sprite = enemy.sp_sprite;
        healthTMP.text = i_health.ToString();
        spriteSize_X = charater.sprite.bounds.size.x;
        spriteSize_Y = charater.sprite.bounds.size.y;

        
        charater.enabled = false;
        entitySkeletonGameObject.transform.localPosition = new Vector3(0,0,0);

        specialSkillSprite = enemy.SpelcialSkillSprite;

        SetSkeletonAnimation(enemy.skeletonDataAsset);
    }

    //������ �ʱ�ȭ �ʼ��Դϴ�
    void SetSkeletonAnimation(SkeletonDataAsset dataAsset)
	{
        entitySkeletonAnimation.ClearState();
        entitySkeletonAnimation.skeletonDataAsset = dataAsset;
        entitySkeletonAnimation.timeScale = 0.5f;
        entitySkeletonAnimation.Initialize(true);    
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

    #region // <<22-11-09 ������ :: ���� ����, ���� ������ ���� ����, �� ����ϰ� ������ �� ���� ������ �ͱ� �ѵ� �ϴ��� �н�>>
    // - ������ ����Ʈ ȣ��θ� Damaged �Լ� ������ �ű�
    // - Burning �Լ� �� ��� ������ ����
    // - Entity ���������� �ǰ� ��� Ȱ��ȭ ���θ� ī��Ʈ�ϴ� ���� ����
    // - �Լ��� �����ϴ� EndCheckingEntity �Լ��� ��� ȿ�� ���� �������� ����, �ش� ����� Damaged �� ����
    // - ���� ������ DestroyTest ����, �ش� ����� EntityManager���� �ٷ� �ߵ���
    // - �����Ǿ� �־��� Entity ������ ���� ���� ���� (����������...)
    // - DamageEffectCoroutine�� ���� �����Ǵ� DestroyEffectCoroutine�� �и�
    // - RefreshEntity�� �������� �� �� ȣ��
    #endregion
    // <<22-11-12 ������ :: ���� ����, �ִ����� ������� ������ ���װ� ���� �� ����>>
    public int Damaged(int _damage, Sprite _enemyDamageSprite, Card _card = null)
    {
        #region StartEntityDamageCheck

        EntityManager.i_checkingEntitiesCount++;

        sb.Clear();
        sb.Append("ī��Ʈ ���� :: ");
        sb.Append(EntityManager.i_checkingEntitiesCount);
        Debug.Log(sb.ToString());

        #endregion

        #region i_health -= _damage;

        NativeArray<int> values = new NativeArray<int>(6, Allocator.TempJob);
        values[0] = _damage;
        //values[1] = i_protection; // Entity�� �ش� ���� ����
        values[2] = i_shield;
        values[3] = (int) i_health;
        values[4] = i_burning;
        //values[5] = Buff_Reduce; // Entity�� �ش� ���� ����

        #region �Է� �� �����

        if (DebugManager.instance.isPrintDamageCalculating)
        {
            Debug.Log("-------------------------------------");
            Debug.Log("�Է� - ������ : " + values[0]);
            //Debug.Log("�Է� - ��ȣ : " + values[1]); // Entity�� �ش� ���� ����
            Debug.Log("�Է� - ���� : " + values[2]);
            Debug.Log("�Է� - ü�� : " + values[3]);
            Debug.Log("�Է� - ȭ�� : " + values[4]);
            // Debug.Log("�Է� - ������ ���� : " + values[5]); // Entity�� �ش� ���� ����
            Debug.Log("-------------------------------------");
        }

        #endregion

        myShieldJob.values = values;
        myShieldJob.isPrint = DebugManager.instance.isPrintDamageCalculating;

        JobHandle firstJob = myShieldJob.Schedule();

        myBurningJob.values = values;
        myBurningJob.isPrint = DebugManager.instance.isPrintDamageCalculating;

        JobHandle secondJob = myBurningJob.Schedule(firstJob);

        StartCoroutine(DamageEffectCoroutine(_enemyDamageSprite));

        secondJob.Complete();

        _damage = values[0];
        //Status_Protection = values[1]; // Entity�� �ش� ���� ����
        i_shield = values[2];
        i_health = values[3];
        i_burning = values[4];
        //Buff_Reduce = values[5]; // Entity�� �ش� ���� ����

        #region ��� �� �����

        if (DebugManager.instance.isPrintDamageCalculating)
        {
            Debug.Log("��� - ������ : " + values[0]);
            //Debug.Log("��� - ��ȣ : " + values[1]); // Entity�� �ش� ���� ����
            Debug.Log("��� - ���� : " + values[2]);
            Debug.Log("��� - ü�� : " + values[3]);
            Debug.Log("��� - ȭ�� : " + values[4]);
            Debug.Log("-------------------------------------");
        }

        #endregion

        values.Dispose();

        if (_damage > 0)
            Utility.onDamaged?.Invoke(_card, _damage);

        #endregion

        #region EndEntityDamageCheck

        EntityManager.i_checkingEntitiesCount--;

        sb.Clear();
        sb.Append("ī��Ʈ ���� :: ");
        sb.Append(EntityManager.i_checkingEntitiesCount);
        Debug.Log(sb.ToString());

        #endregion

        if (i_health <= 0)
        {
            i_health = 0;
            is_die = true;

            StartCoroutine(DestroyEffectCoroutine());
        }

        RefreshEntity();

        return _damage;
    }

    // <<22-10-21 ������ :: ȭ�� �߰�>>
    // <<22-11-09 ������ :: ���� �� ���⼭ ���� ��� ������ �˻����� �ʾƵ� �Ǿ� ����>>
    // <<22-11-12 ������ :: ȭ�� ��� ���� �ø�>>
    //public void Burning()

    public void Attack()
    {
        entitiyPattern.Pattern(this);
        entitiyPattern.ShowNextPattern(this);
    }

    public void ShieldEffect()
	{
		if (!shieldAnim)
		{
            Sequence sequence1 = DOTween.Sequence()
            .Append(this.ShieldObject.transform.DOScale(originShieldScale * 2, 0f))
            .Append(this.ShieldObject.transform.DOScale(originShieldScale, 0.5f));
            Sequence sequence2 = DOTween.Sequence()
           .Append(ShieldSpriteRenderer.DOFade(0, 0.0f))
           .Append(ShieldSpriteRenderer.DOFade(1, 0.3f));
        }
 
    }

    public void RefreshEntity()
    {
        if (0< i_shield)
        {
            ShieldTMP.gameObject.SetActive(true);
            ShieldObject.SetActive(true);
            ShieldObjectBase.SetActive(true);
            ShieldEffect();
            shieldAnim = true;
        }
        else
        {
            ShieldTMP.gameObject.SetActive(false);
            ShieldObject.SetActive(false);
            ShieldObjectBase.SetActive(false);
            shieldAnim = false;
        }
        healthImage.fillAmount = i_health / HEALTHMAX;
        healthTMP.text = i_health.ToString();
        ShieldTMP.text = i_shield.ToString();
    }


    #endregion

    #region Damage

    IEnumerator DamageEffectCoroutine(Sprite _sprite)
    {
        DamagedSpriteRenederer.sprite = _sprite;

        this.originPos = transform.position;

        Sequence sequence1 = DOTween.Sequence()
        .Append(DamagedSpriteRenederer.DOFade(1, 0.15f))
        .Append(DamagedSpriteRenederer.DOFade(0, 0.05f));

        this.transform.DOMove(this.originPos + new Vector3(0.15f, 0, 0), 0.1f);

      //entitySkeletonGameObject.SetActive(false);
      // charater.enabled = true;
        SetSkeletonAnimation(enemy.damagedSkeletonDataAssets);
      //charater.sprite = enemy.EnemyDamagedSprite;

        yield return new WaitForSeconds(0.15f);

        this.transform.DOMove(this.originPos, 0.2f);

        yield return new WaitForSeconds(0.2f);



        if (i_health > 0)
        {
            yield return new WaitForSeconds(0.15f);

            /*            charater.sprite = enemy.sp_sprite;
                        entitySkeletonGameObject.SetActive(true);
                        charater.enabled = false;*/
            SetSkeletonAnimation(enemy.skeletonDataAsset);
        }

    }
    void ResetValue_Shield(bool isMyTurn)
    {
        if (!isMyTurn)
        {
            i_shield = 0;

            RefreshEntity();
        }
    }

    // <<22-10-26 ������ :: �ذ��ߴ�>>
    IEnumerator DestroyEffectCoroutine()
    {
        yield return new WaitForSeconds(0.4f);

        MusicManager.inst?.EnemyDisappear();

        dissolveEffect.Play();
        dissolveEffect.playRate = 2.5f;

        inPlayerCanvas.SetActive(false);
        isDissolving = true;
        //�������� �ʿ���

        yield return new WaitForSeconds(0.4f);

        dissolveEffect.Stop();

        yield return new WaitForSeconds(0.4f);

        EntityManager.Inst.CheckDieEnemy(this);
    }

	#endregion


	#region ���콺Ŭ�� ����������
	/*private void OnMouseOver()
    {
        EntityManager.Inst.EntityMouseOver(this);
    }

    private void OnMouseExit()
    {
        EntityManager.Inst.EntityMouseExit();
    }*/

/*
    private void OnMouseUp()
    {
        EntityManager.Inst.EntityMouseUp();
    }

    private void OnMouseDown()
    {
        EntityManager.Inst.EntityMouseDown();
    }*/
	#endregion
}