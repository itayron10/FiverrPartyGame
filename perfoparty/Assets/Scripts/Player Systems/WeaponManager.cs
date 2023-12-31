using System.Collections;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] string hasWeaponAnimatorBool, shootAnimatorBool;
    [SerializeField] Transform weaponHoldingPoint;
    [SerializeField] Vector3 weaponHoldingLocalPos, weaponHoldingLocalEuler;
    [SerializeField] Vector3 weaponHoldingShootingLocalEuler;
    private Weapon currentWeapon;
    public Weapon GetCurrentWeapon => currentWeapon;
    private PlayerMovement playerMovement;
    private Animator animator;
    public bool canEquip = true;
    private Coroutine shootingCoroutine;
    public float equipCoolDown = 1;



    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animator = playerMovement.animator;
        weaponHoldingPoint.parent = animator.GetComponent<WeaponHoldingPointManager>().GetHoldingParant;
        weaponHoldingPoint.localPosition = weaponHoldingLocalPos;
        weaponHoldingPoint.localEulerAngles = weaponHoldingLocalEuler;
    }

    private void Update()
    {
        animator.SetBool(hasWeaponAnimatorBool, currentWeapon != null);
    }

    public void EquipWeapon(Weapon weapon)
    {
        if (!canEquip) return;
        if (currentWeapon) currentWeapon.Unequip();
        currentWeapon = weapon;
        weapon.Equip(weaponHoldingPoint, this);
    }


    public void UnequipWeapon()
    {
        if (!currentWeapon) return;
        currentWeapon.Unequip();
        HandleUnequipAnimator();
    }

    public void HandleUnequipAnimator()
    {
        SetWeaponToDefault();
        currentWeapon = null;
        if (shootingCoroutine != null) StopCoroutine(shootingCoroutine);
        if (gameObject.activeInHierarchy) PlayerConfigurationManager.Instance.StartCoroutine(EquipingCooldown());
    }

    protected IEnumerator EquipingCooldown()
    {
        canEquip = false;
        yield return new WaitForSeconds(equipCoolDown);
        canEquip = true;
    }

    public void StartShootWeapon()
    {
        if (!currentWeapon) return;
        if (!currentWeapon.CanShoot) return;
        if (shootingCoroutine != null) StopCoroutine(shootingCoroutine);
        shootingCoroutine = StartCoroutine(SetWeaponHoldingToAnimation());
    }

    public void StopShootingWeapon()
    {
        if (!currentWeapon) return;
        SetWeaponToDefault();
    }

    private IEnumerator SetWeaponHoldingToAnimation()
    {
        SetWeaponToShooting();
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(1).Length);
        if (!currentWeapon.IsAutomatic)
        {
            SetWeaponToDefault();
        }
    }

    private void SetWeaponToShooting()
    {
        currentWeapon.StartShoot();
        animator.SetBool(shootAnimatorBool, true);
        weaponHoldingPoint.localEulerAngles = weaponHoldingShootingLocalEuler;
        Debug.Log($"To Shooting Rotation {animator.GetCurrentAnimatorClipInfo(1).Length}");
    }

    private void SetWeaponToDefault()
    {
        if (currentWeapon) currentWeapon.StopShoot();
        weaponHoldingPoint.localEulerAngles = weaponHoldingLocalEuler;
        animator.SetBool(shootAnimatorBool, false);
        Debug.Log("To Normal Rotation");
    }
}
