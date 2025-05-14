using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ControlMap : MonoBehaviour
{
    public Transform contentParent;
    public GameObject rowTemplate;

    private List<KeyValuePair<string, string>> controls = new List<KeyValuePair<string, string>>
    {
        new KeyValuePair<string, string>("W", "Move Forward"),
        new KeyValuePair<string, string>("A", "Move Left"),
        new KeyValuePair<string, string>("S", "Move Backward"),
        new KeyValuePair<string, string>("D", "Move Right"),
        new KeyValuePair<string, string>("Space", "Jump"),
        new KeyValuePair<string, string>("Left Click", "Attack"),
        new KeyValuePair<string, string>("Right Click", "Block"),
        new KeyValuePair<string, string>("E", "Interact"),
        new KeyValuePair<string, string>("Q", "Use Item"),
        new KeyValuePair<string, string>("Tab", "Open Inventory")
    };

    void Start()
    {
        GenerateTable();
    }

    void GenerateTable()
    {
        foreach (var map in controls)
        {
            GameObject newRow = Instantiate(rowTemplate, contentParent);
            newRow.SetActive(true);
            TextMeshProUGUI[] texts = newRow.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = map.Key;
            texts[1].text = map.Value;
        }
    }
}