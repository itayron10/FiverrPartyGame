using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    [SerializeField] Transform[] startPoints;
    [SerializeField] List<PlayerConfiguration> playersToEnterGameMode;
    [SerializeField] List<PlayerConfiguration> playersCurrentlyInGameMode;
    [SerializeField] float minYPositionForPlayers;
    [SerializeField] GameObject countingCanvas;
    [SerializeField] string podiumSceneName;
    [SerializeField] GameObject reloadGameMenu;
    [SerializeField] bool lastGameMode;
    [SerializeField] int scoreAddedByWinningRound = 5;
    [SerializeField] float timeToStartNewRound = 4f;
    private int roundCount = 0;
    private LevelManager levelManager;
    private bool miniGameRunning, roundRunning;


    private void Start()
    {
        miniGameRunning = true;
        levelManager = FindObjectOfType<LevelManager>();
        foreach (PlayerConfiguration playerConfig in PlayerConfigurationManager.Instance.GetPlayerConfigs())
        {
            playersToEnterGameMode.Add(playerConfig);
            playersCurrentlyInGameMode.Add(playerConfig);
            playerConfig.inputHandler.GetComponent<Rigidbody>().MovePosition(startPoints[playerConfig.PlayerIndex].position);
        }
        if (playersToEnterGameMode.Count > 1) StartNewRound();
    }


    private void Update()
    {
        if (miniGameRunning) UpdateMiniGame();
    }


    private void UpdateMiniGame()
    {
        for (int i = 0; i < playersCurrentlyInGameMode.Count; i++)
        {
            GameObject player = playersCurrentlyInGameMode[i].inputHandler.gameObject;
            if (IsPlayerKickedOut(player) && roundRunning)
            {
                playersCurrentlyInGameMode.Remove(playersCurrentlyInGameMode[i]);
                player.SetActive(false);
                if (playersCurrentlyInGameMode.Count <= 1)
                {
                    roundRunning = false;
                    playersCurrentlyInGameMode[0].AddPlayerScore(scoreAddedByWinningRound);
                    StartNewRound();
                }
            }

        }
    }

    private void SetActiveAllPlayers(bool active)
    {
        for (int i = 0; i < playersToEnterGameMode.Count; i++)
        {
            GameObject player = playersToEnterGameMode[i].inputHandler.gameObject;
            player.SetActive(active);
        }
    }

    public virtual void StartNewRound()
    {
        roundCount++;
        if (roundCount > 3) { StartCoroutine(EndMiniGame()); return; }

        for (int i = 0; i < playersToEnterGameMode.Count; i++)
        {
            GameObject player = playersToEnterGameMode[i].inputHandler.gameObject;
            player.SetActive(true);
            player.GetComponent<Rigidbody>().MovePosition(startPoints[playersToEnterGameMode[i].PlayerIndex].position);
            playersCurrentlyInGameMode.Clear();
            foreach (var playerConfig in playersToEnterGameMode) playersCurrentlyInGameMode.Add(playerConfig);
        }
        
        ParticleManager.InstanciateParticleEffect(countingCanvas, transform.position, Quaternion.identity, timeToStartNewRound);
        StopCoroutine(nameof(ActiveNewRound));
        StartCoroutine(ActiveNewRound());
    }

    private IEnumerator ActiveNewRound()
    {
        yield return new WaitForEndOfFrame();
        PlayerConfigurationManager.Instance.SetPlayerInputs(false);
        Debug.Log("Locking input");
        yield return new WaitForSeconds(timeToStartNewRound);
        roundRunning = true;
        Debug.Log("Unlocking input");
        PlayerConfigurationManager.Instance.SetPlayerInputs(true);
    }

    private IEnumerator EndMiniGame()
    {
        miniGameRunning = false;
        for (int i = 0; i < playersToEnterGameMode.Count; i++)
        {
            GameObject player = playersToEnterGameMode[i].inputHandler.gameObject;
            player.SetActive(false);
        }

        PlayerConfigurationManager.Instance.PlayWinnerVideo();
        yield return new WaitForSeconds(8.5f); // 8.5 the time it takes to play each of the winners' videos
        if (PlayerConfigurationManager.Instance.strikeMode)
        {
            SetActiveAllPlayers(true);
            if (lastGameMode) levelManager.LoadLevelByString(podiumSceneName);
            else levelManager.LoadNextLevel();
        }
        else
        {
            Debug.Log("Sending to the menu");
            SetActiveAllPlayers(true);
            levelManager.LoadLevelByIndex(0);
        }
    }

    protected virtual bool IsPlayerKickedOut(GameObject player)
    {
        return (player.transform.position.y < minYPositionForPlayers);
    }
}
