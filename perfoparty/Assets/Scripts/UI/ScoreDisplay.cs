using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    public PlayerConfiguration playerConfig { set; private get; }
    [SerializeField] TextMeshProUGUI displayText;
    [SerializeField] Image displayIcon;


    private void Start()
    {
        displayIcon.sprite = playerConfig.playerCharacter.playerIcon;
        displayText.color = playerConfig.playerCharacter.playerColor;
    }

    private void Update()
    {
        displayText.text = $": {playerConfig.playerScore}";
    }
}
