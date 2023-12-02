using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TntHolder : MonoBehaviour
{
    [SerializeField] Transform tntHoldPoint;
    private TNT currentHoldTnt;
    public TNT GetTnt => currentHoldTnt;

    public void EquipTnt(TNT tnt)
    {
        currentHoldTnt = tnt;
        currentHoldTnt.transform.parent = tntHoldPoint;
        currentHoldTnt.transform.localPosition = Vector3.zero;
        currentHoldTnt.transform.localRotation = Quaternion.identity;
        tnt.SetTNTHolder(this.GetComponent<BasicHealth>());
    }

    public void UnequipTnt()
    {
        if (currentHoldTnt.transform.parent == tntHoldPoint) currentHoldTnt.transform.parent = null;
        currentHoldTnt = null;
    }

}
