using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class SpawnPlayerSetupMenu : MonoBehaviour
{
    [SerializeField] string mainMenuCanvasTag;
    public GameObject playerSetupMenuPrefab;
    public PlayerInput input;

    private void Awake()
    {
        Canvas selectionMenuCanvas = GetMenuCnvas(); 
        GameObject selectMenuInstance = Instantiate(playerSetupMenuPrefab, selectionMenuCanvas.transform);
        selectMenuInstance.GetComponent<PlayerSetupMenuController>().setPlayerIndex(input.playerIndex);
        input.uiInputModule = selectMenuInstance.GetComponentInChildren<InputSystemUIInputModule>();

        Debug.Log("Texture Number: " +input.playerIndex);
    }

    private Canvas GetMenuCnvas()
    {
        foreach (var canvas in FindObjectsOfType<Canvas>())
        {
            if (canvas.CompareTag(mainMenuCanvasTag)) return canvas;
        }

        return null;
    }
}
