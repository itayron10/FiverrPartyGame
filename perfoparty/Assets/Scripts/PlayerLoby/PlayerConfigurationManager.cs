using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using TMPro;
using UnityEngine.Video;

public class PlayerConfigurationManager : MonoBehaviour
{
    public bool strikeMode;
    [SerializeField] GameObject[] meshPrefabs;
    [SerializeField] GameObject[] winningVideos; // should be parallel to the meshPrefabs indexes
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

    public void PlayWinnerVideo()
    {
        /// Make this function play the video of the character who won teh mini game
        Instantiate(GetPlayerWithHighestScore().playerWinningVideo, Vector3.zero, Quaternion.identity);
    }

    private PlayerConfiguration GetPlayerWithHighestScore()
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
        playerInitializer.SetPlayersToSpawnPoints();
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

    public void IncreaseMeshSelectIndex(int configIndex)
    {
        PlayerConfiguration playerConfig = playerConfigs[configIndex];
        if (playerConfig.meshIndex >= meshPrefabs.Length - 1)
            playerConfig.meshIndex = 0;
        else
            playerConfig.meshIndex++;
    }

    public int GetMeshIndex(int configIndex)
    {
        PlayerConfiguration playerConfig = playerConfigs[configIndex];
        return playerConfig.meshIndex;
    }

    public void DecreaseMeshSelectIndex(int configIndex)
    {
        PlayerConfiguration playerConfig = playerConfigs[configIndex];
        if (playerConfig.meshIndex <= 0)
            playerConfig.meshIndex = meshPrefabs.Length - 1;
        else
            playerConfig.meshIndex--;
    }

    public void ReadyPlayer(int index)
    {
        PlayerConfiguration playerConfig = playerConfigs[index];
        playerConfig.isReady = true;
        playerConfig.playerMeshObject = meshPrefabs[playerConfig.meshIndex];
        playerConfig.playerWinningVideo = winningVideos[playerConfig.meshIndex];
        Debug.Log(playerConfig.playerMeshObject);

        PlayerInitializer.instance.InitializePlayers();
        // if the min numbers of players connected and all of them ready load the next scene
/*        if (playerConfigs.Count == minPlayersReadyForLoad && playerConfigs.All(p => p.isReady == true))
        {
            // load next scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }*/
    }

    private void OnLevelWasLoaded(int level)
    {
        //PlayerInitializer.instance.InitializePlayers();
    }
}


[System.Serializable]
public class PlayerConfiguration
{
    public PlayerConfiguration(PlayerInput pi)
    {
        if (pi != null) PlayerIndex = pi.playerIndex;
        Input = pi;
        playerColor = Random.ColorHSV();
        playerScore = 0;
    }

    public Color playerColor { get; set; }

    public PlayerInput Input { get; private set; }
    public InputHandler inputHandler { get; set; }
    public int PlayerIndex { get; private set; }
    public int playerScore { get; private set; }
    public TextMeshProUGUI playerScoreDisplayText { get; private set; }

    public void SetPlayerScoreDisplayText(TextMeshProUGUI textMeshPro)
    {
        playerScoreDisplayText = textMeshPro;
        playerScoreDisplayText.text = $": {playerScore}";
    }

    public void AddPlayerScore(int score)
    {
        playerScore += score;
        if (playerScoreDisplayText != null) playerScoreDisplayText.text = $": {playerScore}";
    }
    public bool isReady { get; set; }
    public Material playerMaterial {get; set;}
    public GameObject playerWinningVideo;
    public GameObject playerMeshObject;
    public int meshIndex = 0;
}