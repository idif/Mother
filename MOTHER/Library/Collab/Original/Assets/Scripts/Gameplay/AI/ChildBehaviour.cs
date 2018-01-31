using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(BubbleDisplay))]
public class ChildBehaviour : CharacterBehaviour {

	public List<ChildTrait> traits = new List<ChildTrait> ();

	private BubbleDisplay thoughtDisplayer;

	[System.NonSerialized]public ChildState activityStatus = ChildState.Idle;
	[System.NonSerialized]public SmartObject currentObject = null;
	[System.NonSerialized]public ChildMood currentMood = ChildMood.Normal;

	public string childName = "Jacob-Kévin";

	public NeedsList needs;
	public SkillsList skills;
	public MoveData moveSettings;

	void Awake()
	{
		if(traits.Contains(ChildTrait.Random))
		{
			ChildTrait firstTrait;
			firstTrait = (ChildTrait)Random.Range(0, ChildTrait.GetNames(typeof(ChildTrait)).Length);

			traits.Remove (ChildTrait.Random);
			traits.Add (firstTrait);
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


	public Skill GetSkill(ChildSkill desiredSkill){
		Skill selectedSkill = null;
		if (skills.charisma.skillEnum == desiredSkill) {
			selectedSkill = skills.charisma;
		}else if (skills.intelligence.skillEnum == desiredSkill) {
			selectedSkill = skills.intelligence;
		}else if (skills.strength.skillEnum == desiredSkill) {
			selectedSkill = skills.strength;
		}

		return selectedSkill;

	}
		
}