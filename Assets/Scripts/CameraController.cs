using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target;
    private Camera cam;

    float pitch = 0;
    float yaw = 0;

    public float lookSensitivity = 2;
    private float shakeTimer;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cam = Camera.main;
    }

    void LateUpdate()
    {
        
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        yaw += mx * lookSensitivity;
        pitch += -my * lookSensitivity;

        pitch = Mathf.Clamp(pitch, 0, 60);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0);

        if(target)
        {
            transform.position = AniMath.Ease(transform.position, target.position, .01f);
            
        }
        //Can't seem to get the camera to return to normal
        //UpdateShake();
    }


    void UpdateShake()
    {
        if(shakeTimer < 0) return;

        shakeTimer -= Time.deltaTime;

        float p = shakeTimer / 1;
        p = p*p;
        p = AniMath.Lerp(1, .98f, p);

        Quaternion randomRot = AniMath.Lerp(Random.rotation, Quaternion.identity, p);
        cam.transform.localRotation  *= randomRot;
    }

    public void Shake(float time)
    {
        if(time > shakeTimer) shakeTimer = time;
    }
}
