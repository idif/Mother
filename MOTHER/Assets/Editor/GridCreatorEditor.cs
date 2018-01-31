using System.Collections;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(GridCreator))]
public class GridCreatorEditor : Editor {

    public override void OnInspectorGUI(){

		DrawDefaultInspector ();

		GridCreator gridScirpt = (GridCreator)target;

		if (GUILayout.Button ("Generate Grid")) {
			gridScirpt.GenerateGrid ();
		}

        if (GUILayout.Button("Update Prefabs"))
        {
            gridScirpt.ChangePrefabProperties();
        }

    }

}
