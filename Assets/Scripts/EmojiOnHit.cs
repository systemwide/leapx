using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmojiOnHit : MonoBehaviour {

	public 	GameObject Emoji;

	

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
			GameObject NewEmoji = GameObject.Instantiate(Emoji);
			NewEmoji.transform.localScale -= new Vector3(0.0F, 0.3F, 0);
			Vector3 offset = new Vector3(0.0f,1,0.0f);
			NewEmoji.transform.position = GameObject.Find("head").transform.position + offset;
			//yield return new WaitForSeconds(1);
			//Debug.Log("gone");
			Destroy(NewEmoji,3);
		}
	}

	//IEnumerator makeSmileEmoji(){

	//}

		//[Command]
    public void CmdInstantiateTaskObjects(Vector3 newPos)
    {
		// GameObject newTask = Instantiate(taskObject, transform.position + newPos, Quaternion.identity) as GameObject;
		// newTask.transform.localScale += new Vector3(10, 10, 10);
		// //Network.Instantiate (TaskObject, newPos, Quaternion.identity, 0);

		// //create checkpoint
		// GameObject newCheckpoint = Instantiate(checkpoint, transform.position + newPos, Quaternion.identity) as GameObject;
		// newCheckpoint.transform.parent = newTask.transform;

		// //disable renderer on the taskObject container so that it can be used as a task boundary
		// newTask.transform.GetComponent<Renderer>().enabled = false;
		// newTask.GetComponent<Rigidbody>().useGravity = true;
		// newTask.GetComponent<Rigidbody>().isKinematic = false;

        // NetworkServer.Spawn(newTask);
        // NetworkServer.Spawn(newCheckpoint);
    }
}
