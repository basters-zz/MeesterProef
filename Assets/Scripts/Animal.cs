﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Animal : MonoBehaviour {
	private int speed; //movement speed
	private Vector3 transformZ;

	private bool isWalking;
	private bool isAlive;
	private bool isSelected;

	private float eatTimer;
	private float peeTimer;
	private float sleepTimer;
	private float soundTimer;
	private float pukeTimer;

	private int killAmount;

	private Animator anim;

	private AudioSource audioSourceAnimal;

	private int id;

	private GameObject selector;

	// Use this for initialization
	public void StartAnimal () {
		selector = null;
		isAlive = true;
		isSelected = false;
		speed = 3; //declare the actual speed
		peeTimer = Random.Range (20, 40);
		sleepTimer = Random.Range (40, 60);
		soundTimer = Random.Range (14, 40);
		transformZ = new Vector3 (0, 0, 1);
		isWalking = true;
		anim = GetComponent<Animator>();
		anim.SetBool("Walking", true);
		eatTimer = Random.Range (10, 19);
		audioSourceAnimal = GetComponent<AudioSource> ();


	}
	public int ID{
		get{ return  id;}	
		set{ id = value;}
	}
	public bool IsWalking
	{
		get{ return  isWalking;}	
		set{ isWalking = value;}
	}
	public bool IsSelected
	{
		get{ return isSelected;}
		set{ isSelected = value;}
	}
	public AudioSource AudioSourceAnimal
	{
		get{ return audioSourceAnimal;}
		set{ audioSourceAnimal = value;}
	}
	public Animator Anim
	{
		get{ return  anim;}	
		set{ anim = value;}
	}
	public bool IsAlive
	{
		get{ return isAlive;}
		set{ isAlive = value;}

	}
	public GameObject Selector
	{
		get{ return selector;}
		set{ selector = value;}
	}
	public float SleepTimer{
		get{ return sleepTimer;}
		set{ sleepTimer = value;}
	}
	public float PeeTimer{
		get{ return peeTimer;}
		set{ peeTimer = value;}
	}
	public float SoundTimer{
		get{ return soundTimer;}
		set{ soundTimer = value;}
	}
	public float PukeTimer{
		get{ return pukeTimer;}
		set{ pukeTimer = value;}
	}
	public int KillAmount{
		get{ return  killAmount;}	
		set{ killAmount = value;}
	}
	
	// Update is called once per frame
	public void UpdateAnimal () {
		if (isWalking) {
			Walk ();
			eatTimer -= Time.deltaTime;
		}
		if (eatTimer <= 0) {
				StartCoroutine (Eat ());
		}


	}
	//Walking is for every animal the same so the Walk function is put in the Animal script
	void Walk(){
		transform.Translate (transformZ * speed * Time.deltaTime);
		Vector3 fwd = transform.TransformDirection (Vector3.forward);
		RaycastHit hit;

		Debug.DrawRay (transform.position, fwd * 3, Color.red);
		if (Physics.Raycast (transform.position, fwd,out hit, 3)) {
			if (hit.collider.CompareTag("Walls")   || hit.collider.CompareTag("Animal")) {
				int randomint = Random.Range (0, 360);
				transform.Rotate (0, randomint, 0);
			}
		}
	}

	//Walking is for every animal the same so the Walk function is put in the Animal script
	IEnumerator Eat(){
		eatTimer = Random.Range (10, 32);
		isWalking = false;
		anim.SetBool ("Walking", false);
		anim.SetBool("Eat", true);
		yield return new WaitForSecondsRealtime (5f);
		anim.SetBool("Eat", false);
		anim.SetBool("Walking", true);
		isWalking = true;


	}
	void OnMouseDown(){
		isAlive = false;
	}

	IEnumerator Puke(){
		yield return 0;	
	}
	void MakeSound(){
		//
	}
	IEnumerator Sleep(){
		yield return 0;
	}
	IEnumerator Pee(){
		yield return 0;
	}

}
