using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [SerializeField] GameObject stunEffect;

    public void PralysisPlayers(GameObject playerWhoPunched)
    {
        foreach (var player in PlayerConfigurationManager.Instance.GetPlayerConfigs())
        {
            if (player.inputHandler.gameObject != playerWhoPunched) StartCoroutine(StunPlayer(player));
        }
    }

    private IEnumerator StunPlayer(PlayerConfiguration player)
    {
        player.Input.DeactivateInput();
        ParticleManager.InstanciateParticleEffect(stunEffect, player.inputHandler.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(3f);
        player.Input.ActivateInput();
    }
}
