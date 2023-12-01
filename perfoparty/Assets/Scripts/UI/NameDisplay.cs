using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameDisplay : MonoBehaviour
{
    [SerializeField] InputHandler inputHandler;
    [SerializeField] TextMeshProUGUI displayText;

    private void Start()
    {
        displayText.text = $"Player {inputHandler.playerConfig.PlayerIndex}";
        displayText.color = inputHandler.playerConfig.playerCharacter.playerColor;
    }

}
