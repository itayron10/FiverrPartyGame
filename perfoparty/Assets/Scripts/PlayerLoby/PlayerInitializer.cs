using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInitializer : MonoBehaviour
{
    [SerializeField] Transform[] playerSpawnPoints;
    [SerializeField] GameObject playerPrefab;
    private List<PlayerConfiguration> initializedPlayers = new List<PlayerConfiguration>();
    public static PlayerInitializer instance;

    private void Awake()
    {
        instance = this;
    }

    //void Start() => InitializePlayers();

    public void InitializePlayers()
    {
        var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();
        for (int i = 0; i < playerConfigs.Length; i++)
        {
            if (initializedPlayers.Contains(playerConfigs[i])) { continue; }
            initializedPlayers.Add(playerConfigs[i]);
            int minPosition = Mathf.Min(i, playerSpawnPoints.Length - 1);
            GameObject player = Instantiate(playerPrefab, playerSpawnPoints[minPosition].position, playerSpawnPoints[minPosition].rotation, gameObject.transform);
            player.GetComponent<InputHandler>().InitializePlayer(playerConfigs[i]);
            player.GetComponent<PlayerMovement>().animator = player.GetComponentInChildren<Animator>();
        }
    }
}
