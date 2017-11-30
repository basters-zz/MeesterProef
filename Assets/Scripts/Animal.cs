using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Animal : MonoBehaviour {
	private int speed; //movement speed
	private Vector3 transformZ;
	private bool isWalking;
	private bool isEating;
	private bool isAlive = true;
	private float eatTimer;
	private float peeTimer;
	private float sleepTimer;
	private float soundTimer;
	private float pukeTimer;
	private int killAmount;
	private Animator anim;
	private AudioSource audioSourceAnimal;
	AnimalList listOfAnimals;
	private int id;
	// Use this for initialization
	public void StartAnimal () {
		killAmount = Random.Range (0,3);
		speed = 3; //declare the actual speed
		peeTimer = Random.Range (20, 40);
		sleepTimer = Random.Range (40, 60);
		soundTimer = Random.Range (14, 40);
		pukeTimer = Random.Range (41, 49);
		transformZ = new Vector3 (0, 0, 1);
		isWalking = true;
		anim = GetComponent<Animator>();
		anim.SetBool("Walking", true);
		eatTimer = Random.Range (10, 19);
		audioSourceAnimal = GetComponent<AudioSource> ();


	}
	/*void AddAnimalToList(){
		listOfAnimals.AllAnimals.Add (this.gameObject);
		if(id == 1){
			listOfAnimals.SheepList.Add (this.gameObject);

		}
		else if(id == 0){
			listOfAnimals.WolfList.Add (this.gameObject);
		}
	}*/
	public int ID{
		get{ return  id;}	
		set{ id = value;}
	}
	public bool IsWalking
	{
		get{ return  isWalking;}	
		set{ isWalking = value;}
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
	
	// Update is called once per frame
	public void UpdateAnimal () {
		if (isWalking) {
			Walk ();
			eatTimer -= Time.deltaTime;
		} else {
			anim.SetBool("Eat", false);
		}
		if(eatTimer <= 0){
			isWalking = false;
			isEating = true;
			if (isEating) {
				StartCoroutine (Eat());
			}
		}

	}
	//Walking is for every animal the same so the Walk function is put in the Animal script
	void Walk(){
		transform.Translate (transformZ * speed * Time.deltaTime);
		Vector3 fwd = transform.TransformDirection (Vector3.forward);
		RaycastHit hit;

		Debug.DrawRay (transform.position, fwd * 3, Color.red);
		if (Physics.Raycast (transform.position, fwd,out hit, 3)) {
			if (hit.collider.CompareTag("Walls")   || hit.collider.CompareTag("Sheep") || hit.collider.CompareTag("Wolf")) {
				int randomint = Random.Range (0, 360);
				transform.Rotate (0, randomint, 0);
			}
		}
	}

	//Walking is for every animal the same so the Walk function is put in the Animal script
	IEnumerator Eat(){
		anim.SetBool ("Walking", false);
		anim.SetBool("Eat", true);
		yield return new WaitForSecondsRealtime (5f);
		anim.SetBool("Eat", false);
		anim.SetBool("Walking", true);
		isWalking = true;
		eatTimer = Random.Range (10, 32);
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
