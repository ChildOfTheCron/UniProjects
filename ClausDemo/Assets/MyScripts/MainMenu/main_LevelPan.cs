using UnityEngine;
using System.Collections;

public class main_LevelPan : MonoBehaviour {

	//Creating bools which are used to toggle the camera pan direction
	bool toggleLeftPan;
	bool toggleRightPan;

	// Use this for initialization
	void Start () {
		//On start can pan camera both ways
		toggleLeftPan = true;
		toggleRightPan = true;
	}
	
	// Update is called once per frame
	void Update () {
		//If we can toggle right and the user wants to do so (1F) then pan the camera
		//Same for togle left
		if (Input.GetAxis("Horizontal") == 1F && toggleRightPan == true) {
			transform.Translate (2F, 0, 0);
		}
		if (Input.GetKey(KeyCode.D) && toggleRightPan == true) {
			transform.Translate (2F, 0, 0);
		}
		if (Input.GetAxis("Horizontal") == -1F && toggleLeftPan == true) {
			transform.Translate (-2F, 0, 0);
		}
		if (Input.GetKey(KeyCode.A) && toggleLeftPan == true) {
			transform.Translate (-2F, 0, 0);
		}
	}

	//If the camera enters a collider set up to block left panning we toggle left panning off
	//We do the same for right panning
	void OnTriggerEnter(Collider other) {
		if (other.name == "BlockLeft") {
			toggleLeftPan = false;
		}
		
		if (other.name == "BlockRight") {
			toggleRightPan = false;
		}
	}

	//When we exit the collider that blocks panning, we reset the bools to true
	void OnTriggerExit(Collider other) {
		if (other.name == "BlockLeft") {
			toggleLeftPan = true;
		}
		
		if (other.name == "BlockRight") {
			toggleRightPan = true;
		}
	}
}
