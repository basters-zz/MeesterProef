using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Wolf : Animal {
	private float peeTimer;
	private float sleepTimer;
	private float pukeTimer;
	private float barkTimer;
	private int killAmount;
	private AudioClip barkSound;
	private AudioClip pukeSound;
	private GameObject PukeSpawn;
	private GameObject PukeParticle;

	void Start(){
		StartAnimal ();
		killAmount = Random.Range (0,3);
		pukeSound = Resources.Load("Audio/Puke")as AudioClip;
		PukeSpawn = transform.GetChild(0).gameObject;
		PukeParticle = Resources.Load("Particles/PukeParticle")as GameObject;
		barkSound = Resources.Load("Audio/Bark")as AudioClip;
		peeTimer = Random.Range (20, 40);
		sleepTimer = Random.Range (50, 60);
		pukeTimer = Random.Range (41, 49);
		barkTimer = Random.Range (14, 40);

	}
	public int KillAmount{
		get{ return killAmount; }
		set{ killAmount = value; }

	}
	void Update(){
		UpdateAnimal ();
		if(IsWalking){
			peeTimer -= Time.deltaTime;
			sleepTimer -= Time.deltaTime;
			pukeTimer -= Time.deltaTime;
			barkTimer -= Time.deltaTime;
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
		if(barkTimer <= 0 || Input.GetKeyDown(KeyCode.B)){
			BarkWolf ();
		}
	}

	void BarkWolf(){
		AudioSourceAnimal.clip = barkSound;
		AudioSourceAnimal.Play ();
		barkTimer = Random.Range (14, 40);
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