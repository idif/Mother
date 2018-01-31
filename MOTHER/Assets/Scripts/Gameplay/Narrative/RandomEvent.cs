using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ConsequenceType{ChildNeeds, ChildMood, ChildrenNeeds, ChildrenMood}

[System.Serializable]
public class EndEvent
{
	public Sprite endingSprite;
	public string endingText;
}
	
[System.Serializable]
public struct Consequence
{
	public ConsequenceType type;
	public float value1;
	public float value2;
	public float value3;
}

[System.Serializable]
public class Choice
{
	public string choiceText;
	public int result;
	public Consequence[] consequences;
}

[System.Serializable]
public class NarrativeContent
{
	public Sprite conceptImage;
	public string contentText;
	public Choice[] choices;
}


public class RandomEvent : SingletonBehaviour <RandomEvent>
	{
	[Header("Tableaux d'events")]
	public NarrativeContent[] narrative;
	public EndEvent[] endingEvents;

	[Header("Time")]
	public float minTime = 10;
	public float maxTime = 30;



	[System.NonSerialized] public int currentNarratifID;
	//creation d'une liste qui va gérer les events deja faits
	[System.NonSerialized] public List<NarrativeContent> remainingEvents = new List<NarrativeContent>();

	void Awake ()
	{
		remainingEvents.AddRange (narrative);
	}
	//FIN creation d'une liste qui va gérer les events deja faits	

	void Start ()
	{

		#if UNITY_EDITOR
		if(remainingEvents.Count <= 0){
			return;
		}
		#endif

		currentNarratifID = Random.Range(0, remainingEvents.Count);
		StartCoroutine(TimerForEvent());
	}

	//début : gestion du choix et des impacts qu'il a sur le gameplay
	public void SelectChoice (int choiceID)
	{
		if (remainingEvents [currentNarratifID].choices [choiceID].consequences != null) 
			{
				foreach (Consequence consequenceTemp in remainingEvents[currentNarratifID].choices[choiceID].consequences) 
				{
					switch (consequenceTemp.type)
					{
					case ConsequenceType.ChildNeeds:
							// appel de la fonction de gestion des besoins d'UN enfant
						break;
					case ConsequenceType.ChildMood:
							// appel de la fonction de gestion du mood d'UN enfant
						break;
					case ConsequenceType.ChildrenNeeds:
							// appel de la fonction de gestion des besoins des enfants
						break;
					case ConsequenceType.ChildrenMood:
							// appel de la fonction de gestion du mood des enfants
						break;

					default :
						break;
					}
				}
			NarrativeUI_Manager.Self ().LastPanelEvent (remainingEvents [currentNarratifID].choices [choiceID].result);
			}
		else {
			NarrativeUI_Manager.Self ().LastPanelEvent (remainingEvents [currentNarratifID].choices [choiceID].result);
		}
	}
	//fin : gestion du choix et des impacts qu'il a sur le gameplay


	//début : gestion UI et choix (random) du narratif
	IEnumerator TimerForEvent()
	{
		float progress = 0;
		float timeBeforeNextEvent = Random.Range(minTime, maxTime);
	
		while (progress < timeBeforeNextEvent)
		{

			progress += GameManager.Self().timeSpeed * Time.deltaTime;
			yield return new WaitForSeconds(0);
		
		}

		NarrativeUI_Manager.Self().ManagementUIOn();
	}
	//fin : choix (random) du narratif


	public void EventEnding()
	{
		//permettre de relancer le cooldown pour un nouvel event et enlever la pause
		remainingEvents.RemoveAt (currentNarratifID); //retirer de la liste l'event qui vient d'être "joué"
	
		if (remainingEvents.Count != 0) 
		{
			currentNarratifID = Random.Range(0, remainingEvents.Count);
			StartCoroutine(TimerForEvent());
		}
		else
		{
			Debug.Log ("No more events left");
		}

		//gestion UI du close panel
		NarrativeUI_Manager.Self().CloseEndingPanel ();
	}
//END : Ending of the events
}