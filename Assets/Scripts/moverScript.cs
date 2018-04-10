using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moverScript : MonoBehaviour {
	public Transform cam;
	// Use this for initialization
	void Start () {
		
		//this.transform.rotation = cam.rotation;	
	}
	
	// Update is called once per frame
	void Update () {
		//this.transform.rotation = cam.rotation; Don't do this does not work
		//this.transform.position = cam.position;
		
	}
}
