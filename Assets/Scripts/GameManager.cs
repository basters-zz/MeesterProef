using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	float cycleMins;
	float cycleCalc;
	float nightCountDown;
	float nightTimer; //How long the night takes
	float dayTimer; // How long it takes to make it day time again

	int totalDayCount;
	int totalDayCountScoreMultiplier;
	int totalSheepCountScore;
	int totalWolfCountScore;
	int totalAnimalCountScore;
	int totalScore;

	Transform directionalLight;
	Vector3 startRotDirectionalLight;

	//Onscreen information
	GameObject score;
	GameObject sheeps;
	GameObject wolves;
	GameObject days;
	GameObject deathScreen;
	GameObject pauseScreen;
	GameObject skipDayScreen;

	GameObject mainCam;

	GameObject nightSwitchPanel;
	Color colorNightSwitch;
	bool startDay;
	bool endDay;



	bool gamePaused;

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


	List<int> highScoreList = new List<int> ();

	GameObject sheepPrefab;
	GameObject wolfPrefab;
	// Use this for initialization
	void Start () {
		Time.timeScale = 1;
		sheepPrefab = Resources.Load ("Prefabs/Sheep") as GameObject;
		wolfPrefab = Resources.Load ("Prefabs/Wolf") as GameObject;
		deathScreen = GameObject.FindGameObjectWithTag ("DeathScreen");
		pauseScreen = GameObject.FindGameObjectWithTag ("PauseScreen");
		skipDayScreen = GameObject.FindGameObjectWithTag ("SkipDay");
		deathScreen.SetActive (false);
		pauseScreen.SetActive (false);
		skipDayScreen.SetActive (false);
		centerSpawnArea = new Vector3 (241.6f, 0.1f, 260f);
		sizeSpawnArea = new Vector3 (293.31f, 0f, 299.8f);
		totalDayCount = 1;
		totalDayCountScoreMultiplier = 0;
		totalSheepCountScore = 0;
		totalWolfCountScore = 0;
		totalAnimalCountScore = 0;
		directionalLight = GameObject.FindGameObjectWithTag ("Sun").transform;
		cycleMins = 10;
		cycleCalc = 0.1f / cycleMins * 1;
		nightCountDown = 300f;
		totalAnimalSpawn = 8;
		nightTimer = 5;
		dayTimer = 5;
		startRotDirectionalLight = new Vector3 (0, -30, -1.525f);
		sheeps = GameObject.Find ("Sheeps");
		score = GameObject.Find ("Score");
		wolves = GameObject.Find ("Wolves");
		days = GameObject.Find ("Days");
		nightSwitchPanel = GameObject.FindGameObjectWithTag ("NightPanel");
		gamePaused = false;
		startDay = false;
		endDay = false;
		colorNightSwitch = new Color (0,0,0,0);
		mainCam = GameObject.FindGameObjectWithTag ("MainCamera");
		shiftPressed = false;
		LoadData ();
		SpawnAnimals ();
	}
	
	// Update is called once per frame
	void Update () {
		CameraController ();
		OnScreenDisplay ();
		DayNightSwitch ();
		CalculateScore ();
		CheckDaySkippable ();
		DefeatChecker ();
		Pause ();
		CheckHighScores ();





		//Temp score Testers
		if(Input.GetKeyDown(KeyCode.F)){
			highScoreList.Add (Random.Range(0, 166));


		}
		if(Input.GetKeyDown(KeyCode.G)){
			highScoreList.Sort ();
			highScoreList.Reverse ();

		}
		//




	}

	void CheckDaySkippable(){
		List<GameObject> wolvesAlive = new List<GameObject>();
		wolvesAlive.AddRange(GameObject.FindGameObjectsWithTag("Wolf"));
		if (wolvesAlive.Count == 0 && !endDay && !startDay) {
			skipDayScreen.SetActive (true);
		} else {
			skipDayScreen.SetActive (false);
		}
	}
		

	void DefeatChecker(){
		List<GameObject> wolvesAlive = new List<GameObject>();
		wolvesAlive.AddRange(GameObject.FindGameObjectsWithTag("Wolf"));

		List<GameObject> sheepsAlive = new List<GameObject>();
		sheepsAlive.AddRange(GameObject.FindGameObjectsWithTag("Sheep"));

		if(wolvesAlive.Count > sheepsAlive.Count && Mathf.Min(wolvesAlive.Count, sheepsAlive.Count) > 25 || sheepsAlive.Count <= 0){
			SaveData ();
			gamePaused = true;
			Time.timeScale = 0;
			AnimalClickable ();
			deathScreen.SetActive (enabled);

		}

	}

	void DayTimeCalculator(){
		List<GameObject> wolvesAlive = new List<GameObject>();
		wolvesAlive.AddRange(GameObject.FindGameObjectsWithTag("Wolf"));
		float extraSeconds = 0;
		float extraMinutesCycle = 0;
		foreach (GameObject LivingWolf in wolvesAlive) {
			extraSeconds += 4;
			extraMinutesCycle += 0.133f;
		}
		cycleMins = 10f + extraMinutesCycle;
		nightCountDown = 300 + extraSeconds;
		return;
	}


	public void ResumeGame(){
		pauseScreen.SetActive (false);
		gamePaused = false;
		AnimalClickable ();
		Time.timeScale = 1;
	}
	void AnimalClickable(){
		List<GameObject> wolvesAlive = new List<GameObject>();
		wolvesAlive.AddRange(GameObject.FindGameObjectsWithTag("Wolf"));

		List<GameObject> sheepsAlive = new List<GameObject>();
		sheepsAlive.AddRange(GameObject.FindGameObjectsWithTag("Sheep"));
		if (gamePaused) {
			foreach (GameObject LivingSheep in sheepsAlive) {
				LivingSheep.GetComponent<BoxCollider> ().enabled = false;
			}
			foreach (GameObject LivingWolf in wolvesAlive) {
				LivingWolf.GetComponent<BoxCollider> ().enabled = false;
			}
		}
		if (!gamePaused) {
			foreach (GameObject LivingSheep in sheepsAlive) {
				LivingSheep.GetComponent<BoxCollider> ().enabled = true;
			}
			foreach (GameObject LivingWolf in wolvesAlive) {
				LivingWolf.GetComponent<BoxCollider> ().enabled = true;
			}
		}
		return;
	}

	void Pause(){
		if(Input.GetKeyDown(KeyCode.Escape)){
			pauseScreen.SetActive(true);
			gamePaused = true;
			AnimalClickable ();
			Time.timeScale = 0;

		}
	}

	public void Restart(){
		SaveData ();
		SceneManager.LoadScene ("GameScene");

	}
	public void ToMainMenu(){
		SaveData ();
		Time.timeScale = 1;
		SceneManager.LoadScene ("MainMenu");
	}
	void DayNightCycle(){
		if (!gamePaused) {
			directionalLight.Rotate (cycleCalc, 0, 0);
			nightCountDown -= Time.deltaTime;
		}
	}

	void CheckHighScores(){

		foreach (var score in highScoreList) {
			if (totalScore > score) {
				highScoreList.Add (totalScore);
				highScoreList.Sort ();
				highScoreList.Reverse ();
				highScoreList.RemoveAt (5);
				return;
			} else {
				//Continue Checking
			}
		}
	}

	public void LoadData(){
		if (File.Exists (Application.persistentDataPath + "/SaveFile.bas")) {
			BinaryFormatter binary = new BinaryFormatter ();
			FileStream fStream = File.Open (Application.persistentDataPath + "/SaveFile.bas", FileMode.Open);
			SaveManager Save = (SaveManager)binary.Deserialize (fStream);
			fStream.Close ();


			highScoreList = Save.HSList;
		} else {


		}

	}
		
	public void SaveData(){
		BinaryFormatter binary = new BinaryFormatter ();
		FileStream fStream = File.Create (Application.persistentDataPath + "/SaveFile.bas");

		SaveManager Save = new SaveManager ();

		Save.HSList = highScoreList;

		binary.Serialize (fStream, Save);
		fStream.Close ();
	}
	public void SkipDay(){
		nightCountDown = 0;
	}

	void DayNightSwitch(){
		if (!gamePaused) {
			if (nightCountDown > 0f) {
				DayNightCycle ();
				endDay = false;
				if (dayTimer != 5f) {
					dayTimer = 5f;
				}
				if (nightTimer != 5f) {
					nightTimer = 5f;
				}
			} else if (nightCountDown <= 0f && startDay != true) {
				endDay = true;
				nightTimer -= Time.deltaTime;
			}

			if (nightTimer <= 0) {
			
				endDay = false;
				startDay = true;
				dayTimer -= Time.deltaTime;
			}

			//Start making it day again
			if (dayTimer > 0 && startDay == true) {


				directionalLight.rotation = Quaternion.Euler (startRotDirectionalLight);

			} else if (dayTimer <= 0) {
				SaveData ();
				totalDayCount += 1;
				RemoveSheep ();
				DefeatChecker ();
				DayTimeCalculator ();
				SpawnAnimals ();
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

	}

	void RemoveSheep(){
		List<GameObject> wolvesAlive = new List<GameObject>();
		wolvesAlive.AddRange(GameObject.FindGameObjectsWithTag("Wolf"));

		List<GameObject> sheepsAlive = new List<GameObject>();
		sheepsAlive.AddRange(GameObject.FindGameObjectsWithTag("Sheep"));

		int sheepsToKill = 0;

		foreach(GameObject WolfAlive in wolvesAlive){
			sheepsToKill += WolfAlive.GetComponent<Wolf>().KillAmount;
		}

		while(sheepsToKill > 0){
			if(sheepsAlive.Count >= 1){
			Destroy (GameObject.FindGameObjectWithTag("Sheep"));
			}
			sheepsToKill -= 1;
		}

		return;


	}
	//Check how many animals are left and display it on screen
	void OnScreenDisplay(){
		sheeps.GetComponent<InputField> ().text = "Sheep: " + GameObject.FindGameObjectsWithTag ("Sheep").Length.ToString ();
		wolves.GetComponent<InputField> ().text = "Wolves: " + GameObject.FindGameObjectsWithTag ("Wolf").Length.ToString ();
		days.GetComponent<InputField> ().text = "Days: " + totalDayCount.ToString();
		score.GetComponent<InputField> ().text = "Score: " + totalScore.ToString();
	}
	void CalculateScore(){
	/*	totalDayCountScoreMultiplier = totalDayCount * 1.5f; 
		totalSheepCountScore = GameObject.FindGameObjectsWithTag ("Sheep").Length * 4;
		totalWolfCountScore = GameObject.FindGameObjectsWithTag ("Wolf").Length * 2;
		totalAnimalCountScore = totalSheepCountScore - totalWolfCountScore;
		totalScore = totalDayCountScoreMultiplier * totalAnimalCountScore;*/

	}

	//Spawn animals
	void SpawnAnimals(){
		totalWolfSpawn = totalAnimalSpawn / Random.Range (2, 5);
		totalSheepSpawn = totalAnimalSpawn - totalWolfSpawn;
		totalAnimalSpawn = Mathf.Round(totalAnimalSpawn * 1.25f);

		while (totalSheepSpawn > 0){
			ChangeSpawnPos ();
			SpawnSheep();
			totalSheepSpawn -= 1;
		}
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
		if (!gamePaused) {
			if (Input.GetKey (KeyCode.LeftShift)) {
				shiftPressed = true;
			} else {
				shiftPressed = false;
			}
			if ((Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.UpArrow)) && mainCam.transform.position.z < 397.7f) {
				if (!shiftPressed) {
					tmpXZ.z += 0.1f;
					mainCam.transform.position = tmpXZ;
				} else {
					tmpXZ.z += 0.3f;
					mainCam.transform.position = tmpXZ;
				}
			}
			if ((Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow)) && mainCam.transform.localPosition.x > 100.8f) {
				if (!shiftPressed) {
					tmpXZ.x -= 0.1f;
					mainCam.transform.position = tmpXZ;
				} else {
					tmpXZ.x -= 0.3f;
					mainCam.transform.position = tmpXZ;
				}
			}
			if ((Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow)) && mainCam.transform.localPosition.x < 389.8f) {
				if (!shiftPressed) {
					tmpXZ.x += 0.1f;
					mainCam.transform.position = tmpXZ;
				} else {
					tmpXZ.x += 0.3f;
					mainCam.transform.position = tmpXZ;
				}
			}
			if ((Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.DownArrow)) && mainCam.transform.localPosition.z > 89.44f) {
				if (!shiftPressed) {
					tmpXZ.z -= 0.1f;
					mainCam.transform.position = tmpXZ;
				} else {
					tmpXZ.z -= 0.3f;
					mainCam.transform.position = tmpXZ;
				}
			}

			if ((Input.GetAxis ("Mouse ScrollWheel") < 0) && mainCam.transform.localPosition.y < 31.3f) {
				tmpY.y += 0.5f;
				mainCam.transform.position = tmpY;
			}
			if ((Input.GetAxis ("Mouse ScrollWheel") > 0) && mainCam.transform.localPosition.y > 7f) {
				tmpY.y -= 0.5f;
				mainCam.transform.position = tmpY;
			}
		}

	}

	//Build Spawn Area
	void OnDrawGizmosSelected(){
		Gizmos.color = new Color (1,0,0.5f,0.4f);
		Gizmos.DrawCube (centerSpawnArea, sizeSpawnArea);
	}

}
