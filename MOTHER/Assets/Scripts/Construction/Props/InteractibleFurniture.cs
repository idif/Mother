using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractibleFurniture : FurnitureBehaviour 
{

	public int widgetId;

	int widgetLayer = 1 << 5;
	private bool isOver = false;

	[System.NonSerialized]public bool isVisibleWidget = false;

	void Update(){
		if (Input.GetMouseButtonDown (0) && !isOver && !EventSystem.current.IsPointerOverGameObject()) {
			RaycastHit hit;
			Ray mouseRay = Camera.main.ScreenPointToRay (Input.mousePosition);
			if(isVisibleWidget && !Physics.Raycast(mouseRay,out hit,float.PositiveInfinity,widgetLayer)){
				ContextualMenu.Self().CloseWidget ();
			}
		}
	}

	protected virtual void OnMouseClickOn()
	{
		if(!isVisibleWidget && !ConstructionScript.isDestructing)
		{
			if (ChildPanelManager.current.panel.activeSelf) {
				ChildPanelManager.current.ClosePanel ();
			}
			ContextualMenu.Self().OpenWidget (widgetId,gameObject,ConstructionScript.Self().furnitureList[propID].name);
			isVisibleWidget = true;
			gameObject.SendMessage ("OnWidgetOpened",SendMessageOptions.DontRequireReceiver);
		}

	}

	void OnMouseIsOver()
	{
		isOver = true;		
	}


	void OnMouseQuit()
	{
		isOver = false;
	}

	protected override void OnDestruction(){
		if (ContextualMenu.Self ().linkedFurniture == this.gameObject) {
			ContextualMenu.Self ().CloseWidget ();
		}
		WhenDestroyed ();
	}

}
