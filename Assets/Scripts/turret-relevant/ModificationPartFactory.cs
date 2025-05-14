using UnityEngine;
public class ModificationPartFactory : TurretPartFactory
{
    public override TurretPart createPart()
    {
        return new GameObject("ModificationPart").AddComponent<ModificationPart>();
    }
} 