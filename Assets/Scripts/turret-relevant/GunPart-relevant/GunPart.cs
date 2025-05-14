using System.Collections;
using UnityEngine;

/// <summary>
/// Represents a gun part, meant to fire, reload, etc. 
/// <para> 
/// TODO: Likely break this up more so that flyweight pattern can be used. Would help performance perhaps?
/// Turning this back to abstract and having extra classes for categories of gunparts that inherit could prove useful for this. 
/// </para>
/// </summary>
public class GunPart : TurretPart
{
    /*
    *
    *   FIELDS
    *
    */


    public bool canFire;
    [SerializeField] private GunPartStats partStats;

    /// <summary>
    /// Current ammo the part has loaded
    /// </summary>
    public int currentAmmo;
    public Rigidbody casing;
    [SerializeField] private float casingOffset;
    public Vector3 casingEjectForce;
    public GunProjectile projectile;

    /*
     *
     * METHODS
     *
     */


    private void Awake(){
        partStats = new GunPartStats();
        partStats.maxAmmo = 10;
        partStats.projectileSpeed = 20f;
        partStats.burstPerShot = 3;
        partStats.burstCooldown = 1;
        partStats.burstFireRate = 5;
        partStats.reloadSpeed = 5;
        canFire = true;
        currentAmmo = partStats.maxAmmo;

    }

    public void Fire(Transform target)
    {
        Debug.Log($"{PartName}: checking if can fire");
        if (CanFire())
        {
            Debug.Log($"{PartName}: starting burstfire routine");
            StartCoroutine(BurstFire(target));
        } 
    }

    public void EjectCasing(Vector3 force)
    {
        Rigidbody ejectedCasing = Instantiate(casing, transform.position + new Vector3(1,1,-.1f) * casingOffset, transform.rotation);
        ejectedCasing.AddRelativeForce(force);
    }

    public void EjectCasing(Vector3 force, Rigidbody casing)
    {
        Rigidbody ejectedCasing = Instantiate(casing, transform.position + Vector3.up * casingOffset, transform.rotation);
        ejectedCasing.AddRelativeForce(force);
    }

    public bool CanFire()
    {
        return canFire && currentAmmo > 0;
    }

    private IEnumerator BurstFire(Transform target)
    {
        canFire = false;
        Debug.Log($"{PartName}: burst firing");

        for (int i = 0; i < partStats.burstPerShot; i++)
        {
            if (currentAmmo <= 0){
                break;
            }
            // Implement actual firing logic here
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
           // lookRotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);

            Debug.Log($"Firing at {target.name} with {partStats.damagePerShot} damage.");
            ShootProjectile(target);
            EjectCasing(casingEjectForce);

            currentAmmo--;
            yield return new WaitForSeconds(1 / partStats.burstFireRate); // Wait between shots in the burst
        }

        // This is the reloading section now
        // Everything else hurt way too fucking much so it's here now sorry man
        // TODO: fix it. seperate it. learn how coroutines actually work. evan if you're doing this please let it be a while from when you threw this in here
        if (currentAmmo <= 0) {
            yield return new WaitForSeconds(partStats.maxAmmo / partStats.reloadSpeed);
            currentAmmo = partStats.maxAmmo;
            canFire = true;
        } else {

            yield return new WaitForSeconds(partStats.burstCooldown); // Yield the cooldown time between bursts
            canFire = true;
        }
    }

private void ShootProjectile(Transform target)
{
    if (projectile != null)
    {
        // Instantiate the projectile GameObject
        GameObject projectileObject = Instantiate(projectile.gameObject, transform.position + transform.forward * 2f, transform.rotation);

        // Get the GunProjectile component from the instantiated GameObject
        GunProjectile projectileInstance = projectileObject.GetComponent<GunProjectile>();

        if (projectileInstance != null)
        {
            // Set the target for the projectile
            projectileInstance.target = target;
            projectileInstance.isTracking = true; // Set to true if you want the projectile to track the target
            projectileInstance.speed = partStats.projectileSpeed;
            Debug.Log($"{PartName}: Firing projectile at {target.name}");
        }
        else
        {
            Debug.LogError("GunProjectile component is missing from the instantiated projectile.");
        }
    }
}

    public override TurretPart InitializePart()
    {
        throw new System.NotImplementedException();
    }
}
