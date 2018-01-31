using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosePanelManager : SingletonBehaviour<ClosePanelManager> {

	void Update () {
		if (Input.GetButtonDown ("Cancel")) {
			
			if (RoomConstructionBar.Self ().panelPivot.activeSelf) {
				
				RoomConstructionBar.Self ().panelPivot.SetActive (false);

			} else if (UITaskBar.Self ().currentPanel != null) {
				
				UITaskBar.Self ().UpdateActivePanel (null);
				ConstructionScript.UnSelectTile ();

			} else if (ContextualMenu.Self ().currentWidget != null) {
				
				ContextualMenu.Self ().CloseWidget ();

			}else if(ConstructionScript.isDestructing){

				ConstructionScript.isDestructing = false;
				ConstructionScript.UnSelectTile ();

			}else{
				
				PauseMenu.Self ().CallPauseMenu ();

			}
		}
	}
}
