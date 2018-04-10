using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spineScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		try{
			if(GameObject.Find("SpineMid")!=null){
				this.transform.position = GameObject.Find("SpineMid").transform.position;
			}
		}
		finally{}
	}
}
