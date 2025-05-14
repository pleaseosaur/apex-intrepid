using System.Collections.Generic;
using UnityEngine;
// TurretBuilder.cs
public class TurretBuilder
{
    private Turret turret;

    public TurretBuilder()
    {
        turret = new GameObject("Turret").AddComponent<Turret>();
        turret.parts = new List<TurretPart>();
        //turret.materialBuildRequirements = new Dictionary<Material, int>();
        //turret.materialRepairRequirements = new Dictionary<Material, int>();
    }

    public TurretBuilder SetCreator(GameObject creator)
    {
        turret.creator = creator;
        return this;
    }

    public TurretBuilder SetPlayerStats(Player_Stats stats)
    {
        turret.stats = stats;
        return this;
    }

    public TurretBuilder SetName(string name)
    {
        turret.turretName = name;
        return this;
    }

    public TurretBuilder SetBase(TurretBase turretBase)
    {
        turret.turretBase = turretBase;
        return this;
    }

    public TurretBuilder AddPart(TurretPart part)
    {
        turret.parts.Add(part);
        part.InitializePart();
        return this;
    }

    //public TurretBuilder AddMaterialBuildRequirement(Material material, int amount)
    //{
    //    if (turret.materialBuildRequirements.ContainsKey(material))
    //    {
    //        turret.materialBuildRequirements[material] += amount;
    //    }
    //    else
    //    {
    //        turret.materialBuildRequirements[material] = amount;
    //    }
    //    return this;
    //}

    //public TurretBuilder AddMaterialRepairRequirement(Material material, int amount)
    //{
    //    if (turret.materialRepairRequirements.ContainsKey(material))
    //    {
    //        turret.materialRepairRequirements[material] += amount;
    //    }
    //    else
    //    {
    //        turret.materialRepairRequirements[material] = amount;
    //    }
    //    return this;
    //}

    public Turret Build()
    {
        turret.InitializeTurret(turret.turretName, turret.parts, turret.turretBase);
        return turret;
    }
}
