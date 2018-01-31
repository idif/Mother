using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ContextualMenu : SingletonBehaviour<ContextualMenu> {

	public Text titleText;

	public GameObject[] widgets;

	[System.NonSerialized]public GameObject currentWidget;
	public GameObject widgetBase;
	[System.NonSerialized] public GameObject linkedFurniture;

	public void OpenWidget(int widgetNb,GameObject originObject, string widgetTitle = ""){
		if (currentWidget != null) {
			CloseWidget ();
		}

		linkedFurniture = originObject;
		currentWidget = widgets [widgetNb];
		widgetBase.SetActive (true);
		currentWidget.SetActive (true);

		titleText.text = widgetTitle;
	}

	public void CloseWidget(){
		if (currentWidget != null) {
			currentWidget.SetActive (false);
			currentWidget = null;
		}
		if (linkedFurniture != null) {
			if (linkedFurniture.GetComponent<InteractibleFurniture> () != null) {
				linkedFurniture.GetComponent<InteractibleFurniture> ().isVisibleWidget = false;
			}
		}
		widgetBase.SetActive(false);
		linkedFurniture = null;
	}

	public void Messenger(string message){
		if (linkedFurniture != null) {
			linkedFurniture.SendMessage (message, SendMessageOptions.DontRequireReceiver);
		}
	}
}
