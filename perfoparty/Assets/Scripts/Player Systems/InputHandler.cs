using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class InputHandler : MonoBehaviour
{
    [SerializeField] InputSystemUIInputModule uiEventSystem;
    [SerializeField] Inputs controls;
    [SerializeField] EventSystem myEventSystem;
    [SerializeField] Canvas playerPauseMenu;
    public PlayerConfiguration playerConfig;
    private PlayerMovement myPlayerMovement;
    private PunchController myPunchController;

    public Inputs GetControls => controls;
    public Canvas GetPlayerPauseMenu => playerPauseMenu;
    public EventSystem GetEventSystem => myEventSystem;

    private void Awake()
    {
        controls = new Inputs();
        myPlayerMovement = GetComponent<PlayerMovement>();
        myPunchController = GetComponent<PunchController>();
    }

    private void OnLevelWasLoaded(int level)
    {
        playerConfig.Input.uiInputModule = uiEventSystem;
    }

    public void InitializePlayer(PlayerConfiguration config)
    {
        playerConfig = config;
        playerConfig.inputHandler = this;
        GameObject playerMeshInstance = Instantiate(playerConfig.playerMeshObject, transform.position, transform.rotation, transform);
        config.Input.onActionTriggered += Input_onActionTriggered;
        config.Input.uiInputModule = uiEventSystem;
    }


    private void Input_onActionTriggered(InputAction.CallbackContext obj)
    {
        if (IsThisAction(controls.Player.Movement.name, obj)) OnMove(obj);

        if (IsThisAction(controls.Player.Jump.name, obj) && obj.performed) myPlayerMovement.Jump();

        if (IsThisAction(controls.Player.Dance.name, obj) && obj.performed) myPlayerMovement.Dance();

        if (IsThisAction(controls.Player.Punch.name, obj) && obj.performed) myPunchController.Punch();

        if (IsThisAction(controls.Player.PauseGame.name, obj) && obj.performed) FindObjectOfType<PauseManager>()?.PreformTogglePause(playerConfig);
        
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
