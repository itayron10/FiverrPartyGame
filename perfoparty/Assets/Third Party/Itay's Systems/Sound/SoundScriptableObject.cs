using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Sound", menuName ="Sound System/Sound")]
public class SoundScriptableObject : ScriptableObject
{
    [Header("Settings")]
    public AudioClip[] audioClips; // list of all the audio clips this sound can play
    public Vector2 volume = new Vector2(1, 1); // the x value is the min value and the y is the max value
    public Vector2 pitch = new Vector2(1, 1); // the x value is the min value and the y is the max value
    [Header("References")]
    // this ID is connected to the sound manager's sound managers, the ID that matches with this one is the audio source that this sound will be played by when there is no custom audio source
    public int soundSourceID;
    // reference for the current clip index out of the audio clips this clip will be played when we play the sound
    private int currentClipIndex = 0;

    // this changes if the audio source is looping or not
    public enum SoundType { soundEffect, music }
    public SoundType soundType;

    // this changes the play oreder when playing this sound more than once
    public enum PlayOrder { random, inOrder }
    public PlayOrder playOrder;

    /// <summary>
    /// this method gets the current audio clip that plays when playing this sound and setting the new sound based on the play order
    /// </summary>
    public AudioClip GetAudioClip()
    {
        // get current clip
        AudioClip currentClip = audioClips[currentClipIndex >= audioClips.Length ? 0 : currentClipIndex];

        // set the next clip
        switch (playOrder)
        {
            case PlayOrder.inOrder:
                currentClipIndex = (currentClipIndex + 1) % audioClips.Length;
                break;
            case PlayOrder.random:
                currentClipIndex = Random.Range(0, audioClips.Length);
                break;
        }
        
        //return the current clip
        return currentClip;
    }
}
