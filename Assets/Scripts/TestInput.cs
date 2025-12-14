using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class TestInput : MonoBehaviour
{
	[SerializeField] private XRNode interactSource;
	
    private void Update()
	{
		InputDevice device = InputDevices.GetDeviceAtXRNode(interactSource);
		bool interact;
		device.TryGetFeatureValue(CommonUsages.primaryTouch, out interact);
		
		Debug.Log(interact ? "Touching" : "Not Touching");
	}
}
