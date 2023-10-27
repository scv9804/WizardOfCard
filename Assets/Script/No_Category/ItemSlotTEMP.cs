using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

using TMPro;

using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotTEMP : SerializedMonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea]
    public string Description;

    public BETA.ItemType Type;

    public Toggle Slot;

    public Image TooltipImage;
    public TMP_Text TooltipTMP;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!Slot.isOn)
        {
            return;
        }

        Tooltip(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip(false);
    }

    public void Tooltip(bool isActive)
    {
        var direction = transform.position.y > Screen.height * 0.5f ? 1.0f : -1.0f;

        TooltipImage.gameObject.SetActive(isActive);
        TooltipImage.transform.position = transform.position - new Vector3(-320.0f, 175.0f * direction, 0.0f);

        TooltipTMP.text = Description;
    }

    public void ToggleEquip(bool isActive)
    {
        BETA.ItemManager.Instance.IsEquiped[Type] = isActive;
    }

    public void Equip(bool isEquip)
    {
        BETA.ItemManager.Instance.Equip(Type, isEquip);
    }
}
