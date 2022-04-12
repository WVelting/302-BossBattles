using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class GoonMovement : MonoBehaviour
{

    private CharacterController pawn;

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

    }
}
