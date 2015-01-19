using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RotateCamera_Minigame_UI : MonoBehaviour {

	//Variables to access the games logic
	RotateCamera_Minigame getGameLogic;
	int Counter;

	// Use this for initialization
	//Setting the timers default settings and starting the co-routine countDown
	//This ticks once per second and will b responsable for counting down the timer
	void Start () {
		Counter = 60;
		getGameLogic = GameObject.Find ("OVRCameraRig").gameObject.GetComponent<RotateCamera_Minigame>();
		InvokeRepeating("countDown", 1, 1F);
	}
	
	// Update is called once per frame
	void Update () {
		//Check to see if game over
		if (Counter == 0F) {
			getGameLogic.setGameOverState();
		}
	}

	//Decrements every second and updates the UI element
	void countDown(){
		Counter--;
		gameObject.GetComponent<Text>().text = Counter.ToString();
	}
}
