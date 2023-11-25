using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private PlayerMovement myPlayerMovement;
    private PunchController myPunchController;
    public PlayerConfiguration playerConfig;
    public Inputs controls;

    private void Awake()
    {
        controls = new Inputs();
        myPlayerMovement = GetComponent<PlayerMovement>();
        myPunchController = GetComponent<PunchController>();
    }


    public void InitializePlayer(PlayerConfiguration config)
    {
        playerConfig = config;
        GameObject playerMeshInstance = Instantiate(playerConfig.playerMeshObject, transform.position, transform.rotation, transform);
        config.Input.onActionTriggered += Input_onActionTriggered;
    }


    private void Input_onActionTriggered(InputAction.CallbackContext obj)
    {
        if (IsThisAction(controls.Player.Movement.name, obj)) OnMove(obj);

        if (IsThisAction(controls.Player.Jump.name, obj) && obj.performed) myPlayerMovement.Jump();

        if (IsThisAction(controls.Player.Dance.name, obj) && obj.performed) myPlayerMovement.Dance();

        if (IsThisAction(controls.Player.Punch.name, obj) && obj.performed) myPunchController.Punch();
        
    }

    public static bool IsThisAction(string actionName, InputAction.CallbackContext obj)
    {
        return obj.action.name == actionName;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (myPlayerMovement) myPlayerMovement.UodateMovement(context.ReadValue<Vector2>());
    }
}
