using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Player_Stats playerStats;
    [SerializeField] private Image healthBarForeground;

    void Update()
    {
        float health = (float)playerStats.health / playerStats.maxHealth; // Calculate health percentage.
        healthBarForeground.fillAmount = health;                          // Set the fill amount of the health bar.
    }
}
