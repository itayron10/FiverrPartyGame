using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameScoreBoardManager : MonoBehaviour
{
    [SerializeField] Transform scoreBoardParant;
    [SerializeField] ScoreDisplay scoreDisplayPrefab;


    private void Start()
    {
        foreach (PlayerConfiguration playerConfig in PlayerConfigurationManager.Instance.GetPlayerConfigs())
        {
            ScoreDisplay scoreDisplayInstance = Instantiate(scoreDisplayPrefab.gameObject, scoreBoardParant).GetComponent<ScoreDisplay>();
            scoreDisplayInstance.playerConfig = playerConfig;

            //playerConfig.SetPlayerScoreDisplayText(scoreDisplayInstance.GetDisplayText);

        }
    }
}
