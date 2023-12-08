using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossMiniGameManager : GameModeManager
{
    private BossAI bossAI;

    protected override void InitializeManager()
    {
        base.InitializeManager();
        bossAI = FindObjectOfType<BossAI>();
    }

    protected override void UpdateMiniGame()
    {
        base.UpdateMiniGame();
        bossAI.active = roundRunning;
    }


}
