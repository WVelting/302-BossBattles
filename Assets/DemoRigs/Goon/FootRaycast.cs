using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootRaycast : MonoBehaviour
{

    public float raycastLength = 2;


    

    ///<summary>The local-space rotation of where the IK spawned</summary>
    private Quaternion startingRot;

    ///<summary>The world-space position of the ground above/below the foot IK</summary>
    private Vector3 groundPosition;
    ///<summary>The world-space rotation for the foot to be aligned w/ ground</summary>
    private Quaternion groundRotation;

    ///<summary>The local-space position to ease towards.</summary>
    private Vector3 targetPos;

    ///<summary>The local-space position of where the IK spawned</summary>
    private Vector3 startingPos;

    private Vector3 footSeparateDir;
    void Start()
    {
        
        //local space starting rotation of the foot
        startingRot = transform.localRotation;
        //local space starting position of IK
        startingPos = transform.localPosition;

        footSeparateDir = (startingPos.x > 0) ? Vector3.right : Vector3.left;
    }

    void Update()
    {
        //FindGround();
        //ease towards target:
        transform.localPosition = AniMath.Ease(transform.localPosition, targetPos, .01f);
    }

    public void SetLocalPosition(Vector3 p)
    {
        targetPos = p;
    }

    public void SendHomePosition()
    {
        targetPos = startingPos;
    }

    public void SetOffsetPosition(Vector3 p, float separateAmount = 0)
    {
        targetPos = startingPos + p + separateAmount * footSeparateDir;
    }

    private void FindGround()
    {
        Vector3 origin = transform.position + Vector3.up * raycastLength / 2;
        Vector3 direction = Vector3.down;

        Debug.DrawRay(origin, direction * raycastLength, Color.red);

        if (Physics.Raycast(origin, direction, out RaycastHit hitInfo, raycastLength))
        {
            //set foot position to space on ground hit by raycast
            groundPosition = hitInfo.point + Vector3.up * startingPos.y;
            //world-space rotation of the foot  Quaternion multiplication is NOT communitive
            Quaternion worldNeutral = transform.parent.rotation * startingRot;
            //set the rotation of foot to match the slope of the normal hit by the raycast
            groundRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal) * worldNeutral;
        }
    }
}
