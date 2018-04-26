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

	private DataLogger dataLogger;

	public InputField participantIdInput;

	public void Start()
	{
		dataLogger = GameObject.FindObjectOfType<DataLogger>();
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
		dataLogger.jsonFilename = "social-data__"+participantId+"__"+dataLogger.json["session"]["start"];
		dataLogger.csvFilename = "location-data__"+participantId+"__"+dataLogger.json["session"]["start"];
		dataLogger.audioFilename = "audio-data__"+participantId+"__"+dataLogger.json["session"]["start"];
	}

}
