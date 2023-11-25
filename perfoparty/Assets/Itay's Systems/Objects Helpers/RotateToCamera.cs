using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToCamera : MonoBehaviour
{
    [SerializeField] bool rotateSmoothly = false;
    [SerializeField] float rotateSpeed = 1f;
    private Camera mainCam;

    private void OnEnable() => mainCam = Camera.main;

    void Update()
    {
        Vector3 cameraDir = -(mainCam.transform.position - transform.position);
        Vector3 cameraDirY = new Vector3(transform.eulerAngles.x, cameraDir.y, transform.eulerAngles.z); 

        if (rotateSmoothly)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(cameraDirY), rotateSpeed * Time.deltaTime);
        else
            transform.rotation = Quaternion.LookRotation(cameraDir);
    }
}
