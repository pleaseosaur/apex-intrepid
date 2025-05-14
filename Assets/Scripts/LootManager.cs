using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class LootManager : MonoBehaviour
{
    // TODO: Implement loot pooling
    
    [System.Serializable]
    public class LootItem
    {
        public GameObject prefab;
        public float dropChance;
    }

    [Header("Enemy Drops Loot Pool")]
    public List<LootItem> lootItems;
    public float cashDropChance = 0.5f;

    [Header("Item Loot Pool")]
    public List<LootItem> items;
    public Vector2 itemSpawnTimeRange = new Vector2(10f, 30f);
    // life span of the item
    public float itemLife = 30f;

    [Header("Special Key")]
    public GameObject keyPrefab;
    public float killThreshold = 30;
    public float initialKeyDropChance = 0f;
    public float keyDropChanceIncrease = 0.01f;
    private float currentKeyDropChance;

    [Header("Spawn Settings")]
    private float spawnRadius = 2f; // Increased from 2f to 10f
    private int maxSpawnAttempts = 30;
    
    private Renderer[] floorRenderers;
    private Player_Stats playerStats;
    private SoundManager soundManager;

    private void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Stats>();
        currentKeyDropChance = initialKeyDropChance;
        floorRenderers = GetComponentsInChildren<Renderer>();
        soundManager = FindObjectOfType<SoundManager>();
        StartCoroutine(SpawnItemsRandomly());
    }

    public void DropLoot(Vector3 position)
    {
        // Check for key drop first
        if (Random.value <= currentKeyDropChance)
        {
            position.y += 1.5f;
            Instantiate(keyPrefab, position, Quaternion.identity);
            soundManager.Play("KeyDrop");
            currentKeyDropChance = initialKeyDropChance; // Reset key drop chance
        }
        else
        {
            if (playerStats.killCount > killThreshold)
            {
                currentKeyDropChance += keyDropChanceIncrease;
            }
            else
            {
                currentKeyDropChance += keyDropChanceIncrease/2;
            }
        
            // Check for cash/loot drop
            if (Random.value <= cashDropChance)
            {
                GameObject loot = GetRandomLoot(lootItems);
                if (loot != null)
                {
                    position.y += 1.5f;
                    Instantiate(loot, position, Quaternion.identity);
                    soundManager.Play("CashDrop");
                }
            }
        }
    }

    private IEnumerator SpawnItemsRandomly()
    {
        while (true)
        {
            float waitTime = Random.Range(itemSpawnTimeRange.x, itemSpawnTimeRange.y);
            yield return new WaitForSeconds(waitTime);

            GameObject item = GetRandomLoot(items);
            if (item != null)
            {
                Vector3 spawnPosition = GetRandomSpawnPosition();
                if (spawnPosition != Vector3.zero)
                {
                    Instantiate(item, spawnPosition, Quaternion.identity);
                    soundManager.Play("ItemDrop");
                    Debug.Log($"Item spawned at position: {spawnPosition}");
                    // TODO: Add a coroutine to destroy the item after a certain amount of time
                }
                else
                {
                    Debug.LogWarning("Failed to find a valid spawn position for item.");
                }
            }
        }
    }

    private GameObject GetRandomLoot(List<LootItem> lootPool)
    {
        float totalChance = 0f;
        foreach (var item in lootPool)
        {
            totalChance += item.dropChance;
        }

        float randomValue = Random.Range(0f, totalChance);
        float currentSum = 0f;

        foreach (var item in lootPool)
        {
            currentSum += item.dropChance;
            if (randomValue <= currentSum)
            {
                return item.prefab;
            }
        }

        Debug.LogWarning("No loot dropped");
        return null; // No loot dropped
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Renderer floorRenderer = floorRenderers[Random.Range(0, floorRenderers.Length)];
        Bounds bounds = floorRenderer.bounds;

        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);
        float randomY = bounds.max.y + 2f;

        return new Vector3(randomX, randomY, randomZ);
    }
}