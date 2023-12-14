using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkourMiniGameManager : GameModeManager
{
    [SerializeField] ParkourPlatform[] parkourPlatforms;
    [SerializeField] float platformFallingCooldown, pickingRaisingCooldown;
    [SerializeField] MeshRenderer selectedPlatformDisplay;
    [SerializeField] PunchGiver punchObjectPrefab;
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
        foreach (var player in playersToEnterGameMode)
        {
            player.inputHandler.GetComponent<PunchController>().hasPunchAbility = false;
            player.inputHandler.GetComponent<PunchController>().SetKnockBack(5);
            player.inputHandler.GetComponent<BasicHealth>().SetCurrentHealth(100);
        }
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
        float random = Random.value;
        Debug.Log(random);
        if (random > 0.7f)
        {
            ParkourPlatform giverPlatform = parkourPlatforms[Random.Range(0, parkourPlatforms.Length)];
            Instantiate(punchObjectPrefab.gameObject, giverPlatform.transform.position + Vector3.up, Quaternion.identity, giverPlatform.transform);
        }
        soundManager.PlaySound(selectingPlatformSound);
        ParkourPlatform parkourPlatform = parkourPlatforms[Random.Range(0, parkourPlatforms.Length)];
        pickedPlatform = true;
        selectedPlatformDisplay.material.SetColor("_Color", parkourPlatform.GetColor);
        return parkourPlatform;
    }
}
