using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoriteSpawner : MonoBehaviour
{
    public GameObject meteoritePrefab;
    public float upperSpawnMaxVariance;
    public float lowerSpawnMaxVariance;
    public float spawnHeight;
    public float spawnTimer;
    public float spawnTimerVariation;
    public float spawnTimerReductionPerSecond;
    public float spawnTimerMinimum;

    private float despawnHeight;
    private float spawnCd;

    private void Start()
    {
        despawnHeight = transform.position.y;
        spawnCd = spawnTimer;
    }

    private void Update()
    {
        if (spawnTimer > spawnTimerMinimum)
        {
            spawnTimer -= spawnTimerReductionPerSecond * Time.deltaTime;
            if (spawnTimer < spawnTimerMinimum) { spawnTimer = spawnTimerMinimum; }
        }

        if (spawnCd > 0)
        {
            spawnCd -= Time.deltaTime;
            return;
        }

        spawnCd = spawnTimer;
        Vector3 baseSpawnPosition = new Vector3(transform.position.x, spawnHeight, transform.position.z);
        Vector3 spawnPositionDisplacement = (Quaternion.Euler(0, Random.Range(0, 360), 0) * Vector3.forward) * upperSpawnMaxVariance;
        Vector3 spawnPosition = baseSpawnPosition + spawnPositionDisplacement;
        GameObject spawnedMeteorite = Magic.Pooling.PoolManager.Spawn(meteoritePrefab, spawnPosition, Quaternion.identity, "meteorites");

        Meteor meteoriteHandler = spawnedMeteorite.GetComponent<Meteor>();
        Vector3 baseTargetPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 targetPositionDisplacement = (Quaternion.Euler(0, Random.Range(0, 360), 0) * Vector3.forward) * lowerSpawnMaxVariance;
        Vector3 targetPosition = baseTargetPosition + targetPositionDisplacement;
        meteoriteHandler.SetTarget(targetPosition);
    }
}
