using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Screen Shake Settings", menuName = "Screen Shake/Settings")]
public class ScreenShakeSettingsSO : ScriptableObject
{
    [SerializeField] float shakeStreangth = 1f, shakeFrequency = 0.1f, shakeDuration = 0.3f;
    public float GetShakeStreangth => shakeStreangth;
    public float GetShakeFrequency => shakeFrequency;
    public float GetShakeDuration => shakeDuration;

}
