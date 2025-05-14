using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// TODO: A builder pattern may be more useful for building turrets?
//public class Autocannon : GunPart
//{
//
//    public override void InitializePart()
//    {
//        this.AddMaterialRequirement(new Material(Material.RawMaterials.Steel), 3);
//        this.AddMaterialRequirement(new Material(Material.RawMaterials.Iron), 1);
//
//        this.MaxAmmo = 10;
//        this.CurrentAmmo = MaxAmmo;
//
//        this.DamagePerShot = 20;
//        this.CritChance = .01f;
//
//        this.reloadSpeed = 3f; 
//    }
//
//    // Start is called before the first frame update
//    void Start()
//    {
//        InitializePart();
//        
//    }
//
//    // Update is called once per frame
//    void Update()
//    {
//        
//    }
//}
