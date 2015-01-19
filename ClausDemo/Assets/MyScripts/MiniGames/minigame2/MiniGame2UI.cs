using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MiniGame2UI : MonoBehaviour {
	
	int Counter;
	MiniGame2GameLogic getGameLogic;
	
	// Use this for initialization
	void Start () {
		//Accessing the game logic
		getGameLogic = GameObject.Find("GameLogicManager").gameObject.GetComponent<MiniGame2GameLogic>();
		Counter = 100;
		InvokeRepeating("countDown", 1, 1F);
	}
	
	// Update is called once per frame
	void Update () {
		if (Counter == 0F) {
			//Below code is buggy as hell - need to fix
			getGameLogic.setGameOverState();
		}
	}

	//Updating the countdown
	void countDown(){
		Counter--;
		gameObject.GetComponent<Text>().text = Counter.ToString();
	}
}
