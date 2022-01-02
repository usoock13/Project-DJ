using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Vector3 start;

    float dist;
    void Start()
    {
        start = this.transform.position;
    }

    void DistDelete()
    {
        Vector3 pos = this.transform.position;
        dist = Vector3.Distance(start, pos);

        if (dist >= 3)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        DistDelete();
    }

}
