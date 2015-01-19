using UnityEngine;
using System.Collections;

public class MiniGame2GameLogic : MonoBehaviour {

	/*
	 * Cannot implicitly use a Skybox object.
	 * We need to get the mat assigned to the skybox, then change the mat
	 * Not a big deal as the mat is the important part
	 * I don't use Texture as I dont want to change 6 textures every time
	*/
	Material tempSkyLeft;
	Material tempSkyRight;
	public Material Infra1;
	public Material Infra2;
	public Material Infra3;

	public GameObject prefab;

	int skyBoxNum;

	/*
	 * Remember to store both cameras skyboxes.
	 * For the Oculus Rift I would need to handle both cameras seperatly (Left and Right eye)
	 * Need to make sure they are exactly the same, as stated in the best practises doc
	*/
	// Use this for initialization
	void Start () {
		//storing the default skybox
		//tempSkyLeft = GameObject.Find ("LeftEyeAnchor").GetComponent<Skybox>().material;
		tempSkyRight = GameObject.Find ("RightEyeAnchor").GetComponent<Skybox>().material;
		//Debug for testing
		//Debug.Log (tempSkyLeft, tempSkyRight);
	}
	
	// Update is called once per frame
	void Update () {

	}

	//Set method used by the Input class
	public void setSky(int x){
		skyBoxNum = x;
	}

	//This might not be the best way forward, gotta rethink this
	void createSceneObjects(){
		if (skyBoxNum == 1) {
			Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
		}
	}

	//Once the timer reaches zero, pause the game state
	public void setGameOverState(){
		Time.timeScale = 0F;
	}

	//Each Oculus Rift eye is changed to the apporiote skybox
	public void changeSkyBox(){
		if (skyBoxNum == 1) {
			GameObject.Find ("LeftEyeAnchor").GetComponent<Skybox> ().material = Infra1;
			GameObject.Find ("RightEyeAnchor").GetComponent<Skybox> ().material = Infra1;
			createSceneObjects();
		}

		if (skyBoxNum == 2) {
			GameObject.Find ("LeftEyeAnchor").GetComponent<Skybox> ().material = Infra2;
			GameObject.Find ("RightEyeAnchor").GetComponent<Skybox> ().material = Infra2;
		}

		if (skyBoxNum == 3) {
			GameObject.Find ("LeftEyeAnchor").GetComponent<Skybox> ().material = Infra3;
			GameObject.Find ("RightEyeAnchor").GetComponent<Skybox> ().material = Infra3;
		}
	}
}
