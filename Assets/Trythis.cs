using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trythis : MonoBehaviour {
	public Transform Cam;
	// Use this for initialization
	void Start () {
		this.transform.position = Cam.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
