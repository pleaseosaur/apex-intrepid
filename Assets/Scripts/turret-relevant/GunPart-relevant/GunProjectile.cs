using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


/// <summary>
/// Class describing what a projectile fired from a gun part should be <br \>
/// </summary>
public class GunProjectile : MonoBehaviour
{
    const int ENEMY_LAYER = 1 << 7;
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
    private Rigidbody rb;

    /// <summary>
    /// The target that the projectile should track, if isTracking is true
    /// </summary>
    public Transform target;

    // TODO: Placeholder, get this from gunpart
    public int damage = 20;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component missing from the projectile.");
            return;
        }

        LaunchProjectile();
    }
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
            Vector3 direction = (target.position - transform.position).normalized;
            rb.velocity = direction * speed;
        }
    }
    void Update()
    {
        if (isTracking && target != null)
        {
            TrackTarget();
        }
        this.transform.position = rb.transform.position;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        // TODO: Handle collision logic here
        Debug.Log($"Projectile collided with {collision.gameObject.name}");

        if (collision.gameObject.layer == ENEMY_LAYER) {
            // TODO: Access through getters and setters better or something
            //Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            //if(enemy != null) {
            //    enemy.stats.currentHealth -= damage;    
            //}
        }
        // Destroy the projectile after collision
        Destroy(gameObject);
    }
}
