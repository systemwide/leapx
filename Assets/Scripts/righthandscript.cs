using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class righthandscript : MonoBehaviour {
	private Transform HR;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		try{
			HR = GameObject.Find("HandRight").transform;
			this.transform.position = HR.transform.position;
		}
		finally{}
	}
}
