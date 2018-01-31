using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RequiredSkill{
	public ChildSkill skill;
	public int requiredLevel = 0;
}

[RequireComponent(typeof(FurnitureBehaviour))]
public class DoorScript : MonoBehaviour 
{
	
	public RequiredSkill[] requiredSkillsList;

	public bool isUnlocked = false;
	[System.NonSerialized]public float unlockProgress = 0;
	[System.NonSerialized] public bool isMouseOverDoor = false;

	FurnitureBehaviour furnitureScript;


	void Awake(){
		furnitureScript = GetComponent<FurnitureBehaviour> ();
	}


	void OnMouseIsOver(){
		isMouseOverDoor = true;
	}


	void OnMouseQuit(){
		isMouseOverDoor = false;
	}


	void OnMouseClickOn(){
		if (ConstructionScript.isDestructing || ConstructionScript.propID != -1) {
			return;
		}

		ContextualMenu.Self ().OpenWidget (0, gameObject, "Locked Door");
	}


	void OnMouseRightClickOn(){
		ChildBehaviour currentKid = (ChildBehaviour)ChildBehaviour.selectedCharacter;
		if(currentKid != null){
			bool skillIsValid = true;
			foreach(RequiredSkill exigedSkill in requiredSkillsList){
				if(currentKid.skillRefs[exigedSkill.skill].skillLevel < exigedSkill.requiredLevel){
					skillIsValid = false;
				}
			}

			if (skillIsValid) {
				//currentKid.doorToOpen = this;
				//currentKid.OnStateChange (ChildState.OpeningDoor);
			}
		}
	}


	void OpenRoom(){
		
		foreach (TileManager tile in GetRoomToOpen()) {
			Destroy (tile.objectOnTile.gameObject);
			tile.objectOnTile = null;
		}

		TileManager doorTile = furnitureScript.tiles[0];
		doorTile.objectOnTile = null;

		List<TileManager> actualRoom = new List<TileManager> ();
		doorTile.GetRoomTiles (ref actualRoom);

		Room oldRoom = furnitureScript.tiles [0].room;

		if (oldRoom != null) {
<<<<<<< HEAD
			new Room (actualRoom,true,oldRoom.roomDeco,oldRoom.name);
=======
			new Room (actualRoom,true,oldRoom.color,oldRoom.name);
>>>>>>> 6918e9b0878999e1061e8a95b659822a79e570be
		}else{
			new Room (actualRoom,true);
		}

		Destroy (furnitureScript.gameObject);

		foreach (FurnitureBehaviour wall in GridCreator.Self().zoneWalls) {
			if (wall.tiles [0].AreTileSurroundingsUnlocked()) {
				wall.canBeDestroyed = true;
			}
		}

	}


	List<TileManager> GetRoomToOpen(){
		
		foreach (TileManager tile in furnitureScript.tiles [0].NeighbourTiles()) {

			if (TileIsInLockedRoom (tile.objectOnTile)) {
				return tile.room.tiles;
			}

		}

		return new List<TileManager> ();

	}


	bool TileIsInLockedRoom(FurnitureBehaviour tileFurniture){
		return tileFurniture != null && tileFurniture.tiles[0].room != null && !tileFurniture.blockRooms && !tileFurniture.canBeDestroyed ? true : false;
	}

}
