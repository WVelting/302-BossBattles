using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class GuardianMovement : MonoBehaviour
{

    public float walkSpeed = 5;
    private Animator anim;


    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float v = Input.GetAxisRaw("Vertical");

        transform.position += transform.forward * v * Time.deltaTime * walkSpeed;

        anim.SetFloat("Speed", Mathf.Abs(v*walkSpeed));

    }
}
