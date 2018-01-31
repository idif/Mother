using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GridCreator : SingletonBehaviour<GridCreator> {

	[Header("Prefabs")]
	public TileManager plane;//the prefab instanciated
    public FurnitureBehaviour zoneWall;
    public FurnitureBehaviour zoneDoor;
    public FurnitureBehaviour fogOfWar;

    [Header("Grid Parameter")]
	public int width = 2;
	public int depth = 2;

	public int gridWidht = 80;
	public int gridDepth = 100;

	public float gridY = 1.05f;

    [Header("Prefabs Changes Properties")]

    public Material newFogOfWarMat;

	public static TileManager[,] grid;

	[HideInInspector]public List<FurnitureBehaviour> zoneWalls = new List<FurnitureBehaviour> ();

	#if UNITY_EDITOR
	public void GenerateGrid () {

		GetGrid ();

			for (int i = 0; i < grid.GetLength (0); i++) {

			if (i >= gridWidht) {
				DestroyImmediate (transform.GetChild (i));
				continue;
			}

				for (int j = 0; j < grid.GetLength (1); j++) {
				
					if (j >= gridDepth) {
						DestroyImmediate (grid [i, j].gameObject);
					}

				}
			}

			grid = new TileManager[gridWidht, gridDepth];

			for (int x = 0; x < grid.GetLength (0); x++) {

				Transform linePivot;

			if (transform.childCount > x) {
				linePivot = transform.GetChild (x);
			}

			else {

					linePivot = new GameObject (x.ToString ()).transform;
					linePivot.parent = transform;
					linePivot.SetSiblingIndex (x);
					linePivot.localPosition = Vector3.zero;
					linePivot.transform.position += new Vector3 (x * width + width / 2, gridY, 0);
				}

				for (int z = 0; z < grid.GetLength (1); z++) {

					if (grid [x, z] == null) {
					
						TileManager gridPlane = GameObject.Instantiate (plane, linePivot);
						gridPlane.transform.localPosition = Vector3.zero;
						gridPlane.transform.position += new Vector3 (0, 0, z * depth + depth / 2);
						grid [x, z] = gridPlane;
						gridPlane.coordinates = new IntVector2 (x, z);
						
						}

				}

			}
	}

    public void ChangePrefabProperties()
    {

        GetGrid();

        for(int i = 0; i < grid.GetLength(0); i++)
        {

            for(int j = 0; j < grid.GetLength(1); j++)
            {

                switch(grid[i, j].tileStartStatus)
                {

<<<<<<< HEAD
                    /*case StartObject.ForOfWar:
=======
                    case StartObject.ForOfWar:
>>>>>>> 6918e9b0878999e1061e8a95b659822a79e570be
                         Renderer[] rendersTemp = grid[i, j].GetComponentsInChildren<Renderer>();

                        foreach(Renderer renderTemp in rendersTemp)
                        {
                            renderTemp.material = newFogOfWarMat;
                        }

<<<<<<< HEAD
                        break;*/

				case StartObject.ZoneWall:
					
					Transform oldWall = grid [i, j].transform.GetChild (0);
					Quaternion oldRotation = oldWall.rotation;
					DestroyImmediate (oldWall.gameObject);
					grid[i,j].objectOnTile = Instantiate (zoneWall, grid [i, j].transform.position, oldRotation, grid [i, j].transform);

					grid[i,j].objectOnTile.SendMessage("UpdateWallMesh");

					break;
=======
                        break;
>>>>>>> 6918e9b0878999e1061e8a95b659822a79e570be

                    default:
                        break;

                }

            }

        }

    }

	#endif

	void Awake(){

		#if UNITY_EDITOR
		if(!Application.isPlaying){
			return;
		}
		#endif

		GetGrid ();

		TileManager.startColor = plane.GetComponent<Renderer> ().sharedMaterial.color;

	}


	void Start(){
		#if UNITY_EDITOR
		if(!Application.isPlaying){
			return;
		}
		#endif

		AssignRooms ();

	}


	void GetGrid(){

		if (transform.childCount > 0) {

			grid = new TileManager[transform.childCount, transform.GetChild (0).childCount];

			for (int x = 0; x < transform.childCount; x++) {

				Transform linePivot = transform.GetChild (x);

				for (int y = 0; y < linePivot.childCount; y++) {
					TileManager tile = linePivot.GetChild (y).GetComponent<TileManager> ();
					if (tile != null) {
						grid [x, y] = tile;
						tile.coordinates = new IntVector2 (x, y);

						FurnitureBehaviour tileFurniture = tile.GetComponentInChildren<FurnitureBehaviour> ();

						if (tileFurniture != null) {
							tile.objectOnTile = tileFurniture;
							tile.objectOnTile.tiles.Add (tile);

							if (tile.tileStartStatus == StartObject.ZoneWall || tile.tileStartStatus == StartObject.ZoneDoor) {
								zoneWalls.Add (tile.objectOnTile);
							}

						}

					}
				}

			}

		} else {
			grid = new TileManager[0,0];

			#if UNITY_EDITOR
			if(Application.isPlaying){
				Debug.LogError ("ERROR : Grid is missing. Maybe you forgot to generate it or deplaced/destroyed it. Please regenerate one with the grid creator script before you launch play mode.");
			}
			#endif

		}

	}

	void AssignRooms(){

		foreach (TileManager tile in grid) {

			if (tile.room == null) {

				List<TileManager> newRoom = new List<TileManager> ();
				tile.GetRoomTiles (ref newRoom);
<<<<<<< HEAD
				new Room (newRoom, true, 0);
=======
				new Room (newRoom, true, Color.white);
>>>>>>> 6918e9b0878999e1061e8a95b659822a79e570be

			}

		}

	}


}
