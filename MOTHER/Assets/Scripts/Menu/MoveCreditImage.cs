using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveCreditImage : MonoBehaviour
{
	public float speed = 15;

	public Vector3 posInit = new Vector3(0,0,0);
	public Vector3 posMax = new Vector3(0,0,0);
 	public RectTransform myRectTransform;

	void Start()
	{
		myRectTransform = GetComponent<RectTransform>();
		posInit = myRectTransform.localPosition;
		posMax = new Vector3(myRectTransform.localPosition.x, 1300, myRectTransform.localPosition.z);
	}

	void Update () 
	{
		if(myRectTransform.localPosition != posMax)
		{
			myRectTransform.localPosition += Vector3.up / speed;
		}

		if(myRectTransform.localPosition.y >= posMax.y)
		{
			myRectTransform.localPosition = posInit;
		}
	}


}
