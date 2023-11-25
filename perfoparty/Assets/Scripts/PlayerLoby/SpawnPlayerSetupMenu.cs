﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class SpawnPlayerSetupMenu : MonoBehaviour
{
    public GameObject playerSetupMenuPrefab;
    public PlayerInput input;

    private void Awake()
    {
        Canvas selectionMenuCanvas = FindObjectOfType<Canvas>(); 
        GameObject selectMenuInstance = Instantiate(playerSetupMenuPrefab, selectionMenuCanvas.transform);
        selectMenuInstance.GetComponent<PlayerSetupMenuController>().setPlayerIndex(input.playerIndex);
        input.uiInputModule = selectMenuInstance.GetComponentInChildren<InputSystemUIInputModule>();

        Debug.Log("Texture Number: " +input.playerIndex);
    }
}
