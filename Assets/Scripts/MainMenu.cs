﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartGame(){
		SceneManager.LoadScene ("GameScene");
	}
	public void HighScore(){
		SceneManager.LoadScene ("Highscore");
	}
	public void HowToPlay(){
		SceneManager.LoadScene ("HowToPlay");
	}
	public void Credits(){
		SceneManager.LoadScene ("Credits");
	}
	public void QuitGame(){
		Application.Quit();
	}
}
