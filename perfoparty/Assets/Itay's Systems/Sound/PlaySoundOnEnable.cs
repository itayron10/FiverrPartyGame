using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnEnable : MonoBehaviour
{
    [Header("References")]
    [SerializeField] SoundScriptableObject sound; // this sound will be played on enable
    private SoundManager soundManager;

    private void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    // play a sound when the object is enabeled
    private void OnEnable() => soundManager?.PlaySound(sound);
}
