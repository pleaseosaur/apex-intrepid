using UnityEngine;
using System.Collections.Generic;

public class InventoryCategoryPanel : MonoBehaviour
{
    public Transform previewParent;
    public List<GameObject> itemPrefabs = new List<GameObject>();

    private int selectedIndex = -1;
    private GameObject currentPreviewObject;

    void Start()
    {
        UpdateDisplay();
    }

    public void CycleSelection()
    {
        selectedIndex = (selectedIndex + 1) % (itemPrefabs.Count + 1); // +1 for "None" option
        UpdateDisplay();
        Debug.Log($"Cycled selection. New index: {selectedIndex}");
    }

    private void UpdateDisplay()
    {
        if (currentPreviewObject != null)
        {
            Destroy(currentPreviewObject);
            Debug.Log("Destroyed previous preview object");
        }

        if (selectedIndex >= 0 && selectedIndex < itemPrefabs.Count)
        {
            GameObject selectedItem = itemPrefabs[selectedIndex];
            currentPreviewObject = Instantiate(selectedItem, previewParent);
            currentPreviewObject.transform.localPosition = Vector3.zero;
            currentPreviewObject.transform.localRotation = Quaternion.identity;
            
            AdjustPreviewScale(currentPreviewObject);
            DisableComponents(currentPreviewObject);

            Debug.Log($"Displayed item: {selectedItem.name}");
        }
        else
        {
            Debug.Log("No item selected (None)");
        }
    }

    private void AdjustPreviewScale(GameObject previewObject)
    {
        // Adjust scale to fit the preview area
        Bounds bounds = CalculateBounds(previewObject);
        float maxDimension = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);
        float desiredSize = 1f; // Adjust this value to fit your UI
        previewObject.transform.localScale *= desiredSize / maxDimension;

        Debug.Log($"Adjusted scale: {previewObject.transform.localScale}");
    }

    private Bounds CalculateBounds(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return new Bounds(obj.transform.position, Vector3.one);

        Bounds bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }
        return bounds;
    }

    private void DisableComponents(GameObject obj)
    {
        // Disable any components that might interfere with the preview
        MonoBehaviour[] scripts = obj.GetComponentsInChildren<MonoBehaviour>();
        foreach (var script in scripts)
        {
            script.enabled = false;
        }

        Collider[] colliders = obj.GetComponentsInChildren<Collider>();
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }

        Debug.Log("Disabled components on preview object");
    }

    public GameObject GetSelectedItem()
    {
        if (selectedIndex >= 0 && selectedIndex < itemPrefabs.Count)
        {
            return itemPrefabs[selectedIndex];
        }
        return null;
    }
}