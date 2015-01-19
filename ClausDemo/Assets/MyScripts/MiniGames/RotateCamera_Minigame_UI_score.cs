using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RotateCamera_Minigame_UI_score : MonoBehaviour {

	RotateCamera_Minigame getGameLogic;
	float score;

	// Use this for initialization
	void Start () {
		getGameLogic = GameObject.Find ("OVRCameraRig").gameObject.GetComponent<RotateCamera_Minigame>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//This is used by the raycasting whenever the appropriote collider is hit
	public void addScore(float x){
		score += x;
		gameObject.GetComponent<Text>().text = score.ToString();

		if (score > 100F) {
			getGameLogic.setGameOverState(); //Game over when the user get 100%
		}
	}

	//Get method
	public float getScore(){
		return score;
	}
}
