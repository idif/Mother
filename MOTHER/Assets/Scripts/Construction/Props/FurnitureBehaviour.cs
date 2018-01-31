using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FurnitureBehaviour : MonoBehaviour {

	[System.NonSerialized]public List<RendererData> propRenderers = new List<RendererData>();

	[System.NonSerialized]public List<Behaviour> toDisableOnDemo = new List<Behaviour>();//stuff to disable on the "demo prop", like nav mesh obstacles & scripts
 
	[System.NonSerialized]public bool isActivate = true;

	[System.NonSerialized] public ChildBehaviour currentUser;

    public bool canBeDestroyed = true;
    public bool blockRooms = false;
	public bool canBePlacedInLine = false;
	public bool canBePlacedInWalls = false;

	public string description;

	public IntVector2 size = new IntVector2 (1,1);
	[System.NonSerialized]public List <TileManager> tiles = new List<TileManager>();

	[System.NonSerialized]public int propID;

	public bool snap;


	void Awake(){
        OnAwake ();
		GameManager.GetAllRenderers(gameObject, ref propRenderers);
		 if (GetComponentInChildren<NavMeshObstacle> () != null) {
			toDisableOnDemo.Add (GetComponentInChildren<NavMeshObstacle> ());
		}
		toDisableOnDemo.Add (this as Behaviour);
	}

	protected virtual void OnDestruction(){
		WhenDestroyed ();
	}

	protected virtual void WhenDestroyed(){

	}

	protected virtual void OnAwake(){

	}

	public virtual void OnPowerCut()
	{}

	public void RenderBackToNormal(){

		foreach (RendererData rendererTemp in propRenderers)
		{
			rendererTemp.renderer.material.color = rendererTemp.defaultColor;
		}

	}


	public void RenderWithColor(Color color){

		foreach (RendererData rendererTemp in propRenderers)
		{
			rendererTemp.renderer.material.color = color;
		}

	}


	public void EnableRenderers(bool enable){
		foreach (RendererData data in propRenderers) {
			data.renderer.enabled = enable;
		}
	}

}
