using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Trajectory{
	public Transform startTransf;
	public Transform destinationTransf;
	[System.NonSerialized] public Vector3 start;
	[System.NonSerialized] public Vector3 destination;
}

public class MovableSpaceShip : MonoBehaviour {

	public float speed = 2;
	public Trajectory[] trajectories;

	int currentTrajectory = 0;
	float progress = 0;

	new Transform transform;

	void Start(){
		transform = gameObject.transform;
		foreach (Trajectory traj in trajectories) {
			traj.start = traj.startTransf.position;
			traj.destination = traj.destinationTransf.position;
		}
	}


	void Update () 
	{
		progress += Time.deltaTime * speed;
		transform.position = Vector3.Lerp (trajectories [currentTrajectory].start, trajectories [currentTrajectory].destination, progress);
		if (progress >= 1) {
			progress = 0;
			if (currentTrajectory >= trajectories.Length - 1) {
				currentTrajectory = 0;
			} else {
				currentTrajectory++;
			}
			transform.LookAt (trajectories [currentTrajectory].destination);
		}
	}
}
