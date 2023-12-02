using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNTMiniGameManager : GameModeManager
{
    private TNTManager tntManager;

    protected override void InitializeManager()
    {
        tntManager = FindObjectOfType<TNTManager>();
        base.InitializeManager();
    }

    public override void StartNewRound()
    {
        base.StartNewRound();

        foreach (PlayerConfiguration player in playersToEnterGameMode)
        {
            if (player.inputHandler.TryGetComponent<BasicHealth>(out BasicHealth basicHealth))
            {
                basicHealth.SetCurrentHealth(basicHealth.GetMaxHealth);
            }
        }

        tntManager.DestroyCurrentTnt();
        tntManager.SpawnNewTnt();
        tntManager.StopAllCoroutines();
        tntManager.StartCoroutine(tntManager.DelayTntActive(4));
    }

    protected override void EndMiniGame()
    {
        base.EndMiniGame();
        tntManager.DestroyCurrentTnt();
    }

    protected override bool IsPlayerKickedOut(GameObject player)
    {
        return player.GetComponent<BasicHealth>().GetCurrentHealth <= 0f || base.IsPlayerKickedOut(player);
    }
}
