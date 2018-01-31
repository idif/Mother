using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TileFeedback{SelectedTile,BuildableTile, NonBuildableTile, DestructTile}

[System.Serializable]public enum StartObject {None, ZoneWall, ZoneDoor, ForOfWar}


public class Room {

	public string name;

	public ChildMood currentMood = ChildMood.Normal;
	public int moodLevel = 0;

	public int roomDeco = 0;
	Color color;

	public List<TileManager> tiles = new List<TileManager> ();

	public Room(List<TileManager> roomTiles, bool setTileColor = false, int roomAmbiance = 0, string roomName = "New Room"){

		tiles.AddRange(roomTiles);

		roomDeco = roomAmbiance;
		color = ConstructionScript.Self ().groundColors [roomDeco];
		name = roomName;

		foreach (TileManager tile in tiles) {
			tile.room = this;
			tile.normalColor = color;

			if (setTileColor) {
				tile.RenderTile ();
			}
		}

		CalculateMoodLevel ();

	}

	public void AddTile(TileManager tile){

		if (tile.room != null) {

			if (tile.room == this) {
				return;
			}

			tile.room.tiles.Remove (tile);
		}

		tiles.Add (tile);
		tile.room = this;
		tile.normalColor = color;
	}


	public void UpdateDeco(int newId){

		roomDeco = newId;
		color = ConstructionScript.Self ().groundColors [roomDeco];

		foreach (TileManager tile in tiles) {

			if (tile.normalColor == tile.tileRenderer.material.color) {
				tile.tileRenderer.material.color = color;
			}

			tile.normalColor = color;

		}


	}


	public void CalculateMoodLevel(){

		List <ChildMood> moodInfluences = new List<ChildMood> ();
		List <int> influencesLevels = new List<int> ();

		foreach (TileManager tile in tiles) {
			
			if (tile.objectOnTile == null) {
				continue;
			}



			if(tile.objectOnTile is CosmeticFurniture) {

				CosmeticFurniture decoFurniture = (CosmeticFurniture)tile.objectOnTile;

				if (moodInfluences.Contains (decoFurniture.givenMood)) {

					influencesLevels [moodInfluences.IndexOf (decoFurniture.givenMood)]++;

				} else {

					moodInfluences.Add (decoFurniture.givenMood);
					influencesLevels.Add (1);

				}

			}

		}

		if (moodInfluences.Count > 0) {

			int chosenMood = MiscUtilities.IndexOfBiggest (influencesLevels);

			currentMood = moodInfluences [chosenMood];
			moodLevel = influencesLevels [chosenMood];


		} else {

			currentMood = ChildMood.Normal;
			moodLevel = 0;
		}

	}


}


[System.Serializable]
public class TileManager : MonoBehaviour {

	[HideInInspector]public MeshRenderer tileRenderer;

    private GameObject feedbackTextCanvas;


	[System.NonSerialized]public FurnitureBehaviour objectOnTile;
	[System.NonSerialized]public Room room = null;
    [System.NonSerialized] public IntVector2 coordinates;


	[HideInInspector]public bool canBuild = false;

	[HideInInspector]public List<TileManager> potentialProp = new List<TileManager>();
	[HideInInspector]public List<TileManager> tilesInBuildZone = new List<TileManager> ();

	public static Color startColor;
	[HideInInspector]public Color normalColor;

    public StartObject tileStartStatus = StartObject.None;
	

    void Awake() {
		
        tileRenderer = gameObject.GetComponent<MeshRenderer>();
		objectOnTile = GetComponentInChildren<FurnitureBehaviour>();
		feedbackTextCanvas = ConstructionScript.Self().FeedbackUI;

    }


    void OnMouseIsOver() {

		if (ConstructionScript.propID != -1) {
			
			OnConstructionOver ();

		} else if (ConstructionScript.isDestructing) {
			
			SelectTile();
			ConstructionScript.CalculatePropLine ();
			OnDestructionOver ();

		}
    }


	void OnConstructionOver(){
		SelectTile ();
		ConstructionScript.CalculatePropLine ();
		CheckBuildability ();

		if (canBuild){
			if (objectOnTile != null) {
				objectOnTile.EnableRenderers (false);
			} else if(ConstructionScript.clickedTile != null){
				Instantiate (ConstructionScript.Self ().inConstructionFx, transform.position, Quaternion.identity, FxTimer.Self ().transform);
			}
		}
	}


