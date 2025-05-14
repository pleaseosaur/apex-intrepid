 using UnityEngine;
 using System.Collections.Generic;
using Unity.VisualScripting;
using System;
/// <summary>
/// Abstract Turret Part
/// </summary>
public abstract class TurretPart : MonoBehaviour
 {
    /*
    *
    *   FIELDS
    *
    */

    public GameObject turretPartPrefab;


    private string _partName;
     /// <summary>
     /// Name of the part
     /// </summary>
     [Serialize] public string PartName {
        get { return _partName; }
        set { _partName = value; }
     }
     private int durability;
     /// <summary>
     /// Durability of the part. TODO: Maybe more complicated damage system? Disable individual parts when broken?
     /// </summary>
     public int Durability {
        get { return durability; }
        set { durability = value; }
     }
     /// <summary>
     /// Material requirements to initially build the turret for each material.
     /// </summary>
     public Dictionary<Material, int> MaterialBuildRequirements {get; private set;}

    /// <summary>
    /// Material requirements to repair the turret for each material.
    /// </summary>
    public Dictionary<Material, int> MaterialRepairRequirements {get; private set;}
     /// <summary>
     /// Used if a part is made up of smaller parts. <para /> 
     /// Not intended to be used right now right now, but could add fun expansion later on. 
     /// </summary>
     public List<TurretPart> subParts;


    /*
     *
     * METHODS
     *
     */

    /// <summary>
    /// Initializes the part. Should be implemented for all parts
    /// </summary>
    public abstract TurretPart InitializePart();


    /// <summary>
    /// Creates a part at a position. Used from turret.cs when adding a new part to a turret.
    /// </summary>
    public void InstantiatePart() {
        
    }


    /// <summary>
    /// Get the part name of a part
    /// </summary>
    /// <returns>A string containing whatever the part name is set to</returns>
    public String getPartName(){
        return _partName;
    }



    /// <summary>
    /// Used to add a material requirement to the build requirement list. 
    /// </summary>
    /// <param name="material">Material to add to list of reqs</param>
    /// <param name="amount">Number of that material to be required</param>
    public void AddMaterialBuildRequirement(Material material, int amount)
    {
        if (MaterialBuildRequirements.ContainsKey(material))
        {
            MaterialBuildRequirements[material] += amount;
        }
        else
        {
            MaterialBuildRequirements.Add(material, amount);
        }
    }
     /// <summary>
    /// Used to add a material requirement to the repair requirement list. 
    /// </summary>
    /// <param name="material">Material to add to list of reqs</param>
    /// <param name="amount">Number of that material to be required</param>
    public void AddMaterialRepairRequirement(Material material, int amount)
    {
        if (MaterialRepairRequirements.ContainsKey(material))
        {
            MaterialRepairRequirements[material] += amount;
        }
        else
        {
            MaterialRepairRequirements.Add(material, amount);
        }
    }
 }
