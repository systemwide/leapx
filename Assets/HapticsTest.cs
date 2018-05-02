using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HapticsTest : MonoBehaviour {

    public SteamVR_TrackedObject controllerLeft;

    // Use this for initialization
    void Start () {
        controllerLeft = GameObject.Find("Controller (left)").GetComponent<SteamVR_TrackedObject>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("entering trigger: " + collider.gameObject.name);
        if (collider.gameObject.GetComponent<VRPlayer>() != null)
        {
            StartCoroutine(LongVibration(1, 1));
        }

    }

    IEnumerator LongVibration(float length, float strength)
    {
        for (float i = 0; i < length; i += Time.deltaTime)
        {
            int index = (int)controllerLeft.index;
            SteamVR_Controller.Input(index).TriggerHapticPulse((ushort)Mathf.Lerp(0, 3999, strength));
            yield return null;
        }
    }
}
