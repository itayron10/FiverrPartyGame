using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInteractable : Interactable
{
    [SerializeField] Weapon weapon;


    public override void Interacte(GameObject interacter)
    {
        base.Interacte(interacter);
        if (interacter.TryGetComponent<WeaponManager>(out WeaponManager weaponManager))
        {
            weaponManager.EquipWeapon(weapon);
            
        }
    }
}
