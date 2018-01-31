using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBehaviour : FurnitureBehaviour {

<<<<<<< HEAD
	[SerializeField]Transform meshesPivot;
	[SerializeField] MeshFilter wallFilter;
	[SerializeField] MeshFilter secondFilter;
	[SerializeField]MeshRenderer wallRenderer;
	[SerializeField]MeshRenderer secondRenderer;
=======
	[SerializeField] MeshFilter wallFilter;
	[SerializeField] MeshFilter secondFilter;
>>>>>>> 6918e9b0878999e1061e8a95b659822a79e570be

	void Start(){

		UpdateWallMesh ();

	}


<<<<<<< HEAD
	bool WallsAreAligned(List<TileManager> neighbourWalls){

		if (neighbourWalls.Count < 2) {
			return false;
		}

		bool isXAligned = true;
		bool isYAligned = true;

		for (int i = 1; i < neighbourWalls.Count; i++) {

			if (neighbourWalls [i].coordinates.x != neighbourWalls [i - 1].coordinates.x) {
				isXAligned = false;
			}

			if (neighbourWalls [i].coordinates.y != neighbourWalls [i - 1].coordinates.y) {
				isYAligned = false;
			}

		}
			
		return isXAligned || isYAligned;

	}


	void AlignToNeighbour(TileManager neighbour, float angleOffest = 0){
		
		if (neighbour.coordinates.x == tiles [0].coordinates.x) {

			if (neighbour.coordinates.y < tiles [0].coordinates.y) {

				meshesPivot.localEulerAngles = new Vector3 (0, 90, 0);

			} else {

				meshesPivot.localEulerAngles = new Vector3 (0, 270, 0);

			}

		} else if (neighbour.coordinates.y == tiles [0].coordinates.y) {

			if (neighbour.coordinates.x > tiles [0].coordinates.x) {

				meshesPivot.localEulerAngles = Vector3.zero;

			} else {

				meshesPivot.localEulerAngles = new Vector3 (0, 180, 0);

			}

		}

		meshesPivot.localEulerAngles = new Vector3 (0, meshesPivot.localEulerAngles.y + angleOffest, 0);

	}


	void AlignCorner(List<TileManager> neighbours){

		if (neighbours.Count < 2) {
			return;
		}

		TileManager xNeighbour = neighbours.Find ((TileManager tile) => {
			return tile.coordinates.x == tiles [0].coordinates.x;
		});

		TileManager yNeighbour = neighbours.Find ((TileManager tile) => {
			return tile.coordinates.y == tiles [0].coordinates.y;
		});

		if (xNeighbour == null || yNeighbour == null) {
			return;
		}

		if (xNeighbour.coordinates.y > tiles [0].coordinates.y) {

			if (yNeighbour.coordinates.x > tiles [0].coordinates.x) {
				meshesPivot.localEulerAngles = new Vector3 (0, 0, 0);
			} else {
				meshesPivot.localEulerAngles = new Vector3 (0, 270, 0);
			}
		} else {

			if (yNeighbour.coordinates.x > tiles [0].coordinates.x) {
				meshesPivot.localEulerAngles = new Vector3 (0, 90, 0);
			} else {
				meshesPivot.localEulerAngles = new Vector3 (0, 180, 0);
			}
		}

	}


	TileManager MainNeighbourOfTCross(List<TileManager> neighbours){

		List<TileManager> xAlignedNeighbours = neighbours.FindAll ((TileManager tile) => {
			return tile.coordinates.x == tiles [0].coordinates.x;
		});

		List<TileManager> yAlignedNeighbours = neighbours.FindAll ((TileManager tile) => {
			return tile.coordinates.y == tiles [0].coordinates.y;
		});

		if (xAlignedNeighbours.Count == 0 || yAlignedNeighbours.Count == 0) {
			return null;
		}

		return xAlignedNeighbours.Count == 1 ? xAlignedNeighbours [0] : yAlignedNeighbours [0];

	}


	public void UpdateWallMesh(){

		List<TileManager> neighbourWalls = tiles [0].NeighbourTiles ().FindAll((TileManager tile) => {return tile.objectOnTile != null && tile.objectOnTile.blockRooms; } );



		if (neighbourWalls.Count == 2 && WallsAreAligned (neighbourWalls)) {
			
			SetModel (ConstructionScript.Self ().normalWall);
			secondFilter.gameObject.SetActive (true);
			AlignToNeighbour (neighbourWalls [0]);
=======
	public void UpdateWallMesh(){

		List<TileManager> neighbours = tiles [0].NeighbourTiles ();

		if (neighbours.Count == 2) {
			
			wallFilter.mesh = ConstructionScript.Self ().normalWall;
			secondFilter.gameObject.SetActive (true);
>>>>>>> 6918e9b0878999e1061e8a95b659822a79e570be

		} else {

			secondFilter.gameObject.SetActive (false);

<<<<<<< HEAD
			switch (neighbourWalls.Count) {

			case 0:
				SetModel (ConstructionScript.Self ().column);
				break;

			case 1:
				SetModel (ConstructionScript.Self ().end);
				AlignToNeighbour (neighbourWalls [0]);
				break;

			case 2:
				SetModel (ConstructionScript.Self ().corner);
				AlignCorner (neighbourWalls);
				break;

			case 3:
				SetModel (ConstructionScript.Self ().tCorner);
				AlignToNeighbour (MainNeighbourOfTCross (neighbourWalls),90);
				break;

			default:
				SetModel (ConstructionScript.Self ().allCorner);
=======
			switch (neighbours.Count) {

			case 0:
				wallFilter.mesh = ConstructionScript.Self ().column;
				break;

			case 1:
				wallFilter.mesh = ConstructionScript.Self ().corner;
				break;

			case 3:
				wallFilter.mesh = ConstructionScript.Self ().tCorner;
				break;

			default:
				wallFilter.mesh = ConstructionScript.Self ().allCorner;
>>>>>>> 6918e9b0878999e1061e8a95b659822a79e570be
				break;

			}

		}

	}

<<<<<<< HEAD
	void SetModel(ModelData model){

		wallFilter.mesh = model.mesh;
		wallRenderer.material = model.materials[0];

	}

=======
>>>>>>> 6918e9b0878999e1061e8a95b659822a79e570be

}
