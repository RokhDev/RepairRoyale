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
    public float knockbackForce;
    public Vector3 originalSize;
    public GameObject orePrefab;
    public ParticleSystem particles;
    public GameObject explosionFx;

    private MeshRenderer meshRenderer;
    private Vector3 target = Vector3.zero;
    private float trueSpeed;
    private float sizeVar;
    private GameObject[] players;
    private Rigidbody[] playerRbs;
    private Player[] playerCms;
    private float originalKnockbackRadius;
    private float originalKnockdownRadius;

    private void Awake()
    {
        originalKnockbackRadius = knockbackRadius;
        originalKnockdownRadius = knockdownRadius;
    }

    private void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        playerRbs = new Rigidbody[players.Length];
        playerCms = new Player[players.Length];
        for (int i = 0; i < players.Length; i++)
        {
            playerRbs[i] = players[i].GetComponent<Rigidbody>();
            playerCms[i] = players[i].GetComponent<Player>();
        }
    }

    void OnEnable()
    {
        transform.localScale = originalSize;
        sizeVar = Random.Range(-sizeVariation, sizeVariation);
        transform.localScale = new Vector3(transform.localScale.x + sizeVar,
                                           transform.localScale.y + sizeVar,
                                           transform.localScale.z + sizeVar);
        trueSpeed = baseSpeed + Random.Range(-speedVariation, speedVariation);

        Color meteorColor = new Color(0.647f, Random.Range(0.313f, 0.647f), 0.313f, 0.0f);
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.color = meteorColor;
        
        ParticleSystem.MainModule particleSettings = particles.main;
        particleSettings.startColor = new ParticleSystem.MinMaxGradient(new Color(meteorColor.r, meteorColor.g, meteorColor.b, 1.0f));

        knockbackRadius = originalKnockbackRadius + sizeVar;
        knockdownRadius = originalKnockdownRadius + sizeVar;
    }
    
    void Update()
    {
        if (meshRenderer.material.color.a < 1.0f)
        {
            meshRenderer.material.color = new Color(meshRenderer.material.color.r,
                                                    meshRenderer.material.color.g,
                                                    meshRenderer.material.color.b,
                                                    meshRenderer.material.color.a + 1 * Time.deltaTime);
        }

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

        Magic.Pooling.PoolManager.Spawn(orePrefab, transform.position, Quaternion.identity, "ores");

        for (int i = 0; i < players.Length; i++)
        {
            float distance = Vector3.Distance(players[i].transform.position, transform.position);
            if (distance <= knockdownRadius)
            {
                playerCms[i].Knockdown();
            }
            else if (distance <= knockbackRadius)
            {
                Vector3 tgtDirection = (players[i].transform.position - transform.position).normalized;
                playerRbs[i].AddForce(tgtDirection * knockbackForce, ForceMode.VelocityChange);
            }
        }

        Magic.Pooling.PoolManager.DeSpawn(gameObject, "meteorites");
    }

    public void SetTarget(Vector3 tgt)
    {
        target = tgt;
    }
}
