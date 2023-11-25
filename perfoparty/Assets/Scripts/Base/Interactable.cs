using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour
{
    ///NOTE: interactables need a collider so that the active interactable check will work

    [Header("Settings")]
    [Tooltip("This Sound will be played when the player interacts with this interactable")]
    // the sound which will be played when the player interacts with this interactable
    [SerializeField] SoundScriptableObject onInteractionSound;
    private SoundManager soundManager;
    public bool isInteractable;
    private bool initialized;

    private void Start() => FindPrivateObjects();


    /// <summary>
    /// can be ovveriden to find different kinds of private objects on the Start method
    /// </summary>
    protected virtual void FindPrivateObjects()
    {
        // set/find private objects
        soundManager = FindObjectOfType<SoundManager>();
        initialized = true;
    }

    /// <summary>
    /// can be ovveriden for different behaviours on interaction
    /// </summary>
    public virtual void Interacte(GameObject interacter)
    {
        if (!initialized) FindPrivateObjects();
        soundManager.PlaySound(onInteractionSound);
        Debug.Log($"You Interacted With this object: {gameObject.name}");
    }
}
