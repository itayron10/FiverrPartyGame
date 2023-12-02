using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] Weapon weapon;


    private void OnTriggerEnter(Collider other)
    {
        if (!weapon.IsEquiped)
        {
            if (other.TryGetComponent<WeaponManager>(out WeaponManager weaponManager))
                weaponManager.EquipWeapon(weapon);
        }
    }

}
