using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkourMiniGameManager : GameModeManager
{
    [SerializeField] ParkourPlatform[] parkourPlatforms;
    [SerializeField] float platformFallingCooldown, pickingRaisingCooldown;
    [SerializeField] MeshRenderer selectedPlatformDisplay;
    [SerializeField] SoundScriptableObject selectingPlatformSound;
    private bool pickedPlatform = false;
    private ParkourPlatform currentFallingPlatform;
    private SoundManager soundManager;


    protected override void InitializeManager()
    {
        base.InitializeManager();
        soundManager = FindObjectOfType<SoundManager>();
    }

    public override void StartNewRound()
    {
        base.StartNewRound();
        ResetAllPlatforms();
    }

    protected override void UpdateMiniGame()
    {
        base.UpdateMiniGame();
        if (!pickedPlatform && roundRunning) StartFallingPlatform();

    }

    private void StartFallingPlatform()
    {
        StartCoroutine(PlatformFalling());
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
        soundManager.PlaySound(selectingPlatformSound);
        ParkourPlatform parkourPlatform = parkourPlatforms[Random.Range(0, parkourPlatforms.Length)];
        pickedPlatform = true;
        selectedPlatformDisplay.material.SetColor("_Color", parkourPlatform.GetColor);
        return parkourPlatform;
    }
}
