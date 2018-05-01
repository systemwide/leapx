using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManager: NetworkBehaviour {

	public GameObject vrCameraRig;
	public GameObject nonVRCameraRig;
	public SteamVR_TrackedObject hmd;
	public SteamVR_TrackedObject controllerLeft;
	public SteamVR_TrackedObject controllerRight;

	public SteamVR_TrackedObject LFootTracker;
	public SteamVR_TrackedObject RFootTracker;
	public SteamVR_TrackedObject LHandTracker;
	public SteamVR_TrackedObject RHandTracker;

	public SteamVR_TrackedObject bodyTracker;

	private DataLogger dataLogger;

	public InputField participantIdInput;

	public void Start()
	{
		dataLogger = GameObject.FindObjectOfType<DataLogger>();
		foreach(String s in Microphone.devices) Debug.Log(s);
	}

	public void enableVR()
	{
		// disable participant id field
		participantIdInput.DeactivateInputField();
		// set data file names (not changeable after this since input is disabled)
		setDataFileNames(participantIdInput.text);
		// now start VR (location/audio data will start logging)
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

	private void setDataFileNames(String participantId) {
		String id = participantId.Length > 0 ? participantId : "000000";
		dataLogger.jsonFilename = "social-data__"+id+"__"+dataLogger.json["session"]["start"];
		dataLogger.csvFilename = "location-data__"+id+"__"+dataLogger.json["session"]["start"];
		dataLogger.audioFilename = "audio-data__"+id+"__"+dataLogger.json["session"]["start"];
	}

}
