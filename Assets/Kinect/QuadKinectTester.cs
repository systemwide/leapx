using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadKinectTester : MonoBehaviour {
	public KinectStreamingPlugin kinect;
	public GameObject rgbQuad;
	public GameObject depthQuad;
	public GameObject uvQuad;
	// Use this for initialization
	void Start () {
		rgbQuad.GetComponent<Renderer>().material.mainTexture = kinect.texColor;
		depthQuad.GetComponent<Renderer>().material.mainTexture = kinect.texDepth;
		uvQuad.GetComponent<Renderer>().material.mainTexture = kinect.texUV;

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
