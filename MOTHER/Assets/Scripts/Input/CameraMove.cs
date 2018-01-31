using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CameraMove : SingletonBehaviour<CameraMove> {

	public float forwardSpeed = 200.0f;// max speed in Unity distance units per second
	public float sideSpeed = 200.0f;
	public float zoomSpeed = 40.0f;

	float speedPrefFactor = 1;
	bool mouseMove = false;

	public float minZRotation = -50;
	public float maxZRotation = 50;
	public float rotationSpeed = 80.0f;//the maximum speed of mouse-based rotation (not in Unity distance units)

	public float screenMoveMarginX = 5;
	public float screenMoveMarginY = 5;

	public float minSpeedMultiplier = 0.2f;

	float lastMouseX;//the position of the mouse on the X axis of the screen, for the last frame
	float lastMouseY;
	float mouseDifferenceX;
	float mouseDifferenceY;
	float newMouseX;//the position of the mouse on the X axis of the screen, for the current frame
	float newMouseY;

	float screenDownMaxY = 0;
	float screenUpMinY = 0;
	float screenLeftMaxX = 0;
	float screenRightMinX = 0;

	float lastZRotation = 0;
	bool isRotating = false;//wether the mouse middle button is pressed or not, used to rotate the camera around the root

	public static float cameraYLerp;

	Transform rotationPivot;
	Transform positionPivot;
	Transform maxOffset;
	CharacterController controller;

	void Start () {
		GetPrefs ();
		screenMoveMarginX /= 100;
		screenMoveMarginY /= 100;
		UpdateScreenSize ();
		controller = transform.parent.parent.GetComponent<CharacterController> ();
		rotationPivot = transform.parent;
		positionPivot = transform.parent.parent;
		maxOffset = transform.parent.Find ("Max Camera Offset");

		#if UNITY_EDITOR

		if (positionPivot == null) {
			Debug.Log ("ERROR : the gameobject with the CameraMove script isn't set up properly (Position Pivot gameobject missing as parent's parent)");
		}

		if(maxOffset == null){
			Debug.Log ("ERROR : a max offset gameobject is required as a child of the Rotation pivot");
		}
		#endif

		cameraYLerp = Mathf.InverseLerp (0, Vector3.Distance (positionPivot.position, maxOffset.position), Vector3.Distance (positionPivot.position, transform.position));
		zoomSpeed /= Vector3.Distance (positionPivot.position, maxOffset.position);

		Cursor.lockState = CursorLockMode.Confined;
	}
	

	void Update () {

		MouseMoveCheck ();

		if (Input.GetAxis ("Mouse ScrollWheel") != 0 && !EventSystem.current.IsPointerOverGameObject()) {
			Zoom (Input.GetAxis ("Mouse ScrollWheel"));
		}

		if (!GameManager.Self().isTyping) 
		{

			if(Input.GetAxis("Zoom By Keyboard") != 0){
				Zoom (Input.GetAxis("Zoom By Keyboard"));
			}

			if(Input.GetAxis ("Horizontal") != 0){
				HorizontalMove (Input.GetAxis ("Horizontal"));
			}

			if(Input.GetAxis ("Vertical") != 0){
				VerticalMove (Input.GetAxis ("Vertical"));
			}
		}


		if (Input.GetAxis ("MMB") != 0 && !isRotating) {
			isRotating = true;
			lastMouseX = Input.mousePosition.x;
			lastMouseY = Input.mousePosition.y;
		}else if(Input.GetAxis ("MMB") == 0 && isRotating){
			isRotating = false;
		}

		ManageRotation ();

	}

	void Zoom(float input){
		cameraYLerp = Mathf.Clamp01(cameraYLerp - input*zoomSpeed*Time.deltaTime);
		transform.position = Vector3.Lerp(positionPivot.position,maxOffset.position,cameraYLerp);
	}

	void MouseMoveCheck(){
		if (mouseMove) {
			float mouseX = Input.mousePosition.x;
			float mouseY = Input.mousePosition.y;

			if (mouseY > screenUpMinY) {
				VerticalMove (Mathf.InverseLerp (screenUpMinY, Screen.height, mouseY));
			} else if (mouseY < screenDownMaxY) {
				VerticalMove (-Mathf.InverseLerp (screenDownMaxY, 0, mouseY));
			}

			if (mouseX > screenRightMinX) {
				HorizontalMove (Mathf.InverseLerp (screenRightMinX, Screen.width, mouseX));
			} else if (mouseX < screenLeftMaxX) {
				HorizontalMove (-Mathf.InverseLerp (screenLeftMaxX, 0, mouseX));
			}
		}
	}

	void HorizontalMove(float speed){
		controller.Move (controller.transform.forward * forwardSpeed * speedPrefFactor * Time.deltaTime*speed*(minSpeedMultiplier+GameManager.Vector3InverseLerp(positionPivot.position,maxOffset.position,transform.position)));
	}

	void VerticalMove(float speed){
		controller.Move (- controller.transform.right * sideSpeed * speedPrefFactor * Time.deltaTime*speed*(minSpeedMultiplier+GameManager.Vector3InverseLerp(positionPivot.position,maxOffset.position,transform.position)));
	}

	void ManageRotation(){
		if (isRotating) {
			newMouseX = Input.mousePosition.x;
			newMouseY = Input.mousePosition.y;
			mouseDifferenceX = newMouseX - lastMouseX;
			mouseDifferenceY = newMouseY - lastMouseY;
			if (Mathf.Abs(mouseDifferenceX) >= Mathf.Abs(mouseDifferenceY)) {
				positionPivot.RotateAround (rotationPivot.position, Vector3.up, ((mouseDifferenceX) * Time.deltaTime * rotationSpeed));
			} else {
				rotationPivot.RotateAround (rotationPivot.position, rotationPivot.TransformVector(Vector3.forward), ((mouseDifferenceY) * Time.deltaTime * rotationSpeed));
				if (rotationPivot.localEulerAngles.z < maxZRotation && rotationPivot.localEulerAngles.z > minZRotation) {
					lastZRotation = rotationPivot.localEulerAngles.z;
				} else {
					rotationPivot.localEulerAngles = new Vector3 (rotationPivot.localEulerAngles.x,rotationPivot.localEulerAngles.y,lastZRotation);
				}
			}
			lastMouseX = newMouseX;
			lastMouseY = newMouseY;
		}
	}

	public void UpdateScreenSize(){
		screenDownMaxY = Screen.height * screenMoveMarginY;
		screenUpMinY = Screen.height * (1 - screenMoveMarginY);
		screenLeftMaxX = Screen.width * screenMoveMarginX;
		screenRightMinX = Screen.width * (1 - screenMoveMarginX);
	}

	void GetPrefs(){
		#if !UNITY_EDITOR
		if(PlayerPrefs.HasKey("Mouse Move Control")){
			mouseMove = (PlayerPrefs.GetInt ("Mouse Move Control", 0) == 1);
		}
			
		if(PlayerPrefs.HasKey("Mouse Move Control")){
			speedPrefFactor = PlayerPrefs.GetFloat ("Camera Move Sensibility", 1);

		if(speedPrefFactor < 0.5f){
			speedPrefFactor = 1;
		}

		}

		#endif
	}
}
