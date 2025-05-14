using UnityEngine;
public class SupportPartFactory : TurretPartFactory
{
    public override TurretPart createPart()
    {
        return new GameObject("SupportPart").AddComponent<SupportPart>();    }
}