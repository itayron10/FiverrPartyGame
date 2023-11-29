using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class WinningVideo : MonoBehaviour
{
    [SerializeField] VideoPlayer video;

    void Start()
    {
        video.targetCamera = Camera.main;
        Destroy(gameObject, 8.5f);
    }

}
