using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRightHandToKinect : MonoBehaviour {
	public Transform LeapHandRight;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		LeapHandRight.position = GameObject.Find("HandRight").transform.position;
	}
}
