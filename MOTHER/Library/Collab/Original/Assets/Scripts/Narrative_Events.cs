﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum ConsequenceType{Zackarium, MegaWatt, Oxygen, ChildrenNeeds, ChildNeeds, ChildMood, ChildrenMood, EventBonus}

[System.Serializable]
public class BonusEvents
{
	
}


[System.Serializable]
public struct Consequence
{
	public ConsequenceType type;
	public float value1;
	public float value2;
}

[System.Serializable]
public class Choice
{
	public string choiceText;
	public Consequence[] consequences;
}

[System.Serializable]
public class UINarrativeGestion
{
	public Image fondNarratif;
	public GameObject fondPanelNarratif;

	public Image conceptArtNarratif;
	public Text contentTextNarra;
	public Text[] choiceTexts;
	public GameObject[] buttonChoice;

	public GameObject EndButton;
}

[System.Serializable]
public class NarrativeContent
{
	public Sprite conceptImage;
	public string contentText;
	public Choice[] choices;

	public Sprite endImage;
	public string endContent;
}
	

public class Narrative_Events : MonoBehaviour 
{
	public UINarrativeGestion gestionUI;
	public NarrativeContent[] narrative;

	[Header("Time")]
	public float minTime = 10;
	public float maxTime = 30;

	[System.NonSerialized]

	int currentNarratifID;
	//creation d'une liste qui va gérer les events deja faits
	List<NarrativeContent> remainingEvents = new List<NarrativeContent>();

	void Awake ()
	{
		remainingEvents.AddRange (narrative);
	}
	//FIN creation d'une liste qui va gérer les events deja faits	

	void Start ()
	{
		currentNarratifID = Random.Range(0, remainingEvents.Count);
		StartCoroutine(TimerForEvent());
	}
		
	//début : gestion du choix et des impacts qu'il a sur le gameplay
	public void SelectChoice (int choiceID)
	{
		//insert code pour close le choix

		foreach (Consequence consequenceTemp in narrative[currentNarratifID].choices[choiceID].consequences) 
		{
			switch (consequenceTemp.type)
			{
			case ConsequenceType.Zackarium:
				GameManager.AddZakarium ((int)consequenceTemp.value1); // appel de la fonction de gestion du Zackarium
				break;
			case ConsequenceType.MegaWatt :
				// appel de la fonction de gestion de l'énergie
				break;
			case ConsequenceType.Oxygen :
				// appel de la fonction de gestion de l'oxygène
				break;
			case ConsequenceType.ChildNeeds :
				// appel de la fonction de gestion des besoins d'UN enfant
				break;
			case ConsequenceType.ChildMood :
				// appel de la fonction de gestion du mood d'UN enfant
				break;
			case ConsequenceType.ChildrenNeeds :
				// appel de la fonction de gestion des besoins des enfants
				break;
			case ConsequenceType.ChildrenMood :
				// appel de la fonction de gestion du mood des enfants
				break;
			case ConsequenceType.EventBonus :
				// appel de la fonction de gestion de l'event suivant
				break;
			default :
				break;
			}
		}

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

		gestionUI.fondPanelNarratif.SetActive(false);
		gestionUI.fondNarratif.enabled = false;
		GameManager.SetTimeSpeed (1.0f);

		//affichage du panel de résultat de l'action narrative
		EndEvent (choiceID);
	}
	//fin : gestion du choix et des impacts qu'il a sur le gameplay


	//début : gestion UI et choix (random) du narratif
	IEnumerator TimerForEvent()
	{
		//attribution des textes/images de la mission choisie au hasard précédemment
		gestionUI.conceptArtNarratif.sprite = remainingEvents[currentNarratifID].conceptImage;
		gestionUI.contentTextNarra.text = remainingEvents [currentNarratifID].contentText;

		for (int i = 0; i < remainingEvents[currentNarratifID].choices.Length; i++) 
		{
			gestionUI.choiceTexts [i].text = remainingEvents [currentNarratifID].choices [i].choiceText;
		}
		//fin d'attribution de l'affichage. 

		yield return new WaitForSeconds(Random.Range(minTime, maxTime));

		//Début de la gestion du Canvas in-game
		for (int i = 0; i < gestionUI.choiceTexts.Length; i++) 
		{
			gestionUI.choiceTexts [i].enabled = true;
			gestionUI.buttonChoice [i].SetActive (true);
		}

		gestionUI.fondPanelNarratif.SetActive(true);
		gestionUI.fondNarratif.enabled = true;
		GameManager.SetTimeSpeed(0.0f);

		if (gestionUI.choiceTexts.Length - remainingEvents [currentNarratifID].choices.Length != 0) 
		{
			for (int i = 0; i < (gestionUI.choiceTexts.Length - remainingEvents [currentNarratifID].choices.Length); i++) 
			{
				gestionUI.choiceTexts [gestionUI.choiceTexts.Length - 1 - i].enabled = false;
				gestionUI.buttonChoice [gestionUI.choiceTexts.Length - 1 - i].SetActive (false);
			}
		}
		//Fin de la gestion Canvas
	}
	//fin : gestion UI et choix (random) du narratif

	void EndEvent(int eventID)
	{
		//gestion du hud et de la pause
		gestionUI.fondPanelNarratif.SetActive(true);
		gestionUI.fondNarratif.enabled = true;
		GameManager.SetTimeSpeed (0.0f);

		//gestion de l'event bonus (désactivation des choix / buttons de choix et affichage du résultat de l'action
		for (int i = 0; i < gestionUI.choiceTexts.Length; i++) 
		{
			gestionUI.choiceTexts [i].enabled = false;
			gestionUI.buttonChoice [i].SetActive (false);
		}

		gestionUI.conceptArtNarratif.sprite = remainingEvents[currentNarratifID].endImage;
		gestionUI.contentTextNarra.text = remainingEvents [currentNarratifID].endContent;

		gestionUI.EndButton.SetActive (true);

	}
}