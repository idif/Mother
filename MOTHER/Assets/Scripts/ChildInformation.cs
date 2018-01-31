using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildInformation : SingletonBehaviour<ChildInformation>
{
	public ChildBehaviour[] children;
	[HideInInspector]public ChildrenChoices scriptInfos;

	void Awake ()
	{
		scriptInfos = ChildrenChoices.Self ();
		for (int i = 0; i < children.Length; i++) {
			children [i].childName = scriptInfos.children [i].childName;
			children [i].traits.Add((ChildTrait)scriptInfos.children [i].trait);
		}
	}
}
