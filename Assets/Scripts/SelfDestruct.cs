using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public string poolName;
    public float lifeTime;

    private float destroyCd;
    private ParticleSystem ps;
    private AudioSource myAudio;

    public void Start()
    {
        ps = GetComponent<ParticleSystem>();
        myAudio = GetComponent<AudioSource>();
    }

    public void OnEnable()
    {
        destroyCd = lifeTime;
        if (ps)
        {
            ps.Play();
        }
        if (myAudio)
        {
            myAudio.Play();
        }
    }

    public void Update()
    {
        if (destroyCd > 0)
        {
            destroyCd -= Time.deltaTime;
            return;
        }

        if (ps)
        {
            ps.Stop();
        }
        Magic.Pooling.PoolManager.DeSpawn(gameObject, poolName);
    }
}
