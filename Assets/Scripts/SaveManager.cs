using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveManager {
	private float score1;
	private float score2;
	private float score3;
	public float Score1
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
	}
}
