using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoadButton : EventButton
{
    private enum LevelLoadType { nextLevel, levelIndex, levelName, currentLevel }
    [SerializeField] LevelLoadType loadType;
    [SerializeField] int loadLevelIndex;
    [SerializeField] string loadLevelName;
    private PauseManager pauseManager;
    private LevelManager levelManager;

    public override void FindPrivateObjects()
    {
        base.FindPrivateObjects();
        pauseManager = FindObjectOfType<PauseManager>();
        levelManager = FindObjectOfType<LevelManager>();
    }

    public override void OnClick()
    {
        base.OnClick();
        pauseManager?.UnpauseGame();
        if (!levelManager) return;
        if (LevelLoadType.nextLevel == loadType)
            levelManager.LoadNextLevel();
        else if (LevelLoadType.levelIndex == loadType)
            levelManager.LoadLevelByIndex(loadLevelIndex);
        else if (LevelLoadType.levelName == loadType)
            levelManager.LoadLevelByString(loadLevelName);
        else if (LevelLoadType.currentLevel == loadType)
            levelManager.LoadCurrentLevel();    


    }
}
