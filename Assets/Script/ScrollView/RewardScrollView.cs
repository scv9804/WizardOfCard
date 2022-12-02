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
    List<GameObject> destroyUiObjects = new List<GameObject>();
 
    public void ClearViewList()
	{
        uiobjects.Clear();
	}

    public void destroyRewardObejct()
	{
        foreach(var item in destroyUiObjects)
		{
            Destroy(item);
		}
	}


    public GameObject SetReward(Item_inven item)
    {
        var newUI = Instantiate(uiPrefab);
        newUI.transform.SetParent(contents.transform);

        //프리팹 내용 설정
        Image itemimage = newUI.GetComponentInChildren<Image>();
        itemimage.sprite = item.Sprite;
        TMP_Text tmptext = newUI.GetComponentInChildren<TMP_Text>();
        tmptext.text = item.Title;

        uiobjects.Add(newUI.GetComponent<RectTransform>());
        destroyUiObjects.Add(newUI);

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
        newUI.transform.SetParent(contents.transform);

        //프리팹 내용 설정
        Image itemimage = newUI.GetComponentInChildren<Image>();
        itemimage.sprite = moneySprite;
        TMP_Text tmptext = newUI.GetComponentInChildren<TMP_Text>();
        tmptext.text = "정수";

        uiobjects.Add(newUI.GetComponent<RectTransform>());
        destroyUiObjects.Add(newUI);

        float y = 0f;

        for (int i = 0; i <= uiobjects.Count; i++)
        {
            y += uiobjects[0].sizeDelta.y + space;
        }
        scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x, y);


        return newUI;
    }

    public GameObject SetReward(GameObject _card)
    {
        var newUI = Instantiate(uiPrefab);
        var newCard = Instantiate(_card);
        newUI.transform.parent = contents.transform;
        newCard.transform.SetParent(newUI.transform);

        //프리팹 내용 설정
        Image itemimage = newUI.GetComponentInChildren<Image>();
        itemimage.enabled = false;
        newCard.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(newUI.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition3D.x, 50,0);


        TMP_Text tmptext = newUI.GetComponentInChildren<TMP_Text>();
        tmptext.text = _card.transform.GetChild(1).GetComponent<TMP_Text>().text;

        uiobjects.Add(newUI.GetComponent<RectTransform>());
        destroyUiObjects.Add(newUI);

        float y = 0f;

        for (int i = 0; i <= uiobjects.Count; i++)
        {
            y += uiobjects[0].sizeDelta.y + space;
        }
        scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x, y);


        return newUI;
    }
}
