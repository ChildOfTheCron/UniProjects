using UnityEngine;
using System.Collections;

// this isnot really used here in the project just for testing purposes






public class RayCast : MonoBehaviour {
	
	float distance = 25.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		// if l mouse is clicked create a ray cast that originates form 
		// mouse clicked pos.
		
		
		if(Input.GetMouseButtonDown(0))
		{
			Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitinfo;
			
			if (Physics.Raycast(rayOrigin, out hitinfo, distance))
			{
				Debug.Log ("you are casting a ray");
				Debug.DrawLine(rayOrigin.direction, hitinfo.point);
				// check to see if we hit something
				if (hitinfo.collider !=null)
				{
					Debug.Log (hitinfo.transform.tag);
				}
				
			}
		}
	}
}
