using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChildPanelManager : MonoBehaviour {

	[System.Serializable]
	public class SkillSlot{
		public RectTransform fillImage;
		public GameObject completeImage;
	}

	[System.Serializable]
	public class SkillBar{
		public SkillSlot[] slots = new SkillSlot[5];

	}

	float needMaxWidth;
	float skillMaxWidth;

	[Header("Handys")]
	public Text handyTextName;	
	public GameObject panelHandy;

	[System.NonSerialized]public BabySitterDrone currentHandy;

	[Header("Children")]
	public RectTransform[] needBars;
	public SkillBar[] skillBars;
	public Text characterName;
	public Text traitText;
	public GameObject panel;

	[System.NonSerialized]public ChildBehaviour currentChild;

	public static ChildPanelManager current;


	void Awake () {
		current = this;
		needMaxWidth = needBars [0].anchorMax.x;
		skillMaxWidth = skillBars [0].slots [0].fillImage.anchorMax.x;
		foreach (SkillBar bar in skillBars) {
			foreach (SkillSlot slot in bar.slots) {
				slot.fillImage.anchorMax = new Vector2 (0, slot.fillImage.anchorMax.y);
				slot.completeImage.SetActive (false);
			}
		}
	}

	public void OpenCharacterPanel (ChildBehaviour characterScript) {
		currentChild = characterScript;
		panel.SetActive (true);
		characterName.text = currentChild.childName;
		traitText.text = currentChild.traits[0].ToString ();
		ResetSkillBars ();
		UpdateData ();
	}

	public void OpenHandyPanel(BabySitterDrone handyScript){
		currentHandy = handyScript;
		panelHandy.SetActive (true);
		handyTextName.text = currentHandy.handyName;
	}

	public void StartTyping()
	{
		GameManager.Self().isTyping = !GameManager.Self().isTyping;
	}

	public void ReturnHomeHandy()
	{
		currentHandy.GoBackHome ();
	}

	public void ChangeHandyName(Text handyNewName)
	{
		if (currentHandy != null) 
		{
			currentHandy.handyName = handyNewName.text;
			handyTextName.text = currentHandy.handyName;
		}
	}

	void Update(){
		if (panel.activeSelf) {
			UpdateData ();
		}
	}

	public void ClosePanel(){
		panel.SetActive (false);
		panelHandy.SetActive (false);

		if (currentChild != null) {
			currentChild.selectionFx.SetActive (false);
			currentChild = null;
		}

		if (currentHandy != null) 
		{
			currentHandy.selectionFx.SetActive (false);
			currentHandy = null;
		}
	}

	void UpdateData(){
		if (currentChild != null){
			UpdateNeedBar (currentChild.needs.hunger, 0);
			UpdateNeedBar (currentChild.needs.sleep, 1);
			UpdateNeedBar (currentChild.needs.fun, 2);

			UpdateSkillBar (currentChild.skills.charisma, 0);
			UpdateSkillBar (currentChild.skills.intelligence, 1);
			UpdateSkillBar (currentChild.skills.strength, 2);
		}
	}

	void UpdateNeedBar(Need need,int barID){
		needBars [barID].anchorMax = new Vector2 (needMaxWidth * need.currentLevel,needBars[barID].anchorMax.y);
	}

	void UpdateSkillBar(Skill skill,int barID){
		for (int i = 0; i < skillBars[barID].slots.Length;i++) {
			if (!skillBars[barID].slots[i].completeImage.activeSelf) {
				
				if(skill.skillLevel > i){
					skillBars[barID].slots[i].completeImage.SetActive(true);
					skillBars[barID].slots[i].fillImage.anchorMax = new Vector2 (skillMaxWidth,skillBars[barID].slots[i].fillImage.anchorMax.y);
				}
				else if(skill.skillLevel == i){
					skillBars[barID].slots[i].fillImage.anchorMax = new Vector2 (skillMaxWidth * skill.currentLevelProgress,skillBars[barID].slots[i].fillImage.anchorMax.y);
				}
			}
		}
	}

	void ResetSkillBars(){
		foreach (SkillBar bar in skillBars) {
			foreach (SkillSlot slot in bar.slots) {
				slot.completeImage.SetActive (false);
				slot.fillImage.anchorMax = new Vector2 (0, slot.fillImage.anchorMax.y);
			}
		}
	}

}
