using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOreText : MonoBehaviour
{
    private Transform targetCamera;
    private TextMesh floatingText;

    private void Start()
    {
        floatingText = GetComponent<TextMesh>();
        targetCamera = GameObject.FindWithTag("MainCamera").transform;
    }

    private void LateUpdate()
    {
        transform.LookAt(targetCamera);
        transform.Rotate(0, 180, 0);
    }

    public void SetText(int ores)
    {
        floatingText.text = "x" + ores;
    }
}
