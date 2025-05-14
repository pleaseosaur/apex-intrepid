using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;


/// <summary>
/// Class describing what a projectile fired from a gun part should be <br \>
/// </summary>
public class Gun_Projectile : MonoBehaviour
{
    int ENEMY_LAYER = 6;
    /// <summary>
    /// True if this projectile should have projectile-level target tracking (ex. heat seeking missle)
    /// </summary>
    [SerializeField]public bool isTracking;
    /// <summary>
    /// The speed at which the projectile travels
    /// </summary>
    public float speed = 20f;

    /// <summary>
    /// Velocity, initially set by GunPart when firing
    /// </summary>
    public Vector3 velocity;

    /// <summary>
    /// Reference to the Rigidbody component
    /// </summary>
    public Rigidbody rb;

    /// <summary>
    /// The target that the projectile should track, if isTracking is true
    /// </summary>
    public Transform target;
    public Gun_Stats gunStats;

    // TODO: Placeholder, get this from gunpart
    public float damage;
    private int maxLifeTime;
    private float timePassed;
    
    public Gun_Projectile(Gun_Stats gunStats) {
        this.gunStats = gunStats;
    }

    void Start()
    {

        maxLifeTime = 20;
        timePassed = 0;
        ENEMY_LAYER = 6;
        //projectilePreFab = GetComponent<GameObject>();
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component missing from the projectile.");
        }

        // Probably not needed, since we are already launching a rigidbody at a speed
        //LaunchProjectile();
    }

    // Likely not needed
    private void LaunchProjectile()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            rb.velocity = direction * speed;
        }
        else
        {
            rb.velocity = transform.forward * speed;
        }
    }    
    private void TrackTarget()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position);
            rb.AddForce(direction * gunStats.projectileSpeed);
        }
    }
    void Update()
    {
        if (isTracking && target != null)
        {
            TrackTarget();
        }
        //this.transform.position = rb.transform.position;

        timePassed += Time.deltaTime;
        if(timePassed >= maxLifeTime){
            Destroy(gameObject);
        }

    }

    public void OnCollisionEnter(Collision collision)
    {
        // TODO: Handle collision logic here
        //Debug.Log($"Projectile collided with {collision.gameObject.name} in layer {collision.gameObject.layer}. Checking to see if it matches layer {ENEMY_LAYER}");

        if (collision.gameObject.layer == ENEMY_LAYER) {
            // TODO: Access through getters and setters better or something
            Enemy enemy = collision.gameObject.GetComponentInParent<Enemy>();
            if(enemy != null) {
                enemy.doDamage(gunStats.damage);    
            } else {
                Debug.Log("Could not get enemy component");
            }
        }
        // Destroy the projectile after collision
        Destroy(gameObject);
    }
}
