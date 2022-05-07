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
        Charge,
        Attack,
        Dead
    }
    private Mode mode;
    private Animator anim;
    private Vector3 vToHero;
    private HeroMovement hero;

    public Transform tail;
    public Transform tailCharge;
    public Transform tailAttack;
    public EnemyProjectile bullets;
    public float visDis = 20;
    public float attackDis = 5;
    public float walkSpeed = .5f;
    public float attackTimer = 2;
    public float health = 150;
    public float deathTimer = 2;
    void Start()
    {
        anim = GetComponent<Animator>();
        hero = FindObjectOfType<HeroMovement>();        

    }

    void Update()
    {
        print(mode);
        vToHero = hero.transform.position - transform.position;
        if(health <= 0)
        {
            mode = Mode.Dead;
        }
        else if(vToHero.sqrMagnitude > visDis*visDis) mode = Mode.Idle;
        else if(vToHero.sqrMagnitude < visDis*visDis && vToHero.sqrMagnitude > attackDis*attackDis) mode = Mode.Aggro;
        else if(vToHero.sqrMagnitude<attackDis*attackDis && attackTimer >= 0) 
        {
            mode = Mode.Charge;
            attackTimer -= Time.deltaTime;
        }
        else if(vToHero.sqrMagnitude<attackDis*attackDis && attackTimer <= 0)
        {
            mode = Mode.Attack;
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
                vToHero.Normalize();
                vToHero.y = 0;
                transform.position+=vToHero*walkSpeed*Time.deltaTime;

                break;
            case Mode.Charge:
                anim.SetBool("isWalking", false);
                anim.SetBool("isIdle", false);
                attackTimer -= Time.deltaTime;
                tail.position = AniMath.Lerp(tail.position, tailCharge.position, .01f);
                break;
            case Mode.Attack:

                tail.position = AniMath.Lerp(tail.position, tailAttack.position, .01f);
                for(int x = 0; x <= 25; x++)
                {
                    Instantiate(bullets, tail.position, Quaternion.LookRotation(vToHero, Vector3.up));
                    


                }
                break;
            case Mode.Dead:
                anim.SetBool("isDead", true);
                deathTimer-=Time.deltaTime;
                if(deathTimer<= 0) Destroy(gameObject);
                break;

        }
    }
}
