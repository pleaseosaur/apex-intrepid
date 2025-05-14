using UnityEngine;
using TMPro;

public class CashDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cashText;
    private GameObject player;
    private Player_Stats playerStats;

    private void Start()
    {
        player = GameObject.Find("Player");
        
        if (cashText == null)
        {
            cashText = transform.Find("Cash").GetComponent<TextMeshProUGUI>();
        }

        if (playerStats == null)
        {
            playerStats = player.GetComponent<Player_Stats>();
        }

        UpdateCashDisplay();
    }

    private void Update()
    {
        UpdateCashDisplay();
    }

    private void UpdateCashDisplay()
    {
        if (cashText != null && playerStats != null)
        {
            cashText.text = playerStats.currentCash.ToString();
        }
    }
}