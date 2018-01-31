using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThoughtBubble{
	public GameObject bubbleObject;
	public ChildState linkedState;
}

public class BubbleDisplay : MonoBehaviour {

	private Camera m_Camera;

	public RectTransform m_Canvas;
	public float displayTime = 4.0f;

	private GameObject currentBubble = null;
	private Transform pivot;
	private List<ThoughtBubble> bubbleList = new List<ThoughtBubble>();

	void Start () {
		m_Camera = Camera.main;
		pivot = m_Canvas.parent;
		for (int i = 0; i < m_Canvas.transform.childCount; i++) {
			Transform child =  m_Canvas.transform.GetChild (i);
			ThoughtBubble bubble = new ThoughtBubble();
			bubble.bubbleObject = child.gameObject;
			//bubble.linkedState = GetStateByName (child.name);
			//if (bubble.linkedState != ChildState.None) {
				bubbleList.Add (bubble);
			//}
		}
	}

	void Update () {
		if (currentBubble != null) {
			pivot.LookAt (m_Camera.transform);
		}
	}

	public void DisplayBubble(ChildState state, bool delay = true){
		if (GetBubbleByState (state) != null) {
			StopBubble ();
			StartCoroutine (DisplayManager (state, delay));
		}
	}

	public void StopBubble(){
		if (currentBubble != null) {
			StopAllCoroutines ();
			currentBubble.SetActive (false);
			currentBubble = null;
		}
	}

	IEnumerator DisplayManager(ChildState state, bool delay){
		currentBubble = GetBubbleByState(state);
		if(currentBubble != null){
			currentBubble.SetActive (true);
			if (delay) {
				yield return new WaitForSeconds (displayTime);
				currentBubble.SetActive (false);
				currentBubble = null;
			}
		}
	}

	/*public static ChildState GetStateByName(string name){
		switch (name) {
		case "Hungry":
			return ChildState.GoingToEat;
		case "Eating":
			return ChildState.Eating;
		case "Tired":
			return ChildState.GoingToSleep;
		case "Sleeping":
			return ChildState.Sleeping;
		case "Bored":
			return ChildState.GoingToPlay;
		case "Lonely":
			return ChildState.GoingToChat;
		case "Chatting":
			return ChildState.Chatting;
		default:
			return ChildState.None;
		}
	}*/

	GameObject GetBubbleByState(ChildState state){
		foreach (ThoughtBubble bubble in bubbleList) {
			if (bubble.linkedState == state) {
				return bubble.bubbleObject;
			}
		}
		return null;
	}
}