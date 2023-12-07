using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NameDisplay : MonoBehaviour
{
    [SerializeField] InputHandler inputHandler;
    [SerializeField] TextMeshProUGUI displayText;
    [SerializeField] Image displayIcon; 

    private void Start()
    {
        displayText.text = $"Player {inputHandler.playerConfig.PlayerIndex}";
        displayText.color = displayIcon.color = inputHandler.playerConfig.playerCharacter.playerColor;
    }

}
