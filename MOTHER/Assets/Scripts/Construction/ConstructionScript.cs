using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]public enum Direction {Right, Backward, Left, Forward}
[System.Serializable]public enum PropCategories {None, Structure, Needs, Education, Production, Cosmetic}

[System.Serializable]
public class Prop{
	public FurnitureBehaviour propPrefab;
	public string name = "Default Prop";
	public int zakariumCost= 0;
}


[System.Serializable]
public class PropCategory{

	public PropCategories category;
	public int [] linkedProps;

}

[System.Serializable]
public struct ModelData{//used to represent 3D models

	public Mesh mesh;
	public Material[] materials;

}


public class ConstructionScript : SingletonBehaviour<ConstructionScript> {


	public static int propID = -1;//the ID of the prop according to the furnitureList. "-1" means no prop (not building).
	public static bool isDestructing = false;

	public static PropCategories selectedCategory = PropCategories.None;

	public static Quaternion propRotation;
    public static Direction propDirection = Direction.Right;

	List<GameObject> demoProps = new List<GameObject> ();
    List<RendererData> demoPropRenderData = new List<RendererData>();

    [Header("Main Lists")]

	public Prop[] furnitureList;
	public PropCategory[] categories;

    [Header("Pivots & References")]

    public Transform PropButtonsPivot;
    public GameObject FeedbackUI;
	public GameObject inConstructionFx;
	public GameObject constructionDoneFx;

	[Header("Aesthetics")]

	public float demoPropAlpha = 0.5f;

	public Color selectedColor;
	public Color buildableColor;
	public Color nonBuildableColor;
	public Color destructColor;

	[Header("Wall Meshes")]

	public ModelData column;
	public ModelData end;
	public ModelData normalWall;
	public ModelData corner;
	public ModelData tCorner;
	public ModelData allCorner;

	public Color[] groundColors;

	public static List<TileManager> tilesToBeBuilt = new List<TileManager> ();

    public static bool lastBuildPositionWasValid = true;

	public static TileManager clickedTile;

	public static Dictionary < PropCategories, PropCategory> buildCategories = new Dictionary<PropCategories, PropCategory>();

    void Awake()
    {
		if (buildCategories.Count == 0) {
			foreach (PropCategory category in categories) {
				buildCategories.Add (category.category, category);
			}
		}

		for (int i = 0; i < furnitureList.Length; i++) {
			furnitureList [i].propPrefab.propID = i;
		}

    }


    void Update () {
		if (propID != -1) {

			if (furnitureList [propID].propPrefab.canBePlacedInLine) {

				if (demoProps.Count > tilesToBeBuilt.Count) {

					for (int i = tilesToBeBuilt.Count; i < demoProps.Count; i++) {
						Destroy (demoProps [i]);
					}

					demoProps.RemoveRange (tilesToBeBuilt.Count, demoProps.Count - tilesToBeBuilt.Count);

				}

				for (int i = 0; i < tilesToBeBuilt.Count; i++) {

					if (demoProps.Count <= i) {
						AddDemoProp (false);
						demoProps [i].transform.rotation = propRotation;
					}

					demoProps [i].transform.position = tilesToBeBuilt [i].transform.position;

				}
					

			} else {

				if (GameManager.Self().selectedTile != null && ( GameManager.Self().selectedTile.canBuild/*objectOnTile == null || (furnitureList[propID].propPrefab.canBePlacedInWalls && GameManager.Self().selectedTile.objectOnTile.propID == 0)*/)) {
					if (demoProps.Count == 0) {
					
						GameObject newDemoProp = AddDemoProp ();

						propRotation = newDemoProp.transform.rotation;
						propDirection = Direction.Forward;

					} else {
						if (demoProps [0].transform.position != GameManager.Self().selectedTile.transform.position) {
							demoProps [0].transform.position = GameManager.Self().selectedTile.transform.position;
						}
						if (!demoProps [0].activeSelf) {
							demoProps [0].SetActive (true);
						}

					}

				} else if (demoProps.Count > 0 && demoProps [0].activeSelf) {
					demoProps [0].SetActive (false);
				}

				RotationCheck ();

			}


		} else if (demoProps.Count > 0) {

			foreach (GameObject demoProp in demoProps) {
				Destroy (demoProp);
			}

			demoProps.Clear ();
		}



	}


	GameObject AddDemoProp(bool onlyProp = true){

		GameObject newProp = GameObject.Instantiate (furnitureList [propID].propPrefab, GameManager.Self().selectedTile.transform.position, Quaternion.identity).gameObject;

		demoProps.Add (newProp);


		newProp.layer = 2;

		for (int i = 0; i < newProp.transform.childCount; i++) {
			newProp.transform.GetChild (i).gameObject.layer = 2;
		}

		FurnitureBehaviour demoScript = newProp.GetComponent<FurnitureBehaviour> ();


		if (demoScript != null) {

			if (onlyProp) {
				demoPropRenderData = demoScript.propRenderers;
			}

			SetDemoPropAlpha ();
			foreach (Behaviour comp in demoScript.toDisableOnDemo) {
				comp.enabled = false;
			}

			SmartObject smartScript = demoScript.GetComponent<SmartObject> ();

			if (smartScript != null) {
				Destroy (smartScript);
			}

			Destroy (demoScript);
		}

		return newProp;

	}


