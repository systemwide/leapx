using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lpalmscript : MonoBehaviour {
	private Transform WL;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		try{
		if(GameObject.Find("WristLeft") != null)
			WL = GameObject.Find("WristLeft").transform;
		}
		finally{
			if(WL != null){
				transform.position = WL.position;
			}
		}
	}
}
