using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class SheepController : MonoBehaviour {
	private float speed; //movement speed
	private Vector3 transformZ;
	private bool isWalking;
	private bool isEating;
	private Animator anim;
	private float eatTimer;
	private float deadSheepTimer;
	public GameObject explosion;
	static bool deadSheep;
	// Use this for initialization
	void Start () {
		speed = 1.5f; //declare the actual speed
		isWalking = true;
		anim = GetComponent<Animator> ();
		eatTimer = Random.Range (10, 25);
		deadSheep = false;
		deadSheepTimer = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
		if (isWalking) {
			Walking ();
			eatTimer -= Time.deltaTime;
		} else {
			anim.SetBool ("IsWalking", false);
		}
		if(eatTimer <= 0){
			isWalking = false;
			isEating = true;
			if (isEating) {
				StartCoroutine (Eating());
			}
		}
		if(deadSheep == true){
			deadSheepTimer -= Time.deltaTime;
			if(deadSheepTimer <= 0){
				deadSheepTimer = 0.5f;
				deadSheep = false;
			}
		}

	}
	void Walking(){
		transform.Translate (transformZ = new Vector3 (0, 0, 2) * speed * Time.deltaTime);
		anim.SetBool("IsWalking", true);
		Vector3 fwd = transform.TransformDirection (Vector3.forward);
		RaycastHit hit;

		Debug.DrawRay (transform.position, fwd * 3, Color.red);
		if (Physics.Raycast (transform.position, fwd, out hit, 3)) {
			if (hit.collider.tag == "Walls" || hit.collider.tag == "Sheep") {
				int randomint = Random.Range (0, 360);
				transform.Rotate (0, randomint, 0);
			}
		}
	}
	IEnumerator Eating(){
		anim.SetBool("IsEating", true);
		yield return new WaitForSecondsRealtime (5f);
		anim.SetBool("IsEating", false);
		isWalking = true;
		eatTimer = Random.Range (10, 32);
	}
	void OnMouseDown(){
		Explode ();
	}
	public void Explode(){

		Instantiate(explosion, transform.position, transform.rotation);
		Destroy(gameObject);
		deadSheep = true;
	}
}
