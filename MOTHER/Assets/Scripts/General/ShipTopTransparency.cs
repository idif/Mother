using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTopTransparency : MonoBehaviour {

	private List<RendererData> renderList = new List<RendererData> ();

	public GameObject[] objectsToDisactivate;

	public float minLerp = 0.6f;
	public float maxLerp = 0.8f;

	private float alphaValue = 0;

	void Start () {
		GameManager.GetAllRenderers (gameObject, ref renderList);
	}
	


	void Update () {
		if (CameraMove.cameraYLerp > minLerp){
			if (CameraMove.cameraYLerp < maxLerp) {
				SetAlphaValue((CameraMove.cameraYLerp - minLerp) / (maxLerp - minLerp));
				if ((CameraMove.cameraYLerp - minLerp) / (maxLerp - minLerp) > 0.6) {
					foreach (GameObject temp in objectsToDisactivate) {
						temp.SetActive (true);
					}
				} else {
					foreach (GameObject temp in objectsToDisactivate) {
						temp.SetActive (false);
					}
				}
			} else if (alphaValue < 1) {
				SetAlphaValue (1);
			}

		}else if(alphaValue > 0){
			SetAlphaValue (0);
		}
	}

	void SetAlphaValue(float newValue){
		alphaValue = newValue;
		foreach (RendererData renderTemp in renderList) {
			GameManager.SetAlpha (ref renderTemp.renderer, alphaValue);
		}
	}
}
