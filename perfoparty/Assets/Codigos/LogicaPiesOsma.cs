using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTrigger2 : MonoBehaviour
{
    public Osma osma;

    // Este método se llama cuando algo entra en el colisionador trigger
    private void OnTriggerEnter(Collider other)
    {
        osma.puedoSaltar = true;
    }

    // Este método se llama cuando algo permanece en el colisionador trigger
    private void OnTriggerStay(Collider other)
    {
        osma.puedoSaltar = true;
    }

    // Este método se llama cuando algo sale del colisionador trigger
    private void OnTriggerExit(Collider other)
    {
        osma.puedoSaltar = false;
    }
}
