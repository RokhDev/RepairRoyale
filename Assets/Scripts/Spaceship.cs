using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    public ParticleSystem ps;
    public float speed = 1.0f;

    private bool hasTakenOff = false;

    private void Update()
    {
        if (!hasTakenOff) { return; }

        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    public void TakeOff ()
    {
        hasTakenOff = true;
        ps.Play();
    }
}
