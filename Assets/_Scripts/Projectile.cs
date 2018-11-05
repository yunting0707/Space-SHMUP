﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    private BoundsCheck bndCheck;
    private Renderer rend;

    [Header("Set Dynamically")]
    public Rigidbody rigid;
    [SerializeField]                                                        // a 
    private WeaponType      _type;                                          // b 

    // This public property masks the field _type and takes action when it is set 
    public WeaponType type
    {                                           // c 
        get
        {
            return (_type);
        }
        set
        {
            SetType(value);                                                // c 
        }
    }

    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        rend = GetComponent<Renderer>();                                     // d 
        rigid = GetComponent<Rigidbody>();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (bndCheck.offUp)
        {                                                // a
            Destroy(gameObject);
        }
    }

    public void SetType(WeaponType eType)
    {                               // e 
        // Set the _type 
        _type = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(_type);
        rend.material.color = def.projectileColor;
    }
}
