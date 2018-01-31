using System.Collections;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileManager)), CanEditMultipleObjects]
public class TileManagerEditor : Editor {
    
	public override void OnInspectorGUI () {
        DrawDefaultInspector();

		TileManager[] tiles = new TileManager[targets.Length];

        if(GUILayout.Button("Generate Object"))
        {
			
			for (int i = 0; i < tiles.Length; i ++) {

				tiles [i] = (TileManager)targets [i];

				FurnitureBehaviour objectOnTile = tiles [i].transform.GetComponentInChildren<FurnitureBehaviour> ();
				if (objectOnTile != null) {
					DestroyImmediate (objectOnTile.gameObject);
				}

				switch (tiles [i].tileStartStatus) {

				case StartObject.ZoneWall:
					tiles [i].BuildProp (GridCreator.Self ().zoneWall);
					break;

				case StartObject.ZoneDoor:
					tiles [i].BuildProp (GridCreator.Self ().zoneDoor);
					break;

				case StartObject.ForOfWar:
					tiles [i].BuildProp (GridCreator.Self ().fogOfWar);
					break;

				default:
					break;
				}

			}
        }

		if (GUILayout.Button ("Rotate Object")) {
			
			for (int i = 0; i < tiles.Length; i ++) {

				tiles [i] = (TileManager)targets [i];

				FurnitureBehaviour objectTemp = tiles [i].transform.GetComponentInChildren<FurnitureBehaviour> ();

				if (objectTemp != null) {
					Transform objectToRotate = objectTemp.transform;
					Quaternion rotation = objectToRotate.rotation;
					rotation.eulerAngles = new Vector3 (objectToRotate.rotation.eulerAngles.x, objectToRotate.rotation.eulerAngles.y + 90, objectToRotate.rotation.eulerAngles.z);
					objectToRotate.rotation = rotation;
				}

			}
		}

    }
	
}
