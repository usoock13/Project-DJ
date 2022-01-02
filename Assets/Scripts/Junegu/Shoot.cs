using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject copyBullet;
    void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(copyBullet, transform.position, copyBullet.transform.rotation);
        }
    }
    void Update()
    {
        Fire();
    }
}
