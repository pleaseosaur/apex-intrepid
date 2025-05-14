using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bazooka_Gun_Stats : Gun_Stats 
{
    // Start is called before the first frame update
    void Start()
    {
       damage = 100;
       burstCount = 1;
       burstSpeed = .5f;
       projectileSpeed = 50;
       fireRate = 3f;
       casingEjectForce = 2;
       casingEjectNumberPerFire = 1;
       price = 10;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
