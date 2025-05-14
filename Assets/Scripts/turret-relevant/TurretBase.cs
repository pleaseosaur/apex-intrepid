using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBase : MonoBehaviour
{

    /*
    *
    *   FIELDS
    *
    */

    [SerializeField] private int maxParts;
    [SerializeField] private float BaseDurability;
    // Start is called before the first frame update


    /*
     *
     * METHODS
     *
     */



    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public TurretBase BuildTurretBase(int maxParts, int BaseDurability){
        throw new NotImplementedException();
    }

}
