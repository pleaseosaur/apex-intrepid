using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Abstract class to give a gun part firing behavior, individual firing types are specified under Gun_Fire_Types/. Those should be attached to their respective prefabs 
/// </summary>
public abstract class Gun_Fire : MonoBehaviour
{
    protected SoundManager soundManager;
    // TODO: Create a way to instantiate all of these things, they need to be created
    public Gun_Stats gunStats;
    /// <summary>
    /// Prefab used for the casing that will be ejected when firing. 
    /// </summary>
    public GameObject casingPrefab;
    public Gun_Projectile gunProjectile;
    
    public bool isOnCooldown;
    /// <summary>
    /// Directions to eject shells in, right, left, up, in that order. 
    /// </summary>
    protected static Vector3[] ejectDirections = {new Vector3(1,1,-1), new Vector3(-1,1,-1), new Vector3(0,2,-1)}; 

    protected abstract IEnumerator fire();

    public Vector3 mostRecentForce;

    /// <summary>
    /// Fires then ejects a casing
    /// </summary>
    public IEnumerator Fire(){
        if(!isOnCooldown){
            isOnCooldown = true;
            //easyFire();
            StartCoroutine(fire());
            yield return new WaitForSeconds(gunStats.fireRate);
            isOnCooldown = false;

        }
        //StartCoroutine(fire());
        //EjectCasing(ejectDirections[0] * gunStats.casingEjectForce, 1f, gunStats.casingEjectNumberPerFire);

    }
    public void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        isOnCooldown = false;
        //gunProjectile = GetComponent<Gun_Projectile>();
        gunStats = GetComponent<Gun_Stats>();
        Debug.Log($"After Gun stats GetComponent call: {gunStats} (type: {gunStats.GetType()})");
        // set default gun stats if not set
        if (gunStats == null)
        {
            Debug.Log("Gunstats was null, setting gunstats");
            gunStats = new Gun_Stats();
        }
        if (casingPrefab.GetComponent<Rigidbody>() == null){
            Debug.LogWarning("Casing prefab has no rigid body, physics will not apply!");
        }
    }

    protected void initialize()
    {
        gunProjectile = GetComponent<Gun_Projectile>();
        gunStats = GetComponent<Gun_Stats>();
        Debug.Log($"After Gun stats GetComponent call: {gunStats} (type: {gunStats.GetType()})");
        // set default gun stats if not set
        if (gunStats == null)
        {
            Debug.Log("Gunstats was null, setting gunstats");
            gunStats = new Gun_Stats();
        }
        if (casingPrefab.GetComponent<Rigidbody>() == null){
            Debug.LogWarning("Casing prefab has no rigid body, physics will not apply!");
        }
    }

    //    public Gun_Fire(){
    //        projectileBody = projectile.GetComponent<Rigidbody>();
    //        casing = projectile.GetComponent<Rigidbody>();
    //        try {
    //            projectileBody = projectile.GetComponent<Rigidbody>();
    //        } catch(Exception e) {
    //            Debug.LogError(e);
    //        }
    //
    //        gunProjectile = new Gun_Projectile(gunStats.damage, gunStats.projectileSpeed, projectileBody);
    //    }

    // Update is called once per frame
    void Update()
    {

    }

/// <summary>
/// Fires a burst at an enemy.
/// </summary>
/// <returns>Cooldown for shots between burst?</returns>
    public IEnumerator DefaultGunBurstFire()
    {
        Debug.Log($"{this}: burst firing");

        for (int i = 0; i < gunStats.burstCount; i++)
        {

            Debug.Log($"shooting in default gun burst fire.");
            ShootProjectile();
            // TODO: derive direction from position on turret maybe? 
            //EjectCasing(ejectDirections[0] * gunStats.casingEjectForce, 1f, gunStats.casingEjectNumberPerFire);

            yield return new WaitForSeconds(1 / gunStats.burstSpeed); // Wait between shots in the burst
        }


    }


    protected void ShootProjectile()
    {
        GameObject projectile = Instantiate(gunProjectile.gameObject, transform.position, transform.rotation);
        Gun_Projectile projectileInstance = projectile.GetComponent<Gun_Projectile>();
        projectileInstance.gunStats = this.gunStats;
        //Debug.Log($"Trying to get body stats in gun fire: {GetComponentInParent<Body_Stats>()}");
        mostRecentForce = (transform.forward * gunStats.projectileSpeed) ;
        projectile.GetComponent<Rigidbody>().AddForce(mostRecentForce);


        //projectile.rb.AddRelativeForce(Vector3.forward * gunStats.projectileSpeed);


    }


    public void EjectCasing(Vector3 force, float casingOffset, int repeat)
    {
        for(int i = 0; i < repeat; i++) {
            // TODO: Make this work with arguments a bit nicer
            Debug.Log($"Instantiating casing prefab (type: {casingPrefab.GetType().Name})");
            GameObject ejectedCasing = Instantiate(casingPrefab, transform.position + Vector3.up * casingOffset, transform.rotation);
            ejectedCasing.GetComponent<Rigidbody>().AddRelativeForce(force);
        }

    }


    public void easyFire() {
        RaycastHit hit;
        //Debug.Log($"We are {gameObject}");
        Physics.Raycast(this.transform.position, this.transform.forward, out hit, 50f, 1 << 6);
        //Debug.Log($"We did run easyfire");

        Debug.DrawLine(transform.position, hit.point, Color.red, 2f);
        var thing = hit.collider;

        if (thing != null) {
            hit.collider.GetComponent<Enemy>().doDamage(gunStats.damage);
        } else {
//            Debug.Log("did not collide with enemy layer");
        }

    }


}
