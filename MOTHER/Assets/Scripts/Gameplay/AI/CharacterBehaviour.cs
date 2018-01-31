using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent (typeof(NavMeshAgent))]
public class CharacterBehaviour : MonoBehaviour {

	[HideInInspector]public NavMeshAgent agent;
	protected Vector3 destination;

	public Animator m_Animator;

	public float moveMargin = 1.1f;
	float baseSpeed;
	Vector3 formerVelocity = Vector3.zero;

	public static CharacterBehaviour selectedCharacter;
	public GameObject selectionFx;

	[HideInInspector]public bool isHome = true;
	[HideInInspector]public bool isBot = false;
	[HideInInspector]public bool isWorking = false;
	[HideInInspector]public Transform target;

	protected void Awake()
	{
		agent = GetComponent<NavMeshAgent> ();
		baseSpeed = agent.speed;

		GameManager.Self().OnSpeedChanged += OneSpeedChanged;
	}

	public void OneSpeedChanged(){

		if (GameManager.Self().timeSpeed <= 0) {
			formerVelocity = agent.velocity;
			agent.velocity = Vector3.zero;
			agent.isStopped = true;
		} else {
			agent.isStopped = false;
			ChangeTimeBasedSpeed ();
		}

		if (m_Animator != null) {
			m_Animator.speed = GameManager.Self().timeSpeed;
		}
	}

	protected virtual void OnMouseClickOn()
	{
		if (selectedCharacter != this) 
		{
			SelectCharacter ();
		}
	}

	public virtual void OnMouseClickElsewhere()
	{
		if (ChildPanelManager.current.currentChild == this || ChildPanelManager.current.currentHandy == this) 
		{
			ChildPanelManager.current.ClosePanel ();
		}

		if (selectedCharacter == this) {
			selectedCharacter = null;
		}

		if (selectionFx != null) {
			selectionFx.SetActive (false);
		}
	}

	public void SelectCharacter()
	{
		if (selectedCharacter != null)
		{
			if (selectedCharacter.selectionFx != null)
			{
				selectionFx.SetActive (false);
			}
		}

		selectionFx.SetActive (true);
		selectedCharacter = this;

	}

	void ChangeTimeBasedSpeed(){

		if (agent.velocity.magnitude > 0) {
			formerVelocity = agent.velocity;
		}

		float formerSpeed = agent.speed;

		agent.speed = baseSpeed * GameManager.Self().timeSpeed;
		agent.velocity = (formerVelocity / formerSpeed) * agent.speed;
			
	}

	public Vector3 GetRandomPoint(Vector3 position,float radius){
		NavMeshHit hit;
		NavMesh.SamplePosition (position + Random.insideUnitSphere * radius, out hit, radius, 1);

		while(NavMesh.Raycast(transform.position,hit.position,out hit, NavMesh.AllAreas)){
			NavMesh.SamplePosition (position + Random.insideUnitSphere * radius, out hit, radius, 1);
		}

		return hit.position;
	}

	public bool CheckIfMoving(){
		if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance &&(!agent.hasPath||agent.velocity.sqrMagnitude == 0f)) {
			return false;
		} else {
			return true;
		}
	}
		
}