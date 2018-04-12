using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spacetransformtohead : MonoBehaviour {
	public Transform myhead;
	private bool found = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (found == false){
			try{
				if (GameObject.Find("Head") != null){
					myhead = GameObject.Find("Head").transform;
					this.transform.position = myhead.transform.position;
					//this.transform.position += Vector3.right * .5f;

					found = true;
				}
			}
			finally{}
		}

		//else{}
	}
}
