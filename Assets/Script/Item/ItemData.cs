using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	public Item_inven item;
	public int id;
	public int slotId;
	private int targetEquipSlot;

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
	public void OnPointerClick(PointerEventData eventData)
    {
		Debug.Log(this.item.Id+"Clicked");
		if (item != null && item.Equipable == false) //장비가 아닐시 == 소모품일시
        {
			useAccept.itemData = this;
			useAccept.Activate(item);
			tooltip.Deactivate();
		}
		else if(item != null && item.Equipable == true)
        {
			useAccept.itemData = this;
			useAccept.Activate(item);
			tooltip.Deactivate();
		}
	}
	public void OnBeginDrag(PointerEventData eventData)
	{
		if (item != null)
		{
			this.transform.SetParent(this.transform.parent.parent);
			this.transform.position = Input.mousePosition;
			GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
	}
	public void OnDrag(PointerEventData eventData)
	{
		if (item != null)
		{
			this.transform.position = Input.mousePosition;		}
	}
	public void OnEndDrag(PointerEventData eventData)
	{
		this.transform.SetParent(inv.slots[slotId].transform);
		this.transform.position = inv.slots[slotId].transform.position;
		GetComponent<CanvasGroup>().blocksRaycasts = true;
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
	public void ItemUse(Item_inven item)
	{
		if (item != null && !item.Equipable)
		{
			HealItemUsed();
			useAccept.Deactivate();
		}
		else if (item != null && item.Equipable && this.slotId < 10)
		{
			EquipItem();
			useAccept.Deactivate();
		}
		else if (item != null && item.Equipable && this.slotId >= 10)
		{
			UnequipItem();
			useAccept.Deactivate();
		}
	}
	public void HealItemUsed()
    {
		PlayerEntity.Inst.Status_Health += item.Healing;

		if (PlayerEntity.Inst.Status_Health > PlayerEntity.Inst.Status_MaxHealth)
		{
			PlayerEntity.Inst.Status_Health = PlayerEntity.Inst.Status_MaxHealth;
		}
		Destroy(gameObject);
		inv.items[slotId] = new Item_inven();
		useAccept.Deactivate();
		inv.AddItem(0);
	}
	public void EquipItem()
    {
		Debug.Log(item.Id);
		switch(item.Type){
			case "Weapon":
				Debug.Log("무기 장착");
				targetEquipSlot = 0;
				Debug.Log(inv.equipitems[0].Id);
				break;
			case "Ear":
				Debug.Log("귀걸이 장착");
				targetEquipSlot = 1;
				break;
			case "Hat":
				Debug.Log("머리 장착");
				targetEquipSlot = 2;
				break;
			case "Suit":
				Debug.Log("옷 장착");
				targetEquipSlot = 3;
				break;
			case "Ring":
				Debug.Log("반지 장착");
				targetEquipSlot = 4;
				break;
		}
		if(inv.equipitems[targetEquipSlot].Id != -1)
        {
			Debug.Log("이미 있는데용");
			Transform equipeditem = inv.equipslots[targetEquipSlot].transform.GetChild(0);
			equipeditem.GetComponent<ItemData>().slotId = this.slotId;
			equipeditem.transform.SetParent(inv.slots[this.slotId].transform);
			equipeditem.transform.position = inv.slots[this.slotId].transform.position;
			//inv.equipitems[targetEquipSlot] = item;
		}
		this.transform.SetParent(inv.equipslots[targetEquipSlot].transform);
		inv.equipitems[targetEquipSlot] = item;
		this.transform.localPosition = Vector2.zero;
		inv.items[slotId] = new Item_inven();
		useAccept.Deactivate();
		this.slotId = inv.equipslots[targetEquipSlot].GetComponent<Slot>().id;

	}
	public void UnequipItem()
    {
		inv.equipitems[this.slotId-10] = new Item_inven();
		for (int i = 0; i < inv.items.Count; i++)
        {
			if (inv.items[i].Id == -1)
            {
				Debug.Log(i);
				this.transform.SetParent(inv.slots[i].transform);
				this.transform.position = inv.slots[i].transform.position;
				inv.items[i] = item;
				this.slotId = i;
				break;
			}
		}
		useAccept.Deactivate();
	}
}