using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BabySitterDrone : DroneBehaviour {

	public float minWaitingTime = 4.0f;
	public float maxWaitingTime = 8.0f;
	public float moveRadius = 20.0f;

	public string handyName = "Handy";
	public GameObject myWorkingPanel;
	public Image myProgressBar;

	[HideInInspector]public Transform mySpawner;

	void Start () 
	{
		isBot = true;
		agent.enabled = true; // a enlever lors du passage sur une prochaine version ! Ne pas oublier de réactiver l'agent sur le préfab
		destination = GetRandomPoint (transform.position,moveRadius);
		target = mySpawner;

		UITaskBar.Self ().workHandyPanel = myWorkingPanel;
		UITaskBar.Self ().workProgressImage = myProgressBar;

	}

	protected override void OnMouseClickOn()
	{
		if (selectedCharacter != this) {
			SelectCharacter ();
		} else {
			ChildPanelManager.current.OpenHandyPanel (this);
		}
	}

	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.A)) 
		{
			GoBackHome ();
		}

		if (!isHome && (Vector3.Distance (this.transform.position, target.position) < 2) && !isWorking) 
		{
			StartCoroutine (ReturnHome (11.0f));
		}

		if (isWorking && (Vector3.Distance (this.transform.position, target.position) < 2))
		{
			StartCoroutine(Work(5.0f));
		}
	}

	public IEnumerator ReturnHome(float timeToBack)
	{
		yield return new WaitForSeconds (timeToBack);
		agent.SetDestination(mySpawner.position);
		yield return new WaitForSeconds (5.0f);
	}

	public void GoBackHome()
	{
		isWorking = false;
		UITaskBar.Self().workHandyPanel.SetActive (false);

		StopAllCoroutines ();
		StartCoroutine(ReturnHome (0.0f));
	}

	public IEnumerator Work(float timeToRepair)
	{
		isWorking = false;
		float progress = 0.0f;

		while (progress < timeToRepair)
		{
			UITaskBar.Self().workHandyPanel.SetActive (true);
			UITaskBar.Self().workProgressImage.fillAmount = progress / timeToRepair;
			progress += Time.deltaTime * GameManager.Self().timeSpeed;
			yield return null;
		}

		UITaskBar.Self().workHandyPanel.SetActive (false);
		StartCoroutine (ReturnHome (5.0f));

		if (target.gameObject.GetComponentInChildren<FurnitureBehaviour> () != null) 
		{
			target.gameObject.GetComponentInChildren<FurnitureBehaviour> ().snap = false;
		}
	}
}
