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
    private int playerIndex;


    public void setPlayerIndex(int pi)
    {
        playerIndex = pi;
        titleText.SetText("Player " + (pi + 1).ToString());

        previewImage.texture = renderTextures[PlayerConfigurationManager.Instance.GetMeshIndex(playerIndex)];
    }

    public void IncreaseIndex()
    {
        PlayerConfigurationManager.Instance.IncreaseCharacterSelectIndex(playerIndex);
        Debug.Log($"Texture Number: {playerIndex}");
        previewImage.texture = renderTextures[PlayerConfigurationManager.Instance.GetMeshIndex(playerIndex)];
    }

    public void DecreaseIndex()
    {
        PlayerConfigurationManager.Instance.DecreaseCharacterSelectIndex(playerIndex);
        previewImage.texture = renderTextures[PlayerConfigurationManager.Instance.GetMeshIndex(playerIndex)];
    }

    public void ReadyPlayer()
    {
        PlayerConfigurationManager.Instance.ReadyPlayer(playerIndex);
        Destroy(gameObject);
        /*menuPanel.gameObject.SetActive(false);
        readyText.gameObject.SetActive(true);*/
    }
}
