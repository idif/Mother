using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TileFeedback{SelectedTile,BuildableTile, NonBuildableTile, DestructTile}

[System.Serializable]public enum StartObject {None, ZoneWall, ZoneDoor, ForOfWar}


public class Room {

	public string name = "default room";

	public ChildMood currentMood = ChildMood.Normal;
	public int moodLevel = 0;

	public Color color;

	public List<TileManager> tiles = new List<TileManager> ();

	public Room(List<TileManager> roomTiles, bool setTileColor = false){

		tiles.AddRange(roomTiles);

		color = Random.ColorHSV ();

		foreach (TileManager tile in tiles) {
			tile.room = this;
			tile.normalColor = color;

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


}


[System.Serializable]
public class TileManager : MonoBehaviour {

    private MeshRenderer tileRenderer;

    private GameObject feedbackTextCanvas;


	[System.NonSerialized]public FurnitureBehaviour objectOnTile;
	[System.NonSerialized]public Room room = null;
    [System.NonSerialized] public IntVector2 coordinates;


	private bool canBuild = false;

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
			
        if (GameManager.selectedTile == this) {
            GameManager.selectedTile = null;
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
			} else if (canBuild && ConstructionScript.Self ().furnitureList [ConstructionScript.propID].zakariumCost <= GameManager.zakarium) {

				Construct ();

			}
		} else if (ConstructionScript.isDestructing) {

			ConstructionScript.clickedTile = this;
			ConstructionScript.CalculatePropLine ();

		} else if (room != null && (objectOnTile == null || !typeof(InteractibleFurniture).IsAssignableFrom(objectOnTile.GetType()))) {

			RoomConstructionBar.Self ().DisplayRoomPanel (this);

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
							new Room (actualRoom,true);
							new Room (oldRoom.tiles.Except(actualRoom).ToList<TileManager>(),true);
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
				
					DestroyObject();

					List<TileManager> actualRoom = new List<TileManager> ();
					GetRoomTiles (ref actualRoom);

					new Room (actualRoom,true);
					
			}

			if (room != null) {
				room.CalculateMoodLevel ();
			}

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
		canBuild = IsObjectBuildable() && (!prop.requiresShipSide || IsTileCloseToTheEdge());

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
			return GridCreator.grid [x, y].objectOnTile != null ? false : true;

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
}