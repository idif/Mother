using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildData : MonoBehaviour {

	public static ChildNeed UtilityToNeed(ObjectUtility utility){

		switch (utility) {
		case ObjectUtility.Food:
			return ChildNeed.Hunger;

		case ObjectUtility.Sleep:
			return ChildNeed.Sleep;

		case ObjectUtility.Fun:
			return ChildNeed.Fun;

		default:
			return ChildNeed.Hunger;


		}

	}

}


	public enum ChildState{Idle, GoToActivity, InActivity}
	public enum ChildMood{Normal, Relaxed, Full, Amused, Happy, Tense, Hurt, Sick, Angry, Punished}

	[System.Serializable]public enum ChildTrait{Random, Bully, Victim, Sporty, Initiative, Speaker, Genius, CoolHeaded, Anxious, SmartyPants, Claustrophobic, Prankster, ImaginaryFriend}
	[System.Serializable]public enum ChildNeed{Hunger,Sleep,Fun}
	[System.Serializable]public enum ChildSkill{Charisma,Intelligence,Strength}


	[System.Serializable]
	public class Need{

		public string name = "Need Name";

		public ChildNeed needEnum;

		public float baseDecreaseSpeed = 0.01f;//speed per second at which the need becomes more & more preoccupant (the satisfaction of a need can only be between 0 & 1)

		public float needTolerance = 0.2f;

		public float currentLevel = 1.0f;
	}


[System.Serializable]
public class Mood{

	public ChildMood mood;
	public int priority = 0; // 0 is the highest priority, the bigger the number the lower the priority

}


[System.Serializable]
public class Desire{

	public ObjectUtility type;
	public float desireStrength = 0;
	[HideInInspector]public float currentLevel = 0;

	public Desire(ObjectUtility nature, float strength){

		type = nature;
		desireStrength = strength;

	}

}


	[System.Serializable]
	public class Skill{

		public ChildSkill skillEnum;

		public string name = "Skill Name";

		public int skillLevel = 0;

		[System.NonSerialized]public float currentLevelProgress = 0.0f;
	}


	[System.Serializable]
	public class NeedsList{

		public Need hunger;
		public Need sleep;
		public Need fun;

	}


	[System.Serializable]
	public class SkillsList{

		public Skill charisma;
		public Skill intelligence;
		public Skill strength;

	}


[System.Serializable]
public class MoveData{
	public float minSpeed = 3;
	public float maxSpeed = 10;

	public float minWaitingTime = 0.75f;
	public float maxWaitingTime = 8.0f;

	public float idleMoveRadius = 25.0f;
	public float waitingZoneRadius = 10.0f;
	public float playGroundRadius = 3.0f;

	public int minIdleGoals = 1;
	public int maxIdleGoals = 4;

	[System.NonSerialized]public Vector3[] idleDestinations;
	[System.NonSerialized]public int currentIdleDestination = 0;
}
