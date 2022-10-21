using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckScrollView : MonoBehaviour
{
    private ScrollRect scrollRect;

    public float space = 50f;

    public GameObject uiPrefab;

    public List<RectTransform> uiobjects = new List<RectTransform>();

    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCards()
	{
        var newUI = Instantiate(uiPrefab, scrollRect.content).GetComponent<RectTransform>();

        uiobjects.Add(newUI);

        float y = 0f;
        Debug.Log("??");

        for (int i = 0 ; i <= uiobjects.Count ; i++)
		{
			if (i % 5 == 0)
			{
                y += uiobjects[0].sizeDelta.y + space;
                Debug.Log(i);
            }
		}
        scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x, y);
    }


}
