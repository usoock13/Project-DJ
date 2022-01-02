using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject copyBullet;

    public bool isDelay;
    public float delayTime = 0.5f;

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(delayTime);
        isDelay = false;
    }

    void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if(!isDelay)
            {
                Instantiate(copyBullet, transform.position, transform.rotation);
                isDelay = true;
                StartCoroutine(Delay());
            }
            else
            {
                Debug.Log("¹Ùº¸");
            }
        }
    }

    void Update()
    {
        Fire();
        
    }
}