    void OnDestructionOver() {
        

        if (objectOnTile != null)
        {
            if (objectOnTile.canBeDestroyed)
            {

				objectOnTile.RenderWithColor (Color.red);

                foreach (TileManager tile in objectOnTile.tiles)
                {
                    tile.RenderTile(true, TileFeedback.DestructTile);
                }
            }

		} else {

			RenderTile(true, TileFeedback.DestructTile);

		}
	}


    void OnMouseQuit() {
			
		if (GameManager.Self().selectedTile == this) {
			GameManager.Self().selectedTile = null;
        }

		if (ConstructionScript.propID != -1) {

			OnConstructionQuit ();

		}else if (ConstructionScript.isDestructing) {
			
			OnDestructionQuit ();

		}
    }


	void OnConstructionQuit(){

		foreach (TileManager tile in tilesInBuildZone) {
			tile.RenderTile (false);
		}

		if (ConstructionScript.propID != -1 && ConstructionScript.Self().furnitureList[ConstructionScript.propID].propPrefab.canBePlacedInWalls && objectOnTile != null) {
			objectOnTile.EnableRenderers (true);
		}

	}


	void OnDestructionQuit(){

		if (ConstructionScript.tilesToBeBuilt.Contains(this)) {
			return;
		}

		if (objectOnTile != null) {
			
			objectOnTile.RenderBackToNormal ();

			foreach (TileManager tile in objectOnTile.tiles) {
				tile.RenderTile (false);
			}

		} else {
			RenderTile (false);
		}
	}


    void OnMouseClickOn() {

		if (ConstructionScript.propID != -1) {
			if (ConstructionScript.Self ().furnitureList [ConstructionScript.propID].propPrefab.canBePlacedInLine) {
				
				ConstructionScript.clickedTile = this;
				ConstructionScript.CalculatePropLine ();
				Instantiate (ConstructionScript.Self ().inConstructionFx, transform.position, Quaternion.identity, FxTimer.Self ().transform);

			} else if (canBuild && ConstructionScript.Self ().furnitureList [ConstructionScript.propID].zakariumCost <= GameManager.Self().zakarium) {

				Construct ();

			}
		} else if (ConstructionScript.isDestructing) {

			ConstructionScript.clickedTile = this;
			ConstructionScript.CalculatePropLine ();

		} else if (room != null &&
		           (objectOnTile == null ||
		           (!typeof(InteractibleFurniture).IsAssignableFrom (objectOnTile.GetType ())) && objectOnTile.canBeDestroyed)) {

			StartCoroutine (WaitToOpenRoom());

		} else {
			RoomConstructionBar.Self ().panelPivot.SetActive (false);
		}
    }


	IEnumerator WaitToOpenRoom(){

		float progress = 0;
		float openingTime = RoomConstructionBar.Self ().timeToOpenRoomPanel;
		RoomConstructionBar.Self ().panelOpeningFeedback.gameObject.SetActive (true);

		while (progress < 1) {

			if (!Input.GetMouseButton (0)) {

				RoomConstructionBar.Self ().panelOpeningFeedback.gameObject.SetActive (false);
				yield break;

			}

			progress += Time.deltaTime / openingTime;
			RoomConstructionBar.Self ().panelOpeningFeedback.fillAmount = progress;
			RoomConstructionBar.Self ().panelOpeningFeedback.transform.position = Input.mousePosition;

			yield return new WaitForSeconds (0);

		}

		RoomConstructionBar.Self ().panelOpeningFeedback.gameObject.SetActive (false);
		RoomConstructionBar.Self ().DisplayRoomPanel (this);

	}


