using UnityEngine;
using System.Collections;

public class RotateFocalObj : MonoBehaviour {
	//Change here to set rotation speed. 
	// Can be made to change with user input if required for example holding down LT+rotate for faster rotate.
	public float rotSpeed = 100.0F;
	// Use this for initialization

	//Storing the Planets Textures
	Material planetBig;
	Material planetMedium;
	Material planetSmall;

	public Material InfraText;

	void Start () {
		planetBig = GameObject.Find("SphereBig").gameObject.renderer.material;
		planetMedium = GameObject.Find("SphereMed").gameObject.renderer.material;
		planetSmall = GameObject.Find("SphereSmall").gameObject.renderer.material;
	}
	
	// This is where we get user input in real time :)
	void Update () {
		float rotationVert = Input.GetAxis("Vertical") * rotSpeed; //Get Vertical axis (y) input
		float rotationHoz = Input.GetAxis("Horizontal") * rotSpeed; //Get Horizontal axis (x) input
		//We do the below because we don't want to run this every frame per get Input (Update runs per frame)
		//This will scale the movement over time and also make it frame independant
		rotationHoz *= Time.deltaTime;
		rotationVert *= Time.deltaTime;
		//The actual movement of the focal point
		transform.Rotate(rotationVert,rotationHoz, 0);

		if (Input.GetAxis("Infra")>0.5F){
			Debug.Log ("Left Trigger Down");
			//planetBig = InfraText;
			//planetMedium = InfraText;
			//planetSmall = InfraText;
			GameObject.Find("SphereBig").gameObject.renderer.material = InfraText;
			GameObject.Find("SphereMed").gameObject.renderer.material = InfraText;
			GameObject.Find("SphereSmall").gameObject.renderer.material = InfraText;
		}
		else{
			GameObject.Find("SphereBig").gameObject.renderer.material = planetBig;
			GameObject.Find("SphereMed").gameObject.renderer.material = planetMedium;
			GameObject.Find("SphereSmall").gameObject.renderer.material = planetSmall;
			}

		exit ();
	}

	//Quick and dirty way to get to the main menu area
	void exit(){
		if (Input.GetKey(KeyCode.Escape)){
			Application.LoadLevel("Main"); //Load back to main menu (scene)
		}
	}
}