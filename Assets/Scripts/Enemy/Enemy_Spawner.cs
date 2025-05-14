using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class Enemy_Spawner : MonoBehaviour
{
    public static Enemy_Spawner Instance { get; private set; }

    public GameObject enemyPrefab;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private int maxEnemies = 150;

    private Queue<GameObject> enemyPool = new Queue<GameObject>();
    private List<GameObject> activeEnemies = new List<GameObject>();
    private bool turretBuilt = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(CheckForWeaponAndSpawn());
    }

    private IEnumerator CheckForWeaponAndSpawn()
    {
        while (!turretBuilt)
        {
            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                if (obj.name.Contains("Weapon") && !obj.name.Contains("Mount"))
                {
                    turretBuilt = true;
                    break;
                }
            }
            yield return new WaitForSeconds(1f);
        }

        StartCoroutine(SpawnEnemyCoroutine());
    }

    private IEnumerator SpawnEnemyCoroutine()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        if (activeEnemies.Count >= maxEnemies)
        {
            return;
        }

        Vector3 spawnPosition = GetRandomPositionOnNavMesh();

        if (spawnPosition != Vector3.zero)
        {
            GameObject enemy = GetEnemyFromPool();
            enemy.transform.position = spawnPosition;
            enemy.SetActive(true);

            NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                if (spawnInterval > 1f)
                {
                    spawnInterval -= 0.01f;
                }
                agent.Warp(spawnPosition);
            }
            else
            {
                Debug.LogWarning("Enemy does not have a NavMeshAgent component.");
            }

            activeEnemies.Add(enemy);
        }
        else
        {
            Debug.LogWarning("Failed to find a valid position on the NavMesh after multiple attempts.");
        }
    }

    private GameObject GetEnemyFromPool()
    {
        if (enemyPool.Count > 0)
        {
            return enemyPool.Dequeue();
        }
        else
        {
            return Instantiate(enemyPrefab);
        }
    }

    public void ReturnEnemyToPool(GameObject enemy)
    {
        enemy.SetActive(false);
        enemyPool.Enqueue(enemy);
        activeEnemies.Remove(enemy);
    }

    private Vector3 GetRandomPositionOnNavMesh()
    {
        for (int i = 0; i < 100; i++)
        {
            Vector3 randomPoint = GetRandomPointInNavMeshBounds();
            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        return Vector3.zero;
    }

    private Vector3 GetRandomPointInNavMeshBounds()
    {
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

        if (navMeshData.vertices.Length == 0)
        {
            return Vector3.zero;
        }

        Bounds bounds = new Bounds(navMeshData.vertices[0], Vector3.zero);
        for (int i = 1; i < navMeshData.vertices.Length; i++)
        {
            bounds.Encapsulate(navMeshData.vertices[i]);
        }

        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}