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
	public SteamVR_TrackedObject RHandTracker;
	public SteamVR_TrackedObject LHandTracker;
	public SteamVR_TrackedObject LFootTracker;
	public SteamVR_TrackedObject RFootTracker;

	public Transform head;
	public Transform body;
	public Transform rightFoot;
	public Transform leftFoot;
	public Transform rightHand;
	public Transform leftHand;

	private GameManager gm;

	[HideInInspector]
	[SyncVar]
	public Vector3 headPos;
	[HideInInspector]
	[SyncVar]
	public Quaternion headRot;
	[HideInInspector]
	[SyncVar]
	public Vector3 bodyPos;
	[HideInInspector]
	[SyncVar]
	public Quaternion bodyRot;
	[HideInInspector]
	[SyncVar]
	public Vector3 leftHandPosSync;
	[HideInInspector]
	[SyncVar]
	public Quaternion leftHandRotSync;
	[HideInInspector]
	[SyncVar]
	public Vector3 rightHandPosSync;
	[HideInInspector]
	[SyncVar]
	public Quaternion rightHandRotSync;
	[HideInInspector]
	[SyncVar]
	public Vector3 leftFootPosSync;
	[HideInInspector]
	[SyncVar]
	public Quaternion leftFootRotSync;
	[HideInInspector]
	[SyncVar]
	public Vector3 rightFootPosSync;
	[HideInInspector]
	[SyncVar]
	public Quaternion rightFootRotSync;

	void Start () {
		head.transform.position = new Vector3(0, 4, 0);
		body.transform.position = new Vector3(0, 2, 0);
		gm = GameObject.FindObjectOfType<GameManager>();
	}

	public void OnConnectedToServer()
	{
		NetworkServer.SetClientReady(connectionToClient);
	}

	private void FixedUpdate()
	{
		if (isLocalPlayer) {
			if (UnityEngine.XR.XRSettings.enabled) {
				if (SteamVR_Rig == null) {

					// grab tracked objects from GameManager

					SteamVR_Rig = gm.vrCameraRig.transform;

					hmd = gm.hmd;

					controllerLeft = gm.controllerLeft;
					controllerRight = gm.controllerRight;

					LHandTracker = gm.LHandTracker;
					RHandTracker = gm.RHandTracker;

					LFootTracker = gm.LFootTracker;
					RFootTracker = gm.RFootTracker;
				}

				// ---- these are the positions we will track

				copyTransform(hmd.transform, head);	

				copyTransform(controllerLeft.transform, body);

				copyTransform(LHandTracker.transform, leftHand);
				copyTransform(RHandTracker.transform, rightHand);
				
				copyTransform(LFootTracker.transform, leftFoot);
				copyTransform(RFootTracker.transform, rightFoot);

				// -------
			}

			// sync positions on the server

			CmdSyncPlayer(head.transform.position, head.transform.rotation, 
							body.transform.position, body.transform.rotation,
								leftHand.transform.position, leftHand.transform.rotation,
									rightHand.transform.position, rightHand.transform.rotation,
										leftFoot.transform.position, leftFoot.transform.rotation,
											rightFoot.transform.position, rightFoot.transform.rotation);

		} else {

			// (runs on all other clients and the server)

			// move positions to the sync vars

			head.position = Vector3.Lerp(head.position, headPos,.2f);
			head.rotation = Quaternion.Slerp(head.rotation, headRot, .2f);

			body.position = Vector3.Lerp(body.position, bodyPos,.2f);
			body.rotation = Quaternion.Slerp(body.rotation, bodyRot, .2f);

			leftHand.position = Vector3.Lerp(leftHand.position, leftHandPosSync,.2f);
			leftHand.rotation = Quaternion.Slerp(leftHand.rotation, leftHandRotSync, .2f);
			rightHand.position = Vector3.Lerp(rightHand.position, rightHandPosSync,.2f);
			rightHand.rotation = Quaternion.Slerp(rightHand.rotation, rightHandRotSync, .2f);

			leftFoot.position = Vector3.Lerp(leftFoot.position, leftFootPosSync,.2f);
			leftFoot.rotation = Quaternion.Slerp(leftFoot.rotation, leftFootRotSync, .2f);
			rightFoot.position = Vector3.Lerp(rightFoot.position, rightFootPosSync,.2f);
			rightFoot.rotation = Quaternion.Slerp(rightFoot.rotation, rightFootRotSync, .2f);
		}
	}

	[Command]
	void CmdSyncPlayer(Vector3 hpos, Quaternion hrot, 
							Vector3 bpos, Quaternion brot, 
								Vector3 lhpos, Quaternion lhrot, 
									Vector3 rhpos, Quaternion rhrot, 
										Vector3 lfpos, Quaternion lfrot, 
											Vector3 rfpos, Quaternion rfrot)
	{

		// head

		head.transform.position = hpos;
		head.transform.rotation = hrot;
		headPos = hpos;
		headRot = hrot;

		// body

		body.transform.position = bpos;
		body.transform.rotation = brot;
		bodyPos = bpos;
		bodyRot = brot;

		// hands

		leftHand.transform.position = lhpos;
		leftHand.transform.rotation = lhrot;
		leftHandPosSync = lhpos;
		leftHandRotSync = lhrot;
		 
		rightHand.transform.position = rhpos;
		rightHand.transform.rotation = rhrot;
		rightHandPosSync = rhpos;
		rightHandRotSync = rhrot;

		// feet

		leftFoot.transform.position = lfpos;
		leftFoot.transform.rotation = lfrot;
		leftFootPosSync = lfpos;
		leftFootRotSync = lfrot;

		rightFoot.transform.position = rfpos;
		rightFoot.transform.rotation = rfrot;
		rightFootPosSync = rfpos;
		rightFootRotSync = rfrot;
	}

	// [Command]
    // public void CmdInstantiateTaskObjects(Vector3 newPos)
    // {
	// 	GameObject newTask = Instantiate(taskObject, transform.position + newPos, Quaternion.identity) as GameObject;
	// 	newTask.transform.localScale += new Vector3(10, 10, 10);
	// 	//Network.Instantiate (TaskObject, newPos, Quaternion.identity, 0);

	// 	//create checkpoint
	// 	GameObject newCheckpoint = Instantiate(checkpoint, transform.position + newPos, Quaternion.identity) as GameObject;
	// 	newCheckpoint.transform.parent = newTask.transform;

	// 	//disable renderer on the taskObject container so that it can be used as a task boundary
	// 	newTask.transform.GetComponent<Renderer>().enabled = false;
	// 	newTask.GetComponent<Rigidbody>().useGravity = true;
	// 	newTask.GetComponent<Rigidbody>().isKinematic = false;

    //     NetworkServer.Spawn(newTask);
    //     NetworkServer.Spawn(newCheckpoint);
    // }

	// private void handleControllerInputs()
	// {
	// 	int indexLeft = (int)controllerLeft.index;
	// 	int indexRight = (int)controllerRight.index;

	// 	handLeft.controllerVelocity = getControllerVelocity(controllerLeft);
	// 	handRight.controllerVelocity = getControllerVelocity(controllerRight);
	// 	handLeft.controllerAngularVelocity = getControllerAngularVelocity(controllerLeft);
	// 	handRight.controllerAngularVelocity = getControllerAngularVelocity(controllerRight);
		
	// 	float triggerLeft = getTrigger(controllerLeft);
	// 	float triggerRight = getTrigger(controllerRight);

	// 	Vector2 joyLeft = getJoystick(controllerLeft);
	// 	Vector2 joyRight = getJoystick(controllerRight);
	// 	handLeft.squeeze(triggerLeft);
	// 	handRight.squeeze(triggerRight);
		
	// 	vehicleDrive(joyLeft, joyRight);
	// }

	// private float getTrigger(SteamVR_TrackedObject controller)
	// {
	// 	return controller.index >= 0 ? SteamVR_Controller.Input((int)controller.index).GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).magnitude : 0.0f;
	// }

	// private Vector2 getJoystick(SteamVR_TrackedObject controller)
	// {
	// 	return controller.index >= 0 ? SteamVR_Controller.Input((int)controller.index).GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad) : Vector2.zero;
	// }

	// private Vector3 getControllerVelocity(SteamVR_TrackedObject controller)
	// {
	// 	Vector3 controllerVelocity = controller.index >= 0 ? SteamVR_Controller.Input((int)controller.index).velocity : Vector3.zero;
	// 	return SteamVR_Rig.localToWorldMatrix.MultiplyVector(controllerVelocity.normalized)*controllerVelocity.magnitude;
	// }

	// private Vector3 getControllerAngularVelocity(SteamVR_TrackedObject controller)
	// {
	// 	Vector3 angularVelocity = controller.index >= 0 ? SteamVR_Controller.Input((int)controller.index).angularVelocity : Vector3.zero;
	// 	return SteamVR_Rig.localToWorldMatrix.MultiplyVector(angularVelocity.normalized) * angularVelocity.magnitude ;
	// }

	private void copyTransform(Transform from, Transform to)
	{
		to.position = from.position;
		to.rotation = from.rotation;
	}
}

