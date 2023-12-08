using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneBulletGun : Weapon
{
    private WeaponManager previousHolder;

    protected override void Reload()
    {
        currentClipAmount = clipCapacity;
        previousHolder = weaponManager;
        previousHolder.equipCoolDown = 5;
        Unequip();
    }


}
