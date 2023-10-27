using Sirenix.OdinInspector;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

public class TEMP : SerializedMonoBehaviour
{
    // ================================================================================ Field

    //

    //[SerializeField, TitleGroup("아이템 효과 데이터")]
    //private List<ItemAbilityData> _data = new List<ItemAbilityData>();

    // ============================================================ EventListener

    // ======================================== EventSystem

    [SerializeField, TitleGroup("이벤트시스템 관련 이벤트")]
    private Event<PointerEventData, CardObject> _cardPointerEnter;

    [SerializeField, TitleGroup("이벤트시스템 관련 이벤트")]
    private Event<PointerEventData, CardObject> _cardPointerExit;

    [SerializeField, TitleGroup("이벤트시스템 관련 이벤트")]
    private Event<PointerEventData, CardObject> _cardPointerClick;

    [SerializeField, TitleGroup("이벤트시스템 관련 이벤트")]
    private Event<PointerEventData, CardObject> _cardBeginDrag;

    [SerializeField, TitleGroup("이벤트시스템 관련 이벤트")]
    private Event<PointerEventData, CardObject> _cardDrag;

    [SerializeField, TitleGroup("이벤트시스템 관련 이벤트")]
    private Event<PointerEventData, CardObject> _cardEndDrag;

    // ======================================== EventSystem

    //[SerializeField, TitleGroup("아이템 관련 이벤트")]
    //private Event _itemEquip;

    //[SerializeField, TitleGroup("아이템 관련 이벤트")]
    //private Event _itemUnequip;

    // ================================================================================ Method

    // ============================================================ Event

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        _cardPointerEnter.AddListener(OnPointerEnter);
        _cardPointerExit.AddListener(OnPointerExit);
        _cardPointerClick.AddListener(OnPointerClick);
        _cardBeginDrag.AddListener(OnBeginDrag);
        _cardDrag.AddListener(OnDrag);
        _cardEndDrag.AddListener(OnEndDrag);

        //foreach (var data in _data)
        //{
        //    _itemEquip.AddListener(data.OnAbilityActive);
        //    _itemUnequip.AddListener(data.OnAbilityDeactive);
        //}
    }

    private void OnDisable()
    {
        _cardPointerEnter.RemoveListener(OnPointerEnter);
        _cardPointerExit.RemoveListener(OnPointerExit);
        _cardPointerClick.RemoveListener(OnPointerClick);
        _cardPointerEnter.RemoveListener(OnBeginDrag);
        _cardDrag.RemoveListener(OnDrag);
        _cardEndDrag.RemoveListener(OnEndDrag);

        //foreach (var data in _data)
        //{
        //    _itemEquip.RemoveListener(data.OnAbilityActive);
        //    _itemUnequip.RemoveListener(data.OnAbilityDeactive);
        //}
    }

    // ============================================================ EventSystem

    private void OnPointerEnter(PointerEventData eventData, CardObject cardObject)
    {
        DebugEvent(nameof(OnPointerEnter), cardObject);
    }

    private void OnPointerExit(PointerEventData eventData, CardObject cardObject)
    {
        DebugEvent(nameof(OnPointerExit), cardObject);
    }

    private void OnPointerClick(PointerEventData eventData, CardObject cardObject)
    {
        DebugEvent(nameof(OnPointerClick), cardObject);
    }

    private void OnBeginDrag(PointerEventData eventData, CardObject cardObject)
    {
        DebugEvent(nameof(OnBeginDrag), cardObject);
    }

    private void OnDrag(PointerEventData eventData, CardObject cardObject)
    {
        DebugEvent(nameof(OnDrag), cardObject);
    }

    private void OnEndDrag(PointerEventData eventData, CardObject cardObject)
    {
        DebugEvent(nameof(OnEndDrag), cardObject);
    }

    // ============================================================ Debug

    private void DebugEvent(string messege, CardObject cardObject)
    {
        $"{cardObject.name}.{messege}()".Log();
    }
}
