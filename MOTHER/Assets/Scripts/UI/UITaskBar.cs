using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITaskBar : SingletonBehaviour<UITaskBar> {

	[HideInInspector]public GameObject currentPanel;

	[System.NonSerialized]public bool objectivesDisplayed = false;


	[HideInInspector]public GameObject workHandyPanel;
	[HideInInspector]public Image workProgressImage;

	private float oldGameSpeed = 1;

	void Update(){

		if (Input.GetButtonDown ("Pause")) {

			if (GameManager.Self ().timeSpeed > 0) {
				oldGameSpeed = GameManager.Self ().timeSpeed;
				GameManager.SetTimeSpeed (0);
			} else {
				GameManager.SetTimeSpeed (oldGameSpeed);
			}

		}

	}

	public void OpenPanelWithoutScrollBar(GameObject panel){
		ConstructionScript.propID = -1;
		if (panel == currentPanel) {
			UpdateActivePanel (null);			
		} else {
			UpdateActivePanel (panel);
		}
	}


	public void SelectBuildCategory(int categoryId){

		ConstructionScript.selectedCategory = (PropCategories)categoryId;

	}


	public void SelectPropByCategory(int buttonId){
		
		int[] roomProps = ConstructionScript.buildCategories [ConstructionScript.selectedCategory].linkedProps;

		if (roomProps.Length > buttonId) {

			SelectProp (roomProps [buttonId]);

		} else {
			Debug.LogError ("ERROR : more buttons than props to build");
		}

	}



	public void SelectProp(int propID){
		if (ConstructionScript.Self().furnitureList [propID].zakariumCost <= GameManager.Self().zakarium) {
<<<<<<< HEAD

			if (ConstructionScript.Self ().furnitureList [propID].propPrefab.canBePlacedInLine) {
				ConstructionScript.propRotation = Quaternion.identity;
				ConstructionScript.propDirection = Direction.Right;
			}

			UpdateConstructionProp (propID);
		}else{
=======
			UpdateConstructionProp (propID);
		}else{

>>>>>>> 6918e9b0878999e1061e8a95b659822a79e570be
			UpdateConstructionProp (-1);
		
		}
	}


	public void SelectDestroy(){
		UpdateConstructionProp (-1);
		ConstructionScript.isDestructing = true;
	}


	public void CloseActivePanel(){
		UpdateActivePanel (null);
	}


	public void UpdateActivePanel(GameObject newActivePanel){
		
		if (currentPanel != null) {
			currentPanel.SetActive (false);
		}

		currentPanel = newActivePanel;
		UpdateConstructionProp (-1);

		if (currentPanel != null) {
			currentPanel.SetActive (true);
		}
			

	}


	public void UpdateConstructionProp(int newPropID = -1){
<<<<<<< HEAD

=======
>>>>>>> 6918e9b0878999e1061e8a95b659822a79e570be
		ConstructionScript.propID = newPropID;
		ConstructionScript.Self().DestroyDemoProp();
		
		ConstructionScript.isDestructing = false;

		if (RoomConstructionBar.Self ().panelPivot.activeSelf) {
			RoomConstructionBar.Self ().panelPivot.SetActive (false);
		}

	}


	public void SetGameSpeed(float  newSpeed){

		if (newSpeed == 0 && GameManager.Self().timeSpeed > 0) {
			oldGameSpeed = GameManager.Self ().timeSpeed;
		}

		GameManager.SetTimeSpeed (newSpeed);
	}


	public static IEnumerator ImageFlasher(Image image, Color color, float time = 5, float interval = 0.5f){
		
		Color baseColor = image.color;
		while (time > 0) {

			if (image.color == color) {
				image.color = baseColor;
			} else {
				image.color = color;
			}

			time -= interval;
			yield return new WaitForSeconds(interval);

		}

		image.color = baseColor;

	}
}