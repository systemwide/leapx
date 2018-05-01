using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.Networking;

public class VRPlayer : NetworkBehaviour {

	public Transform SteamVR_Rig;
	public SteamVR_TrackedObject hmd;
	public SteamVR_TrackedObject controllerLeft;
	public SteamVR_TrackedObject controllerRight;
	public SteamVR_TrackedObject bodyTracker;

	public SteamVR_TrackedObject rightHandTracker;
	public SteamVR_TrackedObject leftHandTracker;
	public SteamVR_TrackedObject LFootTracker;
	public SteamVR_TrackedObject RFootTracker;

	public GameObject leapHandController;
	public Leap.Unity.HandPool leapHandPool;

	public Transform head;
	public Transform body;
	public Transform rightFoot;
	public Transform leftFoot;
	public Transform rightHand;
	public Transform leftHand;

	
	public Transform kinectBodyTransform;

	// Use this for initialization
	[SyncVar]
	Vector3 headPos;
	[SyncVar]
	Quaternion headRot;
	[SyncVar]
	Vector3 bodyPos;
	[SyncVar]
	Quaternion bodyRot;
	[SyncVar]
	Vector3 leftHandPosSync;
	[SyncVar]
	Quaternion leftHandRotSync;
	[SyncVar]
	Vector3 rightHandPosSync;
	[SyncVar]
	Quaternion rightHandRotSync;
	Vector3 leftFootPosSync;
	[SyncVar]
	Quaternion leftFootRotSync;
	[SyncVar]
	Vector3 rightFootPosSync;
	[SyncVar]
	Quaternion rightFootRotSync;

	void Start () {
		head.transform.position = new Vector3(0, 4, 0);
		body.transform.position = new Vector3(0, 2, 0);
	}

	public void OnConnectedToServer()
	{
		// Set as ready
		NetworkServer.SetClientReady(connectionToClient);
	}

	private void FixedUpdate()
	{
		// if (isServer && kinectBodyTransform.gameObject.activeInHierarchy) {
		// 	RpcSyncKinectBodyTransform(kinectBodyTransform.GetComponent<NetworkIdentity>().netId, kinectBodyTransform.position, kinectBodyTransform.localRotation);
		// }

		if (isLocalPlayer) {
			if (UnityEngine.XR.XRSettings.enabled) {
				if (SteamVR_Rig == null) {
					GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();

					SteamVR_Rig = gm.vrCameraRig.transform;
					hmd = gm.hmd;

					controllerLeft = gm.controllerLeft;
					controllerRight = gm.controllerRight;

					LFootTracker = gm.LFootTracker;
					RFootTracker = gm.RFootTracker;

					rightHandTracker = gm.LHandTracker;
					leftHandTracker = gm.RHandTracker;

					bodyTracker = gm.bodyTracker;
				}
				//the controllers are the easy ones, just move them directly
				// copyTransform(controllerLeft.transform, handLeft.transform);
				// copyTransform(controllerRight.transform, handRight.transform);
				//now move the head to the HMD position, this is actually the eye position
				copyTransform(hmd.transform, head);	

						/* can be added when we get the other trackers 
						rightHand.transform.position = rightHandTracker.transform.position;
						rightHand.transform.rotation = rightHandTracker.transform.rotation;
						
						leftHand.transform.position = leftHandTracker.transform.position;
						leftHand.transform.rotation = leftHandTracker.transform.rotation;
						*/

						//FOR FEET HAVE THE GREEN LIGHT FACING TOWARDS YOU
						leftFoot.transform.position = LFootTracker.transform.position;
						leftFoot.transform.rotation = LFootTracker.transform.rotation * Quaternion.Euler(90, 0, 0);
						rightFoot.transform.position = RFootTracker.transform.position;
						rightFoot.transform.rotation = RFootTracker.transform.rotation * Quaternion.Euler(90, 0, 0);

						//FOR THE CHEST HAVE THE GREEN LIGHT FACING UP
						body.transform.position = bodyTracker.transform.position;
						body.transform.rotation = bodyTracker.transform.rotation;;
			}

			CmdSyncPlayer(head.transform.position, head.transform.rotation, body.transform.position, body.transform.rotation, rightFoot.transform.position, rightFoot.transform.rotation,  leftFoot.transform.position, leftFoot.transform.rotation, rightHand.transform.position, rightHand.transform.rotation,  leftHand.transform.position, leftHand.transform.rotation);

		} else {
			//runs on all other clients and  the server
			//move to the syncvars
			head.position = Vector3.Lerp(head.position, headPos,.2f);
			head.rotation = Quaternion.Slerp(head.rotation, headRot, .2f);

			body.position = Vector3.Lerp(body.position, bodyPos,.2f);
			body.rotation = Quaternion.Slerp(body.rotation, bodyRot, .2f);

			rightFoot.transform.position = rightFootPosSync;
			rightFoot.transform.rotation = rightFootRotSync;
			leftFoot.transform.position = leftFootPosSync;
			leftFoot.transform.rotation = leftFootRotSync;

			leftHand.transform.position = leftHandPosSync;
			leftHand.transform.rotation = leftHandRotSync;
			rightHand.transform.position = rightHandPosSync;
			rightHand.transform.rotation = rightHandRotSync;
		}
	}

