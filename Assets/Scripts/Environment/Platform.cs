using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    PlatformEffector2D platformEffector;
    int originMask;

    void Start() {
        platformEffector = GetComponent<PlatformEffector2D>();
        originMask = platformEffector.colliderMask;
    }
    IEnumerator passPlayerCoroutine;
    public void PassPlayer() {
        if(passPlayerCoroutine != null) {
            StopCoroutine(passPlayerCoroutine);
        }
        passPlayerCoroutine = PassPlayerCoroutine();
        StartCoroutine(passPlayerCoroutine);
    }
    public IEnumerator PassPlayerCoroutine() {
        platformEffector.colliderMask = platformEffector.colliderMask & ~LayerMask.GetMask("Player");
        yield return new WaitForSeconds(.25f);
        platformEffector.colliderMask = originMask;
    }
}
