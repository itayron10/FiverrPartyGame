using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interacter : MonoBehaviour
{
    [SerializeField] float interactionRange;
    [SerializeField] LayerMask interactableLayer;
    private InputHandler inputHandler;
    private Interactable currentInteractable;

    private void Start()
    {
        inputHandler = GetComponent<InputHandler>();
        inputHandler.playerConfig.Input.onActionTriggered += Input_onActionTriggered;
    }

    private void Input_onActionTriggered(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (InputHandler.IsThisAction(inputHandler.GetControls.Player.Interact.name, obj) && obj.performed) TryInteract();
    }

    public void TryInteract()
    {
        Collider[] interactables = Physics.OverlapSphere(transform.position, interactionRange, interactableLayer);
        List<Transform> interactablesTransforms = new List<Transform>();
        foreach (var interactableCollider in interactables)
        {
            Debug.Log(interactableCollider.name);
            if (!interactableCollider.TryGetComponent<Interactable>(out Interactable interactable)) continue;
            interactablesTransforms.Add(interactable.transform);
        }

        if (interactablesTransforms.Count <= 0) return;

        currentInteractable = DetectionHelper.GetClosest(interactablesTransforms, transform.position).GetComponent<Interactable>();
        currentInteractable.Interacte(gameObject);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
