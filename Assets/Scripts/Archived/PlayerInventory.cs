using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class PlayerInventory : MonoBehaviour
{
    private Dictionary<ItemType, List<InventoryItem>> inventory = new Dictionary<ItemType, List<InventoryItem>>();

    public void AddItem(GameObject prefab)
    {
        InventoryItem newItem = new InventoryItem(prefab.name);
        
        if (!inventory.ContainsKey(newItem.Type))
        {
            inventory[newItem.Type] = new List<InventoryItem>();
        }
        
        var existingItem = inventory[newItem.Type].FirstOrDefault(item => item.PrefabName == newItem.PrefabName);
        if (existingItem != null)
        {
            existingItem.Quantity++;
        }
        else
        {
            inventory[newItem.Type].Add(newItem);
        }
    }

    public GameObject GetItem(ItemType type)
    {
        if (inventory.ContainsKey(type) && inventory[type].Count > 0)
        {
            var item = inventory[type][0];
            item.Quantity--;
            if (item.Quantity == 0)
            {
                inventory[type].RemoveAt(0);
            }
            return item.Prefab;
        }
        return null;
    }

    public bool HasItem(ItemType type)
    {
        return inventory.ContainsKey(type) && inventory[type].Count > 0;
    }

    public List<ItemType> GetAvailableItemTypes()
    {
        return inventory.Keys.Where(key => inventory[key].Count > 0).ToList();
    }
}