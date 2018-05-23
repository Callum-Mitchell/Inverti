using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapAround : MonoBehaviour {

    private Transform player;
    public float leftBoundary;
    public float rightBoundary;
    private static float wrapDistance = 72f;
	// Use this for initialization
	void Start () {
        player = transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (transform.position.x < leftBoundary) {
            transform.Translate(new Vector3(wrapDistance, 0, 0), Space.World);
        }
        else if (transform.position.x > 36) {
            transform.Translate(new Vector3(-wrapDistance, 0, 0), Space.World);
        }

    }

}