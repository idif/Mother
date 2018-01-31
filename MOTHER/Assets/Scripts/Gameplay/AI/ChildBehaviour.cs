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

	[HideInInspector]public string childName = "Jacob-Kévin";


	public GameObject mesh;

	[Header("Smart Objects Stats")]
	public float minimumAppeal = 0.4f;
	public float appealReductionForDistance = 0.001f;//the higher it is, the less attractive remote props will be

	[Header("Characteristics")]

	public List<ChildTrait> traits = new List<ChildTrait> ();
	public NeedsList needs;
	public SkillsList skills;
	public MoveData moveSettings;

	public Dictionary<ChildNeed, Need> needRefs = new Dictionary<ChildNeed, Need>();
	public Dictionary<ChildSkill,Skill> skillRefs = new Dictionary<ChildSkill, Skill>();

	public List<Desire> desires =  new List<Desire> ();

	public bool isBoy;

	new protected void Awake(){
		
		base.Awake ();

		needRefs.Add (ChildNeed.Hunger, needs.hunger);
		needRefs.Add (ChildNeed.Sleep, needs.sleep);
		needRefs.Add (ChildNeed.Fun, needs.fun);

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


	void Start(){
		ChooseNextAction ();
	}


	public void ChooseNextAction(){

		UpdateDesires ();

		float subjectiveAppeal = 0;
		SmartObject nextObject = GetNextDestination (out subjectiveAppeal);

		if (nextObject != null) {
			
			currentObject = nextObject;
			currentObject.currentUser = this;
			StartCoroutine (WaitUntilDestination ());

		} else {

			StartCoroutine (IdleMove ());

		}

	}


	public IEnumerator IdleMove(){

		Vector3 idleDestination = GetRandomPoint (transform.position, moveSettings.idleMoveRadius);

		agent.SetDestination (idleDestination);

		while (CheckIfMoving ()) {
			yield return new WaitForSeconds (0);
		}

		ChooseNextAction ();
			
	}


	SmartObject GetNextDestination(out float subjectiveAppeal){

		for (int i = 0; i < desires.Count; i++) {

			if (desires [i].currentLevel < minimumAppeal) {
				continue;
			}

			float smallestDistance = float.PositiveInfinity;
			SmartObject closestObject = null;

			for (int j = 0; j < SmartObject.smartObjects [desires [i].type].Count; j++) {

				SmartObject reviewedObject = SmartObject.smartObjects [desires [i].type] [j];

				if (reviewedObject.currentUser != null) {
					continue;
				}

				float sqrDistance = Vector3.SqrMagnitude (transform.position - reviewedObject.transform.position);

				if(sqrDistance < smallestDistance){

					smallestDistance = sqrDistance;
					closestObject = reviewedObject;

				}

			}


			if (closestObject != null ) {

				float objectiveAppeal = closestObject.functionsRefs [desires [i].type].appeal;
				float interest = objectiveAppeal / (smallestDistance * appealReductionForDistance);
				subjectiveAppeal = interest;
				return closestObject;

			}

		}
		
		subjectiveAppeal = 0;
		return null;

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
		

	public void UpdateDesires(){

		CalculateDesireStrengths ();
		SortDesires ();

	}

	public void CalculateDesireStrengths(){
		
		foreach (Desire desire in desires) {

			if (desire.type == ObjectUtility.Food ||
			    desire.type == ObjectUtility.Sleep ||
			    desire.type == ObjectUtility.Fun) {

				CalculateNeedDesire (desire);

			} else if (desire.type == ObjectUtility.Strength ||
			           desire.type == ObjectUtility.Intelligence ||
			           desire.type == ObjectUtility.Charisma) {

				CalculateSkillDesire (desire);

			} else if (desire.type == ObjectUtility.Work) {

				CalculateWorkDesire (desire);

			} else {

				CalculateBadFunDesire (desire);

			}

		}
	}


	public void SortDesires(){

		desires = desires.OrderByDescending (o => o.currentLevel).ToList ();
			
	}



	public void CalculateNeedDesire(Desire desire){

		desire.currentLevel = (1 - needRefs [ChildData.UtilityToNeed(desire.type)].currentLevel) * desire.desireStrength;

	}

	public void CalculateSkillDesire(Desire desire){

		desire.currentLevel = Random.value * desire.desireStrength;

	}

	public void CalculateBadFunDesire(Desire desire){

		desire.currentLevel = currentMood == ChildMood.Angry ? Random.Range (0.25f, 1.0f) * desire.desireStrength : 0;

	}

	public void CalculateWorkDesire(Desire desire){

		desire.currentLevel = Random.value * desire.desireStrength;

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

		skills.charisma.currentLevelProgress =  Random.value;
		skills.intelligence.currentLevelProgress =  Random.value;
		skills.strength.currentLevelProgress =  Random.value;
	}
		
}