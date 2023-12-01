using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] SoundScriptableObject pauseSound, unpauseSound;
    private bool isGamePaused = false;
    private SoundManager soundManager;


    private void Awake() => FindPrivateObjects();
    private void Start()
    {
        UnpauseGame();
    }

    private void FindPrivateObjects()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }


    public void PreformTogglePause(PlayerConfiguration playerConfig)
    {
        HandlePauseSettings(isGamePaused = !isGamePaused, playerConfig);
    }



    public void UnpauseGame(PlayerConfiguration playerConfig = null)
    {
        HandlePauseSettings(isGamePaused = false, playerConfig);
    }

    public void PauseGame(PlayerConfiguration playerConfig = null)
    {
        HandlePauseSettings(isGamePaused = true, playerConfig);
    }


    private void HandlePauseSettings(bool isGamePaused, PlayerConfiguration playerConfig)
    {
        if (playerConfig != null) playerConfig.inputHandler.GetPlayerPauseMenu.gameObject.SetActive(isGamePaused ? true : false);

        soundManager.PlaySound(isGamePaused ? pauseSound : unpauseSound);
        Time.timeScale = isGamePaused ? 0f : 1f;

        foreach (var player in PlayerConfigurationManager.Instance.GetPlayerConfigs())
        {
            if (playerConfig == null)
            {
                player.inputHandler.GetPlayerPauseMenu.gameObject.SetActive(isGamePaused ? true : false);
                HandlePlayerInput(isGamePaused, player);
            }
            else if (!player.Equals(playerConfig))
            {
                HandlePlayerInput(isGamePaused, player);
            }
        }
    }

    private static void HandlePlayerInput(bool isGamePaused, PlayerConfiguration player)
    {
        if (isGamePaused) player.Input.DeactivateInput();
        else player.Input.ActivateInput();
    }
}
