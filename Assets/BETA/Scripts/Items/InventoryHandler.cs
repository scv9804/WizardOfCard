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

        //[SerializeField, TitleGroup("아이템 슬롯")]
        //private ItemSlotTEMP _cloth;

        //[SerializeField, TitleGroup("아이템 슬롯")]
        //private ItemSlotTEMP _earring;

        //[SerializeField, TitleGroup("아이템 슬롯")]
        //private ItemSlotTEMP _hat;

        //[SerializeField, TitleGroup("아이템 슬롯")]
        //private ItemSlotTEMP _wand;

        //[SerializeField, TitleGroup("아이템 슬롯")]
        //private ItemSlotTEMP _ring;

        [SerializeField, TitleGroup("아이템 슬롯")]
        private Dictionary<ItemType, ItemSlotTEMP> _equipmentSlot = new Dictionary<ItemType, ItemSlotTEMP>();

        //

        [SerializeField, TitleGroup("아이템 관련 이벤트")]
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
