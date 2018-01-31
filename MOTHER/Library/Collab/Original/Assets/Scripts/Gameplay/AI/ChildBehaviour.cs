using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

[RequireComponent(typeof(BubbleDisplay))]
public class ChildBehaviour : CharacterBehaviour {

	private BubbleDisplay thoughtDisplayer;

	[System.NonSerialized]public ChildState activityStatus = ChildState.Idle;
	[System.NonSerialized]public SmartObject currentObject = null;
	[System.NonSerialized]public ChildMood currentMood = ChildMood.Normal;

	public string childName = "Jacob-Kévin";

	[Header("Characteristics")]

	public List<ChildTrait> traits = new List<ChildTrait> ();
	public NeedsList needs;
	public SkillsList skills;
	public MoveData moveSettings;

	public Dictionary<ChildSkill,Skill> skillRefs = new Dictionary<ChildSkill, Skill>();

	public ObjectFunction[] desires;

	public bool isBoy;

	new protected void Awake(){
		
		base.Awake ();

		skillRefs.Add (ChildSkill.Charisma, skills.charisma);
		skillRefs.Add (ChildSkill.Intelligence, skills.intelligence);
		skillRefs.Add (ChildSkill.Strength, skills.strength);

		// Caractère random
		if(traits.Contains(ChildTrait.Random))
		{
			ChildTrait firstTrait;
			firstTrait = (ChildTrait)Random.Range(0, ChildTrait.GetNames(typeof(ChildTrait)).Length);

			traits.Remove (ChildTrait.Random);
			traits.Add (firstTrait);
		}
		// Noms randoms
		if (childName == "") 
		{
			if (isBoy) {
				int myNewMaleName = (Random.Range (0, ChildrenChoices.Self ().randomMaleNames.Length));
				childName = ChildrenChoices.Self ().randomMaleNames [myNewMaleName];
			} else {
				int myNewFemaleName = (Random.Range (0, ChildrenChoices.Self ().randomFemaleNames.Length));
				childName = ChildrenChoices.Self ().randomFemaleNames [myNewFemaleName];
			}
		}
	}


	public IEnumerator IdleMove(){

		agent.SetDestination (GetRandomPoint (transform.position, moveSettings.idleMoveRadius));

		while (CheckIfMoving ()) {
			yield return new WaitForSeconds (0);
		}
			
	}


	public IEnumerator WaitUntilDestination(){

		if(currentObject != null){

			agent.SetDestination (currentObject.transform.position);

			while (CheckIfMoving ()) {
				yield return new WaitForSeconds (0);
			}

			currentObject.StartUseObject ();

		}

	}



	protected override void OnMouseClickOn(){
		if (selectedCharacter != this) {
			SelectCharacter ();
		} else {
			ChildPanelManager.current.OpenCharacterPanel (this);
		}
	}


	void RandomizeStats(){
		needs.hunger.currentLevel = Random.Range (0.35f,1);
		needs.sleep.currentLevel = Random.Range (0.35f,1);
		needs.fun.currentLevel =  Random.Range (0.35f,1);;

		skills.charisma.currentLevelProgress =  Random.Range (0,1);
		skills.intelligence.currentLevelProgress =  Random.Range (0,1);
		skills.strength.currentLevelProgress =  Random.Range (0,1);
	}
		
}