using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFAMiniGameManager : GameModeManager
{
    [SerializeField] Transform[] weaponSpawnPoints;
    [SerializeField] Weapon[] weaponPrefabsToSpawn;
    [SerializeField] float newPlayerKnockBack;
    private List<Weapon> weaponInstances = new List<Weapon>();

    protected override void InitializeManager()
    {
        base.InitializeManager();
        for (int i = 0; i < playersToEnterGameMode.Count; i++)
        {
            PunchController punchController = playersToEnterGameMode[i].inputHandler.GetComponent<PunchController>();
            punchController.SetKnockBack(newPlayerKnockBack);
        }
    }

    public override void StartNewRound()
    {
        base.StartNewRound();

        foreach (PlayerConfiguration player in playersToEnterGameMode)
        {
            if (player.inputHandler.TryGetComponent<BasicHealth>(out BasicHealth basicHealth))
            {
                basicHealth.SetCurrentHealth(basicHealth.GetMaxHealth);
            }
        }

        DeleteGuns();

        for (int i = 0; i < weaponPrefabsToSpawn.Length; i++)
        {
            Weapon weaponInstance = Instantiate(weaponPrefabsToSpawn[i].gameObject, weaponSpawnPoints[i].position, Quaternion.identity).GetComponent<Weapon>();
            weaponInstances.Add(weaponInstance);
            Debug.Log("Creating This: " + weaponInstance.name);
        }
    }

    private void DeleteGuns()
    {
        if (weaponInstances.Count > 0)
        {
            for (int i = 0; i < weaponInstances.Count; i++)
            {
                Weapon weapon = weaponInstances[i];
                Debug.Log("Unequiping This: " + weapon.name);
                weapon.Unequip();
                Destroy(weapon.gameObject);
            }
            weaponInstances.Clear();
        }
    }

    public override void StartEndMiniGame()
    {
        base.StartEndMiniGame();
        DeleteGuns();
    }

    protected override bool IsPlayerKickedOut(GameObject player)
    {
        return base.IsPlayerKickedOut(player) || player.GetComponent<BasicHealth>().GetCurrentHealth <= 0;
    }
}
