using UnityEngine;
using System.Collections;

public class main_LevelSelect : MonoBehaviour {
	public float rotSpeed = 100.0F;
	int storeLevel;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		/* Can't get raycasting to work accurately on the quads
		RaycastHit hit;
		Ray ray = new Ray(gameObject.transform.position*2, gameObject.transform.forward);
		Debug.DrawRay(transform.position, gameObject.transform.forward*500);
		
		if(Physics.Raycast(ray, out hit, 500, 9)) //Mathf.Infinity
		{
			Debug.Log(hit.collider.name);
		}
	*/
		if(Input.GetButtonUp("Thrust") || Input.GetKey(KeyCode.Return)){
			switch (storeLevel)
			{
			case 1:
				Debug.Log ("Stored level 1 selected");
				Application.LoadLevel(1);
				break;
			case 2:
				Debug.Log ("Stored level 2 selected");
				Application.LoadLevel(2);
				break;
			case 3:
				Debug.Log ("Stored level 3 selected");
				Application.LoadLevel(3);
				break;
			default:
				Debug.Log ("Default reached");;
				break;
			}
			Debug.Log("A BUTTON PRESSED");
		}
	}
		
	void OnTriggerEnter(Collider other) {
		if (other.name == "G1") {
			Debug.Log("G1 hovered");
			storeLevel = 1;
		}
		if (other.name == "G2") {
			Debug.Log("G2 hovered");
			storeLevel = 2;
		}
		if (other.name == "G3") {
			Debug.Log("G3 hovered");
			storeLevel = 3;
		}
		if (other.name == "G4") {
			Debug.Log("G4 hovered");
			storeLevel = 4;
		}
	}
	
}
