using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class SingleFXTimer : MonoBehaviour {

	ParticleSystem system;

	void Start () {

		system = GetComponent<ParticleSystem> ();
		FxTimer.Self ().AddSystem (system);

		if (!system.main.loop) {
			StartCoroutine (WaitAndDestroy ());
		}
	}

	IEnumerator WaitAndDestroy(){

		yield return new WaitForSeconds (system.main.duration);
		Destroy (gameObject);

	}
	

	void OnDestroy () {
		if (FxTimer.Self () != null) {
			FxTimer.Self ().RemoveSystem (system);
		}
	}
}
