using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class prompt_display : MonoBehaviour {

public string[] promptList = {"What's your major?", "Where are you from?", "What are your Hobbies?", "Do you have a job? If so, what?", 
	"Are you a part of Greek life or any clubs?", "What is your favorite show?", "Do you have any pets?", "Interesting fact about you?"};

public Text promptDisplay_text;
GameObject canvas;

int index = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void OnTriggerEnter(Collider other) {
		Debug.Log("SC Script debug");
		//promptDisplay_text = GameObject.Find("psCanvas").GetComponentInChildren<Text>();

		if (index >= promptList.Length)
		{
			index = 0;
		}
		if (other.name == "Cube"){
			promptDisplay_text.text = promptList[index];
			index++;
		}//if
    }
}


