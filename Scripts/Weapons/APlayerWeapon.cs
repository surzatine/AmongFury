using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using GameKit.Dependencies.Utilities;
using UnityEngine;

public abstract class APlayerWeapon : NetworkBehaviour
{
    public int damage;
    public float maxRange = 2000f;
    public float fireRate = 0.5f;

    public LayerMask weaponHitLayers;

    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private ParticleSystem bloodParticles;
    [SerializeField] private ParticleSystem terrainHitParticles;

    // Bullet Holes
    [SerializeField] GameObject bulletHoleEffect;

    // Sound
    public AudioSource gunFireSound;
    public AudioClip gunFireSoundClip;


    public Transform _cameraTransform;

    private float _lastFireTime;




    public void Awake()
    {
        _cameraTransform = Camera.main.transform;
    }


    public void Fire()
    {
        if (Time.time < _lastFireTime + fireRate)
        {
            return;
        }

        _lastFireTime = Time.time;
        AnimateWeapon();
        if (!Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out RaycastHit hit, maxRange, weaponHitLayers))
        {
            // Debug.Log("No hit");
            return;
        }
        // Debug.Log("Hit");

        if (hit.transform.TryGetComponent(out PlayerHealth health))
        {
            health.TakeDamage(damage, OwnerId);
            Instantiate(bloodParticles, hit.point, Quaternion.LookRotation(hit.normal));
            return;
        }

        Instantiate(terrainHitParticles, hit.point, Quaternion.LookRotation(hit.normal));
        GameObject bulletHoleImpact = Instantiate(bulletHoleEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(bulletHoleImpact, 3f);

    }
    [ServerRpc]
    public virtual void AnimateWeapon()
    {
        // Debug.Log("Weapon Animating");
        PlayMuzzleFlash();
    }
    [ObserversRpc]
    public void PlayMuzzleFlash()
    {

        muzzleFlash.Play();

        gunFireSound = GetComponent<AudioSource>();
        gunFireSound.PlayOneShot(gunFireSoundClip);

        BulletTrails(fireRate);
    }






    // Knife Stabbed
    public abstract void Knife();
    // Bullet Trails
    public abstract void BulletTrails(float fireRate);
}
