using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Constructor : MonoBehaviour
{
    public GameObject currentTurret;
    private Dictionary<string, List<Transform>> availableMountPoints = new Dictionary<string, List<Transform>>();

    /// <summary>
    /// Creates a new turret and sets current turret to it.
    /// </summary>
    /// <param name="basePart"></param>
    /// <param name="position"></param>
    public void StartNewTurret(GameObject basePart, Vector3 position)
    {
        if (basePart.name.StartsWith("Base_"))
        {
            GameObject instance = Instantiate(basePart, position, Quaternion.identity);
            currentTurret = instance;
            Debug.Log("New turret base placed at player's location");
            UpdateAvailableMountPoints();
            return;
        }
        Debug.LogWarning("Cannot start a new turret with a non-base part.");
    }

    /// <summary>
    /// Adds parts to the current turret. The part will go to the first available, empty slot of its mount type.
    /// </summary>
    /// <param name="partPrefab"></param>
    public void AttachPart(GameObject partPrefab)
    {
        if (currentTurret == null)
        {
            Debug.LogWarning("No active turret to attach part to.");
            return;
        }

        string partType = GetPartType(partPrefab.name);
        string mountPointType = GetMountPointType(partType);

        if (availableMountPoints.TryGetValue(mountPointType, out List<Transform> relevantMountPoints) && relevantMountPoints.Count > 0)
        {
            Transform mountPoint = relevantMountPoints[0]; // Use the first available mount point
            
            // Instantiate the part prefab
            Debug.Log("Instantiating part prefab");
            GameObject partInstance = Instantiate(partPrefab, mountPoint.position, mountPoint.rotation, mountPoint);
            
            // Set local position and rotation
            partInstance.transform.localPosition = Vector3.zero;
            partInstance.transform.localRotation = Quaternion.identity;

            relevantMountPoints.RemoveAt(0); // Remove the used mount point
            

            UpdateAvailableMountPoints();
            Debug.Log($"Successfully attached {partType} to the turret.");
            return;
        }

        Debug.LogWarning($"No available mount point for {partType}");
        return;
    }

    /// <summary>
    /// Updates available mount points. Clears them before refreshing.
    /// </summary>
    public void UpdateAvailableMountPoints()
    {
        availableMountPoints.Clear();
        RecursivelyFindMountPoints(currentTurret.transform);
    }

    /// <summary>
    /// Recurses through each child of the given transform and finds each mount point that is not occupied.
    /// </summary>
    /// <param name="parent"></param>
    private void RecursivelyFindMountPoints(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.name.StartsWith("Mount_"))
            {
                // Check if the mount point has no children
                if (child.childCount == 0)
                {
                    string mountType = child.name;
                    if (!availableMountPoints.ContainsKey(mountType))
                    {
                        availableMountPoints[mountType] = new List<Transform>();
                    }
                    availableMountPoints[mountType].Add(child);
                }
            }

            RecursivelyFindMountPoints(child);
        }
    }

    /// <summary>
    /// Strips a GameObject's name.
    /// </summary>
    /// <param name="partName"></param>
    /// <returns></returns>
    private string GetPartType(string partName)
    {
        if (partName.Contains("Base")) return "Base";
        if (partName.Contains("Tower")) return "Tower";
        if (partName.Contains("Shoulder")) return "Shoulder";
        if (partName.Contains("Weapon")) return "Weapon";

        if (partName.Contains("Backpack")) return "Backpack";
        if (partName.Contains("Cockpit")) return "Cockpit";
        if (partName.Contains("Props")) return "Prop";
        return "Unknown";
    }

    /// <summary>
    /// Accepts a part type (returned by GetPartType) and returns the type of mount it can go on.
    /// </summary>
    /// <param name="partType"></param>
    /// <returns></returns>
    private string GetMountPointType(string partType)
    {
        switch (partType)
        {
            case "Tower": return "Mount_Top";
            case "Shoulder": return "Mount_Top";
            case "Weapon": return "Mount_Weapon";
            default: return "Mount_Top";
        }
    }

    /// <summary>
    /// Iterates through available mount points and prints to the log.
    /// </summary>
    public void PrintAvailableMountPoints()
    {
        foreach (var kvp in availableMountPoints)
        {
            Debug.Log($"Mount type: {kvp.Key}, Count: {kvp.Value.Count}");
        }
    }

    /// <summary>
    /// Determines whether or not the current turret has a tower.
    /// </summary>
    /// <returns>true or false</returns>
    public bool HasTower()
    {
        return Has("Tower");
    }

    /// <summary>
    /// Determines whether or not the current turret has a shoulder.
    /// </summary>
    /// <returns>true or false</returns>
    public bool HasShoulder()
    {
        return Has("Shoulder");
    }

    /// <summary>
    /// Determines whether or not the current turret has a Weapon.
    /// </summary>
    /// <returns>true or false</returns>
    public bool HasWeapon()
    {
        return Has("Weapon");
    }

    /// <summary>
    /// Determines whether or not the current turret has a Tower compatable Base.
    /// </summary>
    /// <returns>true or false</returns>
    public bool HasTowerBase()
    {
        return Has("Base_Tower");
    }

    /// <summary>
    /// Determines whether or not the current turret has a Shoulder compatable Base.
    /// </summary>
    /// <returns>true or false</returns>

    public bool HasShoulderBase()
    {
        return Has("Base_Turret");
    }

    /// <summary>
    /// Determines whether or not the current turret has a part of any given type.
    /// </summary>
    /// <param name="part"></param>
    /// <returns>true or false</returns>
    private bool Has(string part)
    {
        if (currentTurret == null)
        {
            return false;
        }

        // Search through all child objects of the current turret
        foreach (Transform child in currentTurret.GetComponentsInChildren<Transform>())
        {
            if (child.name.Contains(part))
            {
                return true;
            }
        }

        return false;
    }
}
