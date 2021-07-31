using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float strafe = Input.GetAxis("Horizontal");
        float forward = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(strafe, 0, forward) * speed * Time.deltaTime);
    }
}
