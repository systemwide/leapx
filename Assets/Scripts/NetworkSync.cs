using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class NetworkSync : NetworkBehaviour
{

    [SyncVar]
    Vector3 networkVel;
    [SyncVar]
    Vector3 networkPos;
    [SyncVar]
    Quaternion networkRot;
    [SyncVar]
    Vector3 networkAVel;

    public float snapThreshold;
    public float snapThresholdRot;
    Rigidbody rb;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        networkPos = transform.position;
        networkVel = rb.velocity;
        networkRot = rb.rotation;
        networkAVel = rb.angularVelocity;

    }

    public override float GetNetworkSendInterval()
    {
        return .1f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

      if (hasAuthority)
      {
          CmdUpdate(rb.position, rb.velocity, rb.rotation, rb.angularVelocity);
      }
      else
      {

            rb.velocity = Vector3.Lerp(rb.velocity, networkVel, .1f);
            Vector3 offset = rb.position - networkPos;
            if (offset.magnitude > snapThreshold)
            {
                rb.position = Vector3.Lerp(rb.position, networkPos, .1f);
            }

            rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, networkAVel, .1f);

            float angle; Vector3 axis;
            Quaternion rOffset = rb.rotation * Quaternion.Inverse(networkRot);
            rOffset.ToAngleAxis(out angle, out axis);
            if (Mathf.Abs(angle) > snapThresholdRot)
            {
                rb.rotation = Quaternion.Slerp(rb.rotation, networkRot, .1f);
            }
		//CmdUpdate (rb.position, rb.velocity, rb.rotation, rb.angularVelocity);
      }

    }
    [Command]
    void CmdUpdate(Vector3 pos, Vector3 vel, Quaternion rot, Vector3 aVel)
    {
        networkPos = pos;
        networkVel = vel;
        networkAVel = aVel;
        networkRot = rot;
    }
}
