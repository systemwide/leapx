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

	public Transform head;
	public Transform body;
	// public Transform cam;
	// public HandController handLeft;
	// public HandController handRight;
	// public Transform feet;
	// public Vector3 lastHipPosition;
	// public bool isWalking;
	// public float thrust;
	
	public Transform kinectBodyTransform;
	// public GameObject littleRover;
	// public Vector3 bigCenter;
	// public Vector3 littleCenter;
	// public GameObject taskObject;
	// public GameObject checkpoint;
	
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
	Vector3 leftHandPos;
	[SyncVar]
	Quaternion leftHandRot;
	[SyncVar]
	Vector3 rightHandPos;
	[SyncVar]
	Quaternion rightHandRot;

	void Start () {
		head.transform.position = new Vector3(0, 4, 0);
		body.transform.position = new Vector3(0, 2, 0);
		// var roverObj = GameObject.Find("BigRover");
		// if (roverObj != null) {
		// 	roverTransform = roverObj.transform;
		// }
			
		// littleRover = GameObject.Find("littleRover");
		// bigCenter = new Vector3(244.133f, 37.995f, 228.52f);
		// littleCenter = new Vector3(1.38f, 0.489f, 0.286f);
	}

	public void OnConnectedToServer()
	{
		// Set as ready
		NetworkServer.SetClientReady(connectionToClient);
	}

	// public void ApplyLocalPositionToVisuals(WheelCollider collider)
	// {
	// 	if (collider.transform.childCount == 0) {
	// 		return;
	// 	}

	// 	Transform visualWheel = collider.transform.GetChild(0);

	// 	Vector3 position;
	// 	Quaternion rotation;
	// 	collider.GetWorldPose(out position, out rotation);

	// 	visualWheel.transform.position = position;
	// 	visualWheel.transform.rotation = rotation;
	// }
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
					// controllerLeft = gm.controllerLeft;
					// controllerRight = gm.controllerRight;
				}
				//the controllers are the easy ones, just move them directly
				// copyTransform(controllerLeft.transform, handLeft.transform);
				// copyTransform(controllerRight.transform, handRight.transform);
				//now move the head to the HMD position, this is actually the eye position
				copyTransform(hmd.transform, head);
				try{
					if(GameObject.Find("SpineMid")!=null){
						Vector3 HeadPosKinect = GameObject.Find("Head").transform.position;
						Vector3 SpinePosKinect = GameObject.Find("SpineMid").transform.position;
						Vector3 diff = HeadPosKinect - SpinePosKinect;
						body.transform.position = hmd.transform.position - diff;
					}
				} catch(UnityException e) {
					
				} finally{

				}
				//move the feet to be in the tracking space, but on the ground (maybe do this with physics to ensure a good foot position later)
				// feet.position = Vector3.Scale(head.position, new Vector3(1, 0, 1)) + Vector3.Scale(SteamVR_Rig.position, new Vector3(0, 1, 0));
				// handleControllerInputs();
			} else {
				// float vertical = Input.GetAxis("Vertical");
				// float horizontal = Input.GetAxis("Horizontal");
				// transform.Translate(vertical * Time.fixedDeltaTime * (new Vector3(0, 0, 1)));
				// transform.Translate(horizontal * Time.fixedDeltaTime * (new Vector3(1, 0, 0)));
			}

			CmdSyncPlayer(head.transform.position,head.transform.rotation, body.transform.position, body.transform.rotation);
			//, handLeft.transform.position, handLeft.transform.rotation, handRight.transform.position, handRight.transform.rotation);
		} else {
			//runs on all other clients and  the server
			//move to the syncvars
			head.position = Vector3.Lerp(head.position, headPos,.2f);
			head.rotation = Quaternion.Slerp(head.rotation, headRot, .2f);
			body.position = Vector3.Lerp(body.position, bodyPos,.2f);
			body.rotation = Quaternion.Slerp(body.rotation, bodyRot, .2f);
			// handLeft.transform.position = leftHandPos;
			// handLeft.transform.rotation = leftHandRot;
			// handRight.transform.position = rightHandPos;
			// handRight.transform.rotation = rightHandRot;
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
	void CmdSyncPlayer(Vector3 pos, Quaternion rot, Vector3 spinePos, Quaternion spineRot)
	{
		head.transform.position = pos;
		head.transform.rotation = rot;
		headPos = pos;
		headRot = rot;

		body.transform.position = spinePos;
		body.transform.rotation = spineRot;
		bodyPos = spinePos;
		bodyRot = spineRot;
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

