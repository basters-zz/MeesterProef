using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;


public class SaveManager : MonoBehaviour {
	public void SaveData(List<int> newScore){
		BinaryFormatter binary = new BinaryFormatter ();
		FileStream fStream = File.Create (Application.persistentDataPath + "/SaveFile.bas");

		SavedData SaveContent = new SavedData ();

		SaveContent.HSList = newScore;

		binary.Serialize (fStream, SaveContent);
		fStream.Close ();
		return;
	}
}
