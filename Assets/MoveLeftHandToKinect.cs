using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeftHandToKinect : MonoBehaviour {
	public Transform LeapHandLeft;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		LeapHandLeft.position = GameObject.Find("HandLeft").transform.position;
	}
}
