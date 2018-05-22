using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private Transform PlayerTransform;

    private float fl_playerSpeed;
    public float fl_speedMultiplier = 1.0f; //Player's horizontal speed varies by level - this adjusts it
    private const float fl_LEAN_MULTIPLIER = 20f; //adjusts how much the player leans into a turn

	// Use this for initialization
	void Start () {
        fl_playerSpeed = 0;
        PlayerTransform = transform; //gameObject.GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        fl_playerSpeed = Input.GetAxis("Horizontal");
        PlayerTransform.Translate(new Vector3(fl_playerSpeed * fl_speedMultiplier, 0, 0), Space.World);
        PlayerTransform.rotation = Quaternion.Euler(90, fl_playerSpeed * fl_LEAN_MULTIPLIER, 0);
	}
}
