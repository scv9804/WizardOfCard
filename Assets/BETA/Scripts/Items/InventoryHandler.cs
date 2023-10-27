using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.UI;

using Sirenix.OdinInspector;

using System;

using TacticsToolkit;

namespace BETA
{
    // ==================================================================================================== InventoryHandler

    public class InventoryHandler : SerializedMonoBehaviour
    {
        //

        //

        //[SerializeField, TitleGroup("������ ����")]
        //private ItemSlotTEMP _cloth;

        //[SerializeField, TitleGroup("������ ����")]
        //private ItemSlotTEMP _earring;

        //[SerializeField, TitleGroup("������ ����")]
        //private ItemSlotTEMP _hat;

        //[SerializeField, TitleGroup("������ ����")]
        //private ItemSlotTEMP _wand;

        //[SerializeField, TitleGroup("������ ����")]
        //private ItemSlotTEMP _ring;

        [SerializeField, TitleGroup("������ ����")]
        private Dictionary<ItemType, ItemSlotTEMP> _equipmentSlot = new Dictionary<ItemType, ItemSlotTEMP>();

        //

        [SerializeField, TitleGroup("������ ���� �̺�Ʈ")]
        private EventDispatcher _itemSlotRefresh;

        //

        //

        private void OnEnable()
        {
            _itemSlotRefresh.Listener += OnItemSLotRefresh;
        }

        private void OnDisable()
        {
            _itemSlotRefresh.Listener -= OnItemSLotRefresh;
        }

        //

        private void OnItemSLotRefresh()
        {
            foreach (var slot in _equipmentSlot)
            {
                var type = slot.Key;

                slot.Value.Slot.isOn = ItemManager.Instance.IsEquiped[type];
            }
        }
    }

    public enum ItemType
    {
        NONE,

        CLOTH,
        EARRING,
        HAT,
        WAND,
        RING,

        CONSUMABLE
    }
}
