using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
	public Item item;
	public int amount;
	public int slotId;

	private Inventory inv;
	private Tooltip tooltip;
	private Vector2 offset;

	void Start()
	{
		inv = GameObject.Find("InventorySystem").GetComponent<Inventory>();
		tooltip = inv.GetComponent<Tooltip>();
	}

	public void OnPointerClick(PointerEventData eventData)
    {
		if (item != null)
        {

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
			this.transform.position = Input.mousePosition;
			Debug.Log("로컬" + this.transform.localPosition);
			Debug.Log(eventData.position);
		}
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
}
