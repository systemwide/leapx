using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC : MonoBehaviour {

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
			if(UI.activeSelf == true){
				Debug.Log("here if");
				UI.SetActive(false);
			}
			else
			{
				Debug.Log("here else");
				UI.SetActive(true);
			}
		}
    }

}

 