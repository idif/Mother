using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxTimer : SingletonBehaviour<FxTimer> {

	ParticleSystem[] systems;
	ParticleSystem.MainModule[] systemsMain;

	void Awake () {
		GameManager.Self().OnSpeedChanged += OnTimeSpeedChanged;

		systems = GetComponentsInChildren<ParticleSystem> (true);
		systemsMain = new ParticleSystem.MainModule[systems.Length];

		for (int i = 0; i < systems.Length; i++) {
			systemsMain[i] =  systems [i].main;
		}
	}


	public void AddSystem(ParticleSystem system){

		List<ParticleSystem> systemsTemp = new List<ParticleSystem>(systems);
		List<ParticleSystem.MainModule> mainTemp = new List<ParticleSystem.MainModule> (systemsMain);

		systemsTemp.Add (system);
		mainTemp.Add (system.main);

		systems = systemsTemp.ToArray();
		systemsMain = mainTemp.ToArray();

	}

	public void RemoveSystem(ParticleSystem system){

		List<ParticleSystem> systemsTemp = new List<ParticleSystem>(systems);
		List<ParticleSystem.MainModule> mainTemp = new List<ParticleSystem.MainModule> (systemsMain);

		systemsTemp.Remove (system);
		mainTemp.Remove (system.main);

		systems = systemsTemp.ToArray();
		systemsMain = mainTemp.ToArray();

	}


	void OnTimeSpeedChanged(){
		for (int i = 0; i < systems.Length; i++) {
			systemsMain[i].simulationSpeed = GameManager.Self().timeSpeed;
		}
	}

}
