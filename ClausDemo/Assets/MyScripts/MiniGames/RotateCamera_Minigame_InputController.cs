using UnityEngine;
using System.Collections;

public class RotateCamera_Minigame_InputController : MonoBehaviour {

	//Defining variables
	RotateCamera_Minigame getGameLogic;
	bool jumpControlStatus;
	bool directionRotStatus;

	// Use this for initialization
	void Start () {
		//Store a reference to the game logic
		getGameLogic = GameObject.Find ("OVRCameraRig").gameObject.GetComponent<RotateCamera_Minigame>();

		//get the initial status of the jumpcontrols and direction controls
		jumpControlStatus = getGameLogic.getControlJump();
		directionRotStatus = getGameLogic.getDirectionRot();

		//pause the game state until a button is pressed to start it up again
		Time.timeScale = 0F;
	}
	
	// Update is called once per frame
	void Update () {
		//Used for debugging purposes
		Debug.Log (jumpControlStatus);
		Debug.Log (directionRotStatus);

		//Get Jump and Rotation status
		jumpControlStatus = getGameLogic.getControlJump();
		directionRotStatus = getGameLogic.getDirectionRot();

		/*For some reason it really matters which way around I increment or decrement the step.
		*I have no idea why but this way will make it work. I'm going to have to read into Quaternion components when I get time
		*So that I can fine tune things like this.
		*The API refrence says "Don't modify this directly unless you know quaternions inside out" for the components
		*So I'll have to leave this for now. Mechanic works though! */
		if((Input.GetAxis("Infra") < 0.5F || Input.GetKey(KeyCode.D)) && jumpControlStatus == true && directionRotStatus == true){
			getGameLogic.rotateTowardsMoon();
		}else{
			if(directionRotStatus == true){
				getGameLogic.rotateAwayMoon();
			}
		}
		
		if((Input.GetAxis("Infra") > 0.5F || Input.GetKey(KeyCode.A)) && jumpControlStatus == true && directionRotStatus == false){
			getGameLogic.rotateTowardsMoon();
		}else{
			if(directionRotStatus == false){
				getGameLogic.rotateAwayMoonNeg();
				//Debug.Log("directionRot == false " + directionRot);
			}
		}
		//Thrust is used to regain control of the camera panning once controlJump takes it away
		if(Input.GetButtonUp("Thrust") || Input.GetKey(KeyCode.W)){
			//jumpControlStatus = true;
			getGameLogic.setControlJump(true);
			Debug.Log("A BUTTON PRESSED");
		}
		//Used to quit back to main menu
		if(Input.GetButtonUp("Start") || Input.GetKey(KeyCode.Escape)){
			//quit();
			Application.LoadLevel(0);
		}

		//Start the game, hide the UI by destroying it as we won't need it again in this scene
		if (Input.anyKey){
			Time.timeScale = 1F;
			Destroy(GameObject.Find("RawImage"));
			Destroy(GameObject.Find("Text"));
			GameObject.Find ("OVRCameraRig").gameObject.GetComponent<RotateCamera_Minigame>().setRayCastOn();
		}

	}
}
