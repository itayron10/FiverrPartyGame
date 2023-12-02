using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] int clipCapacity;
    [SerializeField] float reloadDuration;
    [SerializeField] float shootingCooldown;
    [SerializeField] Projectile projectile;
    [SerializeField] Transform shootingPoint;
    [SerializeField] GameObject shootingEffect;
    [SerializeField] bool automatic;
    [SerializeField] SoundScriptableObject shootingSound, reloadingSound;
    [SerializeField] Collider rbCollider;
    private int currentClipAmount;
    private bool canShoot = true;
    private SoundManager soundManager;
    private Coroutine shootingCoroutine;
    private Rigidbody rb;
    private WeaponManager weaponManager;
    public WeaponManager GetWeaponManager => weaponManager;
    private bool equiped;

    public bool CanShoot => canShoot;
    public bool IsAutomatic => automatic;
    public bool IsEquiped => equiped;



    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
        rb = GetComponent<Rigidbody>();
        currentClipAmount = clipCapacity;
    }


    public void Equip(Transform holdingPoint, WeaponManager weaponManager)
    {
        this.weaponManager = weaponManager;
        transform.parent = holdingPoint;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        rb.isKinematic = equiped = true;
        rbCollider.enabled = false;
    }

    public void Unequip()
    {
        if (weaponManager == null) return;
        weaponManager.HandleUnequipAnimator();
        rb.isKinematic = equiped = false;
        rbCollider.enabled = true;
        transform.SetParent(null);
        weaponManager = null;
    }


    public void StartShoot()
    {
        if (!canShoot) return;
        shootingCoroutine = StartCoroutine(ShootingCoroutine());
    }

    public void StopShoot()
    {
        StopCoroutine(shootingCoroutine);
    }

    private IEnumerator ShootingCoroutine()
    {
        Debug.Log("Shooting");
        yield return new WaitForEndOfFrame();
        currentClipAmount--;
        Instantiate(projectile, shootingPoint.position, shootingPoint.rotation);
        ParticleManager.InstanciateParticleEffect(shootingEffect, shootingPoint.position, shootingPoint.rotation);
        if (soundManager) soundManager.PlaySound(shootingSound);

        if (!automatic)
        {
            StopCoroutine(nameof(ActiveShootCooldown));
            StartCoroutine(ActiveShootCooldown());
        }

        if (currentClipAmount <= 0) Reload();
        yield return new WaitForSeconds(shootingCooldown);
        Debug.Log("Starting coroutine Again");
        if (automatic) shootingCoroutine = StartCoroutine(ShootingCoroutine());
    }


    private void Reload()
    {
        StopAllCoroutines();
        StartCoroutine(ReloadWeapon());
    }

    private IEnumerator ActiveShootCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootingCooldown);
        canShoot = true;
    }

    private IEnumerator ReloadWeapon()
    {
        canShoot = false;
        yield return new WaitForSeconds(reloadDuration);
        currentClipAmount = clipCapacity;
        soundManager.PlaySound(shootingSound);  
        canShoot = true;
    }    

}
