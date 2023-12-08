using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] List<SoundSource> soundSources; // list of all the sound sources the sounds can use

    
    /// <summary>
    /// method for playing any sound it can recieve a custom audio source or use the sound's audio source
    /// which is one of the global sound sources
    /// </summary>
    public void PlaySound(SoundScriptableObject sound, AudioSource audioSource = null)
    {
        // if there is no sound then just log an error
        if (sound == null) { Debug.LogError("The Sound is null, did you forgot to set it?"); return; }
        // get the audio source of the sound if it's not assainged
        if (audioSource == null) audioSource = GetSoundAudioSource(sound);
        // set the audio source settings by the sound
        SetAudioSourceSettings(sound, audioSource);
        // play the audio clip of the sound based on if the sound is sfx or music
        PlayAudioSource(sound, audioSource);
    }

    public void StopSound(AudioSource audioSource)
    {
        audioSource.Stop();
    }


    public static void PlaySoundAtPosition(SoundScriptableObject sound, Vector3 playPosition, float spatialBlend = 1f) 
    {
        // if there is no sound then just log an error
        if (sound == null) { Debug.LogError("The Sound is null, did you forgot to set it?"); return; }
        // create the temp object
        GameObject tempAudioHolder = new GameObject("TempAudio");
        // add to teh audio holder an audio source
        AudioSource audioSource = tempAudioHolder.AddComponent<AudioSource>();
        // set its position
        tempAudioHolder.transform.position = playPosition;
        // set its spatial blend
        audioSource.spatialBlend = spatialBlend;
        // set the audio source settings (volume and such) by the sound settings
        SetAudioSourceSettings(sound, audioSource);
        // play the sound in the position
        AudioClip audioClip = sound.GetAudioClip();
        audioSource.PlayOneShot(audioClip);
        // destroy object after clip duration
        Destroy(tempAudioHolder, audioClip.length);
    }

    private static void PlayAudioSource(SoundScriptableObject sound, AudioSource audioSource)
    {
        if (!audioSource) { return; }
        // plays one shot when it is a sfx and only play it when it is music so we can stop it
        if (sound.soundType == SoundScriptableObject.SoundType.soundEffect)
        {
            audioSource.PlayOneShot(sound.GetAudioClip());
        }
        else
        {
            audioSource.clip = sound.GetAudioClip();
            audioSource.Play();
        }
    }
    private static void SetAudioSourceSettings(SoundScriptableObject sound, AudioSource audioSource)
    {
        if (!audioSource) { return; }
        // set the loop based on the sound type
        audioSource.loop = sound.soundType == SoundScriptableObject.SoundType.music;
        // set the pitch of the audio source based on the min/max pitch of the sound
        audioSource.pitch = GetSoundPitch(sound);
        // set the volume of the audio source based on the min/max volume of the sound
        audioSource.volume = GetSoundVolume(sound);
    }


    // get a random sound volume based on the sound min and max volumes
    private static float GetSoundVolume(SoundScriptableObject sound) => Random.Range(sound.volume.x, sound.volume.y);
    
    // get a random sound pitch based on the sound min and max pitches
    private static float GetSoundPitch(SoundScriptableObject sound) => Random.Range(sound.pitch.x, sound.pitch.y);

    // get the sound's audio source based on the sound sources ID
    private AudioSource GetSoundAudioSource(SoundScriptableObject sound)
    {
        // find the audio source that matches the sound's sound source id
        for (int i = 0; i < soundSources.Count; i++)
        {
            SoundSource soundSource = soundSources[i];
            if (sound.soundSourceID == soundSource.soundSourceID) return soundSource?.audioSource;
        }

        return null;
    }
}

/// <summary>>
/// each sound source is used as audio source with an ID to 
/// connect this audio source with the sounds that has the same ID
/// </summary>

[System.Serializable]
public class SoundSource
{
    [SerializeField] string soundSourceName; // just for a little description of what sounds should be played in this source
    public int soundSourceID; // this ID is used to find the audio source and play the sounds with this ID in that source
    public AudioSource audioSource; // this audio source is connected to teh ID and any sound with that ID will be played with this audio source if it dose'nt have a custom source
}
