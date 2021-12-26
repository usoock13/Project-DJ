using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    GameObject player;
    Vector3 nextPosition;
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update() {
        nextPosition = player.transform.position;
        nextPosition += new Vector3(0, 5, 0);
        transform.position = nextPosition;
    }
}
