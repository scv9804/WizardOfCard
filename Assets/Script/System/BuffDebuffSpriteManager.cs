using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffDebuffSpriteManager : MonoBehaviour
{
	public static BuffDebuffSpriteManager Inst { get; private set; }

	private void Awake()
	{
		Inst = this;
	}
	Vector3 spawnPos = new Vector3(-1 , 0, 0);

	[Header("EntityAttackPatternExplainImages")]
	[SerializeField] [Tooltip("전투의함성")] Sprite warCrySprite;
	[SerializeField] [Tooltip("집중력저하")] Sprite decreasedConcentration;
	[SerializeField] [Tooltip("부식독")] Sprite rustAccid;
	[SerializeField] Sprite shieldSprite;

	[Header("SkillEffectImage")]
	[SerializeField] Sprite warCrySkillSprite;


	[Header("SkillEffectImage")]
	[SerializeField]GameObject defultPrefab;


	#region 버프이미지 프로퍼티화

	public Sprite WarCrySprite	{get{return warCrySprite;}}
	public Sprite DecreasedConcentration { get { return decreasedConcentration; } }
	public Sprite RustAccid { get { return rustAccid; } }


	#endregion
	public Sprite ShieldSprite
	{
		get
		{
			return shieldSprite;
		}
	}

	public IEnumerator SpawnSkill(Entity _entity)
	{
		var temt = Instantiate(defultPrefab);
		temt.transform.localScale = new Vector3(1, 1, 0);
		temt.transform.SetParent(_entity.transform, true);
		Debug.Log(-(_entity.spriteSize_X / 2));
		temt.transform.localPosition = new Vector3(-(_entity.spriteSize_X/2),_entity.spriteSize_Y/2 , 0);
		yield return new WaitForSeconds(1.0f);
		Destroy(temt);
		temt = null;
	}

}
