using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Enemy_Stats : MonoBehaviour
{
    [SerializeField] public int currentHealth = 100;
    [SerializeField] public int maxHealth = 100;
    [SerializeField] public int minHealth = 0;
    
    public UnityEvent OnDeath;
    public UnityEvent OnDamage;
    
    private LootManager lootManager;
    private NavMeshAgent agent;
    private float baseSpeed;
    private Player_Stats playerStats;
    
    // SFX
    private SoundManager soundManager;
    
    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }
    
    private void Awake()
    {
        lootManager = FindObjectOfType<LootManager>();
        agent = GetComponent<NavMeshAgent>();
        baseSpeed = agent.speed;

        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Stats>();
    }

    private void OnEnable()
    {
        ResetHealth();
    }
    
    public void Damage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, minHealth, maxHealth);
        OnDamage.Invoke();
        if (currentHealth <= minHealth)
        {
            Death();
            soundManager.Play("EnemyDeath");
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }

    private void Death()
    {
        OnDeath.Invoke();
        if (lootManager != null)
        {
            lootManager.DropLoot(transform.position);
        }
        // Instead of destroying, we'll return to pool
        Enemy_Spawner.Instance.ReturnEnemyToPool(gameObject);
        playerStats.killCount++;
        Debug.Log($"Kills: {playerStats.killCount}");
        
    }
    
    public void ApplySlowEffect(float multiplier, float duration)
    {
        StartCoroutine(ApplySpeedModifier(multiplier, duration));
    }
    
    private IEnumerator ApplySpeedModifier(float multiplier, float duration)
    {
        if (agent != null)
        {
            agent.speed *= multiplier;
            yield return new WaitForSeconds(duration);
            agent.speed = baseSpeed;
        }
    }
}