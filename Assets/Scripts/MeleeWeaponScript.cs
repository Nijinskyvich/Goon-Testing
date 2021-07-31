using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponScript : MonoBehaviour
{

    public float damage = 10f;
    public float delay = 0.3f;
    public float activeTime = 0.5f;
    public float attackInterval = 2f;
    public float impactForce = 100f;
    private float nextTimeToFire = 0f;
    public BoxCollider killbox;
    public bool attacking = false;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        killbox.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire2") && Time.time >= nextTimeToFire && attacking == false)
        {
            //Debug.Log("Fire");
            nextTimeToFire = Time.time + attackInterval;
            StartCoroutine(attack());
        }
    }

    private IEnumerator attack()
    {
        animator.SetTrigger("Attack");
        attacking = true;
        yield return new WaitForSeconds(delay);
        killbox.enabled = true;
        yield return new WaitForSeconds(activeTime);
        //Debug.Log("Disable");
        attacking = false;
        killbox.enabled = false;
        yield break;
        
    }
}
