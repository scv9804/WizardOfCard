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

    [SerializeField, TitleGroup("�̹���")]
    private Image _frameImage;

    [SerializeField, TitleGroup("�̹���")]
    private Image _artworkImage;

    // ======================================== Text

    [SerializeField, TitleGroup("�ؽ�Ʈ")]
    private TMP_Text _nameTMP;

    [SerializeField, TitleGroup("�ؽ�Ʈ")]
    private TMP_Text _costTMP;

    [SerializeField, TitleGroup("�ؽ�Ʈ")]
    private TMP_Text _descriptionTMP;

    // ============================================================ EventListener

    // ======================================== EventSystem

    [SerializeField, TitleGroup("�̺�Ʈ�ý��� ���� �̺�Ʈ")]
    private Event<PointerEventData, CardObject> _cardPointerEnter;

    [SerializeField, TitleGroup("�̺�Ʈ�ý��� ���� �̺�Ʈ")]
    private Event<PointerEventData, CardObject> _cardPointerExit;

    [SerializeField, TitleGroup("�̺�Ʈ�ý��� ���� �̺�Ʈ")]
    private Event<PointerEventData, CardObject> _cardPointerClick;

    [SerializeField, TitleGroup("�̺�Ʈ�ý��� ���� �̺�Ʈ")]
    private Event<PointerEventData, CardObject> _cardBeginDrag;

    [SerializeField, TitleGroup("�̺�Ʈ�ý��� ���� �̺�Ʈ")]
    private Event<PointerEventData, CardObject> _cardDrag;

    [SerializeField, TitleGroup("�̺�Ʈ�ý��� ���� �̺�Ʈ")]
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
