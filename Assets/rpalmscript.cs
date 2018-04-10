using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rpalmscript : MonoBehaviour {
	private Transform WR;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		try{
			if(GameObject.Find("WristRight") != null){
				WR = GameObject.Find("WristRight").transform;
			}
		}
		finally{
			if( WR != null){
				transform.position = WR.position;
			}
		}

	}
}
