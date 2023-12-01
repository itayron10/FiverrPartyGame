using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake instance;
    private CinemachineVirtualCamera camera;
    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;
    private float shakeTimer;
    private float shakeIntensity;
    private float shakeTimerTime;


    private IEnumerator Start()
    {
        instance = this;
        yield return null;
        camera = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        cinemachineBasicMultiChannelPerlin = camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update() => HandleShake();

    public void Shake(ScreenShakeSettingsSO screenShakeSettings)
    {
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = screenShakeSettings.GetShakeStreangth;
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = screenShakeSettings.GetShakeFrequency;
        shakeTimer = shakeTimerTime = screenShakeSettings.GetShakeDuration;
        shakeIntensity = screenShakeSettings.GetShakeStreangth;
    }
    public void Shake(float streangth = 1f, float frequency = 0.1f, float duration = 0.3f)
    {
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = streangth;
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = frequency;
        shakeTimer = shakeTimerTime = duration;
        shakeIntensity = streangth;
    }

    private void HandleShake()
    {
        if (shakeTimer > 0f)
        {
            shakeTimer -= Time.deltaTime;
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain =
                Mathf.Lerp(shakeIntensity, 0f, 1f - (shakeTimer / shakeTimerTime));
        }
    }
}