	void OnMouseClickUp(){
		
		if (ConstructionScript.propID != -1) {
			
			if (ConstructionScript.Self ().furnitureList [ConstructionScript.propID].propPrefab.canBePlacedInLine) {

				int totalCost = ConstructionScript.Self ().furnitureList [ConstructionScript.propID].zakariumCost
				                * ConstructionScript.tilesToBeBuilt.Count;

				if (totalCost > GameManager.Self ().zakarium) {
					return;
				}

				foreach (TileManager tile in ConstructionScript.tilesToBeBuilt) {

					tile.Construct (false);

				}

				if (totalCost > 0) {
					CallUIFeedback (true, totalCost);
				}

				ConstructionScript.ClearTilesToBuild ();

			} else if (canBuild && ConstructionScript.Self ().furnitureList [ConstructionScript.propID].zakariumCost <= GameManager.Self ().zakarium) {

				Construct ();
			
			}

		} else if (ConstructionScript.isDestructing) {

			int gain = 0;

			foreach (TileManager tile in ConstructionScript.tilesToBeBuilt) {

				gain += tile.Destruct (false);

			}

			if (gain > 0) {
				CallUIFeedback (false, gain);
			}

			ConstructionScript.ClearTilesToBuild ();

		}

		ConstructionScript.clickedTile = null;

	}


	void Construct(bool handleFeedback = true) {

		if (objectOnTile != null) {
			GameManager.AddZakarium (ConstructionScript.Self ().furnitureList [objectOnTile.propID].zakariumCost);
			Destroy (objectOnTile.gameObject);
		}


		int costTemp = ConstructionScript.Self ().furnitureList [ConstructionScript.propID].zakariumCost;
		GameManager.AddZakarium (-costTemp);
		BuildProp (ConstructionScript.Self ().furnitureList [ConstructionScript.propID].propPrefab);
		

		foreach (TileManager tile in tilesInBuildZone) {
			tile.objectOnTile = objectOnTile;
		}
			
		objectOnTile.tiles.AddRange(tilesInBuildZone);

		if (objectOnTile.blockRooms) {
			foreach (TileManager tile in objectOnTile.tiles) {
				if (tile.room != null) {
					tile.tileRenderer.material.color = tile.normalColor = TileManager.startColor;
                    tile.room.tiles.Remove(tile);
                    Room oldRoom = tile.room;
					tile.room = null;

                    if(oldRoom.tiles.Count > 0)
                    {
                        List<TileManager> actualRoom = new List<TileManager>();
                        oldRoom.tiles[0].GetRoomTiles(ref actualRoom);

                        if (oldRoom.tiles.Count != actualRoom.Count)
                        {
							new Room (actualRoom,true,oldRoom.roomDeco);
							new Room (oldRoom.tiles.Except(actualRoom).ToList<TileManager>(),true, oldRoom.roomDeco);
                        }
                    }

				}
			}

			UpdateNeighboursWalls ();

		}

		if (handleFeedback) {
			CallUIFeedback (true, costTemp);
		}
        
		Instantiate (ConstructionScript.Self ().constructionDoneFx, transform.position, Quaternion.identity, FxTimer.Self().transform);

        
		if (ConstructionScript.Self().furnitureList[ConstructionScript.propID].zakariumCost > GameManager.Self().zakarium) {
            ConstructionScript.propID = -1;
			OnConstructionQuit ();

        }


    }


	public void BuildProp(FurnitureBehaviour prop)
	{

		objectOnTile = GameObject.Instantiate(prop, transform.position, ConstructionScript.propRotation);
		objectOnTile.transform.parent = transform;
		objectOnTile.propID = ConstructionScript.propID;
		canBuild = false;

	}


	void UpdateNeighboursWalls(){

		foreach (TileManager tile in NeighbourTiles()) {
			if (tile.objectOnTile != null) {
				tile.objectOnTile.SendMessage ("UpdateWallMesh", SendMessageOptions.DontRequireReceiver);
			}
		}

	}



	public int Destruct(bool handleFeedback = true) {

		int result = 0;

		if (objectOnTile != null && objectOnTile.canBeDestroyed)
        {

			if(!objectOnTile.blockRooms){
				result = DestroyObject(handleFeedback);
			}else{
				
				result = DestroyObject(handleFeedback);

					List<TileManager> actualRoom = new List<TileManager> ();
					GetRoomTiles (ref actualRoom);

					new Room (actualRoom,true);
					
				UpdateNeighboursWalls ();

			}

			if (room != null) {
				room.CalculateMoodLevel ();
			}

        }

		return result;

    }

