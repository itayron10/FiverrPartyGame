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
        var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs();
        for (int i = 0; i < playerConfigs.Count; i++)
        {
            if (initializedPlayers.Contains(playerConfigs[i]) || !playerConfigs[i].isReady) { continue; }
            Debug.Log($"Player Index: {playerConfigs[i].PlayerIndex} Mesh: {playerConfigs[i].playerMeshObject}");
            int minPosition = Mathf.Min(i, playerSpawnPoints.Length - 1);
            GameObject player = Instantiate(playerPrefab, playerSpawnPoints[minPosition].position, playerSpawnPoints[minPosition].rotation, gameObject.transform);
            player.GetComponent<InputHandler>().InitializePlayer(playerConfigs[i]);
            player.GetComponent<PlayerMovement>().animator = player.GetComponentInChildren<Animator>();
            
            initializedPlayers.Add(playerConfigs[i]);
        }
    }

    public void SetPlayersToSpawnPoints()
    {
        var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs();

        for (int i = 0; i < playerConfigs.Count; i++)
        {
            playerConfigs[i].inputHandler.transform.position = playerSpawnPoints[i].position;
        }
    }
}
