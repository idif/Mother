using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceTextManager : SingletonBehaviour<ResourceTextManager> {

	public Text zaka;

	void Start(){
		UpdateResourceTexts ();
	}

	public void UpdateResourceTexts()
	{
		zaka.text = GameManager.Self().zakarium.ToString();
	}
}
