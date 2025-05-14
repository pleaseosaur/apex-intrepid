using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class TurretPartDropper : MonoBehaviour
{
    [System.Serializable]
    public class PartDropChance
    {
        public GameObject partPrefab;
        [Range(0f, 1f)]
        public float dropChance = 1f;
    }

    public List<PartDropChance> partLootPool = new List<PartDropChance>();
    [Range(0f, 1f)]
    public float chanceToDropNothing = 0.2f;

    public void PopulateLootPoolFromFolder(string folderPath = "Prefabs/TurretParts")
    {
        partLootPool.Clear();
        GameObject[] allParts = Resources.LoadAll<GameObject>(folderPath);
        foreach (GameObject part in allParts)
        {
            partLootPool.Add(new PartDropChance { partPrefab = part, dropChance = 1f });
        }
        Debug.Log($"Populated loot pool with {partLootPool.Count} parts from {folderPath}");
    }

    public void DropRandomPart(Vector3 position)
    {
        if (Random.value < chanceToDropNothing)
        {
            Debug.Log("No part dropped this time.");
            return;
        }

        float totalWeight = 0f;
        foreach (var part in partLootPool)
        {
            totalWeight += part.dropChance;
        }

        float randomValue = Random.value * totalWeight;
        foreach (var part in partLootPool)
        {
            if (randomValue <= part.dropChance)
            {
                Instantiate(part.partPrefab, position, Quaternion.identity);
                Debug.Log($"Dropped part: {part.partPrefab.name}");
                return;
            }
            randomValue -= part.dropChance;
        }
    }
}
