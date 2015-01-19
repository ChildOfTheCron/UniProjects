using UnityEngine;
using System.Collections;

public class MiniGame2Input : MonoBehaviour {

	//Required to access the minigame's logic
	MiniGame2GameLogic getGameLogic;

	// Use this for initialization
	void Start () {
		getGameLogic = GameObject.Find("GameLogicManager").gameObject.GetComponent<MiniGame2GameLogic>();

	}
	
	// Update is called once per frame
	// Depending on the button pressed change the skybox and set which game elements should load into the game
	void Update () {
		if (Input.GetButtonUp ("Thrust") || Input.GetKey (KeyCode.Q)) {
			getGameLogic.setSky(1);
			getGameLogic.changeSkyBox();
		}

		if (Input.GetButtonUp ("Thrust") || Input.GetKey (KeyCode.W)) {
			getGameLogic.setSky(2);
			getGameLogic.changeSkyBox();
		}

		if (Input.GetButtonUp ("Thrust") || Input.GetKey (KeyCode.E)) {
			getGameLogic.setSky(3);
			getGameLogic.changeSkyBox();
		}
	}
}
