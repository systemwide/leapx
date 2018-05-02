using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using SimpleJSON;
using UnityEngine.UI;
using UnityEngine.Networking;

public class DataLogger : MonoBehaviour {

	// Note: Application.persistentDataPath == C:\Users\vel\AppData\LocalLow\InterpersonalVR

	public String jsonFilename;
	public String csvFilename;
	public String audioFilename;
	public String activeMic;

	private JSONObject json;
	private TextWriter csv;
	private AudioClip aud;

	private VRPlayer localPlayer;
	private bool logFilesReady;

	private int posLogCounter;
	public int FramesPerLog;

	// Use this for initialization
	void Start () {
		posLogCounter = 0;
		FramesPerLog = 5;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// called every .02 seconds
	void FixedUpdate() {
		if(logFilesReady && GetPlayer() != null && posLogCounter % FramesPerLog == 0) {
			VRPlayer p = GetPlayer();
			// begin logging position data
			csv.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17}",
							p.headPos.x, p.headPos.y, p.headPos.z, 
								p.bodyPos.x, p.bodyPos.y, p.bodyPos.z,
									p.leftHandPosSync.x, p.leftHandPosSync.y, p.leftHandPosSync.z,
										p.rightHandPosSync.x, p.rightHandPosSync.y, p.rightHandPosSync.z,
											p.leftFootPosSync.x, p.leftFootPosSync.y, p.leftFootPosSync.z,
												p.rightFootPosSync.x, p.rightFootPosSync.y, p.rightFootPosSync.z);
			posLogCounter = 0;
		} else {
			posLogCounter++;
		}
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

		// init json data
        json = new JSONObject();
        json.Add("session", new JSONObject());
        json["session"].AsObject.Add("start", new JSONString(DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH'-'mm'-'ss")));
		// TODO: add keys for social data

		// set file names
		jsonFilename = "/social-data__"+id+"__"+json["session"]["start"] + ".json";
		csvFilename = "/position-data__"+id+"__"+json["session"]["start"] + ".csv";
		audioFilename = "/audio-data__"+id+"__"+json["session"]["start"];

		// init csv data
		csv = new StreamWriter(Application.persistentDataPath+csvFilename);
		csv.WriteLine("Head_x,Head_y,Head_z,Torso_x,Torso_y,Torso_z,LHand_x,LHand_y,LHand_z,RHand_x,RHand_y,RHand_z,LFoot_x,LFoot_y,LFoot_z,RFoot_x,RFoot_y,RFoot_z");

		// init audio data
		activeMic = "7- Rift Audio"; 
		aud = Microphone.Start(activeMic, false, 1200, 44100); // 20min file

		logFilesReady = true;
	}

	// finalizes data files
	private void finalizeDataFiles() {

		// finalize json data
		json["session"].AsObject.Add("end", new JSONString(DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH'-'mm'-'ss")));
		File.WriteAllText(Application.persistentDataPath+jsonFilename, json.ToString());
		
		// finalize audio data
		Debug.Log(Application.persistentDataPath+audioFilename);
		SavWav.Save(Application.persistentDataPath+audioFilename, aud);
		Microphone.End(activeMic);
	}

	// gets local VRPlayer for which to log position data
	private VRPlayer GetPlayer() {

        if (localPlayer == null) {
            VRPlayer[] players = FindObjectsOfType<VRPlayer>() as VRPlayer[];
            foreach (VRPlayer player in players) {
                if (player.GetComponent<NetworkIdentity>().isLocalPlayer) {
                    localPlayer = player;
                    break;
                }
            }
        }
        return localPlayer;
    }

}
