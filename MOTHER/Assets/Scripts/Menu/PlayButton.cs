using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class SpaceShip
{
	public Sprite spaceshipName;
	public Sprite spaceshipInfos;
}

[System.Serializable]
public class Outro
{
	public Sprite outroImage;
	public string outroDescription;
	public string outroName;
}

[System.Serializable]
public class Intro
{
	public Sprite introImage;
	public string introDescription;
	public string introName;
}

[System.Serializable]
public class Bio
{
	public Sprite bioImage;
	public string bioDescription;
	public string bioName;
}

[System.Serializable]
public class Props
{
	public Sprite propImage;
	public string propName;
	public string propDescription;
}


public class PlayButton : MonoBehaviour {
    
    public GameObject mainMenu;
    public GameObject optionMenu;
    public GameObject graphisms;
    public GameObject controls;
    public GameObject credit;
	public GameObject loadingScreen;
	public GameObject archives;
	public Transform loadingBar;
	public GameObject playBtn;

	[Header("Panel Choices")]
	public GameObject selectionChildrenSpaceships;
	public GameObject panelShips;
	public Image shipName;
	public Image shipInfos;
	public GameObject[] selectShips;
	public SpaceShip[] spaceships;

	[Header("Props Panel")]
	public Dropdown propDropdown;
	public GameObject panelProps;

	public Image panelPropImage;
	public Text panelPropDescription;

	public Props[] propArray;

	[Header("Archives")]
	public GameObject defaultText;
	public GameObject panelInfoProps;

	[Header("Biographie")]
	public GameObject panelBio;
	public Bio[] bioArray;
	public Dropdown bioDropdown;
	public Image panelBioImage;
	public Text panelBioText;

	[Header("Outro")]
	public GameObject panelOutro;
	public Outro[] outroArray;
	public Dropdown outroDropdown;
	public Image panelOutroImage;
	public Text panelOutroText;

	[Header("Intro")]
	public GameObject panelIntro;
	public Intro[] introArray;
	public Dropdown introDropdown;
	public Image panelIntroImage;
	public Text panelIntroText;

	GameObject currentArchivePanel;

	AsyncOperation loading;

	private List<GameObject> loadingSlots = new List<GameObject>();
	private float loadingStep;
	int shipNumber = 0;

	void Start(){
//--------------------------------------------------------------------------------//
		List<string> bioNameList = new List<string> ();
		List<string> propNameList = new List<string> ();
		List<string> introNameList = new List<string> ();
		List<string> outroNameList = new List<string> ();

		for (int i = 0; i < bioArray.Length; i++) {
			bioNameList.Add(bioArray[i].bioName);	
		}
		for (int i = 0; i < propArray.Length; i++) {
			propNameList.Add(propArray[i].propName);
		}
		bioDropdown.AddOptions (bioNameList);
		propDropdown.AddOptions (propNameList);

		for (int i = 0; i < introArray.Length; i++) {
			introNameList.Add(introArray[i].introName);	
		}
		for (int i = 0; i < outroArray.Length; i++) {
			outroNameList.Add(outroArray[i].outroName);
		}
		introDropdown.AddOptions (introNameList);
		outroDropdown.AddOptions (outroNameList);
//--------------------------------------------------------------------------------//

		for (int i = 0; i < loadingBar.childCount; i++) {
			loadingSlots.Add(loadingBar.GetChild(i).gameObject);
		}
		loadingStep = 1 / loadingSlots.Count;

		for (int i = 0; i < propArray.Length; i++) {
			propDropdown.options [i].text = propArray [i].propName;
		}

		for (int i = 0; i < bioArray.Length; i++) {
			bioDropdown.options [i].text = bioArray [i].bioName;
		}


		for (int i = 0; i < introArray.Length; i++) {
			introDropdown.options [i].text = introArray [i].introName;
		}

		for (int i = 0; i < outroArray.Length; i++) {
			outroDropdown.options [i].text = outroArray [i].outroName;
		}
	}

    public void NextLevelButton()
    {
		StartCoroutine (LoadingTime ());
    }

	IEnumerator LoadingTime(){
		float nextStep = loadingStep;
		int currentSlot = 0;
		loading =  SceneManager.LoadSceneAsync("Spaceship 1");
		mainMenu.SetActive (false);
		loadingScreen.SetActive (true);
		while (!loading.isDone) {
			if (loading.progress >= nextStep && currentSlot < loadingSlots.Count) {
				loadingSlots [currentSlot].SetActive (true);
				nextStep += loadingStep;
				currentSlot++;
			}
			yield return null;
		}

	}

    public void QuitButton()
    {
    	Application.Quit();
    }

