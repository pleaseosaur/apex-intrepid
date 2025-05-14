using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plasma_Gun_Stats : Gun_Stats 
{
    // Start is called before the first frame update
    void Start()
    {
       damage = 10;
       burstCount = 5;
       burstSpeed = .05f;
       projectileSpeed = 100;
       fireRate = .5f;
       casingEjectForce = 2;
       casingEjectNumberPerFire = 1;
       price = 10;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
