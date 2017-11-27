using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveManager {
	//private float score1;
	//private float score2;
//	private float score3;
	private List<int> hsList = new List<int>();


	public List<int>HSList{
		get{ return  hsList;}	
		set{ hsList = value;}
	}

/*	public float Score1
	{
		get{ return  score1;}	
		set{ score1 = value;}
	}


	public float Score2
	{
		get{ return  score2;}	
		set{ score2 = value;}
	}


	public float Score3
	{
		get{ return  score3;}	
		set{ score3 = value;}
	}*/
}
