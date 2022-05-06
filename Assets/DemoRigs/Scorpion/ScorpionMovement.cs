using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ScorpionMovement : MonoBehaviour
{
    
    enum Mode{
        Idle,
        Walk,
        InAir

    }


    private CharacterController pawn;
    private Mode mode = Mode.Idle;
    private Vector3 input;
    private Quaternion targetRot;
    ///<summary>The current vertical velocity in meters/second</summary>
    

    private Camera cam;


    //public Transform groundRing;
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
        GetPlayerInputRelativeToCamera();

        if (pawn.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                velocityY = -jumpImpulse;
            }
        }

        velocityY += gravity * Time.deltaTime;

        pawn.Move((input * walkSpeed + Vector3.down * velocityY) * Time.deltaTime);

        if (pawn.isGrounded) velocityY = 0;
        else mode = Mode.InAir;
        // animate the feet
        Animate();
    }

    private void GetPlayerInputRelativeToCamera()
    {
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        Vector3 camForward = cam.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = Vector3.Cross(Vector3.up, camForward);

        input = camForward * v + camRight * h;
        if (input.sqrMagnitude > 1) input.Normalize();


        float threshold = .1f;
        //set movement mode based on movement input:
        mode = (input.sqrMagnitude > threshold * threshold) ? Mode.Walk : Mode.Idle;
    }

    void Animate()
    {

        if(mode == Mode.Walk) targetRot = Quaternion.LookRotation(input, Vector3.up);
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
    }


    void AnimateWalk()
    {
    }

}