	// [Command]
	// void CmdSyncPlayer(Vector3 pos, Quaternion rot, Vector3 lhpos, Quaternion lhrot, Vector3 rhpos, Quaternion rhrot)
	// {
	// 	head.transform.position = pos;
	// 	head.transform.rotation = rot;
	// 	handLeft.transform.position = lhpos;
	// 	handRight.transform.position = rhpos;
	// 	handLeft.transform.rotation = lhrot;
	// 	handRight.transform.rotation = rhrot;
	// 	headPos = pos;
	// 	headRot = rot;
	// 	leftHandPos = lhpos;
	// 	leftHandRot = lhrot;
	// 	rightHandPos = rhpos;
	// 	rightHandRot = rhrot;
	// }

		[Command]
	void CmdSyncPlayer(Vector3 pos, Quaternion rot, Vector3 spinePos, Quaternion spineRot, Vector3 rightFootPos, Quaternion rightFootRot, Vector3 leftFootPos, Quaternion leftFootRot, Vector3 rightHandPos, Quaternion rightHandRot,Vector3 leftHandPos, Quaternion leftHandRot )
	{
		head.transform.position = pos;
		head.transform.rotation = rot;
		headPos = pos;
		headRot = rot;

		body.transform.position = spinePos;
		body.transform.rotation = spineRot;
		bodyPos = spinePos;
		bodyRot = spineRot;

		 
		rightHand.transform.position = rightHandPos;
		rightHand.transform.rotation = rightHandRot;
		rightHandPosSync = rightHandPos;
		rightHandRotSync = rightHandRot;

		leftHand.transform.position = leftHandPos;
		leftHand.transform.rotation = leftHandRot;
		leftHandPosSync = leftHandPos;
		leftHandRotSync = leftHandRot;

		rightFoot.transform.position = rightFootPos;
		rightFoot.transform.rotation = rightFootRot;
		rightFootPosSync = rightFootPos;
		rightFootRotSync = rightFootRot;

		leftFoot.transform.position = leftFootPos;
		leftFoot.transform.rotation = leftFootRot;
		leftFootPosSync = leftFootPos;
		leftFootRotSync = leftFootRot;
		
	}

	[ClientRpc]
	void RpcSyncKinectBodyTransform(NetworkInstanceId roverId, Vector3 pos, Quaternion rot)
	{
		// GameObject clientRover = ClientScene.FindLocalObject(roverId);

		// Vector3 diff = bigCenter - littleCenter;
		// clientRover.transform.position = littleCenter;

		// Vector3 differencePos = pos - bigCenter;
		// differencePos = differencePos * .01f;
		// clientRover.transform.rotation = rot;
		// clientRover.transform.position = littleCenter + differencePos;
	}

	[Command]
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

	private void copyTransform(Transform from, Transform to)
	{
		to.position = from.position;
		to.rotation = from.rotation;
	}

	private void handleControllerInputs()
	{
		// int indexLeft = (int)controllerLeft.index;
		// int indexRight = (int)controllerRight.index;

		// handLeft.controllerVelocity = getControllerVelocity(controllerLeft);
		// handRight.controllerVelocity = getControllerVelocity(controllerRight);
		// handLeft.controllerAngularVelocity = getControllerAngularVelocity(controllerLeft);
		// handRight.controllerAngularVelocity = getControllerAngularVelocity(controllerRight);
		
		// float triggerLeft = getTrigger(controllerLeft);
		// float triggerRight = getTrigger(controllerRight);

		// Vector2 joyLeft = getJoystick(controllerLeft);
		// Vector2 joyRight = getJoystick(controllerRight);
		// handLeft.squeeze(triggerLeft);
		// handRight.squeeze(triggerRight);
		
		// vehicleDrive(joyLeft, joyRight);
	}

	private float getTrigger(SteamVR_TrackedObject controller)
	{
		return controller.index >= 0 ? SteamVR_Controller.Input((int)controller.index).GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).magnitude : 0.0f;
	}

	private Vector2 getJoystick(SteamVR_TrackedObject controller)
	{
		return controller.index >= 0 ? SteamVR_Controller.Input((int)controller.index).GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad) : Vector2.zero;
	}

	private Vector3 getControllerVelocity(SteamVR_TrackedObject controller)
	{
		Vector3 controllerVelocity = controller.index >= 0 ? SteamVR_Controller.Input((int)controller.index).velocity : Vector3.zero;
		return SteamVR_Rig.localToWorldMatrix.MultiplyVector(controllerVelocity.normalized)*controllerVelocity.magnitude;
	}

	private Vector3 getControllerAngularVelocity(SteamVR_TrackedObject controller)
	{
		Vector3 angularVelocity = controller.index >= 0 ? SteamVR_Controller.Input((int)controller.index).angularVelocity : Vector3.zero;
		return SteamVR_Rig.localToWorldMatrix.MultiplyVector(angularVelocity.normalized) * angularVelocity.magnitude ;
	}

}

