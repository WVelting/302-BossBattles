using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class GoonMovement : MonoBehaviour
{

    private CharacterController pawn;
    public FootRaycast footLeft;
    public FootRaycast footRight;
    public float walkSpreadX = .2f;
    public float walkSpreadY = .4f;
    public float walkSpreadZ = .8f;
    public float walkFootSpeed = 4;

    public float walkSpeed = 5;

    void Start()
    {
        pawn = GetComponent<CharacterController>();    
    }

    void Update()
    {
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        Vector3 move = transform.forward * v + transform.right * h;
        if(move.sqrMagnitude > 1) move.Normalize();

        pawn.SimpleMove(move * walkSpeed);

        // animate the feet
        AnimateWalk();
    }

    delegate void MoveFoot(float time, float x, FootRaycast foot);

    void AnimateWalk()
    {
        //DRY - Don't Repeat Yourself
        
        MoveFoot moveFoot = (t, x, foot)=>{ 
        
            float y = Mathf.Cos(t) * walkSpreadY;
            float z = Mathf.Sin(t) * walkSpreadZ;

            if(y < 0) y = 0;

            y+= .177f;

            if(foot)foot.transform.localPosition = new Vector3(x,y,z);
        };
        float t = Time.time * walkFootSpeed;

        moveFoot.Invoke(t, -walkSpreadX, footLeft);
        moveFoot.Invoke(t+Mathf.PI, walkSpreadX, footRight);
    }
}
