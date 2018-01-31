using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFacer : MonoBehaviour {

	Transform camTransform;
	new Transform transform;

<<<<<<< HEAD
	void Awake () {
		camTransform = MiscUtilities.mainCamera.transform;
		transform = gameObject.transform;
		Update ();
=======
	void Start () {
		camTransform = MiscUtilities.mainCamera.transform;
		transform = gameObject.transform;
>>>>>>> 6918e9b0878999e1061e8a95b659822a79e570be
	}

	void Update(){
		transform.LookAt (camTransform);
	}

}
