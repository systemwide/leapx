using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spineScript : MonoBehaviour {
	public Transform cam;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		try{
			if(GameObject.Find("SpineMid")!=null){
				Vector3 HeadPosKinect = GameObject.Find("Head").transform.position;
				Vector3 SpinePosKinect = GameObject.Find("SpineMid").transform.position;
				Vector3 diff = HeadPosKinect - SpinePosKinect;
				this.transform.position = cam.position - diff;

			}
		}
		finally{}
	}
}
