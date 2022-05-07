using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public PointAt target;
    public float bulletSpeed = 10;
    public float bulletImpactDis = .05f;

    void Start()
    {
        target = FindObjectOfType<PointAt>();
    }

    void Update()
    {
        transform.localPosition += transform.forward*Time.deltaTime*bulletSpeed;

        if((transform.position - target.target.transform.position).sqrMagnitude < .05f) Destroy(gameObject);
        
    }
}
