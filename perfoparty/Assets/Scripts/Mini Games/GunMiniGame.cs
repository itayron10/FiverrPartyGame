using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMiniGame : GameModeManager
{
    [SerializeField] Transform gunSpawnPos;
    [SerializeField] Weapon gunPrefab;
    private Weapon gunInstance;

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

        DeleteGun();

        gunInstance = Instantiate(gunPrefab.gameObject, gunSpawnPos.position, Quaternion.identity).GetComponent<Weapon>();
        Debug.Log("Spawning the gun");

    }

    private void DeleteGun()
    {
        if (!gunInstance) return;
        Debug.Log("Gun instance");
        gunInstance.Unequip();
        Destroy(gunInstance.gameObject);
    }

   
    public override void StartEndMiniGame()
    {
        base.StartEndMiniGame();
        DeleteGun();
        foreach (PlayerConfiguration player in playersToEnterGameMode)
        {
            player.inputHandler.GetComponent<WeaponManager>().equipCoolDown = 1;
        }
    }

    protected override bool IsPlayerKickedOut(GameObject player)
    {
        return player.GetComponent<BasicHealth>().GetCurrentHealth <= 0 || base.IsPlayerKickedOut(player);
    }
}
