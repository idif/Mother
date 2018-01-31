using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildCostText : MonoBehaviour {

	public Text costText;
	public float timeBeforeFade = 1.0f;
	public float fadingTime = 1.5f;

	Transform canvas;
	Camera m_Camera;

	void Start(){
		m_Camera = Camera.main;
		canvas = transform.GetChild (0);
	}

	public void OnConstruction(int cost){
		costText.color = Color.red;
		costText.text = "- " + cost.ToString () + " Z";
		StartCoroutine(Fade ());
	}

	public void OnDestruction(int gain){
		costText.color = Color.green;
		costText.text = "+ " + gain.ToString () + " Z";
		StartCoroutine(Fade ());
	}

	IEnumerator Fade () {
		yield return new WaitForSeconds (timeBeforeFade);
		Color colorTemp = costText.color;
		while (costText.color.a > 0) {
			colorTemp.a -= (Time.deltaTime / fadingTime);
			costText.color = colorTemp;
			yield return null;
		}
		Destroy (gameObject);
	}

	void Update(){
		canvas.LookAt (m_Camera.transform);
	}
}
