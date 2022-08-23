using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
    static public DragSlot Inst;
    public Slot dragSlot;

    [SerializeField]
    private Image imageItem;

    void Start()
    {
        Inst = this;
    }

    public void DragSetImage(Image _itemImage)
    {
        imageItem.sprite = _itemImage.sprite;
        SetColor(1);
    }

    public void SetColor(float _alpha)
    {
        Color color = imageItem.color;
        color.a = _alpha;
        imageItem.color = color;
    }
    
    public void ClearSlot()
	{
        Color color = imageItem.color;
        color.a = 0;
        imageItem.color = color;
    }

    public void AddItem(Item_Inven _item)
	{
        dragSlot.item = _item;
	}
   
}
