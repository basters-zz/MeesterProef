using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : Animal {
	private float peeTimer;
	private float sleepTimer;
	private float soundTimer;
	private AudioClip bleatSound;


	void Start(){
		ID = 1;
		StartAnimal ();
		bleatSound = Resources.Load("Audio/SheepBleat")as AudioClip;
		peeTimer = Random.Range (20, 40);
		sleepTimer = Random.Range (40, 60);
		soundTimer = Random.Range (14, 40);
	}
	void Update(){
		UpdateAnimal ();
		if(IsWalking){
			peeTimer -= Time.deltaTime;
			sleepTimer -= Time.deltaTime;
			soundTimer -= Time.deltaTime;
		}
		if(peeTimer <= 0){
			StartCoroutine(Pee ());
		}
		if(sleepTimer <= 0){
			StartCoroutine(Sleep ());
		}
		if(soundTimer <= 0)
		{
			MakeSound ();
		}
		if(Input.GetKeyDown(KeyCode.H)){
			StartCoroutine (Sleep ());
		}
	}



	void MakeSound(){
		AudioSourceAnimal.clip = bleatSound;
		AudioSourceAnimal.Play ();
		soundTimer = Random.Range (14, 40);

	}

	IEnumerator Pee(){


		IsWalking = false;
		Anim.SetBool ("SheepPee", true);
		Anim.SetBool ("Walking", false);

		yield return new WaitForSeconds (1.2f);

		Anim.SetBool ("SheepPee", false);
		Anim.SetBool ("Walking", true);
		peeTimer = Random.Range (20, 40);
		IsWalking = true;
	}
	IEnumerator Sleep(){


		IsWalking = false;
		Anim.SetBool ("SheepSleep", true);
		Anim.SetBool ("Walking", false);

		yield return new WaitForSeconds (10f);

		Anim.SetBool ("SheepWakeUp", true);
		Anim.SetBool ("SheepSleep", false);

		yield return new WaitForSeconds (1.2f);
		Anim.SetBool ("Walking", true);
		Anim.SetBool ("SheepWakeUp", false);
		sleepTimer = Random.Range (40, 60);
		IsWalking = true;
	}

}