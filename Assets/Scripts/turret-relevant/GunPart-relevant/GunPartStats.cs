using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// A class containing stats relevant to gun parts
/// </summary>
public class GunPartStats : MonoBehaviour
{
    /// <summary>
    /// Max ammo for the part
    /// </summary>
    public int maxAmmo;

    /// <summary>
    /// Damage per individual shot
    /// </summary>
    public float damagePerShot;
    /// <summary>
    /// Percentage chance of a critical hit <br \>
    /// Not used currently
    /// </summary>
    public float critChance;
    /// <summary>
    /// Damage multiplier for when you roll a critical hit <br \>
    /// Not currently used
    /// </summary>
    public float critMultiplier;
    /// <summary>
    /// The number of individual shots to fire in a burst
    /// </summary>
    public int burstPerShot;
    /// <summary>
    /// The duration between bursts
    /// </summary>
    public float burstCooldown;
    /// <summary>
    /// How fast the burst fires each shot in the burst. CAREFUL: 1/burstFireRate seconds. 
    /// </summary>
    public float burstFireRate;
    /// <summary>
    /// Reload speed, units should be rounds/sec
    /// </summary>
    public float reloadSpeed; 
    public float projectileSpeed;

}
