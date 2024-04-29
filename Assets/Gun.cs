using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public Transform spawnPoint;
    public GameObject bullet;
    public float bulletVelocity;
    public string shootKey;

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            Shoot();
        }
    }

    public void Shoot(){
        GameObject spawnedBullet = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);
        spawnedBullet.GetComponent<Rigidbody>().velocity = spawnPoint.forward * bulletVelocity;
        Destroy(spawnedBullet, 5);
    }
}
