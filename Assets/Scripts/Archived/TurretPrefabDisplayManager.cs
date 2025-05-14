using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TurretPrefabDisplayManager : MonoBehaviour
{
    [System.Serializable]
    public class PrefabDisplay
    {
        public RawImage displayImage;
        public Camera renderCamera;
        public Transform prefabSpawnPoint;
        public List<GameObject> prefabs;
        public int currentPrefabIndex = -1;
        public GameObject currentInstance;
    }

    public List<PrefabDisplay> prefabDisplays;
    public int selectedDisplayIndex = -1;

    void Start()
    {
        InitializeDisplays();
    }

    void Update()
    {
        HandleInput();
    }

    void InitializeDisplays()
    {
        foreach (var display in prefabDisplays)
        {
            // Create a new Render Texture
            RenderTexture rt = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
            rt.Create();

            // Assign the Render Texture to the camera
            display.renderCamera.targetTexture = rt;

            // Assign the Render Texture to the RawImage
            display.displayImage.texture = rt;

            // Initialize with the first prefab
            CyclePrefab(display, true);
        }
    }

    void HandleInput()
    {
        for (int i = 0; i < prefabDisplays.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                if (selectedDisplayIndex == i)
                {
                    CyclePrefab(prefabDisplays[i]);
                }
                else
                {
                    selectedDisplayIndex = i;
                    Debug.Log($"Selected display {i}");
                }
            }
        }
    }

    void CyclePrefab(PrefabDisplay display, bool initialize = false)
    {
        if (display.prefabs.Count == 0) return;

        // Destroy the current instance if it exists
        if (display.currentInstance != null)
        {
            Destroy(display.currentInstance);
        }

        // Move to the next prefab or initialize
        if (initialize)
        {
            display.currentPrefabIndex = 0;
        }
        else
        {
            display.currentPrefabIndex = (display.currentPrefabIndex + 1) % display.prefabs.Count;
        }

        // Instantiate the new prefab
        GameObject prefabToSpawn = display.prefabs[display.currentPrefabIndex];
        display.currentInstance = Instantiate(prefabToSpawn, display.prefabSpawnPoint.position, display.prefabSpawnPoint.rotation);

        Debug.Log($"Cycled to prefab {display.currentPrefabIndex} for display {prefabDisplays.IndexOf(display)}");
    }
}