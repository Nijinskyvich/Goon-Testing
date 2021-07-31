using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCollision : MonoBehaviour
{

    public GunScript gs;
    public int Health = 100;
    public Text healthDisplay;


    void start()
    {
      
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Ammo")
        {
            gs.AmmoPickup();
            Destroy(collider.gameObject);
        }

    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        healthDisplay.text = Health.ToString();
    }


}
