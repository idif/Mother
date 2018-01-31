using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveOptionsManager : MonoBehaviour 
{
	public int paramMouseUse = 0;
	public float paramMoveSpeed = 1;

	public Slider mouseSlider;
	public Toggle activeMousecontrol; 

	public void SaveInt()
	{
		IsControlMouse ();
		PlayerPrefs.SetInt("Mouse Move Control", paramMouseUse);
	}

	public void SaveFloat()
	{
		MovespeedValue ();
		PlayerPrefs.SetFloat("Camera Move Sensibility", paramMoveSpeed);
	}

	public void MovespeedValue()
	{
		paramMoveSpeed = mouseSlider.value;
	}

	public void IsControlMouse()
	{
		if(activeMousecontrol.isOn)
		{
			paramMouseUse = 1;
		}
		else
		{
			paramMouseUse = 0;
		}
	}
}
