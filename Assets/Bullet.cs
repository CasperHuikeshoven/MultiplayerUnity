using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Bullet : NetworkBehaviour
{

    public float damage; 

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    [Command]
    public void DealDamage(GameObject player){
        Debug.Log("Hit: " + player);
    }

    void OnCollisionEnter(Collision collision){
        DealDamage(collision.gameObject);
    }
}
