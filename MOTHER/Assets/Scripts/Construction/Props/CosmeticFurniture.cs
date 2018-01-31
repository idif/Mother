using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmeticFurniture : FurnitureBehaviour {

	public ChildMood givenMood;

	void Start () {

		if(tiles[0] != null && tiles[0].room == null){
			return;
		}

		tiles [0].room.CalculateMoodLevel ();

	}

}
	