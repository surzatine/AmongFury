using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class PlayerPistol : APlayerWeapon
{
    // public TrailRenderer bulletTrails;
    // public override void AnimateWeapon()
    // {
    //     Debug.Log("Glock Fire");
    // }
    public override void Knife()
    {
        Debug.Log("Knife Stabbed");
    }
    public override void BulletTrails(float fireRate)
    {
        // Debug.Log("BulletTrails " + fireRate);
        // bulletTrails.emitting = true;
        // bulletTrails.time = fireRate;
    }
}