using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandeler : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerMovement myPlayerMovement;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        myPlayerMovement = GetComponent<PlayerMovement>();

/*        foreach (var player in FindObjectsOfType<PlayerMovement>())
        {
            if (playerInput.playerIndex == player.index) myPlayerMovement = player;
        }*/
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log("wow");
        if (myPlayerMovement) myPlayerMovement.UodateMovement(context.ReadValue<Vector2>());
    }
}
