using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target;

    float pitch = 0;
    float yaw = 0;

    public float lookSensitivity = 2;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
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
    }
}