	int DestroyObject(bool handleFeedback){
		
		int gainTemp = ConstructionScript.Self().furnitureList[objectOnTile.propID].zakariumCost / 2;
		GameManager.AddZakarium(gainTemp);

		if (handleFeedback) {
			CallUIFeedback (false, gainTemp);
		}

		GameObject objectToDestroy = objectOnTile.gameObject;

		foreach (TileManager tile in objectOnTile.tiles)
		{
			tile.objectOnTile = null;
			tile.RenderTile(false);
		}

		objectToDestroy.SendMessage("OnDestruction", SendMessageOptions.DontRequireReceiver);
		Destroy(objectToDestroy);

		return gainTemp;

	}


    void CallUIFeedback(bool constructing, int cost) {
        Transform feedbackUI = Instantiate(feedbackTextCanvas.transform, transform.position, Quaternion.identity) as Transform;
        BuildCostText feedbackText = feedbackUI.gameObject.GetComponent<BuildCostText>();
        if (feedbackText != null) {
            if (constructing) {
                feedbackText.OnConstruction(cost);
            } else {
                feedbackText.OnDestruction(cost);
            }
        }
    }


	public bool CheckBuildability()
    {

		ClearBuildZoneList ();

		canBuild = IsObjectBuildable();

        if (canBuild)
        {
            foreach (TileManager tile in tilesInBuildZone)
            {
                tile.RenderTile(true, TileFeedback.BuildableTile);
            }

            CheckDemoPropColorChange();

        }
        else
        {
            foreach (TileManager tile in tilesInBuildZone)
            {
                tile.RenderTile(true, TileFeedback.NonBuildableTile);
            }

            CheckDemoPropColorChange(Color.red);

        }
        
        ConstructionScript.lastBuildPositionWasValid = canBuild;
		return canBuild;

    }


    void CheckDemoPropColorChange(Color? colorToAssign = null)
    {
        if (ConstructionScript.lastBuildPositionWasValid != canBuild)
        {
           ConstructionScript.Self().SetDemoPropColor(colorToAssign);
        }
    }


    void SelectTile() {
		GameManager.Self().selectedTile = this;
    }


	public void RenderTile(bool specialFeedback = false, TileFeedback feedback = TileFeedback.SelectedTile){
		
		if (specialFeedback) {
			switch (feedback) {
			case TileFeedback.SelectedTile:
				tileRenderer.material.color = ConstructionScript.Self ().selectedColor;
				break;

			case TileFeedback.BuildableTile:
				tileRenderer.material.color = ConstructionScript.Self ().buildableColor;
				break;

			case TileFeedback.NonBuildableTile:
				tileRenderer.material.color = ConstructionScript.Self ().nonBuildableColor;
				break;

			case TileFeedback.DestructTile:
				tileRenderer.material.color = ConstructionScript.Self ().destructColor;
				break;

			default:
				break;

			}
        }
        else
        {
            tileRenderer.material.color = normalColor;
        }

    }


	public void ClearBuildZoneList(){

		foreach (TileManager tile in tilesInBuildZone) {
			tile.RenderTile (false);
		}

		tilesInBuildZone.Clear ();

	}


    public bool IsObjectBuildable(){

        switch (ConstructionScript.propDirection)
        {

            case Direction.Right:
                return IsZoneBuildable(coordinates.x, coordinates.x + ConstructionScript.Self().furnitureList[ConstructionScript.propID].propPrefab.size.y, 1, coordinates.y, coordinates.y - ConstructionScript.Self().furnitureList[ConstructionScript.propID].propPrefab.size.x, -1);
                
            case Direction.Backward:
				return IsZoneBuildable(coordinates.x, coordinates.x - ConstructionScript.Self().furnitureList[ConstructionScript.propID].propPrefab.size.x, -1, coordinates.y, coordinates.y - ConstructionScript.Self().furnitureList[ConstructionScript.propID].propPrefab.size.y, -1);

            case Direction.Left:
				return IsZoneBuildable(coordinates.x, coordinates.x - ConstructionScript.Self().furnitureList[ConstructionScript.propID].propPrefab.size.y, -1, coordinates.y, coordinates.y + ConstructionScript.Self().furnitureList[ConstructionScript.propID].propPrefab.size.x, 1);

            case Direction.Forward:
                return IsZoneBuildable(coordinates.x, coordinates.x + ConstructionScript.Self().furnitureList[ConstructionScript.propID].propPrefab.size.x, 1, coordinates.y, coordinates.y + ConstructionScript.Self().furnitureList[ConstructionScript.propID].propPrefab.size.y, 1);

            default:
                return objectOnTile == null ? true : false;

        }
        
    }


