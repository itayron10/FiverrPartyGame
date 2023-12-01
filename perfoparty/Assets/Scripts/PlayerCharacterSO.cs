using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="New Character", menuName ="Characters/Character")]
public class PlayerCharacterSO : ScriptableObject
{
    public GameObject meshPrefab;
    public Color playerColor;
    public WinningVideo playerWinningVideo;
    public Sprite playerIcon;
}
