using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealTimeRotationSkybox : MonoBehaviour {

	public float speed = 2;

	void Update () {
		RenderSettings.skybox.SetFloat("_Rotation", Time.time / speed); //To set the speed, just multiply the Time.time with whatever amount you want.
	}
}
