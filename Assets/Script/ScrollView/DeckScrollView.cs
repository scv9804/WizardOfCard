using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeckScrollView : MonoBehaviour
{
    private ScrollRect scrollRect;

    public float space = 50f;

    public GameObject uiPrefab;

    public List<RectTransform> uiobjects = new List<RectTransform>();
    public List<GameObject> destroyObejcts = new List<GameObject>();

    void Start()
    {
        scrollRect = GetComponent<ScrollRect>(); 
    }

    public void CemeteryCardSet()
    {
        foreach (var item in destroyObejcts)
        {
            Destroy(item);
        }
        uiobjects.Clear();
        destroyObejcts.Clear();


        foreach (var item in CardManager.Inst.myCemetery)
        {
            var newUI = Instantiate(uiPrefab, scrollRect.content);
            var cardinfo = newUI.AddComponent<Card>();
            cardinfo = item;

            newUI.transform.GetChild(0).GetComponent<TMP_Text>().text = cardinfo.i_manaCost.ToString();
            newUI.transform.GetChild(1).GetComponent<TMP_Text>().text = cardinfo.st_cardName;
            newUI.transform.GetChild(2).GetComponent<TMP_Text>().text = cardinfo.GetCardExplain();

            cardinfo.is_UI_Card = true;
            cardinfo.Setup();

            destroyObejcts.Add(newUI);
            uiobjects.Add(newUI.GetComponent<RectTransform>());
        }

        float y = 0f;

        for (int i = 0; i <= uiobjects.Count; i++)
        {
            if (i % 4 == 0)
            {
                y += uiobjects[0].sizeDelta.y + space;
            }
        }
        scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x, y);
    }

    public void DeckCardSet()
	{
		foreach (var item in destroyObejcts)
		{
            Destroy(item);
		}
        uiobjects.Clear();
        destroyObejcts.Clear();


        foreach (var item in CardManager.Inst.myDeck)
		{
            Debug.Log(111);
            var newUI = Instantiate(uiPrefab, scrollRect.content);
            var cardinfo = newUI.AddComponent<Card>();
            cardinfo = item;

            newUI.transform.GetChild(0).GetComponent<TMP_Text>().text = cardinfo.i_manaCost.ToString();
            newUI.transform.GetChild(1).GetComponent<TMP_Text>().text = cardinfo.st_cardName;
            newUI.transform.GetChild(2).GetComponent<TMP_Text>().text = cardinfo.GetCardExplain();

            cardinfo.is_UI_Card = true;
            cardinfo.Setup();

            destroyObejcts.Add(newUI);
            uiobjects.Add(newUI.GetComponent<RectTransform>());
        }

        float y = 0f;

        for (int i = 0 ; i <= uiobjects.Count ; i++)
		{
			if (i % 4 == 0)
			{
                y += uiobjects[0].sizeDelta.y + space;
            }
		}
        scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x, y);
    }
}
