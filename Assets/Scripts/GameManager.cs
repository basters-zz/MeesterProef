using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameManager : MonoBehaviour {
	float cycleMins;
	float cycleCalc;
	[SerializeField]
	float nightCountDown;
	[SerializeField]
	float nightTimer; //How long the night takes
	[SerializeField]
	float dayTimer; // How long it takes to make it day time again

	float totalDayCount;
	float totalDayCountScoreMultiplier;
	float totalSheepCountScore;
	float totalWolfCountScore;
	float totalAnimalCountScore;
	float totalScore;

	Transform directionalLight;
	Vector3 startRotDirectionalLight;

	//Onscreen information
	GameObject score;
	GameObject sheeps;
	GameObject wolves;
	GameObject days;

	//The sun
	GameObject DirectionalLight;

	GameObject mainCam;

	GameObject nightSwitchPanel;
	Color colorNightSwitch;
	[SerializeField]
	bool startDay;
	[SerializeField]
	bool endDay;

	//These variables are used to figure out how many of each animal should be spawned
	float totalAnimalSpawn;
	float totalWolfSpawn;
	float totalSheepSpawn;

	//This boolean checks if shift is pressed so the camera moves faster
	bool shiftPressed;

	//The local position of the spawn area
	private Vector3 centerSpawnArea;

	//The actual size of the spawn area
	private Vector3 sizeSpawnArea;
	//Declares the final position to spawn a animal at
	private Vector3 spawnPosition;



	[SerializeField]
	GameObject sheepPrefab;
	[SerializeField]
	GameObject wolfPrefab;
	// Use this for initialization
	void Start () {
		centerSpawnArea = new Vector3 (241.6f, 0.1f, 260f);
		sizeSpawnArea = new Vector3 (293.31f, 0f, 299.8f);
		totalDayCount = -1;
		totalDayCountScoreMultiplier = 0;
		totalSheepCountScore = 0;
		totalWolfCountScore = 0;
		totalAnimalCountScore = 0;
		directionalLight = GameObject.FindGameObjectWithTag ("Sun").transform;
		cycleMins = 10;
		cycleCalc = 0.1f / cycleMins * 1;
		nightCountDown = 5.0f;
		totalAnimalSpawn = 8;
		nightTimer = 5;
		dayTimer = 5;
		startRotDirectionalLight = new Vector3 (0, -30, -1.525f);
		sheeps = GameObject.Find ("Sheeps");
		score = GameObject.Find ("Score");
		wolves = GameObject.Find ("Wolves");
		days = GameObject.Find ("Days");
		DirectionalLight = GameObject.FindGameObjectWithTag ("Sun");
		nightSwitchPanel = GameObject.FindGameObjectWithTag ("NightPanel");
		startDay = false;
		endDay = false;
		colorNightSwitch = new Color (0,0,0,0);
		mainCam = GameObject.FindGameObjectWithTag ("MainCamera");
		shiftPressed = false;
		SpawnAnimals ();
	}
	
	// Update is called once per frame
	void Update () {
		CameraController ();
		OnScreenDisplay ();
		DayNightSwitch ();
		CalculateScore ();

	}

	void DayNightCycle(){
		directionalLight.Rotate(cycleCalc, 0, 0);
		nightCountDown -= Time.deltaTime;
	}
		
	public void SaveData(){
		BinaryFormatter binary = new BinaryFormatter ();
		FileStream fStream = File.Create (Application.persistentDataPath + "/SaveFile.bas");

		SaveManager Save = new SaveManager ();
		if (totalScore > Save.Score1) {
			Save.Score3 = Save.Score2;
			Save.Score2 = Save.Score1;
			Save.Score1 = totalScore;
		} else if (totalScore > Save.Score2 && totalScore < Save.Score1) {
			Save.Score3 = Save.Score2;
			Save.Score2 = totalScore;
		} else if (totalScore > Save.Score3 && totalScore < Save.Score2) {
			Save.Score3 = totalScore;
		} else {
			//do nothing
		}
		binary.Serialize (fStream, Save);
		fStream.Close ();
	}
		

	void DayNightSwitch(){

		if (nightCountDown > 0f) {
			DayNightCycle ();
			endDay = false;
			if(dayTimer != 5f){
				dayTimer = 5f;
			}if(nightTimer != 5f){
				nightTimer = 5f;
			}
		}
		else if(nightCountDown <= 0f && startDay != true){
			endDay = true;
			nightTimer -= Time.deltaTime;
		}

		if (nightTimer <= 0) {
			
			endDay = false;
			startDay = true;
			dayTimer -= Time.deltaTime;
		}

		//Start making it day again
		if(dayTimer > 0 && startDay == true){


			directionalLight.rotation = Quaternion.Euler(startRotDirectionalLight);

		} else	if(dayTimer <= 0){
			nightCountDown = 300f;
			SpawnAnimals ();
			SaveData ();
			startDay = false;

		}

		if (startDay == true) {
			colorNightSwitch.a -= 0.025f;
			nightSwitchPanel.GetComponent<Image> ().color = colorNightSwitch;
		} else if (endDay == true) {
			colorNightSwitch.a += 0.025f;
			nightSwitchPanel.GetComponent<Image> ().color = colorNightSwitch;
		} 

	}
	//Check how many animals are left and display it on screen
	void OnScreenDisplay(){
		sheeps.GetComponent<InputField> ().text = "Sheep: " + GameObject.FindGameObjectsWithTag ("Sheep").Length.ToString ();
		wolves.GetComponent<InputField> ().text = "Wolves: " + GameObject.FindGameObjectsWithTag ("Wolf").Length.ToString ();
		days.GetComponent<InputField> ().text = "Days: " + totalDayCount.ToString();
		score.GetComponent<InputField> ().text = "Score: " + totalScore.ToString();
	}
	void CalculateScore(){
		totalDayCountScoreMultiplier = totalDayCount * 1.5f; 
		totalSheepCountScore = GameObject.FindGameObjectsWithTag ("Sheep").Length * 4;
		totalWolfCountScore = GameObject.FindGameObjectsWithTag ("Wolf").Length * 2;
		totalAnimalCountScore = totalSheepCountScore - totalWolfCountScore;
		totalScore = totalDayCountScoreMultiplier * totalAnimalCountScore;
	}

	//Spawn animals
	void SpawnAnimals(){
		totalWolfSpawn = totalAnimalSpawn / Random.Range (2, 5);
		totalSheepSpawn = totalAnimalSpawn - totalWolfSpawn;
		totalDayCount += 1;
		totalAnimalSpawn = Mathf.Round(totalAnimalSpawn * 1.25f);

		while (totalSheepSpawn > 0){
			ChangeSpawnPos ();
			SpawnSheep();
			totalSheepSpawn -= 1;
		}
		//return;
		while (totalWolfSpawn > 0){
			ChangeSpawnPos ();
			SpawnWolf();
			totalWolfSpawn -= 1;
		}
		return;

	}
	//Chage spawn position of the animals
	void ChangeSpawnPos(){
		spawnPosition = centerSpawnArea + new Vector3 (Random.Range(-sizeSpawnArea.x / 2, sizeSpawnArea.x / 2),Random.Range(-sizeSpawnArea.y / 2, sizeSpawnArea.y / 2),Random.Range(-sizeSpawnArea.z / 2, sizeSpawnArea.z / 2));
		spawnPosition.y += 3.1f;
	}
	//Spawn a sheep
	public void SpawnSheep(){
		Quaternion tempRot = Quaternion.Euler (0, Random.Range(-1, 360), 0);
		Instantiate (sheepPrefab, spawnPosition, tempRot);
	}
	//Spawn a wolf
	public void SpawnWolf(){
		Quaternion tempRot = Quaternion.Euler (0, Random.Range(-1, 360), 0);
		Instantiate (wolfPrefab, spawnPosition, tempRot);
	}
	


	void CameraController(){
		Vector3 tmpXZ = new Vector3 (mainCam.transform.position.x, mainCam.transform.position.y, mainCam.transform.position.z);
		Vector3 tmpY = new Vector3 (mainCam.transform.position.x, mainCam.transform.position.y, mainCam.transform.position.z);
		if (Input.GetKey (KeyCode.LeftShift)) {
			shiftPressed = true;
		}
		else{
			shiftPressed = false;
		}
		if ((Input.GetKey (KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && mainCam.transform.position.z < 397.7f) {
			if (!shiftPressed) {
				tmpXZ.z += 0.1f;
				mainCam.transform.position = tmpXZ;
			} else {
				tmpXZ.z += 0.3f;
				mainCam.transform.position = tmpXZ;
			}
		}
		if ((Input.GetKey (KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && mainCam.transform.localPosition.z > 100.8f) {
			if (!shiftPressed) {
				tmpXZ.x -= 0.1f;
				mainCam.transform.position = tmpXZ;
			} else {
				tmpXZ.x -= 0.3f;
				mainCam.transform.position = tmpXZ;
			}
		}
		if ((Input.GetKey (KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && mainCam.transform.localPosition.z < 389.8f) {
			if (!shiftPressed) {
				tmpXZ.x += 0.1f;
				mainCam.transform.position = tmpXZ;
			} else {
				tmpXZ.x += 0.3f;
				mainCam.transform.position = tmpXZ;
			}
		}
		if ((Input.GetKey (KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && mainCam.transform.localPosition.z > 89.44f) {
			if (!shiftPressed) {
				tmpXZ.z -= 0.1f;
				mainCam.transform.position = tmpXZ;
			} else {
				tmpXZ.z -= 0.3f;
				mainCam.transform.position = tmpXZ;
			}
		}

		if((Input.GetAxis("Mouse ScrollWheel") < 0) && mainCam.transform.localPosition.y < 31.3f){
			tmpY.y += 0.5f;
			mainCam.transform.position = tmpY;
		}
		if((Input.GetAxis("Mouse ScrollWheel") > 0) && mainCam.transform.localPosition.y > 7f){
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
