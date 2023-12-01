﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class PlayerConfigurationManager : MonoBehaviour
{
    public bool strikeMode;
    [SerializeField] PlayerCharacterSO[] playerCharacters;
    private List<PlayerConfiguration> playerConfigs;
    private PlayerInitializer playerInitializer;


    public static PlayerConfigurationManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("[Singleton] Trying to instantiate a seccond instance of a singleton class.");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            playerConfigs = new List<PlayerConfiguration>();
        }
    }

    private void Start()
    {
        playerInitializer = GetComponent<PlayerInitializer>();
    }

    private IEnumerator OnLevelWasLoaded(int levelIndex)
    {
        //PlayerInitializer.instance.InitializePlayers();
        yield return new WaitForEndOfFrame();
        if (levelIndex == 0) SetPlayersToPositions();
    }


    public void PlayWinnerVideo()
    {
        /// Make this function play the video of the character who won teh mini game
        Instantiate(GetPlayerWithHighestScore().playerCharacter.playerWinningVideo, Vector3.zero, Quaternion.identity);
    }

    public PlayerConfiguration GetPlayerWithHighestScore()
    {
        PlayerConfiguration playerWithHighestScore = new PlayerConfiguration(null);
        int highestScore = 0;

        foreach (var playerConfig in playerConfigs)
        {
            if (playerConfig.playerScore > highestScore)
            {
                highestScore = playerConfig.playerScore;
                playerWithHighestScore = playerConfig;
            }
        }

        return playerWithHighestScore;
    }

    public (PlayerConfiguration, PlayerConfiguration, PlayerConfiguration) GetThreeHighestScorePlayers()
    {
        PlayerConfiguration playerWithHighestScore = new PlayerConfiguration(null);
        PlayerConfiguration playerWithSecondHighestScore = new PlayerConfiguration(null);
        PlayerConfiguration playerWithThirdHighestScore = new PlayerConfiguration(null);
        int highestScore = -1;
        int secondHighestScore = -1;
        int thirdHighestScore = -1;

        foreach (var playerConfig in playerConfigs)
        {
            if (playerConfig.playerScore > highestScore)
            {
                highestScore = playerConfig.playerScore;
                playerWithHighestScore = playerConfig;
            }
            else if (playerConfig.playerScore > secondHighestScore && playerConfig.playerScore < highestScore)
            {
                secondHighestScore = playerConfig.playerScore;
                playerWithSecondHighestScore = playerConfig;
            }
            else if (playerConfig.playerScore > thirdHighestScore && playerConfig.playerScore < secondHighestScore)
            {
                thirdHighestScore = playerConfig.playerScore;
                playerWithThirdHighestScore = playerConfig;
            }
        }

        return (playerWithHighestScore, playerWithSecondHighestScore, playerWithThirdHighestScore);
    }



    public void HandlePlayerJoin(PlayerInput playerInput)
    {
        Debug.Log("player joined " + playerInput.playerIndex);
        playerInput.transform.SetParent(transform);

        if(!playerConfigs.Any(p => p.PlayerIndex == playerInput.playerIndex))
        {
            playerConfigs.Add(new PlayerConfiguration(playerInput));
        }
    }

    public void SetPlayerUiInputs(InputSystemUIInputModule inputSystemUIInputModule, PlayerConfiguration playerConfig)
    {
        playerConfig.Input.uiInputModule = inputSystemUIInputModule;
    }


    public void SetPlayersToPositions()
    {
        for (int i = 0; i < playerConfigs.Count; i++)
        {
            SetPlayerToPosition(playerConfigs[i], playerInitializer.GetPlayerSpawnPoints[i].position);
        }
    }

    public void SetPlayerToPosition(PlayerConfiguration player, Vector3 pos)
    {
        player.inputHandler.GetComponent<Rigidbody>().MovePosition(pos);
    }

    public void SetAllPlayersUiInput(InputSystemUIInputModule inputSystemUIInputModule)
    {
        foreach (var player in playerConfigs)
        {
            Debug.Log(player.Input.uiInputModule);
            player.Input.uiInputModule = inputSystemUIInputModule;
            Debug.Log(player.Input.uiInputModule);
        }
    }

    public List<PlayerConfiguration> GetPlayerConfigs()
    {
        return playerConfigs;
    }

    public void IncreaseCharacterSelectIndex(int configIndex)
    {
        PlayerConfiguration playerConfig = playerConfigs[configIndex];
        if (playerConfig.characterIndex >= playerCharacters.Length - 1)
            playerConfig.characterIndex = 0;
        else
            playerConfig.characterIndex++;
    }

    public int GetMeshIndex(int configIndex)
    {
        PlayerConfiguration playerConfig = playerConfigs[configIndex];
        return playerConfig.characterIndex;
    }

    public void DecreaseCharacterSelectIndex(int configIndex)
    {
        PlayerConfiguration playerConfig = playerConfigs[configIndex];
        if (playerConfig.characterIndex <= 0)
            playerConfig.characterIndex = playerCharacters.Length - 1;
        else
            playerConfig.characterIndex--;
    }

    public void ReadyPlayer(int index)
    {
        PlayerConfiguration playerConfig = playerConfigs[index];
        playerConfig.isReady = true;
        playerConfig.playerCharacter = playerCharacters[playerConfig.characterIndex];

        PlayerInitializer.instance.InitializePlayers();
        // if the min numbers of players connected and all of them ready load the next scene
/*        if (playerConfigs.Count == minPlayersReadyForLoad && playerConfigs.All(p => p.isReady == true))
        {
            // load next scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }*/
    }
}


[System.Serializable]
public class PlayerConfiguration
{
    public PlayerConfiguration(PlayerInput pi)
    {
        if (pi != null) PlayerIndex = pi.playerIndex;
        Input = pi;
        playerScore = 0;
    }

    public PlayerInput Input { get; private set; }
    public InputHandler inputHandler { get; set; }
    public int PlayerIndex { get; private set; }
    public int playerScore { get; private set; }
/*    public TextMeshProUGUI playerScoreDisplayText { get; private set; }

    public void SetPlayerScoreDisplayText(TextMeshProUGUI textMeshPro)
    {
        playerScoreDisplayText = textMeshPro;
        playerScoreDisplayText.text = $": {playerScore}";
    }*/

    public void AddPlayerScore(int score)
    {
        playerScore += score;
        //if (playerScoreDisplayText != null) playerScoreDisplayText.text = $": {playerScore}";
    }
    public bool isReady { get; set; }
    public PlayerCharacterSO playerCharacter;
    public int characterIndex = 0;
}