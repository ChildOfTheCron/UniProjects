using UnityEngine;
using System.Collections;

public class MiniGame2RayCast : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	/*
	 * A simple raycast, which is used to look at the various objects dotted around the game world.
	*/
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		Ray ray = new Ray(gameObject.transform.position*2, gameObject.transform.forward);
		
		Debug.DrawRay(transform.position, gameObject.transform.forward*500);
		
		if(Physics.Raycast(ray, out hit, 500, 9)) //Mathf.Infinity - don't use
		{
			if(hit.collider.name != null)
			{
				Debug.Log (hit.collider.name);
			}
		}
	}
}
