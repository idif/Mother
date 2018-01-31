using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : SingletonBehaviour<TimeManager> {

[Header("camera")]
	public Transform cameraPos;
	public Transform initPos;
	public Transform sectorChangePos;

	public float timeToSwitch;

[Header("other")]
	public GameObject stormFX;
	public GameObject meteorFx;
	public GameObject supernovaFx;

	public Text textTime;
	public Text endingSector;
	[Header("Close these Panels")]
	public GameObject[] closePanels;

	[Header("")]
	public static float sectorProgress = 0;
	public static int currentStep = 0;

	public int startingDays = 0;//days that were already passed in the first sector

	public static float mins = 0;
	public static float hours = 0;
	public static float days = 0;

	[HideInInspector]public int nbDays = 4;
	float tempDay;
	float progress;
	[HideInInspector]public int sectorTemp = 0;
	public int endGameSector = 6;

	void Start()
	{
		sectorProgress = startingDays * 480;
		SectorSetup (0);

		tempDay = 1.0f;
	}

	void Update () 
	{
		float progress = Time.deltaTime * GameManager.Self().timeSpeed;

		GameManager.Self().time += progress;
		sectorProgress += progress;

		mins = (sectorProgress * 3) % 60;

		hours = (sectorProgress / 20) % 24;

		days = (sectorProgress / 480);

		if (days >= tempDay) 
		{
			sectorTemp++;
			if (sectorTemp >= endGameSector) {
				EndGame ();
				return;
			}
			for (int i = 0; i < closePanels.Length; i++) 
			{
				closePanels [i].SetActive (false);
			}

			endingSector.text = "Sector " + (currentStep + 1).ToString() + " ended. Prepare to go in the sector " + (currentStep + 2).ToString() + ".";
			CameraMove.Self().enabled = false;

			initPos.position = cameraPos.position;
			initPos.rotation = cameraPos.rotation;

			SectorChange ();
			GameManager.SetTimeSpeed (0);
		}

		textTime.text = ((int)hours).ToString ("D2") + " H " + GameManager.RoundToTen (mins).ToString ("D2");
	}

	void SectorChange()
	{
		sectorProgress = 0;
		SectorMenu.Self().OnSectorChanging();
		StartCoroutine(PrepareSectorChange());
		StartCoroutine(SectorMenu.Self().DisplaySectorWarning());

//REINITIALISER LES FXs AVANT DE CHOISIR LE NEXT SECTOR		
		meteorFx.SetActive(false);
		supernovaFx.SetActive(false);
		stormFX.SetActive(false);
	}

	public void NewSectorSetup(int sectorId){
		currentStep++;

		SectorMenu.Self().OnQuitSectorPanel();
		GameManager.SetTimeSpeed (1);
		SectorSetup (sectorId);
	}

	public IEnumerator PrepareSectorChange()
	{
		progress = 0;
		while(cameraPos.position != sectorChangePos.position)
		{
			cameraPos.position = Vector3.Lerp(initPos.position, sectorChangePos.position, progress);
			cameraPos.rotation = Quaternion.Lerp(initPos.rotation, sectorChangePos.rotation, progress);

			progress += Time.deltaTime / timeToSwitch;
			yield return null;
		}
	}

	public IEnumerator ReturnCameraPos(int choice)
	{
		progress = 0;
		while(cameraPos.position != initPos.position)
		{
			cameraPos.position = Vector3.Lerp(sectorChangePos.position, initPos.position, progress);
			cameraPos.rotation = Quaternion.Lerp(sectorChangePos.rotation, initPos.rotation, progress);

			progress += Time.deltaTime / timeToSwitch;
			yield return null;
		}

		NewSectorSetup (choice);
		CameraMove.Self().enabled = true;
	}
		
	void SectorSetup(int sectorId)
	{
		if(sectorId == 0)
		{
			if(SectorMenu.Self().nbCatastropheSector1 == 0)
			{
			}
			
			else if(SectorMenu.Self().nbCatastropheSector1 == 1)
			{
				switch(SectorMenu.Self().catastropheSector1Nb1)
				{
					case 0 : 
						meteorFx.SetActive(true);
						break;
					case 1 :
						stormFX.SetActive(true);
						break;
					case 2 :
						supernovaFx.SetActive(true);
						break;
					default :
						break;
				}
			}
			else if(SectorMenu.Self().nbCatastropheSector1 == 2)
			{
				switch(SectorMenu.Self().catastropheSector1Nb1)
				{
					case 0 : 
						meteorFx.SetActive(true);
						break;
					case 1 :
						stormFX.SetActive(true);
						break;
					case 2 :
						supernovaFx.SetActive(true);
						break;
					default :
						break;
				}
				switch(SectorMenu.Self().catastropheSector1Nb2)
				{
					case 0 : 
						meteorFx.SetActive(true);
						break;
					case 1 :
						stormFX.SetActive(true);
						break;
					case 2 :
						supernovaFx.SetActive(true);
						break;
					default :
						break;
				}		
			}
		}

		if(sectorId == 1)
		{
			if(SectorMenu.Self().nbCatastropheSector2 == 0)
			{
			}
			
			else if(SectorMenu.Self().nbCatastropheSector2 == 1)
			{
				switch(SectorMenu.Self().catastropheSector2Nb1)
				{
					case 0 : 
						meteorFx.SetActive(true);
						break;
					case 1 :
						stormFX.SetActive(true);
						break;
					case 2 :
						supernovaFx.SetActive(true);
						break;
					default :
						break;
				}
			}
			else if(SectorMenu.Self().nbCatastropheSector2 == 2)
			{
				switch(SectorMenu.Self().catastropheSector2Nb1)
				{
					case 0 : 
						meteorFx.SetActive(true);
						break;
					case 1 :
						stormFX.SetActive(true);
						break;
					case 2 :
						supernovaFx.SetActive(true);
						break;
					default :
						break;
				}
				switch(SectorMenu.Self().catastropheSector2Nb2)
				{
					case 0 : 
						meteorFx.SetActive(true);
						break;
					case 1 :
						stormFX.SetActive(true);
						break;
					case 2 :
						supernovaFx.SetActive(true);
						break;
					default :
						break;
				}		
			}	
		}
	}


	public void EndGame()
	{
		//ending of the game;
		UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
	}
}
