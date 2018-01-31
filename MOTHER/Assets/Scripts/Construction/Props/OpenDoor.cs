using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : FurnitureBehaviour {

	public GameObject hautPorte;
	public GameObject basPorte;

	private int i = 0;
	private bool doorOpen = false;
	
	public float speed = 5;
	public float hauteur = 3.65f;

	void Start()
	{
		hauteur = hautPorte.transform.position.y;
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Kid" || other.tag == "bot" && i == 0 && doorOpen == false)
		{
			StartCoroutine(openDoor());
			i++;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.tag == "Kid" || other.tag == "bot" && i == 1 && doorOpen == true)
		{
			StartCoroutine(closeDoor());
			i--;
		}
	}

	IEnumerator openDoor()
	{
		while(hautPorte.transform.position.y > -1)
		{
			hautPorte.transform.position += Vector3.down / speed * Time.deltaTime*GameManager.Self().timeSpeed;
			basPorte.transform.position += Vector3.down / speed * Time.deltaTime*GameManager.Self().timeSpeed;
			yield return null;
		}
		doorOpen = true;

	}

	IEnumerator closeDoor()
	{
		while (hautPorte.transform.position.y <= hauteur)
		{
			hautPorte.transform.position -= Vector3.down / speed * Time.deltaTime*GameManager.Self().timeSpeed;
			basPorte.transform.position -= Vector3.down / speed * Time.deltaTime*GameManager.Self().timeSpeed;
			yield return null;
		}
		doorOpen = false;
	}
}
