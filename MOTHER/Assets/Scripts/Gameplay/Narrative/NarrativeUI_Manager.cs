using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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


public class NarrativeUI_Manager : SingletonBehaviour <NarrativeUI_Manager>
{
	public UINarrativeGestion gestionUI;


	public void ManagementUIOn()
	{
		//attribution des textes/images de la mission choisie au hasard précédemment
		gestionUI.conceptArtNarratif.sprite = RandomEvent.Self().remainingEvents[RandomEvent.Self().currentNarratifID].conceptImage;
		gestionUI.contentTextNarra.text = RandomEvent.Self().remainingEvents [RandomEvent.Self().currentNarratifID].contentText;

		for (int i = 0; i < RandomEvent.Self().remainingEvents[RandomEvent.Self().currentNarratifID].choices.Length; i++) 
		{
			gestionUI.choiceTexts [i].text = RandomEvent.Self().remainingEvents [RandomEvent.Self().currentNarratifID].choices [i].choiceText;
		}
		//fin d'attribution de l'affichage. 

		//Début de la gestion du Canvas in-game
		for (int i = 0; i < gestionUI.choiceTexts.Length; i++) 
		{
			gestionUI.choiceTexts [i].enabled = true;
			gestionUI.buttonChoice [i].SetActive (true);
		}
		
		gestionUI.fondPanelNarratif.SetActive(true);
		gestionUI.fondNarratif.enabled = true; 
		GameManager.SetTimeSpeed(0.0f);

		if (gestionUI.choiceTexts.Length - RandomEvent.Self().remainingEvents [RandomEvent.Self().currentNarratifID].choices.Length != 0) 
		{
			for (int i = 0; i < (gestionUI.choiceTexts.Length - RandomEvent.Self().remainingEvents [RandomEvent.Self().currentNarratifID].choices.Length); i++) 
			{
				gestionUI.choiceTexts [gestionUI.choiceTexts.Length - 1 - i].enabled = false;
				gestionUI.buttonChoice [gestionUI.choiceTexts.Length - 1 - i].SetActive (false);
			}
		}
		//Fin de la gestion Canvas
	}


	public void LastPanelEvent(int eventID)
	{
		//gestion de l'event bonus (désactivation des choix / buttons de choix et affichage du résultat de l'action
		for (int i = 0; i < gestionUI.choiceTexts.Length; i++) 
		{
			gestionUI.choiceTexts [i].enabled = false;
			gestionUI.buttonChoice [i].SetActive (false);
		}

		gestionUI.conceptArtNarratif.sprite = RandomEvent.Self().endingEvents[eventID].endingSprite;
		gestionUI.contentTextNarra.text = RandomEvent.Self().endingEvents[eventID].endingText;

		gestionUI.EndButton.SetActive (true);
	}


	public void CloseEndingPanel()
	{
		gestionUI.EndButton.SetActive (false);

		gestionUI.fondPanelNarratif.SetActive(false);
		gestionUI.fondNarratif.enabled = false;
		GameManager.SetTimeSpeed (1.0f); 
	}
}
