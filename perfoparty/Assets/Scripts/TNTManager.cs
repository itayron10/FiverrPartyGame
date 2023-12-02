using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNTManager : MonoBehaviour
{
    [SerializeField] TNT currentTntInMap;
    [SerializeField] TNT tntPrefab;
    private GameModeManager miniGameManager;


    private void Start()
    {
        miniGameManager = FindObjectOfType<GameModeManager>();
    }

    public void SpawnNewTnt()
    {
        if (!miniGameManager.IsGameRunning) return;
        TNT newTnt = Instantiate(tntPrefab.gameObject, transform.position, Quaternion.identity).GetComponent<TNT>();
        newTnt.SetTntActive(true);
        PlayerConfiguration playerToGetTnt = miniGameManager.GetPlayersCurrentlyInGameMode[Random.Range(0, miniGameManager.GetPlayersCurrentlyInGameMode.Count)];
        playerToGetTnt.inputHandler.GetComponent<TntHolder>().EquipTnt(newTnt);
        currentTntInMap = newTnt;
    }

    public void DestroyCurrentTnt()
    {
        if (!currentTntInMap) return;
        currentTntInMap.GetTntHolder.GetComponent<TntHolder>().UnequipTnt();
        Destroy(currentTntInMap.gameObject);
    }

    public IEnumerator DelayTntActive(float duration)
    {
        currentTntInMap.SetTntActive(false);
        yield return new WaitForSeconds(duration);
        currentTntInMap.SetTntActive(true);
    }

}
