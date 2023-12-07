using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    public List<PlayerConfiguration> GetPlayersCurrentlyInGameMode => playersCurrentlyInGameMode;
    [SerializeField] Transform[] startPoints;
    [SerializeField] float minYPositionForPlayers;
    [SerializeField] GameObject countingCanvas;
    [SerializeField] string podiumSceneName;
    [SerializeField] GameObject reloadGameMenu;
    [SerializeField] bool lastGameMode;
    [SerializeField] int scoreAddedByWinningRound = 5;
    [SerializeField] float timeToStartNewRound = 4f;
    protected List<PlayerConfiguration> playersToEnterGameMode = new List<PlayerConfiguration>();
    protected bool miniGameRunning, roundRunning;
    private List<PlayerConfiguration> playersCurrentlyInGameMode = new List<PlayerConfiguration>();
    private int roundCount = 0;
    private bool initializedManager;
    private LevelManager levelManager;
    private ExplanationManager explanationManager;
    public bool IsGameRunning => miniGameRunning;

    private IEnumerator Start()
    {
        explanationManager = FindObjectOfType<ExplanationManager>();
        SetPlayers();
        yield return new WaitForEndOfFrame();
        PlayerConfigurationManager.Instance.SetPlayerInputs(false);
    }

    private void Update()
    {
        if (!initializedManager && !explanationManager.isDialougeRunning) InitializeManager();
        if (miniGameRunning) UpdateMiniGame();
    }

    private void SetPlayers()
    {
        foreach (PlayerConfiguration playerConfig in PlayerConfigurationManager.Instance.GetPlayerConfigs())
        {
            playersToEnterGameMode.Add(playerConfig);
            playersCurrentlyInGameMode.Add(playerConfig);

            SetPlayerPos(playerConfig.inputHandler.gameObject, startPoints[playerConfig.PlayerIndex].position);
        }
    }

    protected virtual void InitializeManager()
    {
        miniGameRunning = initializedManager = true;
        levelManager = FindObjectOfType<LevelManager>();

        if (playersToEnterGameMode.Count > 1)
        {
            StartNewRound();
        }

    }

    protected virtual void UpdateMiniGame()
    {
        for (int i = 0; i < playersCurrentlyInGameMode.Count; i++)
        {
            GameObject player = playersCurrentlyInGameMode[i].inputHandler.gameObject;
            if (IsPlayerKickedOut(player) && roundRunning)
            {
                KickPlayer(playersCurrentlyInGameMode[i]);
            }

        }
    }

    protected virtual void KickPlayer(PlayerConfiguration player)
    {
        playersCurrentlyInGameMode.Remove(player);
        player.inputHandler.gameObject.SetActive(false);
        if (playersCurrentlyInGameMode.Count <= 1)
        {
            StartCoroutine(EndRound(playersCurrentlyInGameMode[0]));
        }
    }

    private IEnumerator EndRound(PlayerConfiguration playerConfig)
    {
        roundRunning = false;
        playersCurrentlyInGameMode[0].AddPlayerScore(scoreAddedByWinningRound);
        PlayerConfigurationManager.Instance.SetPlayerInputs(false);
        PlayerConfigurationManager.Instance.PlayWinnerVideo(playerConfig);
        yield return new WaitForSeconds(8.5f); // 8.5 the time it takes to play each of the winners' videos
        if (roundCount >= 3)
            EndMiniGame();
        else
            StartNewRound();
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

        for (int i = 0; i < playersToEnterGameMode.Count; i++)
        {
            GameObject player = playersToEnterGameMode[i].inputHandler.gameObject;
            player.SetActive(true);
            SetPlayerPos(player, startPoints[playersToEnterGameMode[i].PlayerIndex].position);
            playersCurrentlyInGameMode.Clear();
            foreach (var playerConfig in playersToEnterGameMode) playersCurrentlyInGameMode.Add(playerConfig);
        }

        ParticleManager.InstanciateParticleEffect(countingCanvas, transform.position, Quaternion.identity, timeToStartNewRound);
        StartCoroutine(ActiveNewRound());
    }

    private void SetPlayerPos(GameObject player, Vector3 pos)
    {
        if (!player.TryGetComponent<Rigidbody>(out Rigidbody rb)) return;
        rb.velocity = Vector3.zero;
        rb.MovePosition(pos);
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


    public virtual void EndMiniGame()
    {
        EndingMiniGame();
        StartEndMiniGame();
    }

    public virtual void StartEndMiniGame()
    {
        /// for deleting stuff when mini game is over
    }

    private void EndingMiniGame()
    {
        miniGameRunning = false;
        for (int i = 0; i < playersToEnterGameMode.Count; i++)
        {
            GameObject player = playersToEnterGameMode[i].inputHandler.gameObject;
            player.SetActive(false);
        }

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
