using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Animal : MonoBehaviour {
	private float speed; //movement speed
	private Vector3 transformZ;
	private bool isWalking;
	private bool isEating;
	private float eatTimer;
	private GameObject explosion; //Gets declared in the inspector
	private Animator anim;
	private AudioSource audioSourceAnimal;
	// Use this for initialization
	public void StartAnimal () {
		explosion = Resources.Load ("Particles/PlasmaExplosion") as GameObject;
		speed = 3f; //declare the actual speed
		transformZ = new Vector3 (0, 0, 1);
		isWalking = true;
		anim = GetComponent<Animator>();
		anim.SetBool("Walking", true);
		eatTimer = Random.Range (10, 19);
		audioSourceAnimal = GetComponent<AudioSource> ();
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
			if (hit.collider.tag == "Walls" || hit.collider.tag == "Sheep") {
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
		Explode ();
	}
	public void Explode(){

		Instantiate(explosion, transform.position, transform.rotation);
		Destroy(gameObject);
	}
}
