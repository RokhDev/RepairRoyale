using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColor : MonoBehaviour
{
    public Color playerColor;
    public MeshRenderer bodyRenderer;
    public ParticleSystem particles;

    void Start()
    {
        bodyRenderer.material.color = playerColor;
        if (particles != null)
        {
            ParticleSystem.MainModule particleSettings = particles.main;
            particleSettings.startColor = new ParticleSystem.MinMaxGradient(playerColor);
        }
    }
}
