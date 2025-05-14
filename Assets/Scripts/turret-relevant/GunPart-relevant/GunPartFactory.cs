using UnityEngine;

public class GunPartFactory : TurretPartFactory
{
    public override TurretPart createPart()
    {
        return new GameObject("GunPart").AddComponent<GunPart>();
    }
}