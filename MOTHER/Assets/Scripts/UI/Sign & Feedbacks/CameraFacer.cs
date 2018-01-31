using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFacer : MonoBehaviour {

	Transform camTransform;
	new Transform transform;

	void Awake () {
		camTransform = MiscUtilities.mainCamera.transform;
		transform = gameObject.transform;
		Update ();
	}

	void Update(){
		transform.LookAt (camTransform);
	}

}
