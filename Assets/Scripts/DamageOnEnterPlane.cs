using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Deals damage to anything that enters this thing. Meant to be used on a plane. Specifically between tesla pylons
/// </summary>
public class DamageOnEnterPlane : MonoBehaviour
{
    SoundManager soundManager;
    // Start is called before the first frame update
    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

/// <summary>
/// Ow voltage change ow ow take a little damage
/// </summary>
/// <param name="other"></param>
    void OnTriggerEnter(Collider other){
        if (other.gameObject.GetComponent<IHealthy>() != null) {
            soundManager.Play("zap");
            other.gameObject.GetComponent<IHealthy>().doDamage(15f);
        }
    }
}
