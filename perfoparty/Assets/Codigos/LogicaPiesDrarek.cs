using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTrigger : MonoBehaviour
{
    public Darek darek;

    // Este método se llama cuando algo entra en el colisionador trigger
    private void OnTriggerEnter(Collider other)
    {
        darek.puedoSaltar = true;
    }

    // Este método se llama cuando algo permanece en el colisionador trigger
    private void OnTriggerStay(Collider other)
    {
        darek.puedoSaltar = true;
    }

    // Este método se llama cuando algo sale del colisionador trigger
    private void OnTriggerExit(Collider other)
    {
        darek.puedoSaltar = false;
    }
}
