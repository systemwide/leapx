using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using SimpleJSON;
using UnityEngine.UI;

public class DataLogger : MonoBehaviour {

	[HideInInspector]
	public String BASE_PATH;
	[HideInInspector]
	public String jsonFilename;
	[HideInInspector]
	public String csvFilename;
	[HideInInspector]
	public String audioFilename;

	public String activeMic;

	[HideInInspector]
	public JSONObject json;
	// [HideInInspector]
	// public CSVObject csv;
	[HideInInspector]
	public AudioClip audioClip;

	// Use this for initialization
	void Start () {
		// add root session object
        json = new JSONObject();
        json.Add("session", new JSONObject());
        // initialize session object
		DateTime now = DateTime.UtcNow;
        json["session"].AsObject.Add("start", new JSONString(now.ToString()));
        json["session"].AsObject.Add("tasks", new JSONArray());
		// init data file paths with default values 
		BASE_PATH = Application.persistentDataPath;
		jsonFilename = "\\social-data__000000__" + now.ToString("yyyy'-'MM'-'dd'T'HH'-'mm'-'ss") + ".json";
		csvFilename = "\\location-data__000000__" + now.ToString("yyyy'-'MM'-'dd'T'HH'-'mm'-'ss") + ".json";
		audioFilename = "\\audio-data__000000__" + now.ToString("yyyy'-'MM'-'dd'T'HH'-'mm'-'ss") + ".json";
		// init 
		activeMic = "7- Rift Audio"; 
		audioClip = Microphone.Start(activeMic, false, 1800, 44100);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// called when either 
	// a) the user un-clicks the play button in the editor, or
    // b) the user clicks the 'End Session' button
    void OnApplicationQuit() {
        // set session end time
        json["session"].AsObject.Add("end", new JSONString(DateTime.UtcNow.ToString()));
		// log session data (social, location, audio) to files
		File.WriteAllText(BASE_PATH+jsonFilename, json.ToString());
		// File.WriteAllText(BASE_PATH+csvFilename, csv.ToString());
		SavWav.Save(audioFilename, audioClip);
		Microphone.End(activeMic);
    }
}
