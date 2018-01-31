using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDecoSelection : SingletonBehaviour<RoomDecoSelection> {

	public Transform feedbackBackground;
	public Transform[] buttons;

	public void SelectDeco(int id){

		feedbackBackground.position = buttons [id].position;
		RoomConstructionBar.Self ().ChangeRoomDeco (id);

	}

}
