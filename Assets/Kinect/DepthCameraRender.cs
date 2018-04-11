using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DepthCameraRender : MonoBehaviour {

	public DepthSourceManager depthCam;
	public ColorSourceManager colorCam;
	public Camera myCam;
	
	// Use this for initialization
	void Start () {
		// Set texture onto our material
		GetComponent<Renderer>().material.SetTexture("_DepthTex",depthCam._DepthTexture);
		GetComponent<Renderer>().material.SetTexture("_ColorTex",colorCam.GetColorTexture());
		GetComponent<Renderer>().material.SetTexture("_UVTex", depthCam._UVTexture);
		
	}
	
	// Update is called once per frame
	void Update () {
	}
	void OnRenderObject()
	{
		if (Camera.current != myCam) {
			return;
		}
		Matrix4x4 v = Camera.current.worldToCameraMatrix;
		Matrix4x4 m = transform.localToWorldMatrix;
		Matrix4x4 MV = m;
		GetComponent<Renderer> ().material.SetPass (0);
		GetComponent<Renderer> ().material.SetMatrix("_ModelViewMatrix", MV);
		Graphics.DrawProcedural (MeshTopology.Points, 512 * 424, 1);

	}

}
