﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WeaponType
{
    none,       // The default / no weapon 
    blaster,    // A simple blaster 
    spread,     // Two shots simultaneously 
    phaser,     // [NI] Shots that move in waves 
    missile,    // [NI] Homing missiles 
    laser,      // [NI]Damage over time 
    shield      // Raise shieldLevel 
}

[System.Serializable]                                                         // a 
public class WeaponDefinition
{                                                // b 
    public WeaponType type = WeaponType.none;
    public string letter;                // Letter to show on the power-up 
    public Color color = Color.white;       // Color of Collar & power-up 
    public GameObject projectilePrefab;          // Prefab for projectiles 
    public Color projectileColor = Color.white;
    public float damageOnHit = 0;           // Amount of damage caused 
    public float continuousDamage = 0;      // Damage per second (Laser) 
    public float delayBetweenShots = 0;
    public float velocity = 20;             // Speed of projectiles 
}

public class Weapon : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
