using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    public ParticleSystem ps;

    private bool hasTakenOff = false;

    private void Update()
    {
        if (!hasTakenOff) { return; }
    }

    public void TakeOff ()
    {
        
    }
}
