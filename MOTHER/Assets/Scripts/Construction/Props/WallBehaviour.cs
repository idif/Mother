using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBehaviour : FurnitureBehaviour {

	[SerializeField] MeshFilter wallFilter;
	[SerializeField] MeshFilter secondFilter;

	void Start(){

		UpdateWallMesh ();

	}


	public void UpdateWallMesh(){

		List<TileManager> neighbours = tiles [0].NeighbourTiles ();

		if (neighbours.Count == 2) {
			
			wallFilter.mesh = ConstructionScript.Self ().normalWall;
			secondFilter.gameObject.SetActive (true);

		} else {

			secondFilter.gameObject.SetActive (false);

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
				break;

			}

		}

	}


}
