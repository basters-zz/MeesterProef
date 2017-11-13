using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : Animal {
	private float peeTimer;
	private float sleepTimer;

	void Start(){
		StartAnimal ();
		peeTimer = Random.Range (20, 40);
		sleepTimer = Random.Range (40, 60);
	}
	void Update(){
		UpdateAnimal ();
		if(IsWalking){
			peeTimer -= Time.deltaTime;
			sleepTimer -= Time.deltaTime;
		}
		if(peeTimer <= 0){
			StartCoroutine(PeeSheep ());
		}
		if(sleepTimer <= 0 || Input.GetKeyDown(KeyCode.P)){
			StartCoroutine(SheepSleep ());

		}
	}

	IEnumerator PeeSheep(){


		IsWalking = false;
		Anim.SetBool ("SheepPee", true);
		Anim.SetBool ("Walking", false);

		yield return new WaitForSeconds (1.2f);

		Anim.SetBool ("SheepPee", false);
		Anim.SetBool ("Walking", true);
		peeTimer = Random.Range (20, 40);
		IsWalking = true;
	}
	IEnumerator SheepSleep(){


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