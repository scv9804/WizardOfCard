using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGruond : MonoBehaviour
{
    //���̾� ���� �νľȵ� ���� ������ ����...
    private void OnMouseDown()
    {
        CardManager.Inst.CancelUseCard();
        UIManager.Inst.optionUI.SetActive(false);
        UIManager.Inst.minimapUI.SetActive(false);
            
    }

}
