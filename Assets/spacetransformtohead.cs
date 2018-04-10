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
		
	GameObject.Find("HandTipRight").GetComponent<Renderer>().enabled = false;
	GameObject.Find("HandTipLeft").GetComponent<Renderer>().enabled = false;
	GameObject.Find("ThumbRight").GetComponent<Renderer>().enabled = false;
	GameObject.Find("ThumbLeft").GetComponent<Renderer>().enabled = false;
					
	GameObject.Find("HandTipRight").GetComponent<LineRenderer>().enabled = false;
	GameObject.Find("HandTipLeft").GetComponent<LineRenderer>().enabled = false;
	GameObject.Find("ThumbRight").GetComponent<LineRenderer>().enabled = false;
	GameObject.Find("ThumbLeft").GetComponent<LineRenderer>().enabled = false;
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
