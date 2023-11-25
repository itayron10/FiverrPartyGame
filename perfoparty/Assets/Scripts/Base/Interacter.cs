using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interacter : MonoBehaviour
{
    [SerializeField] float interactionRange;
    [SerializeField] LayerMask interactableLayer;
    private InputHandler inputHandler;

    private void Start()
    {
        inputHandler = GetComponent<InputHandler>();
        inputHandler.playerConfig.Input.onActionTriggered += Input_onActionTriggered;
    }

    private void Input_onActionTriggered(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (InputHandler.IsThisAction(inputHandler.controls.Player.Interact.name, obj) && obj.performed) TryInteract();
    }

    public void TryInteract()
    {
        Collider[] interactables = Physics.OverlapSphere(transform.position, interactionRange, interactableLayer);
        foreach (var interactableCollider in interactables)
        {
            if (!interactableCollider.TryGetComponent<Interactable>(out Interactable interactable)) continue;
            Debug.Log("wwe");
            interactable.Interacte(gameObject);
            break;
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
