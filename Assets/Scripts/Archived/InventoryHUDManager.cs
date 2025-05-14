using UnityEngine;
using System.Collections.Generic;

public class InventoryHUDManager : MonoBehaviour
{
    public List<InventoryCategoryPanel> categoryPanels;

    void Update()
    {
        for (int i = 0; i < categoryPanels.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                Debug.Log($"Key {i + 1} pressed. Cycling category panel {i} ({categoryPanels[i].name})");
                categoryPanels[i].CycleSelection();
            }
        }
    }

    public GameObject GetSelectedItem()
    {
        foreach (var panel in categoryPanels)
        {
            GameObject selectedItem = panel.GetSelectedItem();
            if (selectedItem != null)
            {
                return selectedItem;
            }
        }
        return null;
    }
}