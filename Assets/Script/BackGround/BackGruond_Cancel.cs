using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGruond_Cancel : MonoBehaviour
{
    //���̾� ���� �νľȵ� ���� ������ ����...
    public void OnMouseUp()
    {
        CardManager.Inst.CancelUseCard();    
    }

}