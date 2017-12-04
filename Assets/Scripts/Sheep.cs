using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : Animal {
	
	private AudioClip bleatSound;

	void Start(){
		ID = 1;
		StartAnimal ();
		bleatSound = Resources.Load("Audio/SheepBleat")as AudioClip;
		PeeTimer = Random.Range (20, 40);
		SleepTimer = Random.Range (40, 60);
		SoundTimer = Random.Range (14, 40);
	}
	void Update(){
		UpdateAnimal ();
		if(IsWalking){
			PeeTimer -= Time.deltaTime;
			SleepTimer -= Time.deltaTime;
			SoundTimer -= Time.deltaTime;
		}
		if(PeeTimer <= 0){
			StartCoroutine(Pee ());
		}
		if(SleepTimer <= 0){
			StartCoroutine(Sleep ());
		}
		if(SoundTimer <= 0)
		{
			MakeSound ();
		}
	}



	void MakeSound(){
		AudioSourceAnimal.clip = bleatSound;
		AudioSourceAnimal.Play ();
		SoundTimer = Random.Range (14, 40);

	}

	IEnumerator Pee(){
		PeeTimer = Random.Range (20, 40);
		IsWalking = false;
		Anim.SetBool ("Walking", false);
		Anim.SetBool ("SheepPee", true);
		yield return new WaitForSeconds (1.2f);
		Anim.SetBool ("SheepPee", false);
		Anim.SetBool ("Walking", true);
		IsWalking = true;
	}
	IEnumerator Sleep(){
		SleepTimer = Random.Range (40, 60);
		IsWalking = false;
		Anim.SetBool ("Walking", false);
		Anim.SetBool ("SheepSleep", true);
		yield return new WaitForSeconds (10f);
		Anim.SetBool ("SheepWakeUp", true);
		Anim.SetBool ("SheepSleep", false);
		yield return new WaitForSeconds (1.2f);
		Anim.SetBool ("SheepWakeUp", false);
		Anim.SetBool ("Walking", true);
		IsWalking = true;
	}

}