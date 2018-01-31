using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class HandySpawner : InteractibleFurniture 
{
	public BabySitterDrone handyModel;
	private BabySitterDrone myHandy;
	public Transform spawnPoint;

	void Start () 
	{
		NavMeshHit hit;
		if (NavMesh.SamplePosition (spawnPoint.position, out hit, 1000000, NavMesh.AllAreas)) 
		{
			myHandy = GameObject.Instantiate (handyModel, hit.position, Quaternion.identity);
			myHandy.mySpawner = this.transform;
		}
	}

	protected override void OnMouseClickOn()
	{
		if (!ConstructionScript.isDestructing) 
		{
			ChildPanelManager.current.OpenHandyPanel (myHandy);
			myHandy.selectionFx.SetActive (true);
			CharacterBehaviour.selectedCharacter = myHandy;
		}
	}

	void OnMouseClickElsewhere()
	{
		if (ChildPanelManager.current.currentHandy == myHandy) 
		{
			ChildPanelManager.current.ClosePanel ();
		}

		myHandy.selectionFx.SetActive (false);
		CharacterBehaviour.selectedCharacter = null;
	}

	protected override void WhenDestroyed () 
	{
		GameManager.Self().OnSpeedChanged -= myHandy.OneSpeedChanged;
		Destroy (myHandy.gameObject);
	}
}
