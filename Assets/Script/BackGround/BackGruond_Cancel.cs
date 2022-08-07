using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGruond_Cancel : MonoBehaviour
{
    //레이어 땜에 인식안될 수도 있으니 조심...
    private void OnMouseDown()
    {
        CardManager.Inst.CancelUseCard();
    
    }

}
