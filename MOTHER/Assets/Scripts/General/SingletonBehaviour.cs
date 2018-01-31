using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonBehaviour<Type> : MonoBehaviour where Type : MonoBehaviour {

	private static Type self;

	public static Type Self(){
		if (self == null) {
			Type[] singletons = Resources.FindObjectsOfTypeAll<Type> ();

			if (singletons.Length < 1) {
				//Debug.LogError ("No Singleton of the correct type were found.");
				return null;
			}else{
				List<Type> noPrefabsList = GameManager.GetRealGameObjects<Type> (singletons);
				if (noPrefabsList.Count > 0) {
					self = noPrefabsList[0];
					return self;
				} else {
					//Debug.LogError ("No Instantiated Singleton of the correct type were found.");
					return null;
				}
			}

		} else {
			return self;
		}
	}
}