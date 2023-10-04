using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region ��ųʸ� �ν�����ȭ ������
/*[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue> , ISerializationCallbackReceiver
{
	[SerializeField] List<TKey> keys = new List<TKey>();
	[SerializeField] List<TValue> values = new List<TValue>();

	//���⼭ ����
	public void OnBeforeSerialize()
	{
		keys.Clear();
		values.Clear();
		foreach (KeyValuePair<TKey,TValue> pair in this)
		{
			keys.Add(pair.Key); values.Add(pair.Value);
		}
	}

	//���⼭ �ε�
	public void OnAfterDeserialize()
	{
		this.Clear();
		if (keys.Count != values.Count) { throw new System.Exception("{0}�� Ű�� �����ϴ� {1} ���� �� �� �ϳ��� �����ϴ�. �ѽ��� �ǵ��� ������ �ּ��� "); }
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
	[SerializeField] [Tooltip("�������Լ�")] Sprite warCrySprite;
	[SerializeField] [Tooltip("���߷�����")] Sprite decreasedConcentration;
	[SerializeField] [Tooltip("�νĵ�")] Sprite rustAccid;
	[SerializeField] [Tooltip("�νĵ�")] Sprite burning;
	[SerializeField] Sprite shieldSprite;

	[Header("SkillEffectImage")]
	[SerializeField] SkillEffect [] skillEffects;


	[Header("SkillEffectImage")]
	[SerializeField]GameObject defultPrefab;

	//�̹��� ��ųʸ��� ��ġ�ϱ�!


	#region �����̹��� ������Ƽȭ

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