    public void OptionsMenu()
    {
        mainMenu.SetActive(false);
        optionMenu.SetActive(true);
        graphisms.SetActive(false);
        controls.SetActive(false);
    }

    public void GraphMenu()
    {
        optionMenu.SetActive(false);
        graphisms.SetActive(true);
    }

    public void ControlMenu()
    {
        optionMenu.SetActive(false);
        controls.SetActive(true);
    }

    public void Credits()
    {
        mainMenu.SetActive(false);
        credit.SetActive(true);
    }

    public void Main()
    {
        mainMenu.SetActive(true);
        optionMenu.SetActive(false);
        credit.SetActive(false);
    }

	public void Archive()
	{
		mainMenu.SetActive (false);
		archives.SetActive (true);
	}

	public void DropdownProp()
	{
		panelPropImage.sprite = propArray[propDropdown.value].propImage;
		panelPropDescription.text = propArray [propDropdown.value].propDescription;
	}

	public void DropdownBio()
	{
		panelBioImage.sprite = bioArray [bioDropdown.value].bioImage;
		panelBioText.text = bioArray [bioDropdown.value].bioDescription;
	}

	public void DropdownIntro()
	{
		panelIntroImage.sprite = introArray [introDropdown.value].introImage;
		panelIntroText.text = introArray [introDropdown.value].introDescription;
	}

	public void DropdownOutro()
	{
		panelOutroImage.sprite = outroArray [outroDropdown.value].outroImage;
		panelOutroText.text = outroArray [outroDropdown.value].outroDescription;
	}

//-------------------------------------------------------------------------------------//
	public void ReturnChoicesMenu()
	{
		if (panelShips.activeSelf) {
			panelShips.SetActive (false);
		} else {
			mainMenu.SetActive (true);
			selectionChildrenSpaceships.SetActive (false);
		}
	}

	public void SelectionSpaceship()
	{
		for (int i = 0; i < selectShips.Length; i++) 
		{
			selectShips [i].SetActive (false);
		}
		selectShips [shipNumber].SetActive (true);
		panelShips.SetActive (false);
		playBtn.SetActive (true);
	}

	public void SpaceShipInfoManagement(int btn)
	{
		if (btn == 0) {
			shipNumber--;
		} else {
			shipNumber++;
		}

		if(shipNumber < 0){
			shipNumber = spaceships.Length - 1;

		} else if(shipNumber >= spaceships.Length)
		{
			shipNumber = 0;
		}
		shipName.sprite = spaceships [shipNumber].spaceshipName;
		shipInfos.sprite = spaceships [shipNumber].spaceshipInfos;

		shipName.preserveAspect = true;
		shipInfos.preserveAspect = true;

	}

	public void OpenPanelInfoShip(int ship)
	{
		panelShips.SetActive (true);
		shipName.sprite = spaceships [ship].spaceshipName;
		shipInfos.sprite = spaceships [ship].spaceshipInfos;

		shipNumber = ship;

		shipName.preserveAspect = true;
		shipInfos.preserveAspect = true;
	}
//-------------------------------------------------------------------------------------//

	public void GestionArchive(int buttonInt)
	{
		switch (buttonInt) {
		case 0:
			if (!panelInfoProps.activeSelf) {
				panelInfoProps.SetActive (true);

				panelOutro.SetActive (false);
				panelIntro.SetActive (false);
				panelBio.SetActive (false);
				currentArchivePanel = panelInfoProps;
			} else {
				panelInfoProps.SetActive (false);
				currentArchivePanel = null;
			}
			break;
		case 1:
			if (!panelOutro.activeSelf) {
				panelOutro.SetActive (true);

				panelInfoProps.SetActive (false);
				panelIntro.SetActive (false);
				panelBio.SetActive (false);
				currentArchivePanel = panelOutro;
			} else {
				panelOutro.SetActive (false);
				currentArchivePanel = null;
			}
			break;
		case 2:
			if (!panelIntro.activeSelf) {
				panelIntro.SetActive (true);

				panelInfoProps.SetActive (false);
				panelOutro.SetActive (false);
				panelBio.SetActive (false);
				currentArchivePanel = panelIntro;
			} else {
				panelIntro.SetActive (false);
				currentArchivePanel = null;
			}
			break;
		case 3:
			if (!panelBio.activeSelf) {
				panelBio.SetActive (true);

				panelInfoProps.SetActive (false);
				panelOutro.SetActive (false);
				panelIntro.SetActive (false);
				currentArchivePanel = panelBio;
			} else {
				panelBio.SetActive (false);
				currentArchivePanel = null;
			}
			break;
		default:
			break;
		}

		if (currentArchivePanel == null) 
		{
			defaultText.SetActive(true);
		}
	}
}