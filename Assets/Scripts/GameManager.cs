using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	private Vector3 tmp;
	float cycleMins;
	float cycleCalc;
	Transform directionalLight;
	//public bool enemySpawn;
	GameObject score;
	GameObject sheeps;
	GameObject wolves;
	GameObject days;
	GameObject DirectionalLight;
	GameObject mainCam;
	int e;
	int[] sheeps1;
	bool shiftPressed;
	List<GameObject> sheeplist = new List<GameObject>();
	// Use this for initialization
	void Start () {
		directionalLight = GameObject.FindGameObjectWithTag ("Sun").transform;
		cycleMins = 10;
		cycleCalc = 0.1f / cycleMins * 1;

		sheeps = GameObject.Find ("Sheeps");
		score = GameObject.Find ("Score");
		wolves = GameObject.Find ("Wolves");
		days = GameObject.Find ("Days");
		DirectionalLight = GameObject.FindGameObjectWithTag ("Sun");
		mainCam = GameObject.FindGameObjectWithTag ("MainCamera");
		shiftPressed = false;
		sheeplist.AddRange (GameObject.FindGameObjectsWithTag ("Sheep"));
	}
	
	// Update is called once per frame
	void Update () {

		CameraController ();
		CheckSheep ();
		CheckWolves ();
		DayNightCycle ();

		if(Input.GetKeyDown(KeyCode.R)){
			KillSheep ();
		}
		Debug.Log (sheeplist.Count);
		/*foreach(GameObject sheep in sheeplist){
			sheep.GetComponent<SheepController>().
		}*/
	}

	void DayNightCycle(){
		directionalLight.Rotate(cycleCalc, 0, 0);
	}

	void CheckSheep(){
		sheeps.GetComponent<InputField> ().text = "Sheep: " + GameObject.FindGameObjectsWithTag ("Sheep").Length.ToString ();

	}
	void CheckWolves(){
		wolves.GetComponent<InputField> ().text = "Wolves: " + GameObject.FindGameObjectsWithTag ("Wolf").Length.ToString ();
	}
	void KillSheep(){
//		sheeps1 = GameObject.FindGameObjectsWithTag ("Sheep").Length;
		foreach(GameObject sheep in sheeplist){
			e = e + Random.Range (0, 3);
			Debug.Log (e);
		}
		Debug.Log ("e: " + e);
	}

	void CameraController(){
		Vector3 tmpXZ = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
		Vector3 tmpY = new Vector3 (mainCam.transform.position.x, mainCam.transform.position.y, mainCam.transform.position.z);
		if (Input.GetKey (KeyCode.LeftShift)) {
			shiftPressed = true;
		}
		else{
			shiftPressed = false;
		}
		if (Input.GetKey (KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
			if (!shiftPressed) {
				tmpXZ.z += 0.1f;
				transform.position = tmpXZ;
			} else {
				tmpXZ.z += 0.3f;
				transform.position = tmpXZ;
			}
		}
		if (Input.GetKey (KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
			if (!shiftPressed) {
				tmpXZ.x -= 0.1f;
				transform.position = tmpXZ;
			} else {
				tmpXZ.x -= 0.3f;
				transform.position = tmpXZ;
			}
		}
		if (Input.GetKey (KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
			if (!shiftPressed) {
				tmpXZ.x += 0.1f;
				transform.position = tmpXZ;
			} else {
				tmpXZ.x += 0.3f;
				transform.position = tmpXZ;
			}
		}
		if (Input.GetKey (KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
			if (!shiftPressed) {
				tmpXZ.z -= 0.1f;
				transform.position = tmpXZ;
			} else {
				tmpXZ.z -= 0.3f;
				transform.position = tmpXZ;
			}
		}

		if(Input.GetAxis("Mouse ScrollWheel") < 0){
			tmpY.y += 0.5f;
			mainCam.transform.position = tmpY;
		}
		if(Input.GetAxis("Mouse ScrollWheel") > 0){
			tmpY.y -= 0.5f;
			mainCam.transform.position = tmpY;
		}


	}
}
