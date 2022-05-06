using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorpionStickyFeet : MonoBehaviour
{

    public AnimationCurve curveStepVert;
    public Transform raycastSource;
    public float offsetLength = 1;
    

    private Quaternion startingRot;

    private Vector3 previousGroundPos;
    private Vector3 groundPos;

    private Quaternion previousGroundRot;
    private Quaternion groundRot;

    private float animLength = .25f;
    private float animCurrentTime = 0;
    private float animOffset = 1;


    private bool isAnimating 
    {
        get
        {
            return (animCurrentTime < animLength);
        }
    }

    
    
    public float raycastLength = 2;
    public float disToWalk = .5f;


    void Start()
    {
       startingRot = transform.localRotation;
       animOffset = offsetLength;

    }

    void Update()
    {
        if(isAnimating)
        {
            if(animOffset > 0) animOffset -= Time.deltaTime;
            else
            {

                animCurrentTime += Time.deltaTime;

                float p = Mathf.Clamp(animCurrentTime/animLength, 0, 1);
                float y = curveStepVert.Evaluate(p);

                //move position
                transform.position = AniMath.Lerp(previousGroundPos, groundPos, p) + new Vector3(0,y,0);

                
                //move rotation
                transform.rotation = AniMath.Lerp(previousGroundRot, groundRot, p);
            }

        }
        else
        {
            transform.position = groundPos;
            transform.rotation = groundRot;

            Vector3 vFromStart = transform.position - raycastSource.position;
            if(vFromStart.sqrMagnitude > disToWalk * disToWalk)
            {
                FindGround();
            }
        }
    }

    private void FindGround()
    {
        Vector3 origin = raycastSource.position + Vector3.up * raycastLength / 2;
        Vector3 direction = Vector3.down;

        Debug.DrawRay(origin, direction * raycastLength, Color.red);

        if (Physics.Raycast(origin, direction, out RaycastHit hitInfo, raycastLength))
        {
            previousGroundPos = groundPos;
            previousGroundRot = groundRot;

            //set foot position to space on ground hit by raycast
            groundPos = hitInfo.point;
            //world-space rotation of the foot  Quaternion multiplication is NOT communitive
            Quaternion worldNeutral = transform.parent.rotation * startingRot;
            //set the rotation of foot to match the slope of the normal hit by the raycast
            groundRot = Quaternion.FromToRotation(Vector3.up, hitInfo.normal) * worldNeutral;

            animCurrentTime = 0;
            animOffset = offsetLength;
        }
    }

}
