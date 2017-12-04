using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	bool shiftPressed;
	[SerializeField]
	float speed;
	[SerializeField]
	float fastSpeed;
	[SerializeField]
	float scrollSpeed;
	// Use this for initialization
	void Start () {
		speed =  0.1f;
		fastSpeed = 0.3f;
		scrollSpeed = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void CameraControlls(){
		Vector3 tmpXZ = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
		Vector3 tmpY = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
		if (Input.GetKey (KeyCode.LeftShift)) {
			shiftPressed = true;
		} else {
			shiftPressed = false;
		}
		if ((Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.UpArrow)) && transform.position.z < 397.7f) {
			if (!shiftPressed) {
				tmpXZ.z += speed;
				transform.position = tmpXZ;
			} else 
			{
				tmpXZ.z += fastSpeed;
				transform.position = tmpXZ;
			}
		}
		if ((Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow)) && transform.localPosition.x > 100.8f) {
			if (!shiftPressed) {
				tmpXZ.x -= speed;
				transform.position = tmpXZ;
			}
			else {
				tmpXZ.x -= fastSpeed;
				transform.position = tmpXZ;
			}
		}
		if ((Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow)) && transform.localPosition.x < 389.8f) {
			if (!shiftPressed) {
				tmpXZ.x += speed;
				transform.position = tmpXZ;
			}
			else {
				tmpXZ.x += fastSpeed;
				transform.position = tmpXZ;
			}
		}
		if ((Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.DownArrow)) && transform.localPosition.z > 89.44f) {
			if (!shiftPressed) {
				tmpXZ.z -= speed;
				transform.position = tmpXZ;
			}
			else {
				tmpXZ.z -= fastSpeed;
				transform.position = tmpXZ;
			}
		}

		if ((Input.GetAxis ("Mouse ScrollWheel") < 0) && transform.localPosition.y < 31.3f) {
			tmpY.y += scrollSpeed;
			transform.position = tmpY;
		}
		if ((Input.GetAxis ("Mouse ScrollWheel") > 0) && transform.localPosition.y > 7f) {
			tmpY.y -= scrollSpeed;
			transform.position = tmpY;
		}


	}

}
