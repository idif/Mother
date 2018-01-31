using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFacer : MonoBehaviour {

	Transform camTransform;
	new Transform transform;

	void Start () {
		camTransform = MiscUtilities.mainCamera.transform;
		transform = gameObject.transform;
	}

	void Update(){
		transform.LookAt (camTransform);
	}

}
