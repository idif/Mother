using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorCursorMove : SingletonBehaviour<SectorCursorMove> {

	[SerializeField] private RectTransform cursor;
	[SerializeField] private RectTransform cursorStart;
	[SerializeField] private RectTransform cursorGoal;

	void Update(){
		cursor.position = new Vector3 (Mathf.Lerp 
									  (cursorStart.position.x, 
									   cursorGoal.position.x,
									   (TimeManager.days)/TimeManager.Self().nbDays),
									   cursor.position.y,
									   cursor.position.z);
	}

}
