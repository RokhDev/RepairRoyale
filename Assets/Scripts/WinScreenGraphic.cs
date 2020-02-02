using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinScreenGraphic : MonoBehaviour
{
    void Start()
    {
        Image image = GetComponent<Image>();
        image.sprite = GameController.winGraphic;
    }
}
