using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]

public class IKControl : MonoBehaviour {

    protected Animator animator;

    public bool ikActive = false;
    public Transform leftHandObj = null;
    public Transform rightHandObj = null;
    public Transform headObj = null;
    public Transform headRobo = null;
    public Transform LeftFootObj;
    public Transform RightFootObj;
    public Transform cam;
    public Transform box;
    public Transform Kinect;
    public Transform hipsObj = null;

    public Dictionary<string, GameObject> gameObjectCache;

    void Awake() {
        this.gameObjectCache = new Dictionary<string, GameObject>();
    }

    void Start() {
        animator = GetComponent<Animator>();
        this.transform.position += new Vector3(0, 1.042749f, 0);
    }
    void LateUpdate() {
        /*
        if (headObj != null) {
            Transform head = animator.GetBoneTransform(HumanBodyBones.Head);
            head.rotation = headObj.rotation; //you can't move stuff like this
            head.position = headObj.position; //you can't move stuff like this
        }
        */
             
            //Transform Lhand = animator.GetBoneTransform(HumanBodyBones.LeftHand);
           // Lhand.rotation = leftHandObj.rotation;
           // Lhand.Rotate(-30,0,90);
            //Lhand.position = leftHandObj.position;

            //Transform Rhand = animator.GetBoneTransform(HumanBodyBones.RightHand);
            //Rhand.rotation = rightHandObj.rotation;
            //Rhand.Rotate(-30,0,-90);
            //Rhand.position = rightHandObj.position;
    }

    public GameObject FindOnce(string name) {
        GameObject foundObj;
        if (!this.gameObjectCache.TryGetValue(name, out foundObj)) {
            foundObj = GameObject.Find(name);
            if (foundObj != null) {
                this.gameObjectCache.Add(name, foundObj);
            }
        }

        return foundObj;
    }

