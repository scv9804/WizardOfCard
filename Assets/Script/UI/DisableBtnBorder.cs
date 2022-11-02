using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableBtnBorder : MonoBehaviour
{
    Image BtnImage;

    void Awake()
    {
        BtnImage = GetComponent<Image>();
        BtnImage.alphaHitTestMinimumThreshold = 0.1f;
    }
}
