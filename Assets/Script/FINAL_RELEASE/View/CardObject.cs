using Sirenix.OdinInspector;

using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// ================================================================================ CardObject

public class CardObject : UnitObject<Card>, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // ================================================================================ Field

    // ============================================================ CardObject

    // ======================================== Image

    [SerializeField, TitleGroup("이미지")]
    private Image _frameImage;

    [SerializeField, TitleGroup("이미지")]
    private Image _artworkImage;

    // ======================================== Text

    [SerializeField, TitleGroup("텍스트")]
    private TMP_Text _nameTMP;

    [SerializeField, TitleGroup("텍스트")]
    private TMP_Text _costTMP;

    [SerializeField, TitleGroup("텍스트")]
    private TMP_Text _descriptionTMP;

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

    // ================================================================================ Meethod

    // ============================================================ EventSystem

    public void OnPointerEnter(PointerEventData eventData)
    {
        _cardPointerEnter.Launch(eventData, this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _cardPointerExit.Launch(eventData, this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _cardPointerClick.Launch(eventData, this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _cardBeginDrag.Launch(eventData, this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _cardDrag.Launch(eventData, this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _cardEndDrag.Launch(eventData, this);
    }
}
