using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaShaderLookAt : MonoBehaviour {

	public Camera mainCamera;
	public Transform pivot;


	void Start()
	{
		mainCamera = Camera.main;
	}
	void Update () 
	{
		pivot.LookAt (new Vector3(mainCamera.transform.position.x, transform.position.y, mainCamera.transform.position.z));	
	}
}