    //a callback for calculating IK
    void OnAnimatorIK() {
        if (animator) {

            //if the IK is active, set the position and rotation directly to the goal. 
            if (ikActive) {
 
                 if (Kinect != null && FindOnce("Head") != null) {
                    //this.transform.rotation = headObj.rotation;
                    GameObject H = FindOnce("Head");
                    GameObject Spine = FindOnce("SpineBase");
                    
                    GameObject LeftShoulder = FindOnce("ShoulderLeft"); 
                    GameObject RightShoulder = FindOnce("ShoulderRight");

                    if (headObj != null) {
                        //Transform head = animator.GetBoneTransform(HumanBodyBones.Head);
                        //head.rotation = headObj.rotation;
                        //head.position = headObj.position;

                    }

                    Vector3 PosDifference = headObj.position -  H.transform.position;
                    //headObj.rotation = H.transform.rotation;
                    //headObj.position = H.transform.position;
                    //cam.position = H.transform.position;
                                       
                    //Set Hips                  
                    Vector3 SpineKinectDiff = (H.transform.position - Spine.transform.position);
                    animator.SetBoneLocalRotation(HumanBodyBones.Hips, Spine.transform.rotation * Quaternion.Euler(0, -90, 0));
                    animator.bodyPosition = headObj.transform.position - SpineKinectDiff;
                    //animator.bodyRotation = Spine.transform.rotation;// * Quaternion.Euler(0 ,90 ,0) ;    

                    //set feet
                    GameObject LF =  FindOnce("FootLeft");
                    Vector3 LFKinectDiff = (H.transform.position - LF.transform.position);
                    Vector3 LFPos = headObj.position - LFKinectDiff;
                    Quaternion LFRot = LF.transform.rotation * Quaternion.Euler(0 ,180 ,0);// * fix;
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);
                    animator.SetIKPosition(AvatarIKGoal.LeftFoot,LFPos);
                    animator.SetIKRotation(AvatarIKGoal.LeftFoot, LFRot);

                    GameObject RF =  FindOnce("FootRight");
                    Vector3 RFKinectDiff = (H.transform.position - RF.transform.position);
                    Vector3 RFPos = headObj.position - RFKinectDiff;
                    Quaternion RFRot = RF.transform.rotation * Quaternion.Euler(0 ,180 ,0);// * fix;
                    animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1);
                    animator.SetIKPosition(AvatarIKGoal.RightFoot, RFPos);
                    animator.SetIKRotation(AvatarIKGoal.RightFoot, RFRot);

                    //set hands
                    GameObject LH =  FindOnce("HandLeft");
                    Vector3 LHKinectDiff = (H.transform.position - LH.transform.position);
                    Vector3 LHPos = headObj.position - LHKinectDiff;
                    //Vector3 PosDifferenceHandLeft = headObj.position -  LH.transform.position;
                    //Vector3 LHPos = LH.transform.position + PosDifferenceHandLeft;
                    Quaternion LHRot = LH.transform.rotation;
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                    animator.SetIKPosition(AvatarIKGoal.LeftHand,LHPos);
                    //animator.SetIKRotation(AvatarIKGoal.LeftHand, LHRot); // I am using the rotation of the controller currently

                    GameObject RH =  FindOnce("HandRight");
                    Vector3 RHKinectDiff = (H.transform.position - RH.transform.position);
                    Vector3 RHPos = headObj.position - RHKinectDiff;
                    Quaternion RHRot = RH.transform.rotation;
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                    animator.SetIKPosition(AvatarIKGoal.RightHand, RHPos);
                    //animator.SetIKRotation(AvatarIKGoal.RightHand, RHRot); // I am using the rotation of the controller currently


                    //set elbows
                    /* 
                    GameObject LE =  FindOnce("ElbowLeft");
                    Vector3 LEPos = LE.transform.position;// + PosDifferenceHandLeft;
                    Quaternion LERot = LE.transform.rotation;
                    animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, 1);
                    animator.SetIKHintPosition(AvatarIKHint.LeftElbow,LEPos);

                    
                    GameObject RE =  FindOnce("ElbowRight");
                    Vector3 REPos = RE.transform.position;// + PosDifferenceHandRight;
                    Quaternion RERot = RE.transform.rotation;
                    animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, 1);
                    animator.SetIKHintPosition(AvatarIKHint.RightElbow,REPos);
                    
                    //set knees
                    GameObject LKnee =  FindOnce("KneeLeft");
                    //Vector3 LKneeKinectDiff = (H.transform.position - LKnee.transform.position);
                    //Vector3 LKneePos = headObj.position - LKneeKinectDiff;
                    Vector3 LKneePos = LKnee.transform.position;// + PosDifference;
                    Quaternion LKneeRot = LKnee.transform.rotation;
                    animator.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, 1);
                    animator.SetIKHintPosition(AvatarIKHint.LeftKnee,LKneePos);

                    GameObject RKnee =  FindOnce("KneeRight");
                    //Vector3 RKneeKinectDiff = (H.transform.position - RKnee.transform.position);
                    //Vector3 RKneePos = headObj.position - RKneeKinectDiff;
                    Vector3 RKneePos = RKnee.transform.position;// + PosDifference;
                    Quaternion RKneeRot = RKnee.transform.rotation;
                    animator.SetIKHintPositionWeight(AvatarIKHint.RightKnee, 1);
                    animator.SetIKHintPosition(AvatarIKHint.RightKnee,RKneePos);
                    */
                    
                }//Kinect
 
                else{
                    if (leftHandObj != null) {
                        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandObj.position);
                        animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandObj.rotation);
                    }

                    // Set the right hand target position and rotation, if one has been assigned
                    if (rightHandObj != null) {
                        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                        animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
                        animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);
                    }

                    if (RightFootObj != null) {
                        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
                        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1);
                        animator.SetIKPosition(AvatarIKGoal.RightFoot, RightFootObj.position);
                        animator.SetIKRotation(AvatarIKGoal.RightFoot, RightFootObj.rotation);
                    }

                    if (LeftFootObj != null) {
                        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
                        animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);
                        animator.SetIKPosition(AvatarIKGoal.LeftFoot, LeftFootObj.position);
                        animator.SetIKRotation(AvatarIKGoal.LeftFoot, LeftFootObj.rotation);
                    }//no Kinect
                }
                    
                    
            }

            //if the IK is not active, set the position and rotation of the hand and head back to the original position
            else {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
            }
        }
    }
}