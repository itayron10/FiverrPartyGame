using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTrigger1 : MonoBehaviour
{
    public Jorda jorda;

    // Este método se llama cuando algo entra en el colisionador trigger
    private void OnTriggerEnter(Collider other)
    {
        jorda.puedoSaltar = true;
    }

    // Este método se llama cuando algo permanece en el colisionador trigger
    private void OnTriggerStay(Collider other)
    {
        jorda.puedoSaltar = true;
    }

    // Este método se llama cuando algo sale del colisionador trigger
    private void OnTriggerExit(Collider other)
    {
        jorda.puedoSaltar = false;
    }
}
