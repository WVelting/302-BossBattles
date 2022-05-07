using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent (typeof(Animator))]
public class HeroMovement : MonoBehaviour
{

    enum Mode{
        Idle,
        Walk,
        InAir,
        Aim,
        Reload,
        Dead

    }

    private PointAt target;
    private CharacterController pawn;
    private Mode mode = Mode.Idle;
    private Vector3 input;
    private float walkTime;
    private Quaternion targetRot;
    ///<summary>The current vertical velocity in meters/second</summary>
    private Camera cam;
    public CameraController camController;
    private Animator anim;
    private Vector3 startingPos;
    private float clipSize = 7;
    private float health = 50;
    private float deathTimer = 5;
    

    public FootRaycast footLeft;
    public FootRaycast footRight;
    public Transform torso;
    public Transform bulletSpawn;
    public Projectile bullet;
    public float footSeparateScalar = -.2f;
    public float walkSpreadY = .4f;
    public float walkSpreadZ = .8f;
    public float walkFootSpeed = 4;
    public float reloadTimer = 5;

    public float walkSpeed = 5;
    public float velocityY = 0;
    public float gravity = 20;
    public float jumpImpulse = 10;
    public float v;
    public float h;

    void Start()
    {
        pawn = GetComponent<CharacterController>();   
        anim = GetComponent<Animator>();
        target = GetComponent<PointAt>();
        cam = Camera.main; 
        startingPos = torso.transform.localPosition;
        AnimateIdle();
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.LeftShift)) 
        {
            walkSpeed = 10;
            walkFootSpeed = 8;
        }
        else{
            walkSpeed = 5;
            walkFootSpeed = 4;
        }
        //print(mode);
        //setting up stuff
        Vector3 vToTarget = transform.position - target.target.position;
        if(Input.GetMouseButton(1) && vToTarget.sqrMagnitude < target.visionDis*target.visionDis) mode = Mode.Aim;
        else mode = Mode.Idle;
        v = Input.GetAxisRaw("Vertical");
        h = Input.GetAxisRaw("Horizontal");

        Vector3 camForward = cam.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = Vector3.Cross(Vector3.up, camForward);

        input = camForward * v + camRight * h;
        if(input.sqrMagnitude > 1) input.Normalize();


        float threshold = .1f;

        if(mode != Mode.Aim && mode != Mode.Dead)
        {
            //set movement mode based on movement input:
            mode = (input.sqrMagnitude > threshold*threshold) ? Mode.Walk : Mode.Idle;

        }

        if(mode == Mode.Walk) targetRot = Quaternion.LookRotation(input, Vector3.up);

        if(pawn.isGrounded)
        {
            if(Input.GetButtonDown("Jump"))
            {
                velocityY = -jumpImpulse;
            }
        }

        velocityY += gravity * Time.deltaTime;

        
        if(mode == Mode.Walk || mode == Mode.InAir || mode == Mode.Idle)
        {
        pawn.Move((input * walkSpeed + Vector3.down * velocityY) * Time.deltaTime);

        }

        if(pawn.isGrounded) velocityY = 0;
        else mode = Mode.InAir;     
        if(Input.GetKeyDown(KeyCode.Q)) health -= 50;
        if(health<= 0) mode = Mode.Dead;   
        // animate the feet
        Animate();
    }

    void Animate()
    {
        if(mode == Mode.Walk) 
        {
            targetRot = Quaternion.LookRotation(input, Vector3.up);
            float eulerY = targetRot.eulerAngles.y;
            eulerY += 90;
            targetRot = Quaternion.Euler(targetRot.eulerAngles.x, eulerY, targetRot.eulerAngles.z);
            transform.rotation = AniMath.Ease(transform.rotation, targetRot, .01f);
        }
        switch(mode)
        {
            case Mode.Idle:
                AnimateIdle();
                anim.SetBool("isIdle", true);
                anim.SetBool("isAiming", false);
                if(Input.GetKey(KeyCode.R)) 
                {
                    anim.SetBool("isReloading", true);
                    clipSize = 7;
                }
                else anim.SetBool("isReloading", false);
                break;
            case Mode.Walk:
                AnimateWalk();
                anim.SetBool("isAiming", false);
                break;
            case Mode.Aim:
                AnimateIdle();
                anim.SetBool("isIdle", false);
                anim.SetBool("isAiming", true);
                anim.SetBool("isReloading", false);
                if(Input.GetMouseButtonDown(0)) AnimateShoot();
                break;
            case Mode.Dead:
                anim.SetBool("isIdle", false);
                anim.SetBool("isAiming", false);
                anim.SetBool("isReloading", false);
                anim.SetBool("isDead", true);
                deathTimer -= Time.deltaTime;
                if(deathTimer <= 0) Destroy(gameObject);
                break;
            case Mode.InAir:
                AnimateInAir();
                break;

                
                
        }
        

    }

    private void AnimateInAir()
    {
    }

    private void AnimateShoot()
    {
        if(clipSize > 0)
        {
            Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
            camController.Shake(.5f);
            clipSize--;
        }

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
