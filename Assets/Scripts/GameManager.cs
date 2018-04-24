using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.Networking;

public class GameManager: NetworkBehaviour {

	public GameObject vrCameraRig;
	public GameObject nonVRCameraRig;
	public SteamVR_TrackedObject hmd;
	public SteamVR_TrackedObject controllerLeft;
	public SteamVR_TrackedObject controllerRight;

	public void enableVR()
	{
		StartCoroutine(doEnableVR());
	}

	public void endSession() {
		Application.Quit ();
	}

	IEnumerator doEnableVR()
	{
        while (UnityEngine.XR.XRSettings.loadedDeviceName != "OpenVR")
		{
			UnityEngine.XR.XRSettings.LoadDeviceByName("OpenVR");
			yield return null;
		}
		UnityEngine.XR.XRSettings.enabled = true;
		vrCameraRig.SetActive(true);
        nonVRCameraRig.SetActive(false);
	}
}
