using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ChildInfos
{
	public string childName;
	public int trait;
}

public class ChildrenChoices : SingletonBehaviour<ChildrenChoices> {

	public Dropdown[] childTrait;
	public Text[] childChoiceName;
	public ChildInfos[] children;

	public string[] randomMaleNames;
	public string[] randomFemaleNames;

	void Awake()
	{
		if (Self () != this) {
			Destroy (this);
		} else {
			DontDestroyOnLoad (this);
		}
	}

	public void SaveChildren() 
	{
		for (int i = 0; i < children.Length; i++) 
		{
			children [i].childName = childChoiceName [i].text;
			children [i].trait = childTrait [i].value;
		}
	}
}
