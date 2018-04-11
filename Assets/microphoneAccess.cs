using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class microphoneAccess : MonoBehaviour {

	public string clipName = "clip1"; // placeholder, we want to generate meaningful name programmatically
	AudioClip newClip;
	string saveLocation;

	// Use this for initialization
	void Start () {


        // list of all microphones
        string[] devicelist = Microphone.devices;


        // output all mics

        foreach(string s in devicelist)
        {
            Debug.Log(s);
        }

		// this is a placeholder name
		string activeMic = "2- USB Audio Device"; 					

		newClip = Microphone.Start(activeMic, false, 1800, 44100);

		// file save path
		saveLocation = Application.persistentDataPath.ToString ();	

		// output save path to console
		Debug.Log ("Save Location" + saveLocation);									
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnApplicationQuit(){
		SavWav.Save ("savedClip", newClip);
		Microphone.End ("Built-in Microphone");
		
	}

//	public static Microphone getActiveMic(){
//					foreach(Microphone mic in Microphone.devices)
//					{
//						if mic.Equals
//	}
}