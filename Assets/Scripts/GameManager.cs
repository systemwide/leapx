using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.Networking;
using UnityEngine.UI;
using CsvHelper;

public class GameManager: NetworkBehaviour {

	public GameObject vrCameraRig;
	public GameObject nonVRCameraRig;
	public SteamVR_TrackedObject hmd;
	public SteamVR_TrackedObject controllerLeft;
	public SteamVR_TrackedObject controllerRight;
	public InputField participantIdInput;

	private DataLogger dataLogger;

	public void Start()
	{
		dataLogger = GameObject.FindObjectOfType<DataLogger>();
		foreach(String s in Microphone.devices) Debug.Log(s);
	}

	public void enableVR()
	{
		// disable participant id input field
		participantIdInput.DeactivateInputField();
		// initialize DataLogger json/csv/audio files
		dataLogger.initDataFiles(participantIdInput.text);
		// hide the participant id input field
		participantIdInput.gameObject.SetActive(false);
		// now start VR
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
