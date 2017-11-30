using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;


public class SaveManager : MonoBehaviour {
	[SerializeField]
	private int maxHighScores;
	void Start(){
		maxHighScores = 5;

	}

	public void SaveData(List<int> newScore){
		BinaryFormatter binary = new BinaryFormatter ();
		FileStream fStream = File.Create (Application.persistentDataPath + "/SaveFile.bas");

		SavedData SaveContent = new SavedData ();
		newScore.Sort ();
		newScore.Reverse ();
		if (newScore.Count == (maxHighScores + 1)) {
			newScore.RemoveAt (maxHighScores);
		}
		SaveContent.HSList = newScore;
		SaveContent.HSList.Sort ();
		SaveContent.HSList.Reverse ();
		//newScore.AddRange (SaveContent.HSList);
		binary.Serialize (fStream, SaveContent);
		fStream.Close ();
		return;
	}
}
