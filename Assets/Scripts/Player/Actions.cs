using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Actions : MonoBehaviour
{
    private Player_Stats playerStats;
    [SerializeField] private float turretPlacementOffset = 0.5f;
    [SerializeField] private float pushForce = 10f;
    [SerializeField] private float pushRadius = 3f;
    private int partCost; // New consolidated variable for all part costs
    private Animator anim;

    [SerializeField] private Turret_Constructor turretConstructor;
    public GameObject prefab;

    private GameObject closestTurret;

    [SerializeField] private List<GameObject> weaponList = new List<GameObject>();
    private int currentWeaponIndex = 0;
    private Gun_Stats gunStats;
    
    private float lastPickupTime = 0f;
    private const float PICKUP_COOLDOWN = 0.1f; // 100ms cooldown
    
    // SFX
    private SoundManager soundManager;
    
    // test variables for death mechanics -- remove when confirmed working
    [SerializeField] private float killRadius = 5f;

    private int costIncrease     = 5;

    public int baseSum = 0;
    public int shoulderSum = 0;
    public int weaponSum = 0;

    void Start()
    {
        playerStats = GetComponent<Player_Stats>();
        anim = GetComponent<Animator>();
        gunStats = weaponList[0].GetComponent<Gun_Stats>();
        soundManager = FindObjectOfType<SoundManager>();
    }

    void Update()
    {
        if (Busy())
        {
            return;
        }

        detectTurret();

        if (Input.anyKeyDown)
        {
            switch (Input.inputString.ToLower())
            {
                case "p":
                    Push();
                    break;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            // AudioSource.PlayClipAtPoint(buildSound, transform.position);
            Build();
        }
    }

    void Push()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, pushRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                Rigidbody rb = hitCollider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 direction = hitCollider.transform.position - transform.position;
                    rb.AddForce(direction.normalized * pushForce, ForceMode.Impulse);
                }
            }
        }
    }

    void Build()
    {
        UpdatePartCost();

        if (turretConstructor.currentTurret == null) // no currently selected turret, only action player can take is place a base.
        {
            if (prefab.name.Contains("Base"))
            {
                PlaceBase();
            }
        }
        else // a turret is currently selected, can place body or weapons.
        {
            if (prefab.name.Contains("Shoulder"))
            {
                if (turretConstructor.HasShoulderBase() && !turretConstructor.HasShoulder())
                {
                    AttachShoulder();
                }
            }
            else if (prefab.name.Contains("Weapon"))
            {
                if (turretConstructor.HasShoulder())
                {
                    AttachWeapon();
                }
            }
        }
    }

    void UpdatePartCost()
    {
        Component[] components = prefab.GetComponents<Component>();
        foreach (Component component in components)
        {
            if (component.GetType().Name.Contains("Stat"))
            {
                System.Reflection.FieldInfo costField = component.GetType().GetField("price");
                if (costField != null)
                {
                    partCost = (int)costField.GetValue(component);
                    return;
                }
            }
        }
        Debug.LogWarning("No 'Stat' component with 'cost' field found on the prefab.");
    }

    void PlaceBase()
    {
        if (playerStats.cash < partCost + baseSum)
        {
            Debug.Log("Not enough cash for base.");
            return;
        }
        else
        {
            playerStats.SpendCash(partCost + baseSum);
            baseSum += costIncrease;
            soundManager.Play("BuildTurret");
            anim.SetBool("Building", true);
        }

        Vector3 spawnLocation = transform.position + Vector3.up * turretPlacementOffset + transform.forward * 2;
        turretConstructor.StartNewTurret(prefab, spawnLocation);
    }

    void AttachShoulder()
    {
        if (playerStats.cash < partCost + shoulderSum)
        {
            Debug.Log("Not enough cash for shoulder.");
            return;
        }
        else
        {
            playerStats.SpendCash(partCost + shoulderSum);
            shoulderSum += costIncrease;
            soundManager.Play("BuildTurret");
            anim.SetBool("Building", true);
        }

        if (turretConstructor.currentTurret != null)
        {
            if (!turretConstructor.HasShoulder() && turretConstructor.HasShoulderBase())
            {
                turretConstructor.AttachPart(prefab);
                turretConstructor.PrintAvailableMountPoints();
            }
            else
            {
                Debug.LogWarning("Cannot attach shoulder. Turret needs a tower first.");
            }
        }
        else
        {
            Debug.LogWarning("No turret base to attach gun to. Place a base and a tower first.");
        }
    }

    void AttachWeapon()
    {
        if (playerStats.cash < partCost + weaponSum)
        {
            Debug.Log("Not enough cash for weapon.");
            return;
        }
        else
        {
            playerStats.SpendCash(partCost + weaponSum);
            weaponSum += costIncrease;
            soundManager.Play("BuildTurret");
            anim.SetBool("Building", true);
        }

        if (turretConstructor.currentTurret != null)
        {
            if (turretConstructor.HasTower() || turretConstructor.HasShoulder())
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, pushRadius);
                turretConstructor.AttachPart(prefab);
                turretConstructor.PrintAvailableMountPoints();
            }
            else
            {
                Debug.LogWarning("Cannot attach gun. Turret needs a tower first.");
            }
        }
        else
        {
            Debug.LogWarning("No turret base to attach gun to. Place a base and a tower first.");
        }
    }

    private void detectTurret()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 1f, transform.forward, 2f);

        float closestDistance = float.MaxValue;
        turretConstructor.currentTurret = null;

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Base"))
            {
                float distance = Vector3.Distance(transform.position, hit.collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    turretConstructor.currentTurret = hit.collider.gameObject;
                    turretConstructor.UpdateAvailableMountPoints();
                }
            }
        }
    }
    
    // pick up items
    void OnTriggerStay(Collider other)
    {
        if (IsPickupItem(other.tag) && Time.time - lastPickupTime > PICKUP_COOLDOWN)
        {
            InventoryItem item = new InventoryItem(other.gameObject.name);
            item.ApplyEffects(playerStats);
            Destroy(other.gameObject);
            lastPickupTime = Time.time;
        }
    }
    
    bool IsPickupItem(string tag)
    {
        return tag == "Pickup";
    }

    /// <summary>
    /// This method checks if the player is "busy":
    /// <list>
    /// -If they are taunting
    /// </list>
    /// </summary>
    /// <returns>true if busy, false if not</returns>
    bool Busy()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Taunt")                   //  Prevent movement if actively taunting.
            || anim.GetCurrentAnimatorStateInfo(0).IsName("Dead")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("Build");
    }
}