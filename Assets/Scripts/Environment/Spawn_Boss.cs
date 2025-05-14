using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class Spawn_Boss : MonoBehaviour
{
    [SerializeField] private List<GameObject> bossObjects = new List<GameObject>();
    [SerializeField] private Player_Stats playerStats;
    [SerializeField] private int killThreshold = 10;
    [SerializeField] private float spawnDelay = 5f;
    [SerializeField] private GameObject nav;

    private bool playerInTrigger = false;
    private bool bossSpawned = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            CheckAndSpawnBoss();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }

    private void CheckAndSpawnBoss()
    {
        if (!bossSpawned && playerStats.killCount >= killThreshold && playerInTrigger)
        {
            StartCoroutine(SpawnBossWithDelay());
            bossSpawned = true;
        }
    }

    private IEnumerator SpawnBossWithDelay()
    {
        yield return new WaitForSeconds(spawnDelay);
        SpawnBoss();
        RebakeNavMesh();
    }

    public void RebakeNavMesh()
    {
        NavMeshSurface surface = nav.GetComponent<NavMeshSurface>();
        
        if (surface != null)
        {
            // Clear the existing NavMesh
            UnityEngine.AI.NavMesh.RemoveAllNavMeshData();
            
            // Rebuild the NavMesh
            surface.BuildNavMesh();
            
            Debug.Log("NavMesh rebaked successfully.");
        }
        else
        {
            Debug.LogError("No NavMeshSurface component found on the specified GameObject.");
        }
    }

    private void SpawnBoss()
    {
        foreach (GameObject obj in bossObjects)
        {
            if (obj != null)
            {
                obj.SetActive(!obj.activeSelf);
            }
        }
    }

    private void Update()
    {
        if (!bossSpawned && playerStats.killCount >= killThreshold && playerInTrigger)
        {
            CheckAndSpawnBoss();
        }
    }
}