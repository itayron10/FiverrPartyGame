using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Podium : MonoBehaviour
{
    [SerializeField] Transform firstPodium, secondPodium, thirdPodium;

    private void Start()
    {
        PlayerConfigurationManager playerConfigurationManager = PlayerConfigurationManager.Instance;
        (PlayerConfiguration highestScore, PlayerConfiguration secondHighestScore, PlayerConfiguration thirdHighestScore) = playerConfigurationManager.GetThreeHighestScorePlayers();
        playerConfigurationManager.SetPlayerToPosition(highestScore, firstPodium.position);
        playerConfigurationManager.SetPlayerToPosition(secondHighestScore, secondPodium.position);
        playerConfigurationManager.SetPlayerToPosition(thirdHighestScore, thirdPodium.position);
    }
}
