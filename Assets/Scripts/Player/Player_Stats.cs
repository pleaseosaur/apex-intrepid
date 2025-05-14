using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;

public class Player_Stats : MonoBehaviour, IHealthy
{
    [SerializeField] public int maxTurrets = 4;
    [SerializeField] public int health = 100;
    [SerializeField] public int maxHealth = 100;
    public int killCount = 0;
    public int cash = 100;
    public int currentCash { get; private set; }
    [SerializeField] public Dictionary<Material, int> currentMaterials = new Dictionary<Material, int>();

    public Dictionary<string, int> inventory = new Dictionary<string, int>();

    private Animator anim;
    private Rigidbody rb;
    private GameOverManager gameOverManager;
    
    public Image flashImage;
    public AnimationCurve flashCurve;
    public float flashDuration = 0.5f;
    public UnityEngine.Material flashMaterial;

    private bool shieldActive = false;
    private float baseSpeed = 5f;
    private float currentSpeed;
    
    // dynamic HUD elements
    public FadingText healthChangeText;
    public FadingText cashChangeText;
    private int previousCash;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        gameOverManager = FindObjectOfType<GameOverManager>();
        flashMaterial = new UnityEngine.Material(flashMaterial);
        currentSpeed = baseSpeed;
        currentCash = cash;
        previousCash = cash;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (health <= 0)
        {
            anim.SetBool("Dead", true);
            gameOverManager.TriggerGameOver();
        }
    }

    public void Damage(int amount, Vector3 force)
    {
        if (shieldActive)
        {
            shieldActive = false;
            Debug.Log("Shield absorbed damage");
            return;
        }
        
        health -= amount;
        health = Mathf.Clamp(health, 0, maxHealth); // Ensure health stays within bounds

        rb.AddForce(force, ForceMode.Impulse);

        StartCoroutine(FlashEffect());
    }
    
    // -- flash effect not working --
    IEnumerator FlashEffect()
    {
        if (flashMaterial == null) yield break;

        float elapsedTime = 0f;

        while (elapsedTime < flashDuration)
        {
            elapsedTime += Time.deltaTime;
            float flashAmount = flashCurve.Evaluate(elapsedTime / flashDuration);
            flashMaterial.SetFloat("_FlashAmount", flashAmount);
            yield return null;
        }

        flashMaterial.SetFloat("_FlashAmount", 0);
    }

    public void doDamage(float damage)
    {
        Debug.Log($"Trying to deal {damage} amount of damage");
        Damage((int) damage, Vector3.zero);
    }

    public void healHealth(float health)
    {
        AddHealth((int) health);
        Debug.Log($"Player health increased by {health}");
    }

    public void setHealth(float health)
    {
        this.health = Mathf.Clamp((int)health, 0, maxHealth);
    }
    
    public void AddHealth(int amount)
    {
        setHealth(health + amount);
        if (healthChangeText != null)
        {
            healthChangeText.ShowText($"+{amount} HP");
        }
    }
    
    public void AddCash(int amount)
    {
        cash += amount;
        currentCash = cash;
        UpdateCashChangeText();
    }
    
    public void SpendCash(int amount)
    {
        cash -= amount;
        currentCash = cash;
        UpdateCashChangeText();
    }
    
    public void IncreaseSpeed(float multiplier)
    {
        currentSpeed *= multiplier;
    }
    
    public void ActivateShield()
    {
        shieldActive = true;
    }
    
    // double speed for 5 seconds
    public void ApplySpeedBoost()
    {
        StartCoroutine(SpeedBoost());
    }
    
    IEnumerator SpeedBoost()
    {
        IncreaseSpeed(2f);
        yield return new WaitForSeconds(5f);
        IncreaseSpeed(0.5f);
    }
    
    public void ApplyItemEffects(InventoryItem item)
    {
        item.ApplyEffects(this);
    }
    
    private void UpdateCashChangeText()
    {
        if (cashChangeText != null)
        {
            int difference = cash - previousCash;
            string prefix = difference >= 0 ? "+" : "-";
            cashChangeText.ShowText($"{prefix}${Mathf.Abs(difference)}");
        }
        previousCash = cash;
    }
}
