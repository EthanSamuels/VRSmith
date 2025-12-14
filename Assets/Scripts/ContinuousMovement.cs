using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;

public class ContinuousMovement : MonoBehaviour
{
    public XRNode moveSource, turnSource;
    public float speed = 1f, fallingSpeed = 0;
    public float turnLimit = 0.75f, turnDegree = 45.0f;

    private XROrigin rig;
    private Vector2 moveAxis, turnAxis;
    private CharacterController character;
    private bool hasTurned = false;
    
    void Start()
    {
        character = GetComponent<CharacterController>();
        rig = GetComponent<XROrigin>();
    }

    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(moveSource);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out moveAxis);
        device = InputDevices.GetDeviceAtXRNode(turnSource);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out turnAxis);
    }

    private void FixedUpdate(){
        CapsuleFollowHeadset();

        //Move direction facing
        Quaternion headYaw = Quaternion.Euler(0, rig.GetComponentInChildren<Camera>().transform.eulerAngles.y, 0);
        Vector3 direction = headYaw * new Vector3(moveAxis.x, 0, moveAxis.y);
        character.Move(direction * Time.fixedDeltaTime * speed);
        
        //Keep grounded
        character.Move(Vector3.up * fallingSpeed * Time.fixedDeltaTime);

        //Turn turnDegree degrees when initially turnAxis.x > turnLimit
        if(!hasTurned){
            if(turnAxis.x > turnLimit){
                transform.rotation *= Quaternion.Euler(0, turnDegree, 0);
                hasTurned = true;
            }
            else if(turnAxis.x < -turnLimit){
                transform.rotation *= Quaternion.Euler(0, -turnDegree, 0);
                hasTurned = true;
            }
        }
        else if(turnAxis.x > -turnLimit && turnAxis.x < turnLimit) hasTurned = false;
    }

    private void CapsuleFollowHeadset(){
        Vector3 capsuleCenter = transform.InverseTransformPoint(rig.GetComponentInChildren<Camera>().transform.position);
        character.center = new Vector3(capsuleCenter.x, 1, capsuleCenter.z);
    }
}
