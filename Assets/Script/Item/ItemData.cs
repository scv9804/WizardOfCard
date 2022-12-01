using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	public Item_inven item;
	public int id;
	public int slotId;
	//private int targetEquipSlot;

	private Inventory inv;
	private Tooltip tooltip;
	private UseAccept useAccept;
	private Vector2 offset;

	void Awake()
	{
		inv = GameObject.Find("InventorySystem").GetComponent<Inventory>();
		tooltip = inv.GetComponent<Tooltip>();
		useAccept = inv.GetComponent<UseAccept>();
	}
	
	public void TooltipDeActive()
	{
		tooltip.Deactivate();
	}

	public void OnPointerClick(PointerEventData eventData)
    {
		Debug.Log(this.item.Id+"Clicked");
		if (item != null && item.Equipable == false && item.OwnPlayer) //장비가 아닐시 == 소모품일시
        {
			useAccept.itemData = this;
			inv.itemdata = this;
			useAccept.Activate(item);
			tooltip.Deactivate();
		}
		else if(item != null && item.Equipable == true && item.OwnPlayer)
        {
			useAccept.itemData = this;
			inv.itemdata = this;
			useAccept.Activate(item);
			tooltip.Deactivate();
		}
	}
	public void OnBeginDrag(PointerEventData eventData)
	{
		if (item != null && item.OwnPlayer)
		{
			this.transform.SetParent(this.transform.parent.parent);
			this.transform.position = Input.mousePosition;
			GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
	}
	public void OnDrag(PointerEventData eventData)
	{
		if (item != null && item.OwnPlayer)
		{
			this.transform.position = Input.mousePosition;	
		}
	}
	public void OnEndDrag(PointerEventData eventData)
	{
		if (item.OwnPlayer)
		{
			this.transform.SetParent(inv.slots[slotId].transform);
			this.transform.position = inv.slots[slotId].transform.position;
			GetComponent<CanvasGroup>().blocksRaycasts = true;
		}
	}
	public void OnPointerDown(PointerEventData eventData)
	{
		offset = eventData.position - new Vector2(this.transform.localPosition.x, this.transform.localPosition.y);
	}
	public void OnPointerEnter(PointerEventData eventData)
	{
		tooltip.Activate(item);
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		tooltip.Deactivate();
	}
	
}