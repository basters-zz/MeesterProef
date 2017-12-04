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

	// Use this for initialization
	void Start () {
		highScorePrefab = Resources.Load ("Prefabs/HighScore") as GameObject;
		panel = GameObject.FindGameObjectWithTag ("Panel");
		this.GetComponent<LoadManager> ().LoadData (highScores);
		SortScores ();
	}
	void SortScores(){
		foreach (var score in highScores) {
			GameObject TempPrefab = Instantiate (highScorePrefab, panel.transform);
			TempPrefab.transform.GetChild (0).GetComponent<Text> ().text = score.ToString();
			physicalScores.Add (TempPrefab);
		}
		foreach (var scoreObj in physicalScores) {
			Vector3 tempPos = scoreObj.transform.position;
			tempPos.y -= (physicalScores.IndexOf (scoreObj) * 75);
			scoreObj.transform.position = tempPos;
		}
		return;
	}
	public void ToMainMenu(){
		SceneManager.LoadScene ("MainMenu");
	}
}
