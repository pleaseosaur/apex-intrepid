using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class HUDMenu : MonoBehaviour
{
    [System.Serializable]
    public class Item
    {
        public string name;
        public GameObject prefab;
    }

    [System.Serializable]
    public class Category
    {
        public string name;
        public List<Item> items;
    }
    
    
    private Player_Stats playerStats;

    public List<Category> categories;
    public TextMeshProUGUI leftCategoryText, centerCategoryText, rightCategoryText;
    public TextMeshProUGUI topItemText, middleItemText, bottomItemText;
    public GameObject itemPanel; // Panel for top, middle, and bottom item text
    public GameObject pricePanel;
    public TextMeshProUGUI priceText;
    
    private int currentCategoryIndex = 0;
    private int currentItemIndex = 0;
    private bool inCategoryView = true;
    
    private float scrollSpeed = 0.5f;
    private float scrollThreshold = 0.075f;
    private float scrollAccumulator = 0f;

    private Actions actions;

    void Start()
    {
        actions = GameObject.FindGameObjectWithTag("Player").GetComponent<Actions>();
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Stats>();
        actions.prefab = categories[currentCategoryIndex].items[currentItemIndex].prefab;
        UpdateCategoryDisplay();
        itemPanel.SetActive(false);
        UpdatePriceDisplay();
        pricePanel.SetActive(false);
    }

    void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        scrollAccumulator += scrollInput * scrollSpeed;

        if (Mathf.Abs(scrollAccumulator) >= scrollThreshold)
        {
            int scrollDirection = (int)Mathf.Sign(scrollAccumulator);

            if (inCategoryView)
            {
                ScrollCategories(scrollDirection);
            }
            else
            {
                ScrollItems(scrollDirection);
            }

            // Reset accumulator, preserving remainder
            scrollAccumulator -= scrollThreshold * scrollDirection;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetMouseButtonDown(2))
        {
            ToggleView();
            actions.prefab = categories[currentCategoryIndex].items[currentItemIndex].prefab;
        }
    }

    void ScrollCategories(int direction)
    {
        currentCategoryIndex = (currentCategoryIndex + direction + categories.Count) % categories.Count;
        UpdateCategoryDisplay();
    }

    void ScrollItems(int direction)
    {
        Category currentCategory = categories[currentCategoryIndex];
        currentItemIndex = (currentItemIndex + direction + currentCategory.items.Count) % currentCategory.items.Count;
        UpdateItemDisplay();
        UpdatePriceDisplay();
    }

    void UpdateCategoryDisplay()
    {
        int leftIndex = (currentCategoryIndex - 1 + categories.Count) % categories.Count;
        int rightIndex = (currentCategoryIndex + 1) % categories.Count;

        leftCategoryText.text = categories[leftIndex].name;
        centerCategoryText.text = categories[currentCategoryIndex].name;
        rightCategoryText.text = categories[rightIndex].name;
    }

    void UpdateItemDisplay()
    {
        Category currentCategory = categories[currentCategoryIndex];
        int topIndex = (currentItemIndex - 1 + currentCategory.items.Count) % currentCategory.items.Count;
        int bottomIndex = (currentItemIndex + 1) % currentCategory.items.Count;

        topItemText.text = currentCategory.items[topIndex].name;
        middleItemText.text = currentCategory.items[currentItemIndex].name;
        bottomItemText.text = currentCategory.items[bottomIndex].name;
        
        UpdatePriceDisplay();
    }

    // void UpdatePriceDisplay()
    // {
    //     if (!inCategoryView)
    //     {
    //         GameObject currentPrefab = GetCurrentItemPrefab();
    //         Gun_Stats gunStats = currentPrefab.GetComponent<Gun_Stats>();
    //         priceText.text = gunStats.price.ToString();
    //     }
    // }

    void UpdatePriceDisplay()
    {
        if (!inCategoryView)
        {
            GameObject currentPrefab = GetCurrentItemPrefab();

            switch (categories[currentCategoryIndex].name)
            {
                case "Weapons":
                    if (categories[currentCategoryIndex].items[currentItemIndex].name == "Bazooka")
                    {
                        Bazooka_Gun_Stats bazookaStats = currentPrefab.GetComponent<Bazooka_Gun_Stats>();
                        priceText.text = (bazookaStats.price + actions.weaponSum).ToString();
                    }
                    else
                    {
                        Gun_Stats gunStats = currentPrefab.GetComponent<Gun_Stats>();
                        priceText.text = (gunStats.price + actions.weaponSum).ToString();
                    }
                    break;
                case "Shoulders":
                    Body_Stats bodyStats = currentPrefab.GetComponent<Body_Stats>();
                    priceText.text = (bodyStats.price + actions.shoulderSum).ToString();
                    break;
                case "Bases":
                    Base_Stats baseStats = currentPrefab.GetComponent<Base_Stats>();
                    priceText.text = (baseStats.price + actions.baseSum).ToString();
                    break;
                default:
                    priceText.text = "ERR0R";
                    break;
            }
        }
    }

    void ToggleView()
    {
        inCategoryView = !inCategoryView;
        itemPanel.SetActive(!inCategoryView);
        pricePanel.SetActive(!inCategoryView);
        centerCategoryText.gameObject.SetActive(inCategoryView);

        if (!inCategoryView)
        {
            currentItemIndex = 0;
            UpdateItemDisplay();
        }
        else
        {
            UpdateCategoryDisplay();
        }
        UpdatePriceDisplay();
    }

    public string GetCurrentSelectionName()
    {
        if (inCategoryView)
            return categories[currentCategoryIndex].name;
        else
            return categories[currentCategoryIndex].items[currentItemIndex].name;
    }

    public GameObject GetCurrentItemPrefab()
    {
        if (!inCategoryView)
            return categories[currentCategoryIndex].items[currentItemIndex].prefab;
        return null;
    }
}