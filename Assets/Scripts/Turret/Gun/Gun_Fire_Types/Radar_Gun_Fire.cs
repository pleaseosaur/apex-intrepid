using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar_Gun_Fire : Gun_Fire
{
    protected override IEnumerator fire()
    {
        //yield return easyFire();
        for (int i = 0; i < gunStats.burstCount; i++)
        {
            Collider[] enemies = Physics.OverlapSphere(transform.position, 500f, 1 << 6);
            if (enemies.Length > 0) {

                Enemy selected = enemies[Random.Range(0, enemies.Length)].GetComponentInParent<Enemy>();
                StartCoroutine(selected.Confuse(gunStats.damage));
            }
            yield return new WaitForSeconds(gunStats.burstSpeed); // Wait between shots in the burst
        
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
    // Start is called before the first frame update
    //void Start()
    //{
        //gunStats = GetComponentInParent<Bazooka_Gun_Stats>();

       //base.initialize(); 
    //}

    // Update is called once per frame
    void Update()
    {
        
    }    

}
