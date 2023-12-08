using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Podium : MonoBehaviour
{
    [SerializeField] Transform[] poduimSpots;
       

    private IEnumerator Start()
    {
        //List<PlayerConfiguration> sortedPlayers = PlayerConfigurationManager.Instance.GetPlayerConfigs().OrderByDescending(player => player.playerScore).ToList();
        
        PlayerConfigurationManager.Instance.SetPlayerInputs(false);
        
/*        PlayerConfigurationManager playerConfigurationManager = PlayerConfigurationManager.Instance;
        for (int i = 0; i < sortedPlayers.Count; i++)
        {
            playerConfigurationManager.SetPlayerToPosition(sortedPlayers[i], poduimSpots[i].position);
            sortedPlayers[i].inputHandler.GetComponent<PlayerMovement>().Dance();
            sortedPlayers[i].inputHandler.transform.localEulerAngles = new Vector3(0, 180, 0);
        }
*/
        yield return new WaitForSeconds(10f);
        PlayerConfigurationManager.Instance.SetPlayerInputs(true);
        FindObjectOfType<LevelManager>().LoadLevelByIndex(0);
    }

    private void Update()
    {
        List<PlayerConfiguration> sortedPlayers = PlayerConfigurationManager.Instance.GetPlayerConfigs().OrderByDescending(player => player.playerScore).ToList();
        PlayerConfigurationManager playerConfigurationManager = PlayerConfigurationManager.Instance;
        for (int i = 0; i < sortedPlayers.Count; i++)
        {
            playerConfigurationManager.SetPlayerToPosition(sortedPlayers[i], poduimSpots[i].position);
            sortedPlayers[i].inputHandler.GetComponent<PlayerMovement>().Dance();
            sortedPlayers[i].inputHandler.transform.localEulerAngles = new Vector3(0, 180, 0);
        }
    }



}
