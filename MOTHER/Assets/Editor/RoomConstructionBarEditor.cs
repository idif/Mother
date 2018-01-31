using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoomConstructionBar))]
public class RoomConstructionBarEditor : Editor {

	public override void OnInspectorGUI(){
		DrawDefaultInspector ();

		RoomConstructionBar inspectedScript = (RoomConstructionBar)target;

		if (GUILayout.Button ("Set Texts")) {
			inspectedScript.SetTexts ();
		}

	}

}
