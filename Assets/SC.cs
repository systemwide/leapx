using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC : MonoBehaviour {

	public 	GameObject button;
	public GameObject UI;
	

	// Use this for initialization
	void Start () {
		//UI.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log("SC Script debug");

		if (other.name == "Cube"){
        if(UI.active == true){
			UI.SetActive(false);
		}
		else
		{
			UI.SetActive(true);
		}
		}
    }

}

 