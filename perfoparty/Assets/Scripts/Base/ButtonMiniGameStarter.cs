using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMiniGameStarter : GameStarterInteractable
{
    [SerializeField] Animator animator;
    [SerializeField] string pressAnimatorTrigger;

    public override void Interacte(GameObject interacter)
    {
        base.Interacte(interacter);
        animator.SetTrigger(pressAnimatorTrigger);
    }

    public override void StartGame()
    {
        base.StartGame();
        PlayerConfigurationManager.Instance.strikeMode = false;
    }
}
