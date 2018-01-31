using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomConstructionBar : SingletonBehaviour<RoomConstructionBar> {

	public GameObject furnitureBar;
	public GameObject[] furnitureButtons;
	public Text[] furnitureButtonsTexts;

	[Space()]

	[Header("Room Panel")]
	public float timeToOpenRoomPanel = 1;
	public GameObject panelPivot;
	public Image panelOpeningFeedback;
	[SerializeField]InputField roomName;
	[SerializeField]Text roomMood;
	[SerializeField]Image roomSprite;
	public float timeToOpenPanel = 0.5f;

	public List<Sprite> moodSprites = new List<Sprite>();
	Room currentRoom;

	void OnDisable(){
		furnitureBar.SetActive (false);
	}

	public void ChangeRoomName(Text roomNameText)
	{
		currentRoom.name = roomNameText.text;
		roomName.text = roomNameText.text;
	}

<<<<<<< HEAD

	public void ChangeRoomDeco(int decoId){

		currentRoom.UpdateDeco (decoId);

	}


=======
>>>>>>> 6918e9b0878999e1061e8a95b659822a79e570be
	public void DisplayRoomPanel(TileManager tile){
		
		if (tile.room == null) {
			return;
		}
		currentRoom = tile.room;
		panelPivot.SetActive (true);

		roomSprite.sprite = moodSprites [(int)tile.room.currentMood];

		panelPivot.transform.position = tile.transform.position;

		roomName.text = tile.room.name;

<<<<<<< HEAD
		RoomDecoSelection.Self ().feedbackBackground.localPosition = RoomDecoSelection.Self ().buttons [currentRoom.roomDeco].localPosition;

=======
>>>>>>> 6918e9b0878999e1061e8a95b659822a79e570be
		if (tile.room.currentMood == ChildMood.Normal) {

			roomMood.text = "No special mood for this room";

		} else {
			
			roomMood.text = tile.room.currentMood.ToString () + " level " + tile.room.moodLevel.ToString ();

		}

	}


	public void OnSelectedRoomTypeChanged(){

		if (ConstructionScript.selectedCategory == PropCategories.None) {
			furnitureBar.SetActive (false);
			UpdateButtons (0);
			return;
		}

		furnitureBar.SetActive (true);
		UpdateButtons (ConstructionScript.buildCategories [ConstructionScript.selectedCategory].linkedProps.Length);

	}

	void UpdateButtons(int buttonsToDisplay){

		#if UNITY_EDITOR
		if(buttonsToDisplay > furnitureButtons.Length){
			Debug.LogError("ERROR IN RoomConstructionBar SCRIPT : Not enough buttons for all the potential furnitures");
		}
		#endif

		for (int i = 0; i < furnitureButtons.Length; i++) {

			if (i < buttonsToDisplay) {
				furnitureButtons [i].SetActive (true);
				furnitureButtonsTexts [i].text = ConstructionScript.Self ().furnitureList [ConstructionScript.buildCategories [ConstructionScript.selectedCategory].linkedProps [i]].name;



			} else {
				furnitureButtons [i].SetActive (false);
			}

		}

	}

	#if UNITY_EDITOR

	public void SetTexts(){
		furnitureButtonsTexts = new Text[furnitureButtons.Length];

		for (int i = 0; i < furnitureButtons.Length; i++) {
			furnitureButtonsTexts [i] = furnitureButtons [i].GetComponentInChildren<Text> ();
		}

		Debug.Log ("Furniture Buttons Texts references have been automatically set.");

	}

	#endif

}
