﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [Header("Set in Inspector: Enemy")]
    public float speed = 10f;     
    public float fireRate = 0.3f;  
    public float health = 10;
    public int score = 100;

    private BoundsCheck bndCheck;

    void Awake()
    {  
        bndCheck = GetComponent<BoundsCheck>();
    }

    public Vector3 pos
    {     
        get
        {
            return (this.transform.position);
        }
        set
        {
            this.transform.position = value;
        }
    }

    void Start () {
		
	}
	
	
	void Update () {
        Move();

        if (bndCheck != null && bndCheck.offDown)
        {
                Destroy(gameObject);
        }
    }

    public virtual void Move()
    {  
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }

    void OnCollisionEnter(Collision coll)
    {
        GameObject otherGO = coll.gameObject;                                  // a
        if (otherGO.tag == "ProjectileHero")
        {                               // b
            Destroy(otherGO);        // Destroy the Projectile
            Destroy(gameObject);     // Destroy this Enemy GameObject
        }
        else
        {
            print("Enemy hit by non-ProjectileHero: " + otherGO.name);     // c
        }
    }
}
