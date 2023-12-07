using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkourPlatform : MonoBehaviour
{
    [SerializeField] Color platformColor;
    [SerializeField] Vector3 fallingPos;
    [SerializeField] ScreenShakeSettingsSO platformFallingShake;
    [SerializeField] float speed;
    [SerializeField] SoundScriptableObject fallingSound;
    private Vector3 normalPos;
    private bool isDown;
    private CinemachineShake cinemachineShake;
    private SoundManager soundManager;

    public Color GetColor => platformColor;
    public bool IsDown => isDown;

    private void Awake()
    {
        normalPos = transform.position;
    }   

    private void Start()
    {
        cinemachineShake = CinemachineShake.instance;
        soundManager = FindObjectOfType<SoundManager>();
    }

    private void Update()
    {
        if (isDown)
               transform.position = Vector3.Lerp(transform.position, fallingPos, Time.deltaTime * speed);
        else
            transform.position = Vector3.Lerp(transform.position, normalPos, Time.deltaTime * speed);
    }

    public void Fall()
    {
        /// Platform should fall down here
        //transform.position = Vector3.Lerp(transform.position, fallingPos, Time.deltaTime * speed);
        cinemachineShake.Shake(platformFallingShake);
        soundManager.PlaySound(fallingSound);
        Debug.Log("Shaking " + cinemachineShake);
        isDown = true;
    }


    public void Raise()
    {
        /// Platform should raise up here
        //transform.position = Vector3.Lerp(transform.position, normalPos, Time.deltaTime * speed);
        isDown = false;
    }

}
