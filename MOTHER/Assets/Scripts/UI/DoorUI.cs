using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorUI : MonoBehaviour {

	public ContextualMenu widgetScript;
	DoorScript currentDoor;
	public Text[] requiredSkillTexts;
	public RectTransform progressGauge;

	private float maxAnchorX;

	void Awake(){
		maxAnchorX = progressGauge.anchorMax.x;
		progressGauge.anchorMax = new Vector2 (0, progressGauge.anchorMax.y);
	}
	
	void OnEnable(){
		currentDoor = widgetScript.linkedFurniture.GetComponent<DoorScript> ();
		if (currentDoor != null) {
			for(int i = 0; i < currentDoor.requiredSkillsList.Length;i++){
				requiredSkillTexts [i].text = currentDoor.requiredSkillsList [i].skill.ToString () + " level " + currentDoor.requiredSkillsList [i].requiredLevel.ToString ();
			}

				StartCoroutine (UpdateUnlock ());
		}
	}

	void OnDisable(){
		currentDoor = null;
		StopAllCoroutines ();
	}

	IEnumerator UpdateUnlock(){
		while (currentDoor.unlockProgress < 1) {
			progressGauge.anchorMax = new Vector2 (maxAnchorX * currentDoor.unlockProgress, progressGauge.anchorMax.y);
			yield return 0;
		}
	}
}