	void RotationCheck(){

		if ((Input.GetMouseButtonDown (1) || Input.GetKeyDown (KeyCode.LeftShift)) && GameManager.Self().selectedTile != null) {

			propRotation.eulerAngles = new Vector3 (propRotation.eulerAngles.x, propRotation.eulerAngles.y + 90, propRotation.eulerAngles.z);
			propDirection = GetNextDirectionToTheRight (propDirection);

			foreach (GameObject demoProp in demoProps) {
				demoProp.transform.rotation = propRotation;
			}

			GameManager.Self().selectedTile.CheckBuildability ();

		}

	}


	public static void UnSelectTile(){
		
		if (GameManager.Self ().selectedTile != null) {
			foreach (TileManager tile in GameManager.Self().selectedTile.tilesInBuildZone) {
				tile.RenderTile ();
			}

			if (ConstructionScript.propID != -1 && ConstructionScript.Self().furnitureList[ConstructionScript.propID].propPrefab.canBePlacedInWalls && GameManager.Self().selectedTile.objectOnTile != null) {
				GameManager.Self().selectedTile.objectOnTile.EnableRenderers (true);
			}

			GameManager.Self ().selectedTile = null;
		}

		ClearTilesToBuild ();

	}


	public static void ClearTilesToBuild(){

		foreach (TileManager tile in tilesToBeBuilt) {

			tile.RenderTile (false);

			if (isDestructing && tile.objectOnTile != null) {
				tile.objectOnTile.RenderBackToNormal ();
			}

		}

		tilesToBeBuilt.Clear ();

	}


	public static void CalculatePropLine(){

		ClearTilesToBuild ();

		if (GameManager.Self().selectedTile == null || clickedTile == null) {
			return;
		}

		int xDif = GameManager.Self().selectedTile.coordinates.x - clickedTile.coordinates.x;
		int yDif = GameManager.Self().selectedTile.coordinates.y - clickedTile.coordinates.y;

		if (Mathf.Abs (xDif) > Mathf.Abs (yDif)) {
			
			if (xDif > 0) {

				for (int i = clickedTile.coordinates.x; i <= GameManager.Self().selectedTile.coordinates.x; i++) {
					AddToTilesToBuild(GridCreator.grid[i,clickedTile.coordinates.y]);
				}

			} else {

				for (int i = GameManager.Self().selectedTile.coordinates.x; i <= clickedTile.coordinates.x; i++) {
					AddToTilesToBuild(GridCreator.grid[i,clickedTile.coordinates.y]);
				}

			}

		} else {

			if (yDif > 0) {

				for (int i = clickedTile.coordinates.y; i <= GameManager.Self().selectedTile.coordinates.y; i++) {
					AddToTilesToBuild(GridCreator.grid[clickedTile.coordinates.x,i]);
				}

			} else {

				for (int i = GameManager.Self().selectedTile.coordinates.y; i <= clickedTile.coordinates.y; i++) {
					AddToTilesToBuild(GridCreator.grid[clickedTile.coordinates.x,i]);
				}

			}

		}

	}


	public static void AddToTilesToBuild(TileManager tile){

		if (propID != -1 && !tile.CheckBuildability ()) {

			return;

		} else if (isDestructing && (tile.objectOnTile == null || tile.objectOnTile.canBeDestroyed)) {
			tile.RenderTile (true, TileFeedback.DestructTile);

			if (tile.objectOnTile != null) {
				tile.objectOnTile.RenderWithColor (Color.red);
			}

		}

		tilesToBeBuilt.Add (tile);

	}
		

	void SetDemoPropAlpha(){

		if(demoProps.Count < 1 || propID < 0 || furnitureList[propID].propPrefab.canBePlacedInLine){
			return;
		}

		foreach(RendererData propRenderer in demoPropRenderData)
        {
			GameManager.SetAlpha (ref propRenderer.renderer, demoPropAlpha);
			if (propRenderer.renderer.material.HasProperty ("Color")) {
				propRenderer.defaultColor = propRenderer.renderer.material.color;
			}
		}

	}

    public void SetDemoPropColor(Color? color = null)
    {

		if(demoProps.Count < 1 || propID < 0 || furnitureList[propID].propPrefab.canBePlacedInLine) {
            return;
        }

        bool setDefaultColor = false;
        Color colorToApply = Color.white;
        

        try
        {
            colorToApply = (Color)color;
            colorToApply.a = demoPropAlpha;
        }

        catch
        {
            setDefaultColor = true;
        }

        foreach(RendererData renderTemp in demoPropRenderData)
        {
            renderTemp.renderer.material.color = setDefaultColor ? renderTemp.defaultColor : colorToApply;
        }

    }

	public void DestroyDemoProp(){
		if (demoProps.Count > 0) {
			
			foreach (GameObject demoProp in demoProps) {
				Destroy (demoProp);
			}

			demoProps.Clear ();

		}
	}


    Direction GetNextDirectionToTheRight (Direction originalDirection)
    {

        switch (originalDirection) {

            case Direction.Right:
                return Direction.Backward;
            case Direction.Backward:
                return Direction.Left;
            case Direction.Left:
                return Direction.Forward;
            case Direction.Forward:
                return Direction.Right;
            default:
                return Direction.Right;

        }

    }

}