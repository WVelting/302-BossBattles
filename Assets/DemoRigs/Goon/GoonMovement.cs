using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class GoonMovement : MonoBehaviour
{

    enum Mode{
        Idle,
        Walk,
        InAir

    }


    private CharacterController pawn;
    private Mode mode = Mode.Idle;
    private Vector3 input;
    private float walkTime;
    private Quaternion targetRot;
    ///<summary>The current vertical velocity in meters/second</summary>
    

    public FootRaycast footLeft;
    public FootRaycast footRight;
    private Camera cam;
    public float footSeparateScalar = -.2f;
    public float walkSpreadY = .4f;
    public float walkSpreadZ = .8f;
    public float walkFootSpeed = 4;

    public float walkSpeed = 5;
    public float velocityY = 0;
    public float gravity = 20;
    public float jumpImpulse = 10;

    void Start()
    {
        pawn = GetComponent<CharacterController>();   
        cam = Camera.main; 
    }

    void Update()
    {
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        Vector3 camForward = cam.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = Vector3.Cross(Vector3.up, camForward);

        input = camForward * v + camRight * h;
        if(input.sqrMagnitude > 1) input.Normalize();


        float threshold = .1f;
        //set movement mode based on movement input:
        mode = (input.sqrMagnitude > threshold*threshold) ? Mode.Walk : Mode.Idle;

        if(mode == Mode.Walk) targetRot = Quaternion.LookRotation(input, Vector3.up);

        if(pawn.isGrounded)
        {
            if(Input.GetButtonDown("Jump"))
            {
                velocityY = -jumpImpulse;
            }
        }

        velocityY += gravity * Time.deltaTime;
        
        pawn.Move((input * walkSpeed + Vector3.down * velocityY) * Time.deltaTime);

        if(pawn.isGrounded) velocityY = 0;
        else mode = Mode.InAir;
        // animate the feet
        Animate();
    }

    void Animate()
    {
        transform.rotation = AniMath.Ease(transform.rotation, targetRot, .01f);
        switch(mode)
        {
            case Mode.Idle:
                AnimateIdle();
                break;
            case Mode.Walk:
                AnimateWalk();
                break;
            case Mode.InAir:
                AnimateInAir();
                break;
        }
        

    }

    private void AnimateInAir()
    {
        //TODO:

        //lift legs
        //lift hands
        //adjust spikes/hair
        //use vertical velocity
    }

    void AnimateIdle()
    {
        footLeft.SendHomePosition();
        footRight.SendHomePosition();
        walkTime = 0;
    }

    delegate void MoveFoot(float time, FootRaycast foot);

    void AnimateWalk()
    {
        //DRY - Don't Repeat Yourself
        
        MoveFoot moveFoot = (t, foot)=>{ 
        
            float y = Mathf.Cos(t) * walkSpreadY; //vertical movement
            float lateral = Mathf.Sin(t) * walkSpreadZ; //forward/backward
            
            Vector3 localDir = foot.transform.parent.InverseTransformDirection(input);
            Vector3 separateDir = Vector3.Cross(Vector3.up, localDir);


            float z = lateral * localDir.z;
            float x = lateral * localDir.x;

            float alignment = Mathf.Abs(Vector3.Dot(localDir, Vector3.forward));
            
            



            if(y < 0) y = 0;


            if(foot)foot.SetOffsetPosition(new Vector3(x,y,z), footSeparateScalar * alignment);
        };

        walkTime += Time.deltaTime * input.sqrMagnitude * walkFootSpeed;


        
        moveFoot.Invoke(walkTime, footLeft);
        moveFoot.Invoke(walkTime+Mathf.PI, footRight);
    }
}
