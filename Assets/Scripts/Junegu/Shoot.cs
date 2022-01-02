using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    Rigidbody2D rigidbody2D;
    public Transform pos;
    public GameObject copyBullet;

    public float shootPower = 100;
    
    void Start()
    {
        rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
    }

    void fire()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(copyBullet, pos.position, transform.rotation);
            rigidbody2D.AddForce(Vector2.right * shootPower);
        }
    }

    void Update()
    {
        fire();
    }
}
