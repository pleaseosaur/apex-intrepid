using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Stats : MonoBehaviour
{
    /// <summary>
    /// Damage per individual shot.
    /// </summary>
    public float damage;

    /// <summary>
    /// The number of individual shots to fire in a burst.
    /// </summary>
    public int burstCount;
    /// <summary>
    /// The time between shots during a burst.
    /// </summary>
    public float burstSpeed;

    /// <summary>
    /// The speed of fired projectiles.
    /// </summary>
    public float projectileSpeed;
    /// <summary>
    /// True if the projectile should be a hit scan projectile. Projectile speed will be ignored in this case
    /// </summary>
    public bool projectileIsHitScan;

    /// <summary>
    /// The time between bursts.
    /// </summary>
    public float fireRate;

/// <summary>
/// How hard you want a casing to eject from the gun part
/// </summary>
    public float casingEjectForce;

    /// <summary>
    /// The number of casings to eject per firing cycle. Likely should be equal to burst count
    /// </summary>
    public int casingEjectNumberPerFire;

    public int price = 10;

    public Gun_Stats()
    {
        this.damage          = 1f;
        this.burstCount      = 1;
        this.burstSpeed      = 1f;
        this.projectileSpeed = 1f;
        this.fireRate        = 1f;
        this.projectileIsHitScan = false;
        this.casingEjectNumberPerFire = (int) this.burstCount;
    }

    public Gun_Stats(float damage, int burstCount, float burstSpeed, float projectileSpeed, float fireRate)
    {
        this.damage          = damage;
        this.burstCount      = burstCount;
        this.burstSpeed      = burstSpeed;
        this.projectileSpeed = projectileSpeed;
        this.fireRate        = fireRate;
    }
}
