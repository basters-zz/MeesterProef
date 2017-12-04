using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Wolf : Animal {

	private float pukeTimer;

	private int killAmount;
	private AudioClip barkSound;
	private AudioClip pukeSound;
	private GameObject PukeSpawn;
	private GameObject PukeParticle;

	void Start(){
		ID = 0;
		StartAnimal ();
		killAmount = Random.Range (0,3);
		pukeSound = Resources.Load("Audio/Puke")as AudioClip;
		PukeSpawn = transform.GetChild(0).gameObject;
		PukeParticle = Resources.Load("Particles/PukeParticle")as GameObject;
		barkSound = Resources.Load("Audio/Bark")as AudioClip;
		PeeTimer = Random.Range (20, 40);
		SleepTimer = Random.Range (50, 60);
		pukeTimer = Random.Range (41, 49);
		SoundTimer = Random.Range (14, 40);

	}
	public int KillAmount{
		get{ return killAmount; }
		set{ killAmount = value; }

	}
	void Update(){
		UpdateAnimal ();
		if(IsWalking){
			PeeTimer -= Time.deltaTime;
			SleepTimer -= Time.deltaTime;
			pukeTimer -= Time.deltaTime;
			SoundTimer -= Time.deltaTime;
		}
		if(PeeTimer <= 0){
			StartCoroutine(Pee ());
		}
		if(SleepTimer <= 0){
			StartCoroutine(Sleep ());

		}
		if(pukeTimer <= 0){
			StartCoroutine(Puke ());

		}
		if(SoundTimer <= 0){
			MakeSound ();
		}
	}

	void MakeSound(){
		AudioSourceAnimal.clip = barkSound;
		AudioSourceAnimal.Play ();
		SoundTimer = Random.Range (14, 40);
	}

	IEnumerator Pee(){
		PeeTimer = Random.Range (20, 40);

		IsWalking = false;

		Anim.SetBool ("Walking", false);
		Anim.SetBool ("WolfPee", true);
		yield return new WaitForSeconds (2.5f);

		Anim.SetBool ("WolfPee", false);
		Anim.SetBool ("Walking", true);

		IsWalking = true;

	}
	IEnumerator Puke(){
		pukeTimer = Random.Range (41, 49);

		IsWalking = false;
		Anim.SetBool ("Walking", false);
		Anim.SetBool ("WolfPuke", true);

		AudioSourceAnimal.clip = pukeSound;
		AudioSourceAnimal.Play ();
		GameObject tempPuke = Instantiate (PukeParticle, PukeSpawn.transform.position, Quaternion.Euler(new Vector3(-264.93f, 0f, 0f)));
		tempPuke.transform.SetParent(transform);
		tempPuke.transform.position = PukeSpawn.transform.position;

		yield return new WaitForSeconds (2.5f);

		Anim.SetBool ("WolfPuke", false);
		Anim.SetBool ("Walking", true);

		IsWalking = true;

	}
	IEnumerator Sleep(){
		SleepTimer = Random.Range (50, 60);

		IsWalking = false;
		Anim.SetBool ("Walking", false);
		Anim.SetBool ("WolfSleep", true);


		yield return new WaitForSeconds (10f);
		Debug.Log ("TEST");
		Anim.SetBool ("WolfSleep", false);
		Anim.SetBool ("WolfWakeUp", true);


		yield return new WaitForSeconds (1.2f);

		Anim.SetBool ("WolfWakeUp", false);
		Anim.SetBool ("Walking", true);

		IsWalking = true;

	}
}