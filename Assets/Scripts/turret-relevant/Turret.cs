using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Turret : MonoBehaviour
{
    /*
    *
    *   FIELDS
    *
    */

    public GameObject creator; // Generally will be the player
    public Player_Stats stats; // TODO: Get this through the player somehow
    public string turretName;
    public TurretBase turretBase;
    public List<TurretPart> parts;
    public LayerMask enemyLayer;
    [SerializeField] private TurretStats turretStats; 
    private Transform target;
    private Dictionary<Material, int> materialBuildRequirements;
    private Dictionary<Material, int> materialRepairRequirements;

    void Start()
    {
        turretStats = new TurretStats();
        // TODO: these are temporary
        turretStats.rotationSpeed = 10f;
        turretStats.detectionRadius = 100f;
        Debug.Log($"Enemy layer: {enemyLayer.value}");
    }

    public void InitializeTurret(string name, List<TurretPart> partList, TurretBase tBase)
    {
        turretName = name;
        parts = partList;
        this.turretBase = tBase;

        foreach (TurretPart part in parts)
        {
            part.InitializePart();
        }

        if (CheckMaterialBuildRequirements())
        {
            // TODO: Instantiate turret and stuff here
        }
        else
        {
            Debug.Log("Not enough materials to build the turret.");
        }
    }

    private void InitializeTurret(List<TurretPart> partList)
    {
        this.turretName = "";
        foreach (TurretPart part in partList)
        {
            part.InitializePart();
            turretName += part.PartName;
        }
        this.turretName += " Turret";
        parts = partList;

        if (CheckMaterialBuildRequirements())
        {
            BuildTurret(turretName, turretBase, parts);
        }
        else
        {
            Debug.Log("Not enough materials to build the turret.");
        }
    }

    void Update()
    {
        FindTarget();
        if (target != null)
        {
            Debug.Log("Trying to fire at target");
            FireAtTarget();
        }
    }

    // TODO: Figure this out more. Maybe refactor alongsize InitializeTurret() to better seperate behaviour
    public void BuildTurret(string name, TurretBase tBase, List<TurretPart> partList)
    {
        InitializeTurret(name, partList, tBase);
        Debug.Log("Turret built successfully.");
    }

    public bool CheckMaterialBuildRequirements()
    {
        // Logic to check if materials are sufficient
        foreach (var requirement in materialBuildRequirements)
        {
            // If the current material count for a given material is less than the required number of material, return false
            if (stats.currentMaterials[requirement.Key] < requirement.Value)
            {
                return false;
            }
        }
        return true;
    }

    private void FindTarget()
    {
        Debug.Log("Finding target...");
        Collider[] hits = Physics.OverlapSphere(transform.position, turretStats.detectionRadius, enemyLayer);
        Debug.Log($"Number of hits: {hits.Length}");
        // TODO: Create an ordering method so that we can order based on different priorities.
        var orderedByProximity = hits.OrderBy(c => (transform.position - c.transform.position).sqrMagnitude).ToArray();
        foreach (var hit in hits)
        {
            Debug.Log($"Hit: {hit.name}, Layer: {LayerMask.LayerToName(hit.gameObject.layer)}");
        }

        if (hits.Length > 0)
        {
            target = orderedByProximity[0].transform;
            Debug.Log($"Target found: {target.name}");
            foreach (GunPart gp in parts)
            {
                // TODO: Constrain looking up and down range, and rotate around the correct y axis. Probably rotate around y axis of gunpart and not body?
                Vector3 direction = (target.position - gp.transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                gp.transform.rotation = Quaternion.Slerp(gp.transform.rotation, lookRotation, Time.deltaTime * turretStats.rotationSpeed);
            }
        }
        else
        {
            target = null;
            Debug.Log("No target found");
        }
    }

    private void FireAtTarget()
    {
        // TODO: (low priority) Make this work better for any possible composite parts that contain a gunpart
        foreach (var iterPart in parts)
        {
            if (iterPart is GunPart)
            {
                GunPart part = iterPart as GunPart;

                    Debug.Log("Attempting to fire at target");
                    part.Fire(target);


            }
            else
            {
                Debug.Log("Part is not a gunpart");
            }
        }
    }

    public string GetStatsAsString()
    {
        string stats = $"Name: {turretName}\n";
        stats += $"Detection Radius: {turretStats.detectionRadius}\n";
        stats += $"Parts:\n";
        foreach (var part in parts)
        {
            stats += $"- {part.PartName}\n";
        }
        return stats;
    }
}
