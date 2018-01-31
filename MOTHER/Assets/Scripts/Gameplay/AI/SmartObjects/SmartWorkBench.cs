using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartWorkBench : FurnitureSmartObject
{
	public Transform workPivot;
	public float polishedZackarium = 0;
	public float miningSpeed = 10;
	public float MaxZackarium = 401;

	Vector3 oldPosition;
	Quaternion oldRotation;

	[HideInInspector]public bool isWorking;

	public override void StartUseObject()
	{
		if (currentUser != null) {
			isWorking = true;
			StartCoroutine(StartToWork ());
		}
	}

	IEnumerator StartToWork()
	{
		oldPosition = currentUser.transform.position;
		oldRotation = currentUser.transform.rotation;

		currentUser.transform.position = workPivot.position;
		currentUser.transform.rotation = workPivot.rotation;

		while (isWorking && polishedZackarium < MaxZackarium) {
			Debug.Log ("polished zackarium = " + polishedZackarium);
			polishedZackarium += (miningSpeed * GameManager.Self().timeSpeed * Time.deltaTime);
			yield return new WaitForSeconds (0.2f);
		}
		isWorking = false;
		StopWorking ();
	}

	void StopWorking()
	{
		if(!isWorking)
		{
			currentUser.transform.position = oldPosition;
			currentUser.transform.rotation = oldRotation;
		}
		GameManager.AddZakarium((int)polishedZackarium);
		polishedZackarium = 0;
		EndUseObject ();
	}
}
