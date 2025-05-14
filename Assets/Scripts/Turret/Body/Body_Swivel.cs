using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class Body_Swivel : MonoBehaviour
{
    [SerializeField] private string[] target = {"Enemy"};

    //private Transform nearestTarget;
    private int targetLayer = 1<<6;

    private Body_Stats bodyStats;
    [SerializeField] private Collider[] targets;
    /// <summary>
    /// List of firable parts that we should try to fire when we are looking at a target
    /// </summary>
    private Gun_Fire[] gun_Fires;
    private float deadZone = 2f;

    void Start()
    {
        gun_Fires = GetComponentsInChildren<Gun_Fire>();
        bodyStats = GetComponent<Body_Stats>();
        if (bodyStats == null)
        {
            Debug.LogError("Body_Stats component not found on this object!");
        }
    }

    void Update()
    {
        FindNearestTarget();
        faceTarget();
        gun_Fires = GetComponentsInChildren<Gun_Fire>();

        if (bodyStats.target != transform && gun_Fires != null && gun_Fires.Length >= 1 && Vector3.Distance(bodyStats.target.position, transform.position) > deadZone) {
           // Debug.Log("Target was something else");
             foreach(Gun_Fire gf in gun_Fires) {
                StartCoroutine(gf.Fire());
            }
        } else {
            //Debug.Log("Target was our own transform");
        }
    }


    /// <summary>
    /// Locates the nearest GameObject with the specified target tag.
    /// </summary>
    void FindNearestTarget()
    {

        targets = Physics.OverlapSphere(transform.position, bodyStats.detectionRadius, targetLayer);
        // order all targets based on proximity to turret, then set our target to the first. or default i guess. 
        bodyStats.target = targets.OrderBy(c => Vector3.Distance(transform.position, c.transform.position)).FirstOrDefault()?.transform ?? transform;

    }

    /// <summary>
    /// Rotates the object about the y-axis toward the nearest target.
    /// </summary>
    void faceTarget()
    {
        if (bodyStats.target != null)
        {
            Vector3 direction = bodyStats.target.position- transform.position;
            direction.y = 0; // Ensures rotation only around the y-axis

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, bodyStats.speed * Time.deltaTime);
            }
        }
    }
}