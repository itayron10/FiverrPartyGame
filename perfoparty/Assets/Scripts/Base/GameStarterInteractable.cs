using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameStarterInteractable : Interactable
{
    [SerializeField] string LevelNameOnGameStart;
    [SerializeField] TextMeshProUGUI votesDisplay;
    private List<GameObject> interactedPlayers = new List<GameObject>();
    private PlayerConfigurationManager configurationManager;
    private int playerVotes, votesToStartGame;
    private LevelManager levelManager;

    protected override void FindPrivateObjects()
    {
        base.FindPrivateObjects();
        levelManager = FindObjectOfType<LevelManager>();
        votesDisplay.text = $"{playerVotes}/{votesToStartGame}";
        configurationManager = PlayerConfigurationManager.Instance;
    }

    private void Update()
    {
        if (configurationManager.GetPlayerConfigs().Count > 0) votesToStartGame = Mathf.Max(2, (configurationManager.GetPlayerConfigs().Count / 2) + 1);
        votesDisplay.text = $"{playerVotes}/{votesToStartGame}";
    }

    public override void Interacte(GameObject interacter)
    {
        if (interactedPlayers.Contains(interacter)) return;
        base.Interacte(interacter);
        interactedPlayers.Add(interacter);
        playerVotes++;
        if (playerVotes >= votesToStartGame) StartGame();
    }

    public virtual void StartGame()
    {
        levelManager.LoadLevelByString(LevelNameOnGameStart);
        PlayerConfigurationManager.Instance.strikeMode = true;
    }
}
