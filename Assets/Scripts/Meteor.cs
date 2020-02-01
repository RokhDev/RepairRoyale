using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    public float sizeVariation;
    public float baseSpeed;
    public float speedVariation;
    public float knockdownRadius;
    public float knockbackRadius;
    public ParticleSystem particles;
    public GameObject explosionFx;

    private MeshRenderer meshRenderer;
    private Vector3 target = Vector3.zero;
    private float trueSpeed;
    private float sizeVar;

    void OnEnable()
    {
        sizeVar = Random.Range(-sizeVariation, sizeVariation);
        transform.localScale = new Vector3(transform.localScale.x + sizeVar,
                                           transform.localScale.y + sizeVar,
                                           transform.localScale.z + sizeVar);
        trueSpeed = baseSpeed + Random.Range(-speedVariation, speedVariation);

        Color meteorColor = new Color(0.647f, Random.Range(0.313f, 0.647f), 0.313f, 0.01f);
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.color = meteorColor;
        
        ParticleSystem.MainModule particleSettings = particles.main;
        particleSettings.startColor = new ParticleSystem.MinMaxGradient(meteorColor);

        knockbackRadius += sizeVar;
        knockdownRadius += sizeVar;
    }
    
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, trueSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            CollideWithFloor();
        }
    }

    private void CollideWithFloor()
    {
        if (explosionFx)
        {
            Magic.Pooling.PoolManager.Spawn(explosionFx, transform.position, Quaternion.identity, "explosionFx");
        }
        Magic.Pooling.PoolManager.DeSpawn(gameObject, "meteorites");
    }

    public void SetTarget(Vector3 tgt)
    {
        target = tgt;
    }
}
