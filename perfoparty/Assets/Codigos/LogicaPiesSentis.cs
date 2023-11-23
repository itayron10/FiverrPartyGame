using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTrigger4 : MonoBehaviour
{
    public Sentis sentis;

    // Este método se llama cuando algo entra en el colisionador trigger
    private void OnTriggerEnter(Collider other)
    {
        sentis.puedoSaltar = true;
    }

    // Este método se llama cuando algo permanece en el colisionador trigger
    private void OnTriggerStay(Collider other)
    {
        sentis.puedoSaltar = true;
    }

    // Este método se llama cuando algo sale del colisionador trigger
    private void OnTriggerExit(Collider other)
    {
        sentis.puedoSaltar = false;
    }
}
