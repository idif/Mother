using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatBubble : MonoBehaviour {

	List<GameObject> chatBubbles = new List<GameObject>();
	GameObject chosenBubble;
	void Awake(){
		for (int i = 0; i < transform.childCount; i++) {
			chatBubbles.Add (transform.GetChild (i).gameObject);
		}
	}

	void OnEnable(){
		if(chatBubbles.Count > 0){
			chosenBubble = chatBubbles [Random.Range (0,chatBubbles.Count - 1)];
			chosenBubble.SetActive (true);
		}
	}

	void OnDisable(){
		if(chosenBubble != null){
			chosenBubble.SetActive (false);
		}
	}
}
