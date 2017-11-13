using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : Animal {
	private float peeTimer;
	private float sleepTimer;
	private float pukeTimer;

	void Start(){
		StartAnimal ();
		peeTimer = Random.Range (20, 40);
		sleepTimer = Random.Range (50, 60);
		pukeTimer = Random.Range (41, 49);

	}
	void Update(){
		UpdateAnimal ();
		if(IsWalking){
			peeTimer -= Time.deltaTime;
			sleepTimer -= Time.deltaTime;
			pukeTimer -= Time.deltaTime;
		}
		if(peeTimer <= 0){
			StartCoroutine(PeeWolf ());
		}
		if(sleepTimer <= 0 || Input.GetKeyDown(KeyCode.P)){
			StartCoroutine(SleepWolf ());

		}
		if(pukeTimer <= 0 || Input.GetKeyDown(KeyCode.O)){
			StartCoroutine(PukeWolf ());

		}
	}

	IEnumerator PeeWolf(){


		IsWalking = false;
		Anim.SetBool ("WolfPee", true);
		Anim.SetBool ("Walking", false);

		yield return new WaitForSeconds (2.5f);

		Anim.SetBool ("WolfPee", false);
		Anim.SetBool ("Walking", true);
		peeTimer = Random.Range (20, 40);
		IsWalking = true;
	}
	IEnumerator PukeWolf(){


		IsWalking = false;
		Anim.SetBool ("WolfPuke", true);
		Anim.SetBool ("Walking", false);

		yield return new WaitForSeconds (2.5f);

		Anim.SetBool ("WolfPuke", false);
		Anim.SetBool ("Walking", true);
		pukeTimer = Random.Range (41, 49);
		IsWalking = true;
	}
	IEnumerator SleepWolf(){


		IsWalking = false;
		Anim.SetBool ("WolfSleep", true);
		Anim.SetBool ("Walking", false);

		yield return new WaitForSeconds (10f);

		Anim.SetBool ("WolfWakeUp", true);
		Anim.SetBool ("WolfSleep", false);

		yield return new WaitForSeconds (1.2f);
		Anim.SetBool ("Walking", true);
		Anim.SetBool ("WolfWakeUp", false);
		sleepTimer = Random.Range (50, 60);
		IsWalking = true;
	}
}