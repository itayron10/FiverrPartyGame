using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkourMiniGameManager : GameModeManager
{
    [SerializeField] ParkourPlatform[] parkourPlatforms;
    [SerializeField] float platformFallingCooldown, pickingRaisingCooldown;
    [SerializeField] MeshRenderer selectedPlatformDisplay;
    private bool pickedPlatform = false;
    private ParkourPlatform currentFallingPlatform;
    private Coroutine fallingPlatformCoroutine;


    public override void StartNewRound()
    {
        base.StartNewRound();
        ResetAllPlatforms();
    }

    protected override void UpdateMiniGame()
    {
        base.UpdateMiniGame();
        if (!pickedPlatform) StartFallingPlatform();

    }

    private void StartFallingPlatform()
    {
        fallingPlatformCoroutine = StartCoroutine(PlatformFalling());
    }

    private void ResetAllPlatforms()
    {
        foreach (ParkourPlatform platform in parkourPlatforms) platform.Raise();
    }

    private IEnumerator PlatformFalling()
    {
        pickedPlatform = true;
        currentFallingPlatform = PickRandomPlatform();
        yield return new WaitForSeconds(platformFallingCooldown);
        currentFallingPlatform.Fall();
        yield return new WaitForSeconds(pickingRaisingCooldown);
        currentFallingPlatform.Raise();
        pickedPlatform = false;
    }

    private ParkourPlatform PickRandomPlatform()
    {
        ParkourPlatform parkourPlatform = parkourPlatforms[Random.Range(0, parkourPlatforms.Length)];
        pickedPlatform = true;
        selectedPlatformDisplay.material.SetColor("_Color", parkourPlatform.GetColor);
        return parkourPlatform;
    }
}
