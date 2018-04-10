using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lefthandscript : MonoBehaviour {
	private Transform HL;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		try{
			HL = GameObject.Find("HandLeft").transform;
			transform.position = HL.position;
		}
		finally{}
	}
}
