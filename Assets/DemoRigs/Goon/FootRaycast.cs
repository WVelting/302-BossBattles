using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootRaycast : MonoBehaviour
{

    public float raycastLength = 2;


    private float distanceBetweenGroundAndIK = 0;
    private Quaternion startingRot;

    void Start()
    {
        //the height "y" above the ground plane when the ground plane is set to "0"
        distanceBetweenGroundAndIK = transform.localPosition.y;
        //local space starting rotation of the foot
        startingRot = transform.localRotation;
    }

    void Update()
    {

        Vector3 origin = transform.position + Vector3.up * raycastLength/2;
        Vector3 direction = Vector3.down;

        Debug.DrawRay(origin, direction * raycastLength, Color.red);
        
        if(Physics.Raycast(origin, direction, out RaycastHit hitInfo, raycastLength))
        {
            //set foot position to space on ground hit by raycast
            transform.position = hitInfo.point + Vector3.up * distanceBetweenGroundAndIK;
            //world-space rotation of the foot  Quaternion multiplication is NOT communitive
            Quaternion worldNeutral = transform.parent.rotation * startingRot;
            //set the rotation of foot to match the slope of the normal hit by the raycast
            transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal) * worldNeutral;
        }
    }
}
