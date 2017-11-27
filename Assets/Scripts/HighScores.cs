using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class HighScores : MonoBehaviour {
	GameObject highScorePrefab;
	GameObject panel;
	public List<int> highScores = new List<int>(); 
	public List<GameObject> physicalScores = new List<GameObject>();
	Dictionary<string, GameObject> dictionary;
	// Use this for initialization
	void Start () {
		
		highScorePrefab = Resources.Load ("Prefabs/HighScore") as GameObject;
		panel = GameObject.FindGameObjectWithTag ("Panel");

		LoadData ();
		SortScores ();

	}

	void Update(){
		/*if(Input.GetKeyDown(KeyCode.A)){
			highScores.Add (Random.Range(0, 166));


		}
		if(Input.GetKeyDown(KeyCode.B)){
			highScores.Sort ();
			highScores.Reverse ();

		}*/


	}

	void SortScores(){
		foreach (var score in highScores) {
			GameObject TempPrefab = Instantiate (highScorePrefab, panel.transform);
			TempPrefab.transform.GetChild (0).GetComponent<Text> ().text = score.ToString();
			physicalScores.Add (TempPrefab);
		
		}
		foreach (var scoreObj in physicalScores) {
			Debug.Log(physicalScores.IndexOf (scoreObj));
			Vector3 tempPos = scoreObj.transform.position;
			tempPos.y -= (physicalScores.IndexOf (scoreObj) * 75);
			scoreObj.transform.position = tempPos;
		}
		return;


	}
		
	public void LoadData(){
		if(File.Exists(Application.persistentDataPath + "/SaveFile.bas")){
			BinaryFormatter binary = new BinaryFormatter ();
			FileStream fStream = File.Open (Application.persistentDataPath + "/SaveFile.bas", FileMode.Open);
			SaveManager Save = (SaveManager)binary.Deserialize(fStream);
			fStream.Close ();
			highScores = Save.HSList;
		}

	}

	public void ToMainMenu(){
		SceneManager.LoadScene ("MainMenu");
	}
}
