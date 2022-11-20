using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RewardScrollView : MonoBehaviour
{
    [SerializeField]private ScrollRect scrollRect;

    public float space = 50f;

    public GameObject uiPrefab;
    public GameObject contents;
    [SerializeField] Sprite moneySprite;
    public List<RectTransform> uiobjects = new List<RectTransform>();

 
    public void ClearViewList()
	{
        uiobjects.Clear();
	}

    public GameObject SetReward(Item_inven item)
    {
        var newUI = Instantiate(uiPrefab);
        newUI.transform.parent = contents.transform;

        //프리팹 내용 설정
        Image itemimage = newUI.GetComponentInChildren<Image>();
        itemimage.sprite = item.Sprite;
        TMP_Text tmptext = newUI.GetComponentInChildren<TMP_Text>();
        tmptext.text = item.Title;

        uiobjects.Add(newUI.GetComponent<RectTransform>());

        float y = 0f;
        
        for (int i = 0; i <= uiobjects.Count; i++)
        {
            y += uiobjects[0].sizeDelta.y + space;
        }
        scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x, y);
  

        return newUI;
    }
    public GameObject SetReward(int _money)
    {
        var newUI = Instantiate(uiPrefab);
        newUI.transform.parent = contents.transform;

        //프리팹 내용 설정
        Image itemimage = newUI.GetComponentInChildren<Image>();
        itemimage.sprite = moneySprite;
        TMP_Text tmptext = newUI.GetComponentInChildren<TMP_Text>();
        tmptext.text = "정수";

        uiobjects.Add(newUI.GetComponent<RectTransform>());

        float y = 0f;

        for (int i = 0; i <= uiobjects.Count; i++)
        {
            y += uiobjects[0].sizeDelta.y + space;
        }
        scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x, y);


        return newUI;
    }


}
