using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SavedData {

	private List<int> hsList = new List<int>();


	public List<int>HSList{
		get{ return  hsList;}	
		set{ hsList = value;}
	}

}
