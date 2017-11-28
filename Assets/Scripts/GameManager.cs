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

	private GameObject explosion; 

	List<int> highScoreList = new List<int> ();
	AnimalList ListOfAnimals;
	SaveManager saveManager;
	LoadManager loadManager;
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
		loadManager.LoadData (highScoreList);
		explosion = Resources.Load ("Particles/PlasmaExplosion") as GameObject;
		SpawnAnimals ();
	}
	
	// Update is called once per frame
	void Update () {
		
		OnScreenDisplay ();
		DayNightSwitch ();
		CheckDaySkippable ();
		DefeatChecker ();
		Pause ();
		CheckHighScores ();


		if(gamePaused == false){
			mainCam.GetComponent<CameraController> ().CameraControlls ();
		}


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
	void KillAnimals()
	{
		foreach (var animal in ListOfAnimals.AllAnimals) {
			if(GetComponent<Animal>().IsAlive == true){
				if(GetComponent<Animal>().ID == 0){
					PositivePoints ();
					Instantiate(explosion, animal.transform.position, animal.transform.rotation);
					ListOfAnimals.WolfList.Remove (animal.gameObject);
					ListOfAnimals.AllAnimals.Remove (animal.gameObject);
					Destroy (animal.gameObject);
				}
				else if(GetComponent<Animal>().ID == 1){
					NegativePoints ();
					Instantiate(explosion, animal.transform.position, animal.transform.rotation);
					ListOfAnimals.SheepList.Remove (animal.gameObject);
					ListOfAnimals.AllAnimals.Remove (animal.gameObject);
					Destroy (animal.gameObject);
				}

			}
		}


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
			saveManager.SaveData (highScoreList);
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
		saveManager.SaveData (highScoreList);
		SceneManager.LoadScene ("GameScene");

	}
	public void ToMainMenu(){
		saveManager.SaveData (highScoreList);
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
				saveManager.SaveData (highScoreList);
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
			NegativePoints ();
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
	void PositivePoints(){
		totalScore += 5;

	}

	void NegativePoints(){
		totalScore -= 2;
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
	



	//Build Spawn Area
	void OnDrawGizmosSelected(){
		Gizmos.color = new Color (1,0,0.5f,0.4f);
		Gizmos.DrawCube (centerSpawnArea, sizeSpawnArea);
	}

}
