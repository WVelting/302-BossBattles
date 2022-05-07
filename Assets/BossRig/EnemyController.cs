using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    enum Mode 
    {
        Idle,
        Aggro,
        Hit,
        Attack,
        Dead
    }
    private Mode mode;
    private Animator anim;
    private Vector3 input;
    public HeroMovement hero;
    public float visDis = 20;
    public float attackDis = 5;
    public float walkSpeed = .5f;
    void Start()
    {
        anim = GetComponent<Animator>();
        hero = FindObjectOfType<HeroMovement>();        
        mode = Mode.Idle;

    }

    void Update()
    {
        mode = Mode.Idle;
        print(mode);
        Vector3 vToHero = hero.transform.position - transform.position;
        if(vToHero.sqrMagnitude < visDis*visDis && vToHero.sqrMagnitude > attackDis*attackDis) mode = Mode.Aggro;
        else if(vToHero.sqrMagnitude<attackDis*attackDis) mode = Mode.Attack;
        if(mode == Mode.Aggro)
        {
            vToHero.Normalize();
            vToHero.y = 0;
            transform.position+=vToHero*walkSpeed*Time.deltaTime;
        }
        
        AnimStates();
    }

    void AnimStates()
    {



        switch(mode)
        {
            case Mode.Idle:
                anim.SetBool("isIdle", true);
                break;
            case Mode.Aggro:
                anim.SetBool("isWalking", true);
                anim.SetBool("isIdle", false);
                break;
            case Mode.Attack:
                anim.SetBool("isWalking", false);
                anim.SetBool("isIdle", false);
                break;

        }
    }
}
