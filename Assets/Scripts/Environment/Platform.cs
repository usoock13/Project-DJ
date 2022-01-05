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
        platformEffector.colliderMask = platformEffector.colliderMask & ~LayerMask.GetMask("Player");
    }
    void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Player") {
            platformEffector.colliderMask = originMask;
        }
    }
}
