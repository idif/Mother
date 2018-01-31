using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RaycastManager : MonoBehaviour {

	GameObject lastHitTile;
	GameObject lastHitGeneral;
	GameObject lastClickedObject;

	int generalLayerMask = ~(1 << 13 | 1 << 2 | 1 << 11);
	int tileLayerMask = 1 << 11;

	void Update () {
		RayCaster (generalLayerMask, ref lastHitGeneral,true);
		RayCaster (tileLayerMask, ref lastHitTile);
	}

	void RayCaster(int layerMask, ref GameObject lastHitObject, bool checkClickElswhere = false){
		RaycastHit mouseCrateHit;
		Ray mouseCrateRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (mouseCrateRay, out mouseCrateHit, float.PositiveInfinity, layerMask) && !EventSystem.current.IsPointerOverGameObject ()) {

			if (lastHitObject != null && mouseCrateHit.collider.gameObject != lastHitObject) {
				lastHitObject.SendMessage ("OnMouseQuit", SendMessageOptions.DontRequireReceiver);
			}

			if (mouseCrateHit.collider.gameObject != lastHitObject) {
				mouseCrateHit.collider.gameObject.SendMessage ("OnMouseIsOver", SendMessageOptions.DontRequireReceiver);
			}
			
			lastHitObject = mouseCrateHit.collider.gameObject;

			if (Input.GetMouseButtonDown (0)) {
				if (checkClickElswhere) {
					if (lastClickedObject != lastHitObject) {
						if (lastClickedObject != null) {
							lastClickedObject.SendMessage ("OnMouseClickElsewhere", SendMessageOptions.DontRequireReceiver);
						}
						lastClickedObject = lastHitObject;
					}
				}
				lastHitObject.SendMessage ("OnMouseClickOn", SendMessageOptions.DontRequireReceiver);

			} else if (Input.GetMouseButtonUp (0)) {

				lastHitObject.SendMessage ("OnMouseClickUp", SendMessageOptions.DontRequireReceiver);

			}

			if (Input.GetMouseButtonDown (1)) {
				lastHitObject.SendMessage ("OnMouseRightClickOn", SendMessageOptions.DontRequireReceiver);
			}

		} else {
			
			if (lastHitObject != null) {
				lastHitObject.SendMessage ("OnMouseQuit", SendMessageOptions.DontRequireReceiver);
				lastHitObject = null;
			}

			if (Input.GetMouseButtonUp (0) && (ConstructionScript.propID != -1 || ConstructionScript.isDestructing)) {
				ConstructionScript.CalculatePropLine ();
				ConstructionScript.clickedTile = null;
			}
		}
	}
		
}