using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartBed : FurnitureSmartObject {

	public float refilingSpeed = 0.01f;
	public Transform sleepPivot;

	public override void StartUseObject(){

		if (currentUser != null) {
			StartCoroutine (Sleeping ());
		}

	}


	IEnumerator Sleeping(){

		Vector3 oldPosition = currentUser.mesh.transform.localPosition;
		Quaternion oldRotation = currentUser.mesh.transform.localRotation;
		currentUser.mesh.transform.position = sleepPivot.position;
		currentUser.mesh.transform.rotation = sleepPivot.rotation;

		while (currentUser.needs.sleep.currentLevel < 1) {

			currentUser.needs.sleep.currentLevel += refilingSpeed * GameManager.Self ().timeSpeed * Time.deltaTime;
			yield return new WaitForSeconds (0);

		}

		currentUser.mesh.transform.localPosition = oldPosition;
		currentUser.mesh.transform.localRotation = oldRotation;

		EndUseObject ();

	}

}
