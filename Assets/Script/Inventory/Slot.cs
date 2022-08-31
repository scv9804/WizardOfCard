using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
	public Item_Inven item;
	public Image itemIcon;
	public int slotnum;
	enum SlotState { Quick, Inven, Equiment};

	[SerializeField] SlotState slotState;

	public void UpdateSlotUI()
	{
		itemIcon.sprite = item.itemIcon;
		itemIcon.gameObject.SetActive(true);
		if (this.item.itemName != "")
			SetColor(1);
		else
		{
			SetColor(0);
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (slotState == SlotState.Inven || slotState == SlotState.Quick)
		{
			try
			{
				bool isUse = item.Use();
				if (isUse)
				{
					ClearSlot();
				}
				Debug.Log("아이템을 사용했습니다.");
			}
			catch
			{
				Debug.Log((slotnum + 1) + "번쨰의 " + "Slot안에 Item이 없습니다.");
			}
		}
		
	}
	private void SetColor(float _alpha)
	{
		Color color = itemIcon.color;
		color.a = _alpha;
		itemIcon.color = color;
	}

	public void AddItem(Item_Inven _item)
	{
		this.item = _item;
		SetColor(1);			
	}

	public void ClearSlot()
	{
		item = null;
		itemIcon.sprite = null;
		SetColor(0);
	}


	public void OnBeginDrag(PointerEventData eventData)
	{
		if (slotState == SlotState.Inven)
		{
			if (item.itemName != "")
			{
				DragSlot.Inst.dragSlot = this;
				DragSlot.Inst.DragSetImage(itemIcon);
				DragSlot.Inst.transform.position = eventData.position;
				DragSlot.Inst.SetColor(1);
			}
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (slotState == SlotState.Inven  )
		{
			if (item.itemName != "")
			{
				DragSlot.Inst.transform.position = eventData.position;
				DragSlot.Inst.SetColor(1);
			}
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
	
		DragSlot.Inst.SetColor(0);
		DragSlot.Inst.dragSlot = null;
	}

	public void OnDrop(PointerEventData eventData)
	{
		if (DragSlot.Inst.dragSlot != null)
		{
			ChangeSlot();
		}
	}


	private void ChangeSlot()
	{
		Item_Inven _tempItem = item;

		AddItem(DragSlot.Inst.dragSlot.item);
		
		if (_tempItem != null)
			DragSlot.Inst.AddItem(_tempItem);
		else
			DragSlot.Inst.ClearSlot();

		Inventory.Inst.onChangeItem();
	}

	
}
