using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class HighScores : MonoBehaviour {
	GameObject highScore1;
	GameObject highScore2;
	GameObject highScore3;
	// Use this for initialization
	void Start () {
		highScore1 = GameObject.FindGameObjectWithTag ("HS1");
		highScore2 = GameObject.FindGameObjectWithTag ("HS2");
		highScore3 = GameObject.FindGameObjectWithTag ("HS3");
		LoadData ();
	}
		
	public void LoadData(){
		if(File.Exists(Application.persistentDataPath + "/SaveFile.bas")){
			BinaryFormatter binary = new BinaryFormatter ();
			FileStream fStream = File.Open (Application.persistentDataPath + "/SaveFile.bas", FileMode.Open);
			SaveManager Save = (SaveManager)binary.Deserialize(fStream);
			fStream.Close ();

			highScore1.GetComponent<Text> ().text = "NR 1: " + Save.Score1.ToString ();
			highScore2.GetComponent<Text> ().text = "NR 2: " + Save.Score2.ToString ();
			highScore3.GetComponent<Text> ().text = "NR 3: " + Save.Score3.ToString ();
		}

	}

	public void GoToMain(){
		Application.LoadLevel ("MainMenu");
	}
}
