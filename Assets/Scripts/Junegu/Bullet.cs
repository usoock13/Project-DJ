using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float bulletSpeed = 35f;

    void Start()
    {
        StartCoroutine(TimeDelete());
        Debug.Log("น฿ป็");
    }

    IEnumerator TimeDelete()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    void BulletThrow() {
        transform.Translate(Vector2.up * bulletSpeed * Time.deltaTime);
    }
    void Update()
    {
        BulletThrow();
    }

}
