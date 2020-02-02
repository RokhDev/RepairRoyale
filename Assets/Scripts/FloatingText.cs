using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public Transform targetCamera;
    public GameController gameManager;

    private int savedOres = 0;
    private TextMesh floatingText;

    private void Start()
    {
        floatingText = GetComponent<TextMesh>();
    }

    private void Update()
    {
        transform.LookAt(targetCamera);
        transform.Rotate(0,180,0);
    }

    public void SetText(int ores)
    {
        savedOres += ores;
        float repairPercent = savedOres * 100 / gameManager.repairsToWin;
        if (repairPercent > 100) { repairPercent = 100; }

        floatingText.text = repairPercent.ToString("F0") + "%\nRepaired!";
    }
}
