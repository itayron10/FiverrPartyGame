using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeButton : EventButton
{
    [SerializeField] InputHandler inputHandler;

    public override void OnClick()
    {
        base.OnClick();
        FindObjectOfType<PauseManager>()?.UnpauseGame(inputHandler.playerConfig);
    }
}
