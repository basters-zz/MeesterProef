using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalList : MonoBehaviour {

	List<GameObject> sheepList = new List<GameObject>();
	List<GameObject> wolfList = new List<GameObject>();
	// Use this for initialization
	void Start () {
		
	}

	/*public List<GameObject>AllAnimals
	{
		get{ return  allAnimals;}	
		set{ allAnimals = value;}
	}*/
	public List<GameObject>SheepList
	{
		get{ return  sheepList;}	
		set{ sheepList = value;}
	}
	public List<GameObject>WolfList
	{
		get{ return  wolfList;}	
		set{ wolfList = value;}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
