using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class GunScript : MonoBehaviour
{

    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 1f;
    public float impactForce = 100f;
    public float spread = 0.2f;
    public float bulletsPerShoot = 8;
    public float reloadTime;

    public int clipSize = 6;
    public int maxAmmo = 12;
    public int currentAmmo;
    public int totalCurrentAmmo = 12;

    public Animator animator;
    public Camera fpsCam;
    public ParticleSystem muzzleflash;
    public GameObject impactEffect;
    private Vector3 direction;
    private Vector3 spreadtemp;
    private bool isReloading = false;
    private float nextTimeToFire = 0f;

    public Text ammoDisplay;



    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = clipSize;
    }

    void onEnable()
    {
        Debug.Log("On enable is working");
        //isReloading = false;
       // animator.SetBool("Reloading", false);
        

    }

    
    // Update is called once per frame
    void Update()
    {
            
            
        ammoDisplay.text = currentAmmo.ToString() + "/" + (totalCurrentAmmo - currentAmmo).ToString();
        if (isReloading)
        {
            return;
        }
        if (currentAmmo <= 0 && totalCurrentAmmo >0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && currentAmmo> 0)
        {
            
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading");
        animator.SetBool("Reloading", true);
        yield return new WaitForSeconds(reloadTime -0.25f);
        animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(0.25f);
        isReloading = false;
        if (totalCurrentAmmo >= clipSize)
        {
            currentAmmo = clipSize;
        }
        else
        {
            currentAmmo = totalCurrentAmmo;
        }
        
    }

    void Shoot()
    {
        muzzleflash.Play();
        RaycastHit hit;
        currentAmmo--;
        totalCurrentAmmo--;
        animator.SetTrigger("Shoot");

        for (int i = 0; i < bulletsPerShoot; i++)
        {
            direction = fpsCam.transform.forward;
            spreadtemp = fpsCam.transform.up * Random.Range(-1f, 1f); // add random up or down (because random can get negative too)
            spreadtemp += fpsCam.transform.right * Random.Range(-1f, 1f); // add random left or right
            direction += spreadtemp * spread;// * Random.Range(0f, spread * 10f);
            if (Physics.Raycast(fpsCam.transform.position, direction, out hit, range))
            {
                Debug.DrawLine(fpsCam.transform.position, hit.point, Color.red,4f);
                
                GoonAI goon = hit.transform.GetComponent<GoonAI>();
                if (goon != null)
                {
                    goon.TakeDamage(damage);
                }

                if (hit.rigidbody != null && goon == null)
                {

                    hit.rigidbody.AddForce(-hit.normal * impactForce);
                }

                GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 2f);
            }
            else
            {
                Debug.DrawRay(fpsCam.transform.position, direction*10, Color.blue,4f);
            }
        }
    }

    public void AmmoPickup()
    {
        totalCurrentAmmo = maxAmmo - 6 + currentAmmo;
    }

    /**
    void OnTriggerEnter(Collision cube)
    {
        if (cube.gameObject.tag == "Ammo")
        {
            totalCurrentAmmo = maxAmmo - 6 + currentAmmo;
            Destroy(cube.gameObject);

        }
    }**/
}
