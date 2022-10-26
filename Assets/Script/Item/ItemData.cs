using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	public Item_inven item;
	public int id;
	public int amount;
	public int slotId;

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
			EquipItem();

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
		if (item != null && item.Equipable == false)
		{
			HealItemUsed();
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

    }
}
