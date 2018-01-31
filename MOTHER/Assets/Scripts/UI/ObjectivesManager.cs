using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class WidgetObjectives
{
	public Image panel;
	public Text missionText;
	public Image imageMission;
	public Text currentValue;
	public Text maxValue;

	[System.NonSerialized]public int currentMissionId = -1;//the id of the currently displayed mission. To use in missionList. -1 means no mission is currently displayed.
}

[System.Serializable]
public class Objective
{
	[System.NonSerialized]public bool isCompleted = false;

	public Sprite missionLogo;

	[System.NonSerialized]public int currentSlot = -1;// 0, 1 or 2 for the corresponding mission display slots : -1 means "not currently displayed"
	[System.NonSerialized]public int currentValue = 0;

	public int requiredValue;
	public string mission;
	public int furnitureUnlockReward = -1;
	public int zakReward = 0;

	public bool IsAvailable(){
		if (!isCompleted && currentSlot < 0) {
			return true;
		} else {
			return false;
		}
	}
}

public class ObjectivesManager : SingletonBehaviour<ObjectivesManager>
{

	public Text TextMeuble;

	public WidgetObjectives[] slotList;

	public Objective[] missionList;

	public Sprite recompenseZackarium;
	public Sprite recompenseLavaLamp;

	void Start () 
	{
		for (int i = 0; i < slotList.Length; i++) {
			MissionPick (i);
		}
	}

	/*public void MissionCompletion(int missionId, int progress){
		
		if (missionList [missionId].currentSlot != -1) {
			missionList [missionId].currentValue += progress;
			slotList [missionList [missionId].currentSlot].currentValue.text = missionList [missionId].currentValue.ToString();

			if (missionList [missionId].currentValue >= missionList [missionId].requiredValue) {

				if (UITaskBar.Self ().objectivesDisplayed) {
					StartCoroutine (UITaskBar.ImageFlasher (slotList [missionList [missionId].currentSlot].panel,new Color(255,255,0)));
				} else {
					StartCoroutine (UITaskBar.ImageFlasher (UITaskBar.Self ().ObjectivesButton.image,new Color(255,255,0)));
				}

				//rewards
				GameManager.AddZakarium(missionList [missionId].zakReward);
				if (missionList [missionId].furnitureUnlockReward > 0) {
					ConstructionScript.Self().UnlockFurniture (missionList [missionId].furnitureUnlockReward);
					StartCoroutine (AffTextGainMeuble (ConstructionScript.Self().furnitureList [missionList [missionId].furnitureUnlockReward].name));
				}

				int formerSlot = missionList [missionId].currentSlot;


				//set the mission as 'done'
				missionList [missionId].isCompleted = true;
				missionList [missionId].currentSlot = -1;


				if (RemainingMissions () > 0) {

					MissionPick (formerSlot);//chosing a new mission

				} else{

					//greying the mission panel if all missions have been done
					slotList [formerSlot].panel.GetComponent<Image> ().color = new Color32 (100, 100, 100, 50);

				}
			}

		}

	}*/

	void MissionPick(int slot){

		if (RemainingMissions () > 0) {

			if (slotList [slot].currentMissionId != -1) {//if a mission is already displayed
				missionList[slotList[slot].currentMissionId].currentSlot = -1;//set the mission slot to 'not slot' (-1)
			}

			int newMissionId = 0;

			do {
				newMissionId = Random.Range (0, missionList.Length);


			} while(!missionList [newMissionId].IsAvailable() || slotList[slot].currentMissionId == newMissionId);

			missionList [newMissionId].currentSlot = slot;
			slotList [slot].currentMissionId = newMissionId;

			slotList [slot].currentValue.text =  missionList [newMissionId].currentValue.ToString();
			slotList [slot].maxValue.text = missionList [newMissionId].requiredValue.ToString();
			slotList [slot].imageMission.sprite = missionList [newMissionId].missionLogo;
			slotList [slot].missionText.text = missionList [newMissionId].mission;

		}
	}

	public int RemainingMissions(){
		int result = 0;
		foreach (Objective mission in missionList) {
			if (mission.IsAvailable()) {
				result++;
			}
		}
		return result;
	}


// Fonction du click bouton (UI)
	public void OnSkipMissionClicked(int slot)
	{

		if (RemainingMissions () > 0) {
			MissionPick (slot);
		}

	}
		


// Fade le texte "le gain des meubles"
	IEnumerator AffTextGainMeuble(string nameFurniture)
	{
		TextMeuble.gameObject.SetActive(true);
		TextMeuble.color = new Color(1, 1, 1, 1);
		TextMeuble.text = "  You unlocked : " +  nameFurniture;
		yield return new WaitForSeconds(4.0f);
		StartCoroutine(FadeTextGainMeuble());	
	}

	IEnumerator FadeTextGainMeuble()
	{	
		while (TextMeuble.color.a > 0)
        {
			TextMeuble.color = new Color(1, 1, 1, TextMeuble.color.a - Time.deltaTime*GameManager.Self().timeSpeed);
            yield return 0;
        }
	}

}		