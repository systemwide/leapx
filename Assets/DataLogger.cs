using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using SimpleJSON;
//using CsvHelper;
using UnityEngine.UI;

public class DataLogger : MonoBehaviour {

	public String BASE_PATH;
	public String jsonFilename;
	public String csvFilename;
	public String audioFilename;
	public String activeMic;

	private JSONObject json;
	private TextWriter csv;
	private AudioClip aud;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// called when either 
	// a) the user un-clicks the play button in the editor, or
    // b) the user clicks the 'End Session' button
    void OnApplicationQuit() {
		finalizeDataFiles();
    }



	// --------- HELPERS ------------



	// called from GameManager, when participant id is finalized, upon clicking Start VR button
	public void initDataFiles(String participantId) {

		String id = participantId.Length > 0 ? participantId : "000000";
		jsonFilename = "social-data__"+id+"__"+json["session"]["start"] + ".json";
		csvFilename = "position-data__"+id+"__"+json["session"]["start"] + ".csv";
		audioFilename = "audio-data__"+id+"__"+json["session"]["start"];

		// init json data
        json = new JSONObject();
        json.Add("session", new JSONObject());
        json["session"].AsObject.Add("start", new JSONString(DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH'-'mm'-'ss")));
		// TODO: add keys for social data

		// init csv data
		csv = new StreamWriter(Application.persistentDataPath+csvFilename);
		csv.WriteLine("{0},{1},{2},{3},{4}", 1, 2, 3, 4, 5);

		// init audio data
		activeMic = "7- Rift Audio"; 
		aud = Microphone.Start(activeMic, false, 1800, 44100);
	}

	// finalizes data files
	private void finalizeDataFiles() {

		// finalize json data
		json["session"].AsObject.Add("end", new JSONString(DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH'-'mm'-'ss")));
		File.WriteAllText(Application.persistentDataPath+jsonFilename, json.ToString());
		
		// finalize csv data
		/*
			csv data doesn't need to be finalized, as the data has been
			streaming to the file throughout the session.
		*/
		
		// finalize audio data
		SavWav.Save(Application.persistentDataPath+audioFilename, aud);
		Microphone.End(activeMic);
	}
}
