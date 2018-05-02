using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManager: MonoBehaviour {

	public GameObject vrCameraRig;
	public GameObject nonVRCameraRig;

	public SteamVR_TrackedObject hmd;
	public SteamVR_TrackedObject controllerLeft;
	public SteamVR_TrackedObject controllerRight;
	public SteamVR_TrackedObject LFootTracker;
	public SteamVR_TrackedObject RFootTracker;
	public SteamVR_TrackedObject LHandTracker;
	public SteamVR_TrackedObject RHandTracker;

	public InputField participantIdInput;
	public Button participantIdSubmitButton;

	private DataLogger dataLogger;
	private NetworkManagerHUD nmhud;

	public void Start()
	{
		dataLogger = GameObject.FindObjectOfType<DataLogger>();
		nmhud = GameObject.FindObjectOfType<NetworkManagerHUD>();
		nmhud.showGUI = false;
		// foreach(String s in Microphone.devices) Debug.Log(s);
	}

	public void enableVR()
	{
		StartCoroutine(doEnableVR());
	}

	public void endSession() {
		Application.Quit ();
	}

	public void submitParticipantId() {
		// disable participant id input field
		participantIdInput.interactable = false;
		participantIdSubmitButton.interactable = false;
		// initialize DataLogger json/csv/audio files
		dataLogger.initDataFiles(participantIdInput.text);
		// show network HUD when finished
		nmhud.showGUI = true;
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
