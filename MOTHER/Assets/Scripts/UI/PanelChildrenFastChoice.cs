using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Children
{
	public ChildBehaviour childB;
	public Text nameOfTheChild;
	public Image imageOfTheChild;
}

[System.Serializable]
public class Informations
{
	public string name;
	public Sprite childImage;
}


public class PanelChildrenFastChoice : MonoBehaviour 
{
	public RectTransform destination;

	public Vector2 posInit = Vector2.zero;
	public Vector2 posMax = Vector2.zero;
	public RectTransform panelChildren;
 	public Button buttonPanelChildren;

 	public Text textBar;

	public float speed = 80;
	public bool isAffich = false;

	public Children[] childChoice;
	public Informations[] infos;


	void Start () 
	{
		posInit = panelChildren.anchoredPosition;
		posMax = destination.anchoredPosition;

		for(int i = 0; i < childChoice.Length; i++)
		{
			childChoice[i].nameOfTheChild.text = infos[i].name;
			childChoice[i].imageOfTheChild.sprite = infos[i].childImage;
		}
	}
	

	public void CallChild(int child) 
	{
		if(ChildBehaviour.selectedCharacter != null)
		{
			ChildBehaviour.selectedCharacter.OnMouseClickElsewhere();
		}
		childChoice[child].childB.SelectCharacter();
	}

	public void OnDestroy()
	{
		StopAllCoroutines();
	}

	public void OpenPanel()
	{
		posMax = destination.anchoredPosition;
		if(!isAffich)
		{
			buttonPanelChildren.interactable = false;
			StartCoroutine(AffWidgetObj());
		}
		else
		{
			buttonPanelChildren.interactable = false;
			StartCoroutine(CacheWidgetObj());
		}
	}

	IEnumerator AffWidgetObj()
	{
		isAffich = true;
		while(panelChildren.anchoredPosition.x >= posMax.x)
		{
			panelChildren.anchoredPosition += Vector2.left / speed;
			yield return null;			
		}

		Vector2 tmp = panelChildren.anchoredPosition;
		tmp.x = posMax.x;
		panelChildren.anchoredPosition = tmp;
		textBar.text = ">";
		buttonPanelChildren.interactable = true;
	}

	IEnumerator CacheWidgetObj()
	{
		isAffich = false;
		while(panelChildren.anchoredPosition.x <= posInit.x)
		{
			panelChildren.anchoredPosition -= Vector2.left / speed;
			yield return null;			
		}

		Vector2 tmp = panelChildren.anchoredPosition;
		tmp.x = posInit.x;
		panelChildren.anchoredPosition = tmp;
		buttonPanelChildren.interactable = true;
		textBar.text = "<";
	}
}
