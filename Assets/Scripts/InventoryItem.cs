using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public enum ItemType
{
    Prop,
    Heart,
    Speed,
    Shield,
    Star,
    Thunder,
    Dollar,
    Key,
    Upgrade
}

public class InventoryItem
{
    public GameObject Prefab { get; private set; }
    public string Name => Prefab.name;
    public string PrefabName { get; private set; }
    public ItemType Type { get; private set; }
    public int Quantity { get; set; }
    
    private SoundManager soundManager = GameObject.FindObjectOfType<SoundManager>();
    
    public InventoryItem(string prefabName)
    {
        PrefabName = prefabName;
        Type = InferItemType(prefabName);
        Quantity = 1;
    }

    private ItemType InferItemType(string prefabName)
    {

        if (prefabName.StartsWith("Heart"))
        {
            return ItemType.Heart;
        }
        if (prefabName.StartsWith("Speed"))
        {
            return ItemType.Speed;
        }
        if (prefabName.StartsWith("Upgrade"))
        {
            return ItemType.Upgrade;
        }
        if (prefabName.StartsWith("Shield"))
        {
            return ItemType.Shield;
        }
        if (prefabName.StartsWith("Star"))
        {
            return ItemType.Star;
        }
        if (prefabName.StartsWith("Thunder"))
        {
            return ItemType.Thunder;
        }
        if (prefabName.StartsWith("Dollar"))
        {
            return ItemType.Dollar;
        }
        if (prefabName.StartsWith("Hedra"))
        {
            return ItemType.Key;
        }
        Debug.LogWarning($"Unknown item type for prefab: {prefabName}");
        return ItemType.Prop; // Default to Prop if unknown
    }

    public void ApplyEffects(Player_Stats playerStats)
    {
        switch (Type)
        {
            case ItemType.Heart:
                playerStats.AddHealth(5);
                soundManager.Play("HeartPickup");
                break;
            case ItemType.Speed:
                playerStats.IncreaseSpeed(1.2f);
                soundManager.Play("SpeedPickup");
                break;
            case ItemType.Upgrade:
                ApplyUpgradeEffect();
                soundManager.Play("TurretUpgrade");
                break;
            case ItemType.Shield:
                playerStats.ActivateShield();
                soundManager.Play("ShieldPickup");
                break;
            case ItemType.Star:
                playerStats.ApplySpeedBoost();
                soundManager.Play("SpeedBoost");
                break;
            case ItemType.Thunder:
                ApplyThunderEffect();
                soundManager.Play("EnemySlowdown");
                break;
            case ItemType.Dollar:
                var value = Random.Range(1, 51);
                playerStats.AddCash(value);
                soundManager.Play("CashPickup");
                break;
            case ItemType.Key:
                ApplyKeyEffect();
                soundManager.Play("KeyPickup");
                break;
            default:
                Debug.LogWarning($"No effect defined for item type: {Type}");
                break;
        }
    }

    private void ApplyUpgradeEffect()
    {
        var bazookaStats = GameObject.FindObjectsOfType<Bazooka_Gun_Stats>();

        foreach (var gun in bazookaStats)
        {
            gun.damage++;
            Debug.Log($"Incremented Damage for {gun.gameObject.name}. New value: {gun.damage}");
        }
    }

    private void ApplyThunderEffect()
    {
        Enemy_Stats[] enemies = GameObject.FindObjectsOfType<Enemy_Stats>();
        foreach (var enemy in enemies)
        {
            enemy.ApplySlowEffect(0.5f, 5f);
        }
        Debug.Log("Thunder effect applied to all enemies");
    }
    
    private void ApplyKeyEffect()
    {
        Debug.Log("Key effect applied");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level_2");
    }
}