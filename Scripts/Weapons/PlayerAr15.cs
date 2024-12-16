using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class PlayerAr15 : APlayerWeapon
{
    // public override void AnimateWeapon()
    // {
    //     Debug.Log("AR15 Fire");
    // }
    public override void Knife()
    {
        Debug.Log("Knife Stabbed");
    }
    public override void BulletTrails(float fireRate)
    {
        // Debug.Log("BulletTrails " + fireRate);
    }
}
