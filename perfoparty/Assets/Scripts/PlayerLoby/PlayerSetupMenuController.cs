using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerSetupMenuController : MonoBehaviour
{
    [SerializeField] RenderTexture[] renderTextures;
    [SerializeField] TextMeshProUGUI titleText, readyText;
    [SerializeField] RawImage previewImage;
    [SerializeField] GameObject menuPanel;
    [SerializeField] Button readyButton;
    [SerializeField] SoundScriptableObject clickSound;
    private SoundManager soundManager;
    private int playerIndex;


    private void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    private void OnLevelWasLoaded(int level)
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    public void setPlayerIndex(int pi)
    {
        playerIndex = pi;
        titleText.SetText("Player " + (pi + 1).ToString());

        previewImage.texture = renderTextures[PlayerConfigurationManager.Instance.GetPlayerCharacterIndex(playerIndex)];
    }

    public void IncreaseIndex()
    {
        soundManager.PlaySound(clickSound);
        PlayerConfigurationManager.Instance.IncreaseCharacterSelectIndex(playerIndex);
        Debug.Log($"Texture Number: {playerIndex}");
        previewImage.texture = renderTextures[PlayerConfigurationManager.Instance.GetPlayerCharacterIndex(playerIndex)];
    }

    public void DecreaseIndex()
    {
        soundManager.PlaySound(clickSound);
        PlayerConfigurationManager.Instance.DecreaseCharacterSelectIndex(playerIndex);
        previewImage.texture = renderTextures[PlayerConfigurationManager.Instance.GetPlayerCharacterIndex(playerIndex)];
    }

    public void ReadyPlayer()
    {
        soundManager.PlaySound(clickSound);
        PlayerConfigurationManager.Instance.ReadyPlayer(playerIndex);
        Destroy(gameObject);
        /*menuPanel.gameObject.SetActive(false);
        readyText.gameObject.SetActive(true);*/
    }
}
