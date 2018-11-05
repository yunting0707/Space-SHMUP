﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour {
    static public Hero S;

    [Header("Set in Inspector")]
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;
    public float gameRestartDelay = 2f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 40;

    [Header("Set Dynamically")]
    [SerializeField]
    private float _shieldLevel = 1;

    private GameObject lastTriggerGo = null;
    // Declare a new delegate type WeaponFireDelegate 
    public delegate void WeaponFireDelegate();                               // a 
    // Create a WeaponFireDelegate field named fireDelegate. 
    public WeaponFireDelegate fireDelegate;

    void Awake()
    {
        if (S == null)
        {
            S = this; 
        }
        else
        {
            Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S!");
        }
        fireDelegate += TempFire;
    }





    void Start () {
		
	}
	
	

	void Update () {
        float xAxis = Input.GetAxis("Horizontal");                            
        float yAxis = Input.GetAxis("Vertical");                           

        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;
                  
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);
        //       if (Input.GetKeyDown(KeyCode.Space))
        //     {                           // a
        //        TempFire();
        //   }
        if (Input.GetAxis("Jump") == 1 && fireDelegate != null)
        {            // d 
            fireDelegate();                                                  // e 
        }
    }
    void TempFire()
    {                                                        // b
        GameObject projGO = Instantiate<GameObject>(projectilePrefab);
        projGO.transform.position = transform.position;
        Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
        //   rigidB.velocity = Vector3.up * projectileSpeed;
        Projectile proj = projGO.GetComponent<Projectile>();                 // h 
        proj.type = WeaponType.blaster;
        float tSpeed = Main.GetWeaponDefinition(proj.type).velocity;
        rigidB.velocity = Vector3.up * tSpeed;
    }
}
    void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        //print("Triggered: " + go.name);
        if (go == lastTriggerGo)
        {                                           // c
            return;
        }
        lastTriggerGo = go;                                                  // d

        if (go.tag == "Enemy")
        {  // If the shield was triggered by an enemy
            shieldLevel--;        // Decrease the level of the shield by 1
            Destroy(go);          // … and Destroy the enemy                 // e
        }
        else
        {
            print("Triggered by non-Enemy: " + go.name);                       // f
        }
    }

    public float shieldLevel
    {
        get
        {
            return (_shieldLevel);                                          // a
        }
        set
        {
            _shieldLevel = Mathf.Min(value, 4);                             // b
            // If the shield is going to be set to less than zero
            if (value < 0)
            {                                                 // c
                Destroy(this.gameObject);
                Main.S.DelayedRestart(gameRestartDelay);
            }
        }
    }

}
