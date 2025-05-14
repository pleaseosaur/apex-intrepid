using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar_Gun_Stats : Gun_Stats 
{
    // Start is called before the first frame update
    void Start()
    {
       damage = 5;
       burstCount = 1;
       burstSpeed = .1f;
       projectileSpeed = 50;
       fireRate = 1f;
       casingEjectForce = 2;
       casingEjectNumberPerFire = 1;
       price = 10;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
