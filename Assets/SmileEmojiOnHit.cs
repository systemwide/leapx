using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmileEmojiOnHit : MonoBehaviour {

	public 	GameObject SmileEmoji;

	

	// Use this for initialization
	void Start () {
		//UI.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		if (other.name == "Cube"){
			//StartCoroutine(makeSmileEmoji());
			GameObject Smile = GameObject.Instantiate(SmileEmoji);
			Vector3 offset = new Vector3(1,0.5f,1);
			Smile.transform.position = GameObject.Find("head").transform.position + offset;
			//yield return new WaitForSeconds(1);
			//Debug.Log("gone");
			Destroy(Smile,3);
		}
	}

	//IEnumerator makeSmileEmoji(){

	//}
}
