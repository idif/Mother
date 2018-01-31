using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public struct IntVector2{

	public int x;
	public int y;

	public IntVector2(int newX = 0, int newY = 0){

		x = newX;
		y = newY;

	}

}

public class RendererData{

	public Renderer renderer;
	public Color defaultColor;

}

public class GameManager : SingletonBehaviour<GameManager> {

	[HideInInspector]public float timeSpeed = 1; //0 means game is paused & 1 means normal speed
	[HideInInspector]public float time = 0; // time in seconds of "game time" since the launch of the scene (paused time isn't taken in account)

	public delegate void timeMessage();

	public timeMessage OnSpeedChanged;

	[HideInInspector]public bool isTyping = false;

	[HideInInspector]public TileManager selectedTile = null;//the current tile the mouse is on

	[HideInInspector]public bool energyStorm = false;

	public int zakarium = 1000;


	public void Typing(bool newIsTyping)
	{
		isTyping = newIsTyping;
	}

	public static void SetAlpha(ref Renderer renderer,float newAlpha){
		if (renderer.material.HasProperty ("_Color")) {
			Color newColor = renderer.material.color;
			newColor.a = newAlpha;
			renderer.material.color = newColor;
		}
	}

	public static void AddZakarium(int zakToAdd){
		Self().zakarium += zakToAdd;
		ResourceTextManager.Self ().UpdateResourceTexts ();
	}


	public static void GetAllRenderers(GameObject objectToCheck,ref List<RendererData> list){
		Renderer compToAdd = objectToCheck.GetComponent<Renderer> ();
		if (compToAdd != null) {
			RendererData tempData = new RendererData();
			tempData.renderer = compToAdd;
			if(tempData.renderer.material.HasProperty("_Color")){
			tempData.defaultColor = compToAdd.material.color;
			}
			list.Add (tempData);
		}
		for (int i = 0; i < objectToCheck.transform.childCount; i++) {
			GetAllRenderers (objectToCheck.transform.GetChild (i).gameObject, ref list);
		}
	}



	public static float Vector3InverseLerp(Vector3 vectorA,Vector3 vectorB, Vector3 value){
		Vector3 aB = vectorB - vectorA;
		Vector3 aV = value - vectorA;
		return Vector3.Dot (aV,aB) / Vector3.Dot(aB,aB);
	}



	public static void SetTimeSpeed(float newSpeed){
		Self().timeSpeed = newSpeed;
		Self().OnSpeedChanged ();
	}



	public static Transform GetTransformByName(Transform searchTransform,string name){//search all the childs, grandchilds... etc, of a gameobject and returns one that has the specified name (if any)

		if (searchTransform.gameObject.name == name) {
			return searchTransform;
		} else {
			foreach(Transform transf in searchTransform.GetComponentsInChildren<Transform>(true)){
				if (transf.gameObject.name == name) {
					return transf;
				}
			}
		}
		return null;
	}



	public static List<Type> GetRealGameObjects<Type>(Type[] originalArray) where Type : MonoBehaviour{//When given a list of gameobjects (instantiated in the scene and /or prefabs), filters the prefabs to return only actually instantiated gameobjects

		List<Type> returnList = new List<Type> ();

		for(int i = 0; i < originalArray.Length;i++){

			if (originalArray [i].hideFlags != HideFlags.HideAndDontSave && originalArray [i].hideFlags != HideFlags.NotEditable) {

				#if UNITY_EDITOR

				if(!string.IsNullOrEmpty(AssetDatabase.GetAssetPath(originalArray[i].transform.root.gameObject))){
					continue;
				}

				#endif

				returnList.Add (originalArray [i]);

			}

		}

		return returnList;

	}



	public static int RoundToTen(float number){
		return((int)(number / 10) * 10);
	}

}
