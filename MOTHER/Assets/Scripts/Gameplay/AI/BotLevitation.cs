using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotLevitation : MonoBehaviour {

	public float amplitude = 2.5f;

	Transform m_transform;
	float baseY;

	void Start () {
		m_transform = transform;
		baseY = m_transform.position.y;
	}

	void Update () {
		m_transform.position = new Vector3 (transform.position.x, baseY + Mathf.Cos(GameManager.Self().time)*amplitude, transform.position.z);
	}
}
