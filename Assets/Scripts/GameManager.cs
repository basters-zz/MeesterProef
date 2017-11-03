using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	private Vector3 tmp;
	float cycleMins;
	float cycleCalc;
	Transform directionalLight;

	//Onscreen information
	GameObject score;
	GameObject sheeps;
	GameObject wolves;
	GameObject days;

	//The sun
	GameObject DirectionalLight;

	GameObject mainCam;

	//These variables are used to figure out how many of each animal should be spawned
	int totalAnimalSpawn;
	int totalWolfSpawn;
	int totalSheepSpawn;

	//This boolean checks if shift is pressed so the camera moves faster
	bool shiftPressed;

	//The local position of the spawn area
	private Vector3 centerSpawnArea;

	//The actual size of the spawn area
	private Vector3 sizeSpawnArea;
	//Declares the final position to spawn a animal at
	private Vector3 spawnPosition;


	List<GameObject> spawnPosList = new List<GameObject>();


	[SerializeField]
	GameObject sheepPrefab;
	// Use this for initialization
	void Start () {
		centerSpawnArea = new Vector3 (241.6f, 0.1f, 260f);
		sizeSpawnArea = new Vector3 (293.31f, 0f, 299.8f);
		directionalLight = GameObject.FindGameObjectWithTag ("Sun").transform;
		cycleMins = 10;
		cycleCalc = 0.1f / cycleMins * 1;
		totalAnimalSpawn = 5;

		sheeps = GameObject.Find ("Sheeps");
		score = GameObject.Find ("Score");
		wolves = GameObject.Find ("Wolves");
		days = GameObject.Find ("Days");
		DirectionalLight = GameObject.FindGameObjectWithTag ("Sun");
		mainCam = GameObject.FindGameObjectWithTag ("MainCamera");
		shiftPressed = false;

		spawnPosList.AddRange (GameObject.FindGameObjectsWithTag ("SpawnPoint"));
		SpawnAnimals ();
	}
	
	// Update is called once per frame
	void Update () {

		CameraController ();
		OnScreenDisplay ();
		DayNightCycle ();

		Debug.Log (spawnPosList.Count);
	}

	void DayNightCycle(){
		directionalLight.Rotate(cycleCalc, 0, 0);
	}
	//Check how many animals are left and display it on screen
	void OnScreenDisplay(){
		sheeps.GetComponent<InputField> ().text = "Sheep: " + GameObject.FindGameObjectsWithTag ("Sheep").Length.ToString ();
		wolves.GetComponent<InputField> ().text = "Wolves: " + GameObject.FindGameObjectsWithTag ("Wolf").Length.ToString ();
	}

	//Spawn animals
	void SpawnAnimals(){
		totalWolfSpawn = totalAnimalSpawn / Random.Range (2, 5);
		totalSheepSpawn = totalAnimalSpawn - totalWolfSpawn;
		while (totalSheepSpawn > 0){
			ChangeSpawnPos ();
			SpawnSheep();
			totalSheepSpawn -= 1;
		}
		return;

	}
	//Chage spawn position of the animals
	void ChangeSpawnPos(){
		spawnPosition = centerSpawnArea + new Vector3 (Random.Range(-sizeSpawnArea.x / 2, sizeSpawnArea.x / 2),Random.Range(-sizeSpawnArea.y / 2, sizeSpawnArea.y / 2),Random.Range(-sizeSpawnArea.z / 2, sizeSpawnArea.z / 2));
	}
	//Spawn a sheep
	public void SpawnSheep(){
		int tempInt = spawnPosList.Count;
		Quaternion tempRot = Quaternion.Euler (0, Random.Range(-1, 360), 0);
		Instantiate (sheepPrefab, spawnPosition, tempRot);
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
			SpawnAnimals ();
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

	//Build Spawn Area
	void OnDrawGizmosSelected(){
		Gizmos.color = new Color (1,0,0.5f,0.4f);
		Gizmos.DrawCube (centerSpawnArea, sizeSpawnArea);
	}
}
