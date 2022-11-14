using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	public Item_inven item;
	public int id;
	public int slotId;
	public ItemData newEquip;
	public ItemData Equiped;

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
		else if (item != null && item.Equipable)
		{
			EquipItem();
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
		inv.items[slotId] = null;
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
				this.transform.SetParent(inv.equipslots[0].transform);
				this.transform.localPosition = Vector2.zero;
				inv.equipitems[0] = item;
				Debug.Log(inv.equipitems[0].Id);
				break;
			case "Sub":
				Debug.Log("서브 장착");
				this.transform.SetParent(inv.equipslots[1].transform);
				this.transform.localPosition = Vector2.zero;
				inv.equipitems[1] = item;
				break;
			case "Hat":
				Debug.Log("머리 장착");
				this.transform.SetParent(inv.equipslots[2].transform);
				this.transform.localPosition = Vector2.zero;
				inv.equipitems[2] = item;
				break;
			case "Suit":
				Debug.Log("옷 장착");
				this.transform.SetParent(inv.equipslots[3].transform);
				this.transform.localPosition = Vector2.zero;
				inv.equipitems[3] = item;
				break;
			case "Accessory":
				Debug.Log("악세장착");
				this.transform.SetParent(inv.equipslots[4].transform);
				this.transform.localPosition = Vector2.zero;
				inv.equipitems[4] = item;
				break;
		}
		inv.items[slotId] = null;
		inv.items[slotId] = new Item_inven();
		useAccept.Deactivate();
		if (inv.equipslots[slotId] = null)
        {
			//Equiped = inv.equipslots[slotId];
			newEquip.id = item.Id;
			Debug.Log(Equiped.id);
			inv.AddItem(item.Id);
        }
    }
}