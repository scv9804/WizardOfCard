using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGruond : MonoBehaviour
{
    //레이어 땜에 인식안될 수도 있으니 조심...
    private void OnMouseDown()
    {
        CardManager.Inst.CancelUseCard();
        UIManager.Inst.optionUI.SetActive(false);
        UIManager.Inst.minimapUI.SetActive(false);
            
    }

}
