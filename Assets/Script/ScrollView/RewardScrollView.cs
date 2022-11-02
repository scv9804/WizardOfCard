using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardScrollView : MonoBehaviour
{
    private ScrollRect scrollRect;

    public float space = 50f;

    public GameObject uiPrefab;

    public List<RectTransform> uiobjects = new List<RectTransform>();

    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
    }

    
    public void SetReward(Item_inven item)
    {
        var newUI = Instantiate(uiPrefab, scrollRect.content).GetComponent<RectTransform>();

        newUI.GetChild(0).GetComponent<Image> =;

        uiobjects.Add(newUI);

        float y = 0f;
        
        for (int i = 0; i <= uiobjects.Count; i++)
        {
            y += uiobjects[0].sizeDelta.y + space;
        }
        scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x, y);
    }


}
