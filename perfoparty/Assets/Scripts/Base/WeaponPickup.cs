using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] Weapon weapon;
    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!weapon.IsEquiped)
        {
            if (other.TryGetComponent<WeaponManager>(out WeaponManager weaponManager))
                weaponManager.EquipWeapon(weapon);
        }
    }

    private void Update()
    {
        if (transform.position.y < -2f && transform.parent == null)
        {
            transform.position = startPos;
        }
    }


}
