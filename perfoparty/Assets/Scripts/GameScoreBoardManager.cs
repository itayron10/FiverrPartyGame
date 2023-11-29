using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameScoreBoardManager : MonoBehaviour
{
    [SerializeField] Transform scoreBoardParant;
    [SerializeField] GameObject scoreDisplayPrefab;
    [SerializeField] List<GameObject> scoreDisplays;

    private void Start()
    {
        foreach (PlayerConfiguration playerConfig in PlayerConfigurationManager.Instance.GetPlayerConfigs())
        {
            playerConfig.SetPlayerScoreDisplayText(Instantiate(scoreDisplayPrefab, scoreBoardParant).GetComponentInChildren<TextMeshProUGUI>());
        }
    }
}
