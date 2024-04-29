using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GunBehaviour : NetworkBehaviour
{

    public Gun[] guns;
    public string shootKey;

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && hasAuthority){
            Shoot();
        }
    }

    public void Shoot(){
        GameObject spawnedBullet = Instantiate(guns[0].bullet, guns[0].spawnPoint.position, guns[0].spawnPoint.rotation);
        spawnedBullet.GetComponent<Rigidbody>().velocity = guns[0].spawnPoint.forward * guns[0].bulletVelocity;
        NetworkServer.Spawn(spawnedBullet, connectionToServer);
        Destroy(spawnedBullet, 5);
    }
}

[System.Serializable]
public class Gun{
    public GameObject gun;
    public Transform spawnPoint;
    public GameObject bullet;
    public float bulletVelocity;
    
}
