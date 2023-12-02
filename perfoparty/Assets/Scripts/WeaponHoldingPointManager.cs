using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHoldingPointManager : MonoBehaviour
{
    [SerializeField] Transform animationHoldingPoint;
    public Transform GetHoldingParant => animationHoldingPoint;
}
