using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotationDirectionalLight : MonoBehaviour {

	public float speed = 2;
		

	void Update () 
	{
		transform.Rotate(Vector3.up * Time.time / speed);
	}
}
