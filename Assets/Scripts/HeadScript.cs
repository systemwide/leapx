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
		try{
			if(GameObject.Find("Head")!=null){
				this.transform.position = GameObject.Find("Head").transform.position;
				//this.transform.rotation = cam.rotation;
			}
		}
		finally{
			
		}
		//LMRig.position = GameObject.Find("Head").transform.position;
	}
}
