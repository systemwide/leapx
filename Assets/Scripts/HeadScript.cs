using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadScript : MonoBehaviour {
	// Use this for initialization
	public Transform cam;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//Move to kinect
		/*
		try{
			if(GameObject.Find("Head")!=null){
				
				//this.transform.position = GameObject.Find("Head").transform.position;
				
			}
		}
		finally{	
		}
		*/
		this.transform.position = cam.position;
		this.transform.rotation = cam.rotation;
		//LMRig.position = GameObject.Find("Head").transform.position;
	}
}
