using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]public enum ObjectUtility {Food, Sleep, Fun, BadFun, Strength, Intelligence, Charisma, Work}

[System.Serializable]
public class ObjectFunction{

	public ObjectUtility utility;
	public float appeal;

}

public class SmartObject : MonoBehaviour {


	public static Dictionary<ObjectUtility,List<SmartObject>> smartObjects = new Dictionary<ObjectUtility, List<SmartObject>>
	{

		{ObjectUtility.Food,new List<SmartObject>()},
		{ObjectUtility.Sleep,new List<SmartObject>()},
		{ObjectUtility.Fun,new List<SmartObject>()},

		{ObjectUtility.BadFun,new List<SmartObject>()},

		{ObjectUtility.Strength,new List<SmartObject>()},
		{ObjectUtility.Intelligence,new List<SmartObject>()},
		{ObjectUtility.Charisma,new List<SmartObject>()},

		{ObjectUtility.Work,new List<SmartObject>()}

	};


	public ObjectFunction[] functions;


	public Dictionary<ObjectUtility,ObjectFunction> functionsRefs = new Dictionary<ObjectUtility, ObjectFunction> ();


	[HideInInspector]public ChildBehaviour currentUser = null;


	protected void Awake(){
		foreach (ObjectFunction function in functions) {

			functionsRefs.Add (function.utility, function);

		}

	}


	protected void Start(){
			foreach (ObjectFunction function in functions) {
				smartObjects [function.utility].Add (this);
			}
	}


	public bool TryUseObject(ChildBehaviour newUser){

		if (currentUser != null) {
			return false;
		} else {
			currentUser = newUser;
			return true;
		}

	}


	public virtual void StartUseObject(){



	}


	public virtual void EndUseObject(){

		if (currentUser == null) {
			return;
		}

		ChildBehaviour oldUser = currentUser;
		oldUser.currentObject = null;
		currentUser = null;

		oldUser.ChooseNextAction ();

	}
		
	void OnDestroy(){
		foreach (ObjectFunction function in functions) {
			if (smartObjects [function.utility].Contains (this)) {
				smartObjects [function.utility].Remove (this);
			}
		}
	}


}
