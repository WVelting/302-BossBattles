using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private EnemyController enemy;
    public PointAt target;
    public float bulletSpeed = 10;
    public float bulletImpactDis = .05f;
    public float lifeSpan = 10;

    void Start()
    {
        enemy = FindObjectOfType<EnemyController>();
    }

    void Update()
    {
        transform.localPosition += transform.forward*Time.deltaTime*bulletSpeed;
        lifeSpan -= Time.deltaTime;

        if((transform.position - enemy.transform.position).sqrMagnitude < bulletImpactDis*bulletImpactDis) 
        {
            enemy.health -= 10;
            Destroy(gameObject);
        }
        if(lifeSpan <= 0) Destroy(gameObject);
        
    }
}
