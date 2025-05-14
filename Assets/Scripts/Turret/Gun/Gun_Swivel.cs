using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Swivel : MonoBehaviour
{
    [SerializeField] private Body_Stats bodyStats; // from parent
    [SerializeField] private Gun_Fire gunFire; 
    [SerializeField] private Gun_Stats gunStats;

    [SerializeField] private float verticalLimit = 30f; // Up and down limit in degrees
    [SerializeField] private float horizontalLimit = 2f; // Left and right limit in degrees
    [SerializeField] private float deadZone = 3f; // Do not fire if target is within this distance to us

    void Start()
    {
        bodyStats = GetComponentInParent<Body_Stats>();
        gunFire = GetComponent<Gun_Fire>();
        //        Debug.Log($"{this}'s parent is {}");
        if (bodyStats == null)
        {
            Debug.LogError("Body_Stats component not found on this object!");
        }
    }

    void Update()
    {
        FaceTarget();
        // Firing logic below
        // This should work if our target is ourselves, in the case that there is no enemy target
       // if(bodyStats != null && Vector3.Distance(bodyStats.target.position, transform.position) > deadZone){
       //    Debug.Log("Trying to fire");
       //    gunFire.Fire(); 

       // }
    }

    /// <summary>
    /// Rotates the object toward the target with angle limitations.
    /// </summary>
    void FaceTarget()
    {
        if (bodyStats != null && bodyStats.target != null)
        {
            Vector3 targetPosition = bodyStats.target.position + Vector3.up * 1f + bodyStats.target.forward * 1f;
            Vector3 targetDirection = targetPosition - transform.position;
            Vector3 limitedDirection = LimitRotation(targetDirection);

            if (limitedDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(limitedDirection);
                Vector3 rotation = Quaternion.Lerp(transform.rotation, targetRotation, bodyStats.speed * Time.deltaTime).eulerAngles;
                transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
            }
        }
    }

    /// <summary>
    /// Limits the rotation based on vertical and horizontal angle constraints.
    /// </summary>
    Vector3 LimitRotation(Vector3 targetDirection)
    {
        Vector3 localDirection = transform.InverseTransformDirection(targetDirection);

        float horizontalAngle = Mathf.Atan2(localDirection.x, localDirection.z) * Mathf.Rad2Deg;
        float verticalAngle = -Mathf.Atan2(localDirection.y, new Vector2(localDirection.x, localDirection.z).magnitude) * Mathf.Rad2Deg;

        // Clamp
        horizontalAngle = Mathf.Clamp(horizontalAngle, -horizontalLimit, horizontalLimit);
        verticalAngle = Mathf.Clamp(verticalAngle, -verticalLimit, verticalLimit);

        // Convert back to a direction
        return transform.TransformDirection(Quaternion.Euler(verticalAngle, horizontalAngle, 0) * Vector3.forward);
    }
}
