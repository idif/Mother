using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TileFeedback{SelectedTile,BuildableTile, NonBuildableTile, DestructTile}
<<<<<<< HEAD

[System.Serializable]public enum StartObject {None, ZoneWall, ZoneDoor, ForOfWar}


public class Room {

	public string name = "default room";

	public ChildMood currentMood = ChildMood.Normal;
	public int moodLevel = 0;

	public Color color;

	public List<TileManager> tiles = new List<TileManager> ();

	public Room(List<TileManager> roomTiles, bool setTileColor = false, Color roomColor = Color.clear){

		tiles.AddRange(roomTiles);

		color = roomColor == Color.clear ? Random.ColorHSV () : roomColor;

		foreach (TileManager tile in tiles) {
			tile.room = this;
			tile.normalColor = color;
=======
public enum ProximityStatus{NoRoom,OneRoom,SameType,DifferentTypes}

[System.Serializable]public enum StartObject {None, ZoneWall, ZoneDoor, ForOfWar}

public class Room {

	public RoomType type;
	public List<TileManager> tiles = new List<TileManager> ();

	public Room(RoomType roomType, List<TileManager> roomTiles, bool setTileColor = false){

		type = roomType;
		tiles.AddRange(roomTiles);

		foreach (TileManager tile in tiles) {
			tile.room = this;
			tile.normalColor = ConstructionScript.roomTypes [type].tilesColor;
>>>>>>> 6918e9b0878999e1061e8a95b659822a79e570be

			if (setTileColor) {
				tile.RenderTile ();
			}
		}

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
<<<<<<< HEAD
		tile.normalColor = color;
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


=======
		tile.normalColor = ConstructionScript.roomTypes [type].tilesColor;
	}

}


public struct ProximityResult{
	
	public ProximityStatus status;
	public List <Room> closeRooms;

	public ProximityResult(ProximityStatus resultStatus, List<Room> rooms){

		status = resultStatus;
		closeRooms = rooms;

	}

>>>>>>> 6918e9b0878999e1061e8a95b659822a79e570be
}


[System.Serializable]
public class TileManager : MonoBehaviour {

    private MeshRenderer tileRenderer;

    private GameObject feedbackTextCanvas;


	[System.NonSerialized]public FurnitureBehaviour objectOnTile;
	[System.NonSerialized]public Room room = null;
    [System.NonSerialized] public IntVector2 coordinates;


	private bool canBuild = false;

<<<<<<< HEAD
=======
	private List<TileManager> potentialRoom = new List<TileManager> ();

>>>>>>> 6918e9b0878999e1061e8a95b659822a79e570be
	[HideInInspector]public List<TileManager> potentialProp = new List<TileManager>();
	[HideInInspector]public List<TileManager> tilesInBuildZone = new List<TileManager> ();

	public static Color startColor;
	[HideInInspector]public Color normalColor;

    public StartObject tileStartStatus = StartObject.None;
	

    void Start() {
		
        tileRenderer = gameObject.GetComponent<MeshRenderer>();
		normalColor = startColor;

        feedbackTextCanvas = ConstructionScript.Self().FeedbackUI;

		objectOnTile = GetComponentInChildren<FurnitureBehaviour>();

    }


    void OnMouseIsOver() {

		if (ConstructionScript.propID != -1) {
			
			SelectTile ();
			ConstructionScript.CalculatePropLine ();
			CheckBuildability ();

		} else if (ConstructionScript.isDestructing) {
			
			SelectTile();
			ConstructionScript.CalculatePropLine ();
			OnDestructionOver ();

<<<<<<< HEAD
=======
		} else if (ConstructionScript.currentlyPlacedRoom != RoomType.None) {
			OnRoomPlacementOver ();
		} else if (ConstructionScript.isDestructingRoom) {
			OnRoomDestructionOver ();
>>>>>>> 6918e9b0878999e1061e8a95b659822a79e570be
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


<<<<<<< HEAD
=======
	void OnRoomPlacementOver(){
		if((objectOnTile == null || !objectOnTile.blockRooms) && room == null){
			
			GetRoomTiles (ref potentialRoom);

			foreach (TileManager tile in potentialRoom) {
				tile.tileRenderer.material.color = ConstructionScript.roomTypes [ConstructionScript.currentlyPlacedRoom].tilesColor;
			}

		}
	}


	void OnRoomDestructionOver(){
		
		if (room != null) {
			
			foreach (TileManager tile in room.tiles) {
				tile.RenderTile (true, TileFeedback.DestructTile);

				if (tile.objectOnTile != null) {
					tile.objectOnTile.RenderWithColor (Color.red);
				}

			}

		}

	}


>>>>>>> 6918e9b0878999e1061e8a95b659822a79e570be
    void OnMouseQuit() {
			
        if (GameManager.selectedTile == this) {
            GameManager.selectedTile = null;
        }

		if (ConstructionScript.propID != -1) {

			OnConstructionQuit ();

		}else if (ConstructionScript.isDestructing) {
			
			OnDestructionQuit ();

<<<<<<< HEAD
=======
		} else if (ConstructionScript.currentlyPlacedRoom != RoomType.None) {
			
			OnRoomPlacementQuit ();

		} else if (ConstructionScript.isDestructingRoom) {
			
			OnRoomDestructionQuit ();

>>>>>>> 6918e9b0878999e1061e8a95b659822a79e570be
		}
    }


	void OnConstructionQuit(){

		foreach (TileManager tile in tilesInBuildZone) {
			tile.RenderTile (false);
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


<<<<<<< HEAD
=======
	void OnRoomPlacementQuit(){
		if (room == null) {
			foreach (TileManager tile in potentialRoom) {
				tile.tileRenderer.material.color = normalColor;
			}
		}

		potentialRoom.Clear ();

	}


	void OnRoomDestructionQuit(){
		if (room != null) {
			foreach (TileManager tile in room.tiles) {
				tile.RenderTile ();

				if (tile.objectOnTile != null) {
					tile.objectOnTile.RenderBackToNormal ();
				}

			}
		}
	}


>>>>>>> 6918e9b0878999e1061e8a95b659822a79e570be
    void OnMouseClickOn() {

		if (ConstructionScript.propID != -1) {
			if (ConstructionScript.Self ().furnitureList [ConstructionScript.propID].propPrefab.canBePlacedInLine) {
				ConstructionScript.clickedTile = this;
				ConstructionScript.CalculatePropLine ();
			} else if (canBuild && ConstructionScript.Self ().furnitureList [ConstructionScript.propID].zakariumCost <= GameManager.zakarium) {

				Construct ();

			}
		} else if (ConstructionScript.isDestructing) {

			ConstructionScript.clickedTile = this;
			ConstructionScript.CalculatePropLine ();

<<<<<<< HEAD
		} else if (room != null && (objectOnTile == null || !typeof(InteractibleFurniture).IsAssignableFrom(objectOnTile.GetType()))) {

			RoomConstructionBar.Self ().DisplayRoomPanel (this);
=======
		}else if (ConstructionScript.currentlyPlacedRoom != RoomType.None) {

			SetRoom ();

		} else if (ConstructionScript.isDestructingRoom) {

			DestructRoom ();
>>>>>>> 6918e9b0878999e1061e8a95b659822a79e570be

		}
    }


	void OnMouseClickUp(){
		
		if (ConstructionScript.propID != -1){
			
			if (ConstructionScript.Self ().furnitureList [ConstructionScript.propID].propPrefab.canBePlacedInLine) {

				foreach (TileManager tile in ConstructionScript.tilesToBeBuilt) {

					tile.Construct ();

				}

				ConstructionScript.ClearTilesToBuild ();

			} else if (canBuild && ConstructionScript.Self ().furnitureList [ConstructionScript.propID].zakariumCost <= GameManager.zakarium) {

				Construct ();
			
			}

		} else if (ConstructionScript.isDestructing) {

			foreach (TileManager tile in ConstructionScript.tilesToBeBuilt) {

				tile.Destruct ();

			}

			ConstructionScript.ClearTilesToBuild ();

		}

		ConstructionScript.clickedTile = null;

	}


    void Construct() {

		if (ConstructionScript.propID == -1) {
			return;
		}

		
        int costTemp = ConstructionScript.Self().furnitureList[ConstructionScript.propID].zakariumCost;
        GameManager.AddZakarium(-costTemp);
        BuildProp(ConstructionScript.Self().furnitureList[ConstructionScript.propID].propPrefab);

<<<<<<< HEAD
=======
        switch (ConstructionScript.propID) {
            case 3:
                ObjectivesManager.Self().MissionCompletion(3, 1);
                break;
            case 12:
                ObjectivesManager.Self().MissionCompletion(4, 1);
                break;
            case 13:
                ObjectivesManager.Self().MissionCompletion(4, 1);
                break;
            default:
                break;
        }
>>>>>>> 6918e9b0878999e1061e8a95b659822a79e570be

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
<<<<<<< HEAD
							new Room (actualRoom,true);
							new Room (oldRoom.tiles.Except(actualRoom).ToList<TileManager>(),true);
=======
							new Room (oldRoom.type,actualRoom,true);
							new Room (oldRoom.type,oldRoom.tiles.Except(actualRoom).ToList<TileManager>(),true);
>>>>>>> 6918e9b0878999e1061e8a95b659822a79e570be
                        }
                    }

				}
			}
		}

        CallUIFeedback(true, costTemp);
        
        
        
        if (ConstructionScript.Self().furnitureList[ConstructionScript.propID].zakariumCost > GameManager.zakarium) {
            ConstructionScript.propID = -1;
        }


    }


    public void Destruct() {


		if (objectOnTile != null && objectOnTile.canBeDestroyed)
        {

			if(!objectOnTile.blockRooms){
				DestroyObject();
			}else{
				
<<<<<<< HEAD
					DestroyObject();

					List<TileManager> actualRoom = new List<TileManager> ();
					GetRoomTiles (ref actualRoom);

					new Room (actualRoom,true);
					
			}

			if (room != null) {
				room.CalculateMoodLevel ();
			}

=======
				ProximityResult objectProximity = ObjectProximityStatus();
				if (objectProximity.status != ProximityStatus.DifferentTypes) {
					
					DestroyObject();

					if (objectProximity.status == ProximityStatus.SameType || objectProximity.status == ProximityStatus.OneRoom) {

						List<TileManager> actualRoom = new List<TileManager> ();
						objectProximity.closeRooms [0].tiles [0].GetRoomTiles (ref actualRoom);

						new Room (objectProximity.closeRooms [0].type, actualRoom,true);

					}

				}
			}
>>>>>>> 6918e9b0878999e1061e8a95b659822a79e570be
        }
    }

	void DestroyObject(){
		
		int gainTemp = ConstructionScript.Self().furnitureList[objectOnTile.propID].zakariumCost / 2;
		GameManager.AddZakarium(gainTemp);
		CallUIFeedback(false, gainTemp);

		GameObject objectToDestroy = objectOnTile.gameObject;

		foreach (TileManager tile in objectOnTile.tiles)
		{
			tile.objectOnTile = null;
			tile.RenderTile(false);
		}

		objectToDestroy.SendMessage("OnDestruction", SendMessageOptions.DontRequireReceiver);
		Destroy(objectToDestroy);

	}


<<<<<<< HEAD
=======
	void SetRoom(){
		if(IsRoomBuildable(potentialRoom) && (objectOnTile == null || objectOnTile.canBeDestroyed)){
			new Room (ConstructionScript.currentlyPlacedRoom, potentialRoom);
		}
	}


>>>>>>> 6918e9b0878999e1061e8a95b659822a79e570be
	void DestructRoom(){
		if (room != null) {
			Room roomTemp = room;
			foreach (TileManager tile in room.tiles) {
				if (tile.objectOnTile != null) {
					tile.Destruct();
				}
				tile.room = null;
				tile.normalColor = startColor;
				tile.RenderTile ();
			}
			roomTemp.tiles.Clear ();
		}
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

		FurnitureBehaviour prop = ConstructionScript.Self ().furnitureList [ConstructionScript.propID].propPrefab;
<<<<<<< HEAD
		canBuild = IsObjectBuildable() && (!prop.requiresShipSide || IsTileCloseToTheEdge());
=======
		canBuild = IsObjectBuildable() && (prop.blockRooms || (room != null && ConstructionScript.roomTypes[room.type].allowedPropsId.Contains(prop.propID))) && (!prop.requiresShipSide || IsTileCloseToTheEdge());
>>>>>>> 6918e9b0878999e1061e8a95b659822a79e570be

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


    public void BuildProp(FurnitureBehaviour prop)
    {

        objectOnTile = GameObject.Instantiate(prop, transform.position, ConstructionScript.propRotation);
        objectOnTile.transform.parent = transform;
        objectOnTile.propID = ConstructionScript.propID;
        canBuild = false;

    }


    void SelectTile() {
            GameManager.selectedTile = this;
    }


	public void RenderTile(bool specialFeedback = false, TileFeedback feedback = TileFeedback.SelectedTile){
<<<<<<< HEAD
		
=======
>>>>>>> 6918e9b0878999e1061e8a95b659822a79e570be
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

<<<<<<< HEAD
=======
	public ProximityResult ObjectProximityStatus()//wether there's zero room, one room, 2 rooms of the same type or 2 rooms of different types around this object
    {

        if(objectOnTile == null)
        {
			return TileProximityStatus();
        }

		List<Room> closeRooms = new List<Room> ();
		List<RoomType> closeRoomsTypes = new List<RoomType> ();

        foreach(TileManager tile in objectOnTile.tiles)
        {
			foreach (Room closeRoom in tile.TileProximityStatus().closeRooms) {
				if(!closeRooms.Contains(closeRoom)){
					closeRooms.Add (closeRoom);
					if(!closeRoomsTypes.Contains(closeRoom.type)){
						closeRoomsTypes.Add (closeRoom.type);
					}
				}
			}
        }

		switch (closeRooms.Count) {
		case 0:
			return new ProximityResult (ProximityStatus.NoRoom, closeRooms);

		case 1:
			return new ProximityResult (ProximityStatus.OneRoom, closeRooms);

		default:
			if (closeRoomsTypes.Count > 1) {
				return new ProximityResult (ProximityStatus.DifferentTypes, closeRooms);
			} else {
				return new ProximityResult (ProximityStatus.SameType, closeRooms);
			}
		}

    }

	public ProximityResult TileProximityStatus()
    {

		List<Room> closeRooms = new List<Room> ();
		List<RoomType> closeRoomsTypes = new List<RoomType> ();

        if(coordinates.x < GridCreator.grid.GetLength(0) - 1)
        {
			GridCreator.grid [coordinates.x + 1, coordinates.y].PerformRoomCheck (ref closeRooms, ref closeRoomsTypes);
        }

		if (coordinates.x > 0)
        {
			GridCreator.grid [coordinates.x - 1, coordinates.y].PerformRoomCheck (ref closeRooms, ref closeRoomsTypes);
        }

		if (coordinates.y < GridCreator.grid.GetLength(1) - 1)
        {
			GridCreator.grid [coordinates.x, coordinates.y + 1].PerformRoomCheck (ref closeRooms, ref closeRoomsTypes);
        }

		if (coordinates.y > 0)
        {
			GridCreator.grid [coordinates.x, coordinates.y - 1].PerformRoomCheck (ref closeRooms, ref closeRoomsTypes);
        }

		switch(closeRooms.Count){

			case 0:
				return new ProximityResult(ProximityStatus.NoRoom,closeRooms);

			case 1:
				return new ProximityResult(ProximityStatus.OneRoom,closeRooms);

		default:
			
			if (closeRoomsTypes.Count > 1) {
				return new ProximityResult (ProximityStatus.DifferentTypes, closeRooms);
			} else {
				return new ProximityResult (ProximityStatus.SameType, closeRooms);
			}

				}
    }

	void PerformRoomCheck(ref List<Room> closeRooms, ref List<RoomType> closeRoomsTypes){
		
		if(room == null || closeRooms.Contains(room)){
			return;
		}

		closeRooms.Add (room);

		if(!closeRoomsTypes.Contains(room.type)){
			closeRoomsTypes.Add (room.type);
		}
						
	}

>>>>>>> 6918e9b0878999e1061e8a95b659822a79e570be

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
<<<<<<< HEAD
				return IsZoneBuildable(coordinates.x, coordinates.x - ConstructionScript.Self().furnitureList[ConstructionScript.propID].propPrefab.size.x, -1, coordinates.y, coordinates.y - ConstructionScript.Self().furnitureList[ConstructionScript.propID].propPrefab.size.y, -1);

            case Direction.Left:
				return IsZoneBuildable(coordinates.x, coordinates.x - ConstructionScript.Self().furnitureList[ConstructionScript.propID].propPrefab.size.y, -1, coordinates.y, coordinates.y + ConstructionScript.Self().furnitureList[ConstructionScript.propID].propPrefab.size.x, 1);
=======
			return IsZoneBuildable(coordinates.x, coordinates.x - ConstructionScript.Self().furnitureList[ConstructionScript.propID].propPrefab.size.x, -1, coordinates.y, coordinates.y - ConstructionScript.Self().furnitureList[ConstructionScript.propID].propPrefab.size.y, -1);

            case Direction.Left:
			return IsZoneBuildable(coordinates.x, coordinates.x - ConstructionScript.Self().furnitureList[ConstructionScript.propID].propPrefab.size.y, -1, coordinates.y, coordinates.y + ConstructionScript.Self().furnitureList[ConstructionScript.propID].propPrefab.size.x, 1);
>>>>>>> 6918e9b0878999e1061e8a95b659822a79e570be

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
			return GridCreator.grid [x, y].objectOnTile != null ? false : true;

		}

		return false;
	}


<<<<<<< HEAD
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
=======
    public void GetRoomTiles(ref List<TileManager> roomTiles)
    {

        if((objectOnTile  == null || !objectOnTile.blockRooms) && !roomTiles.Contains(this))
        {
            roomTiles.Add(this);

            if (coordinates.x > 0)
            {
                GridCreator.grid[coordinates.x - 1, coordinates.y].GetRoomTiles(ref roomTiles);
            }

            if (coordinates.y > 0)
            {
                GridCreator.grid[coordinates.x, coordinates.y - 1].GetRoomTiles(ref roomTiles);
            }

            if (coordinates.x < GridCreator.grid.GetLength(0) - 1)
            {
                GridCreator.grid[coordinates.x +1, coordinates.y].GetRoomTiles(ref roomTiles);
            }

            if (coordinates.y < GridCreator.grid.GetLength(1) - 1)
            {
                GridCreator.grid[coordinates.x, coordinates.y +1].GetRoomTiles(ref roomTiles);
            }

        }

    }
>>>>>>> 6918e9b0878999e1061e8a95b659822a79e570be


	bool IsRoomBuildable(List<TileManager> roomTiles)//check if some of tiles in the same enclosed area are already assigned to a room
	{
		foreach (TileManager tile in roomTiles) {
			if (tile.room != null) {
				return false;
			}
		}
		return true;
	}

   public bool IsTileCloseToTheEdge()
    {

        return ((coordinates.x == 0 && ConstructionScript.propDirection == Direction.Forward)
            || (coordinates.y == 0 && ConstructionScript.propDirection == Direction.Left)
            || (coordinates.x == GridCreator.grid.GetLength(0) - 1 && ConstructionScript.propDirection == Direction.Backward)
            || (coordinates.y == GridCreator.grid.GetLength(1) - 1 && ConstructionScript.propDirection == Direction.Right))
            ? true : false;
     
    }

<<<<<<< HEAD
	public void OnMouseRightClickOn()
	{
		if (CharacterBehaviour.selectedCharacter != null) 
		{
			if (CharacterBehaviour.selectedCharacter.isBot) 
			{
				CharacterBehaviour.selectedCharacter.StopAllCoroutines();
				CharacterBehaviour.selectedCharacter.coroutineLaunched = false;

				CharacterBehaviour.selectedCharacter.isHome = false;
				CharacterBehaviour.selectedCharacter.target = this.transform;

				if (objectOnTile != null && objectOnTile.snap) 
				{
					CharacterBehaviour.selectedCharacter.isWorking = true;
				}
			}

			CharacterBehaviour.selectedCharacter.agent.destination = this.transform.position;
		}
	}
=======

>>>>>>> 6918e9b0878999e1061e8a95b659822a79e570be
}