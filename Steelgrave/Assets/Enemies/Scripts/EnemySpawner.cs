using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.Contracts;

using System.Security.Cryptography;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform tankTarget;
    public float spawnRadius = 10f;
    public float spawnInterval = 3f;

    public bool isSpawning = false;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 1f, spawnInterval);
    }

    private void SpawnEnemy()
    {
        if (isSpawning)
        {
            if (enemyPrefabs.Length == 0 || tankTarget == null) return;

            //choose a random enemy prefab

            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            //random position around the tank
            Vector2 offset = Random.insideUnitCircle.normalized * spawnRadius;
            Vector3 spawnPos = tankTarget.transform.position + new Vector3(offset.x, 0, offset.y);

            //instantiate and assign target
            GameObject enemyObj = Instantiate(prefab, spawnPos, Quaternion.identity);

            EnemyController enemy = enemyObj.GetComponent<EnemyController>();

            if (enemy != null)
            {
                enemy.target = tankTarget;
                enemyObj.transform.localScale = (Vector3.one * enemy.stats.size);
            }
        }

        
    }
}
