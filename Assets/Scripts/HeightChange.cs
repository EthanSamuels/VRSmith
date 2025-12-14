using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightChange : MonoBehaviour
{
	[SerializeField] private float changeBy = 0.2f;
	[SerializeField] private CharacterController cc;
	
    public void AddHeight()
	{
		cc.height += changeBy;
	}
	
	public void LoseHeight()
	{
		cc.height -= changeBy;
	}
}
