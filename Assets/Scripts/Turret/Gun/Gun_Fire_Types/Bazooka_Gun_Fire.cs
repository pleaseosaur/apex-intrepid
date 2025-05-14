using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bazooka_Gun_Fire : Gun_Fire
{
    // Start is called before the first frame update
    // void Start()
    // { 
    //     //gunStats = GetComponentInParent<Bazooka_Gun_Stats>();
    //     //base.initialize(); 
    //     soundManager = FindObjectOfType<SoundManager>();
    // }
    protected override IEnumerator fire()
    {
        //yield return easyFire();
        for (int i = 0; i < gunStats.burstCount; i++)
        {

            //Debug.Log($"Firing with {gunStats.damage} damage.");
            ShootProjectile();
            soundManager.Play("BazookaFire");
            //EjectCasing(ejectDirections[0] * gunStats.casingEjectForce, 1f, gunStats.casingEjectNumberPerFire);
            //Debug.Log("Yielding");
            yield return new WaitForSeconds(gunStats.burstSpeed); // Wait between shots in the burst
            soundManager.Play("Reload");
        }
    }
    public new IEnumerator easyFire() {
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

        yield return new WaitForSeconds(gunStats.fireRate);

    }
    

    // Update is called once per frame
    void Update()
    {
        
    }    

}
