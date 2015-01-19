using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RotateCamera_Minigame : MonoBehaviour {

	/* Version 0.1 - When attached to the Camera in a scene, will rotate the camera based on a different rotation
	*This different rotation is gathered by taking it from a different game object that I set up in the scene.
	*This allows me to change the target rotation easily and visually */

	/* Version 0.2 - The rotation will now occur on start up but if the left trigger on a pad is selected
	 * the camera will start to pan towards its original rotation */

	/* Version 0.3 - The camera will now spin towards a different objects rotation along two different axis
	 * well the same axis just around different rotations (-step and +step).
	 * In both cases the user can use Left Trigger on the pad to slowly balance to camera back to the center.
	 * Direction will be one direction in one moment and then another after 5 seconds */

	/* Version 0.4 - Replaced IEnumerator timeToTurn() and StartCoroutine(timeToTurn()); and yield waitForSeconds with InvokeRepeating
	 * This is a better solution and is easier to manipulate
	 * Added a boolean toggle to timeToTurn to switch between the two rotations (-Step and +Step) */

	/* Version 0.5 - Added a Jump mechanic. Every 2 seconds the camera will "Jump" in a direction (-Step / +Step)
	 * This will happen regardless of user input.
	 * This is a basic version and the plan is to have the user adjust their input based on the jump boolean.
	 * For now this just takes control away from the player and make the "Out of Control" spin, feel more out of control. */

	/* Version 0.6 - Added a thrust mechanic. This mechanic allows the player to regain control after the Jump mechanic takes place
	 * If the camera "Jump" out of control the player can press A for thrust to regain control and continue rebalancing
	 * Added a secondary trigger (right trigger) to rebalance the camera to the center when rotating to the right.
	 * Now Left Trigger centers from Left rotation and Right Trigger centers from Right Rotation
	 * Refactored code that deals with camera rotation to make better use of the second trigger */

	/* Version 0.7 - Added pause state and basic controls UI. 
	 * On any key game starts, UI gets destroyed
	 * Added Escape functionality. Start button on pads - Escape on keyboard */

	/* Version 0.8 - Added Raycasting when camera is balanced directly on planet.
	 * Added timer, no code hook up yet */

	/* Version 0.9 - I refactored all the Input code into a new Class.
	 * Divided (refactoed) the UI class into two classes, UI_SCore and UI_Time.
	 * Timer works fine now
	 */

	float speed; //Speed of the rotation
	Transform target; //Target rotation (transform really)
	Transform storeOrig;//Used to store camera's original rotation (transform really)

	//Direction toggle (To switch between left and right on axis)
	//Control Jump to "Jump" the camera in a direction determinted by directionRot ever x seconds. User input is ignored after a jump
	bool directionRot = true;
	bool controlJump = true;
	bool raycastOn;

	//Added after refactoring
	RotateCamera_Minigame_UI_score scoreUpdate;

	// Use this for initialization
	void Start () {
		//Initialization of game objects here, to use their rotations in Update
		//Assigning the speed as well
		speed = -60F;
		target = GameObject.Find("Rot1").gameObject.transform; //Placeholder object, could be anything really
		scoreUpdate = GameObject.Find ("scr").gameObject.GetComponent<RotateCamera_Minigame_UI_score>();
		//Can't do "storeOrig = gameObject.transform;" because the rotations won't be correct
		//And trying gameObject.transform.rotation causes issues with the RotateTowards function
		storeOrig = GameObject.Find("ResetRot").gameObject.transform;

		//Initialization of the coroutine and all its required variables
		//Giving the direction a default via directionRot
		directionRot = true;
		controlJump = true;

		//Toggle raycast as off until the game starts
		raycastOn = false;

		//Start up the Repeats!
		InvokeRepeating("timeToTurn", 5, 5F);
		InvokeRepeating("controlJumper", 2, 2F);
	}

	/*Refactoring section
	 *  Adding get/set methods that the new classes need access too
	 */
	public bool getControlJump(){
		return controlJump;
	}
	public void setControlJump(bool x){
		controlJump = x;
	}
	public bool getDirectionRot(){
		return directionRot;
	}
	public void setDirectionRot(bool x){
		directionRot = x;
	}
	//Toggle Raycast on/off
	public void setRayCastOn(){
		raycastOn = true;
	}
	public void setRayCastOff(){
		raycastOn = false;
	}

	//Checks whether or not the user is in control of the camera panning
	void controlJumper(){
		//Adding a boolean toggle
		if(controlJump == true){
			controlJump = false;
		}else{
			controlJump = true;
		}
	}

	//Time left before the camera will spin along the x-axis in the other direction
	void timeToTurn(){
		//Adding a boolean toggle
		if(directionRot == true){
			directionRot = false;
		}else{
			directionRot = true;
		}
	}

	//Callsed by UI_Score or UI_Timer
	//Populates the message and sets the game to a paused state
	public void setGameOverState(){
		GameObject.Find ("vicMsg").gameObject.GetComponent<Text>().text = "Mission Complete!";
		GameObject.Find ("vicMsg2").gameObject.GetComponent<Text>().text = "Press Start or Escape to return to main menu.";
		Time.timeScale = 0F;
	}

	//A generic quit out of game function
	public void quit(){
		Application.LoadLevel(0);
	}

	//This is used for the camera rotation along the x-axis.
	//Not as simple as incrementing along the x-axis, need to rotate towards another object
	public void rotateTowardsMoon(){
		//Frame independant over time - rotates towards target rotation
		float step = speed * Time.deltaTime;
		transform.rotation = Quaternion.RotateTowards(transform.rotation, storeOrig.rotation, -step);
	}

	public void rotateAwayMoon(){
		//Frame independant over time - rotates towards target rotation
		float step = speed * Time.deltaTime;
		transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, step);
	}
	public void rotateAwayMoonNeg(){
		//Frame independant over time - rotates towards target rotation
		float step = speed * Time.deltaTime;
		transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, -step);
	}

	// Update is called once per frame
	void Update () {

		//Used for debugging purposes
		Debug.Log (controlJump);
		Debug.Log (directionRot);

		//The racast that checks to see if the Triton moon has been hit.
		//Updates the score if this is the case
		RaycastHit hit;
		Ray ray = new Ray(gameObject.transform.position*2, gameObject.transform.forward);
		
		Debug.DrawRay(transform.position, gameObject.transform.forward*500);
		
		if(Physics.Raycast(ray, out hit, 500, 9) && raycastOn == true)
		{
			//Debug.Log(hit.collider.name);
			if(hit.collider.name == "pSphere1")
			{
				scoreUpdate.addScore(0.1F);
			}
		}
		//Once the score reaches 100% stop the raycasting
		if (scoreUpdate.getScore () > 100F)
		{
			setRayCastOff();
		}

	}
}
