using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundButton : EventButton
{
    public AudioMixer audioMixer;
    [SerializeField] float precentage;


    public override void OnClick()
    {
        base.OnClick();
        SetMasterVolume(precentage);
    }


    public void SetMasterVolume(float percentage)
    {
        percentage = Mathf.Clamp(percentage, 0f, 100f);

        float volume = Mathf.Log10(percentage / 100f) * 20f;

        audioMixer.SetFloat("MasterVol", volume);
    }
}
