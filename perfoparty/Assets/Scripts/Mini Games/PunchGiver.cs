using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchGiver : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PunchController>(out PunchController punch))
        {
            punch.hasPunchAbility = true;
            Destroy(gameObject);
        }
    }
}
