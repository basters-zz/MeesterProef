using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class LoadManager : MonoBehaviour {
	// Use this for initialization
	void Start () {


		LoadData ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void LoadData(){
		if(File.Exists(Application.persistentDataPath + "/SaveFile.bas")){
			BinaryFormatter binary = new BinaryFormatter ();
			FileStream fStream = File.Open (Application.persistentDataPath + "/SaveFile.bas", FileMode.Open);
			SaveManager Save = (SaveManager)binary.Deserialize(fStream);
			fStream.Close ();

		}

	}
}
