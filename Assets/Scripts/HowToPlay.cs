using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HowToPlay : MonoBehaviour {
	private GameObject controlsPanel;
	private GameObject rulesPanel;
	private GameObject differencePanel;
	// Use this for initialization
	void Start () {
		controlsPanel = transform.GetChild (0).gameObject;
		rulesPanel = transform.GetChild (1).gameObject;
		differencePanel = transform.GetChild (2).gameObject	;

	}

	public void ActivateRulesPanel(){
		controlsPanel.SetActive (false);
		rulesPanel.SetActive (true);
	}
	public void ActivateDifferencePanel(){
		rulesPanel.SetActive (false);
		differencePanel.SetActive (true);
	}
	public void ToMainMenu(){
		SceneManager.LoadScene ("MainMenu");
	}
}
