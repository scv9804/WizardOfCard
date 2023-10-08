using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 딕셔너리 인스펙터화 구현부
/*[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue> , ISerializationCallbackReceiver
{
	[SerializeField] List<TKey> keys = new List<TKey>();
	[SerializeField] List<TValue> values = new List<TValue>();

	//여기서 저장
	public void OnBeforeSerialize()
	{
		keys.Clear();
		values.Clear();
		foreach (KeyValuePair<TKey,TValue> pair in this)
		{
			keys.Add(pair.Key); values.Add(pair.Value);
		}
	}

	//여기서 로드
	public void OnAfterDeserialize()
	{
		this.Clear();
		if (keys.Count != values.Count) { throw new System.Exception("{0}의 키와 대응하는 {1} 값이 둘 중 하나가 없습니다. 한쌍이 되도록 설정해 주세요 "); }
		for (int i = 0; i < keys.Count; i++) { this.Add(keys[i], values[i]); } 
	}
}*/
#endregion

[System.Serializable]
struct SkillEffect
{
	public string skillName;
	public Sprite skillSprite;
}

public class BuffDebuffManager : MonoBehaviour
{
	public static BuffDebuffManager Inst { get; private set; }

	private void Awake()
	{
		Inst = this;
	}
	private void Start()
	{
	}

	Vector3 spawnPos = new Vector3(-1 , 0, 0);
	
	[Header("EntityAttackPatternExplainImages")]
	[SerializeField] [Tooltip("전투의함성")] Sprite warCrySprite;
	[SerializeField] [Tooltip("집중력저하")] Sprite decreasedConcentration;
	[SerializeField] [Tooltip("부식독")] Sprite rustAccid;
	[SerializeField] [Tooltip("부식독")] Sprite burning;
	[SerializeField] Sprite shieldSprite;

	[Header("SkillEffectImage")]
	[SerializeField] SkillEffect [] skillEffects;


	[Header("SkillEffectImage")]
	[SerializeField]GameObject defultPrefab;

	//이미지 딕셔너리로 서치하기!


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

	//public IEnumerator SpawnSkillEffect(Entity _entity, string _name)
	//{
	//	foreach (var item in skillEffects)
	//	{
	//		if (item.skillName != _name)
	//			continue;
	//		var temt = Instantiate(defultPrefab);
	//		temt.transform.localScale = new Vector3(1, 1, 0);
	//		temt.transform.SetParent(_entity.transform, true);
	//		Debug.Log(-(_entity.spriteSize_X / 2));
	//		temt.transform.localPosition = new Vector3(-(_entity.spriteSize_X / 2), _entity.spriteSize_Y / 2, 0);
	//		temt.GetComponent<SpriteRenderer>().sprite = item.skillSprite;
	//		yield return new WaitForSeconds(1.0f);
	//		Destroy(temt);
	//		temt = null;
	//	}
	//}

}
