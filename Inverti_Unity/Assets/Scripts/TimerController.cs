using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour {

    private float time_secs;

	// Use this for initialization
	void Start () {

        time_secs = 0.0f;

	}
	
	// Update is called once per frame
	void FixedUpdate () {

        time_secs += Time.deltaTime;
        GetComponent<Text>().text = time_secs.ToString(format: "0#.##");

	}
}
