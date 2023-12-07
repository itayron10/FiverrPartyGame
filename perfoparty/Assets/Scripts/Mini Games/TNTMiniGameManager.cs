using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNTMiniGameManager : GameModeManager
{
    [SerializeField] float newPlayerKnockBack;
    private TNTManager tntManager;

    protected override void InitializeManager()
    {
        tntManager = FindObjectOfType<TNTManager>();
        base.InitializeManager();
        for (int i = 0; i < playersToEnterGameMode.Count; i++)
        {
            PunchController punchController = playersToEnterGameMode[i].inputHandler.GetComponent<PunchController>();
            punchController.SetKnockBack(newPlayerKnockBack);
        }
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

    public override void StartEndMiniGame()
    {
        base.StartEndMiniGame();
        tntManager.DestroyCurrentTnt();
    }

    protected override void KickPlayer(PlayerConfiguration player)
    {
        base.KickPlayer(player);
        if (GetPlayersCurrentlyInGameMode.Count > 1)
        {
            if (player.inputHandler.GetComponent<TntHolder>().GetTnt != null)
            {
                tntManager.DestroyCurrentTnt();
                tntManager.SpawnNewTnt();
            }
        }

    }

    protected override bool IsPlayerKickedOut(GameObject player)
    {
        return player.GetComponent<BasicHealth>().GetCurrentHealth <= 0f || base.IsPlayerKickedOut(player);
    }
}
