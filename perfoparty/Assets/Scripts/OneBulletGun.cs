using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneBulletGun : Weapon
{
    protected override void Reload()
    {
        currentClipAmount = clipCapacity;
        Unequip();
    }
}