	bool IsZoneBuildable(int startX, int endX, int stepX, int startY, int endY, int stepY)
    {

		bool result = true;

		if (stepX > 0) {
			
			for (int x = startX; x < endX; x += stepX) {
					
				if (!IsLineBuildable (x, startY, endY, stepY)) {
					result = false;
				}
					
			}

		} else {
			
			for (int x = startX; x > endX; x += stepX) {

				if (!IsLineBuildable (x, startY, endY, stepY)) {
					result = false;
				}

			}

		}

		return result;
    }


	bool IsLineBuildable(int x, int startY, int endY, int stepY){

		bool result = true;

		if (stepY > 0) {

			for (int y = startY; y < endY; y += stepY) {
				if(!IsTileBuildable (x, y)){
					result = false;
				}
			}

		} else {

			for (int y = startY; y > endY; y += stepY) {
				if(!IsTileBuildable (x, y)){
					result = false;
				}
			}

		}

		return result;

	}


	bool IsTileBuildable(int x, int y){
		
		if(x < GridCreator.grid.GetLength (0) && y < GridCreator.grid.GetLength (1) && x >= 0 && y >= 0){
            
			tilesInBuildZone.Add (GridCreator.grid [x, y]);
			return GridCreator.grid [x, y].objectOnTile == null || 
				(ConstructionScript.Self().furnitureList[ConstructionScript.propID].propPrefab.canBePlacedInWalls && GridCreator.grid [x, y].objectOnTile.propID == 0 && GridCreator.grid [x, y].objectOnTile.canBeDestroyed ) ?
				true : false;

		}

		return false;
	}


	public void GetRoomTiles(ref List<TileManager> roomTiles)
	{

		if((objectOnTile  == null || !objectOnTile.blockRooms) && !roomTiles.Contains(this))
		{
			roomTiles.Add(this);

			foreach (TileManager tile in NeighbourTiles()) {

				tile.GetRoomTiles(ref roomTiles);

			}

		}

	}


	public List<TileManager> NeighbourTiles(){

		List<TileManager> result = new List<TileManager> ();

		if (coordinates.x > 0) {
			result.Add (GridCreator.grid [coordinates.x - 1, coordinates.y]);
		}

		if (coordinates.y > 0) {
			result.Add (GridCreator.grid [coordinates.x, coordinates.y - 1]);
		}

		if (coordinates.x < GridCreator.grid.GetLength(0) - 1) {
			result.Add (GridCreator.grid [coordinates.x + 1, coordinates.y]);
		}

		if (coordinates.y < GridCreator.grid.GetLength(1) - 1) {
			result.Add (GridCreator.grid [coordinates.x, coordinates.y + 1]);
		}

		return result;

	}

	public bool AreTileSurroundingsUnlocked(){

		foreach (TileManager tile in NeighbourTiles()) {

			if (tile.objectOnTile != null && !tile.objectOnTile.canBeDestroyed && !tile.objectOnTile.blockRooms) {
				return false;
			}

		}

		return true;

	}


	bool IsRoomBuildable(List<TileManager> roomTiles)//check if some of tiles in the same enclosed area are already assigned to a room
	{
		foreach (TileManager tile in roomTiles) {
			if (tile.room != null) {
				return false;
			}
		}
		return true;
	}


	public void OnMouseRightClickOn()
	{
		if (CharacterBehaviour.selectedCharacter != null) 
		{
			if (CharacterBehaviour.selectedCharacter.isBot) 
			{
				CharacterBehaviour.selectedCharacter.StopAllCoroutines();
				UITaskBar.Self().workHandyPanel.SetActive (false);
				CharacterBehaviour.selectedCharacter.isHome = false;
				CharacterBehaviour.selectedCharacter.target = this.transform;

				if (objectOnTile != null && objectOnTile.snap) 
				{
					CharacterBehaviour.selectedCharacter.isWorking = true;
				}

				if (objectOnTile != null && !objectOnTile.snap)
				{
					return;
				}
			}

			CharacterBehaviour.selectedCharacter.agent.destination = this.transform.position;
		}
	}
}