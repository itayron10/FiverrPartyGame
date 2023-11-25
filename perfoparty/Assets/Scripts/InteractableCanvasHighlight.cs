using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class InteractableCanvasHighlight : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Interactable interactable;
    [SerializeField] Canvas highlightCanvas;

    private void Update()
    {
        // setting a canvas enabled based on the interactable canInteract bool
        if (!highlightCanvas || !interactable) { return; }
        bool isInteractable = interactable.isInteractable;
        if (highlightCanvas.gameObject.activeSelf != isInteractable) highlightCanvas.gameObject.SetActive(isInteractable);
    }
}
