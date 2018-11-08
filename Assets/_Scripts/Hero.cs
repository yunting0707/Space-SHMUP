using System.Collections;
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
    public Weapon[] weapons;

    [Header("Set Dynamically")]
    [SerializeField]
    private float _shieldLevel = 1;
    private GameObject lastTriggerGo = null;
    // Declare a new delegate type WeaponFireDelegate 
    public delegate void WeaponFireDelegate();                               // a 
    // Create a WeaponFireDelegate field named fireDelegate. 
    public WeaponFireDelegate fireDelegate;

    void Start()
    {

            S = this;
        ClearWeapons();
        weapons[0].SetType(WeaponType.blaster);
  //      fireDelegate += TempFire;
    }

	
	

	void Update () {
        float xAxis = Input.GetAxis("Horizontal");                            
        float yAxis = Input.GetAxis("Vertical");                           

        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;
                  
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);
        //       if (Input.GetKeyDown(KeyCode.Space)){                           // a
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
        else if (go.tag == "PowerUp")
        {
            // If the shield was triggered by a PowerUp
            AbsorbPowerUp(go);
        }
        else
        {
            print("Triggered by non-Enemy: " + go.name);                       // f
        }
    }
    public void AbsorbPowerUp(GameObject go)
    {
        PowerUp pu = go.GetComponent<PowerUp>();
        switch (pu.type)
        {
            case WeaponType.shield:                                          // a
                shieldLevel++;
                break;

            default:                                                         // b
                if (pu.type == weapons[0].type)
                { // If it is the same type  // c
                    Weapon w = GetEmptyWeaponSlot();
                    if (w != null)
                    {
                        // Set it to pu.type
                        w.SetType(pu.type);
                    }
                }
                else
                { // If this is a different weapon type               // d
                    ClearWeapons();
                    weapons[0].SetType(pu.type);
                }
                break;
        }
        pu.AbsorbedBy(this.gameObject);
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
    Weapon GetEmptyWeaponSlot()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].type == WeaponType.none)
            {
                return (weapons[i]);
            }
        }
        return (null);
    }

    void ClearWeapons()
    {
        foreach (Weapon w in weapons)
        {
            w.SetType(WeaponType.none);
        }
    }
}
