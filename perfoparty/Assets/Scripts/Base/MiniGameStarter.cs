using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;

public class MiniGameStarter : GameStarterInteractable
{
    [SerializeField] GameObject strikeModeInteracter;
    [SerializeField] Vector3 miniGameSelectionSpawnPos;
    [SerializeField] GameObject miniGamePlayerSelectionPrefab;

    public override void StartGame()
    {
        Destroy(strikeModeInteracter);
        Instantiate(miniGamePlayerSelectionPrefab, miniGameSelectionSpawnPos, Quaternion.identity);
        PlayerConfigurationManager.Instance.strikeMode = false;
        Destroy(gameObject);
    }
}
