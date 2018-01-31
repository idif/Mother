using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SectorMenu : SingletonBehaviour<SectorMenu> 
{
	public GameObject sectorPanel;
	public float warningDisplayTime = 4.0f;
	public GameObject[] sectorWidgets;
	public GameObject sectorChangeWidget;

	public Sprite[] catastrophes;
	public Sprite noCatastrophe;

	public Image[] catastropheImage;
	[HideInInspector]public int nbCatastropheSector1;
	[HideInInspector]public int catastropheType;
	[HideInInspector]public int nbCatastropheSector2;

	[HideInInspector]public int catastropheSector1Nb1;
	[HideInInspector]public int catastropheSector1Nb2;

	[HideInInspector]public int catastropheSector2Nb1;
	[HideInInspector]public int catastropheSector2Nb2;

	public void OnSectorChanging()
	{
		nbCatastropheSector1 = Random.Range(0, 3);
		switch(nbCatastropheSector1)
		{
			case 0 :
				catastropheImage [0].sprite = noCatastrophe;
				catastropheImage[0].enabled = true;
				break;
			case 1 :
			catastropheSector1Nb1 = Random.Range(0, catastrophes.Length);
			catastropheImage[0].sprite = catastrophes[catastropheSector1Nb1];
				catastropheImage[0].enabled = true;
				break;
			case 2 :
				catastropheSector1Nb1 = Random.Range(0, catastrophes.Length);
				catastropheImage[0].sprite = catastrophes[catastropheSector1Nb1];
				catastropheImage[0].enabled = true;

				catastropheSector1Nb2 = Random.Range(0, catastrophes.Length);
			while(catastropheSector1Nb2 == catastropheSector1Nb1)
				{
				catastropheSector1Nb2 = Random.Range(0, catastrophes.Length);
				}
				catastropheImage[1].sprite = catastrophes[catastropheSector1Nb2];
				catastropheImage[1].enabled = true;
				break;
			default :
				break;
		}

		nbCatastropheSector2 = Random.Range(0, 3);
		while (nbCatastropheSector1 == nbCatastropheSector2) 
		{
			nbCatastropheSector2 = Random.Range(0, 3);
		}

		switch(nbCatastropheSector2)
		{
			case 0 :
				catastropheImage [2].sprite = noCatastrophe;
				catastropheImage[2].enabled = true;
				break;
			case 1 :
				catastropheSector2Nb1 = Random.Range(0, catastrophes.Length);
				while (catastropheSector2Nb1 == catastropheSector1Nb1) 
				{
					catastropheSector2Nb1 = Random.Range(0, catastrophes.Length);
				}
				catastropheImage[2].sprite = catastrophes[catastropheSector2Nb1];
				catastropheImage[2].enabled = true;
				break;
			case 2 :
				catastropheSector2Nb1 = Random.Range(0, catastrophes.Length);
				while (catastropheSector2Nb1 == catastropheSector1Nb1) 
				{
					catastropheSector2Nb1 = Random.Range(0, catastrophes.Length);
				}
				catastropheSector2Nb1 = Random.Range(0, catastrophes.Length);
				catastropheImage[2].sprite = catastrophes[catastropheSector2Nb1];
				catastropheImage[2].enabled = true;
				
				catastropheSector2Nb2 = Random.Range(0, catastrophes.Length);
				while(catastropheSector2Nb2 == catastropheSector2Nb1)
				{
					catastropheSector2Nb2 = Random.Range(0, catastrophes.Length);
				}
				catastropheImage[3].sprite = catastrophes[catastropheSector2Nb2];
				catastropheImage[3].enabled = true;
				break;
			default :
				break;
		}
	}

	public void OnQuitSectorPanel()
	{
		catastropheImage[0].enabled = false;
		catastropheImage[1].enabled = false;

		catastropheImage[2].enabled = false;
		catastropheImage[3].enabled = false;
	}

	public IEnumerator DisplaySectorWarning()
	{
		sectorChangeWidget.SetActive (true);
		yield return new WaitForSeconds(warningDisplayTime);
		sectorChangeWidget.SetActive(false);
		DisplayMenu ();
	}

	void DisplayMenu(){
		sectorPanel.SetActive(true);
	}



//GESTION DES BOUTONS DU PANEL SECTEUR
	public void JustOnMouseOver(int buttonId) 
	{
		sectorWidgets[buttonId].SetActive(true);
	}

	public void JustOnMouseExit(int buttonId)
	{
		sectorWidgets[buttonId].SetActive(false);
	}

	public void OnSectorClicked(int choice)
	{
		TimeManager.Self().StopAllCoroutines();
		StartCoroutine(TimeManager.Self().ReturnCameraPos(choice));
		sectorPanel.SetActive(false);
		sectorWidgets[0].SetActive(false);
		sectorWidgets[1].SetActive(false);
	}
}
