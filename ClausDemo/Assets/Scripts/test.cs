using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

	// Create a variable to set the max distance of the raycast
	float distance = 25.0f;
	bool gameOver = false;
	bool lookedAtObjU = false;
	bool lookedAtObjD = false;
	bool lookedAtObjL = false;
	bool lookedAtObjR = false;
	float progress = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		// if 'a' key is pressed cast a ray from camera center outward
		if (Input.GetKeyDown("a"))
		{
			Ray rayOrigin = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
			RaycastHit hitinfo;


			if (Physics.Raycast(rayOrigin, out hitinfo, distance))
			{
				// in order to check if the raycast is working and its returning the names
				print ("I'm looking at " + hitinfo.collider.name);


				// after checking that i am hitting something check if its EmptyObjL

				if(hitinfo.collider.name == "EmptyObjL")
				{
					print ("I am looking at empty object left");
					if(lookedAtObjL != true)
					{
						lookedAtObjL = true;
						progress = progress + 25.0f;
					}
				}

				// after checking that i am hitting something check if its EmptyObjR
				
				if(hitinfo.collider.name == "EmptyObjR")
				{
					print ("I am looking at empty object right");
					if(lookedAtObjR != true)
					{
						lookedAtObjR = true;
						progress = progress + 25.0f;
					}
				}

				// after checking that i am hitting something check if its EmptyObjUp
				
				if(hitinfo.collider.name == "EmptyObjUp")
				{
					print ("I am looking at empty object up");
					if(lookedAtObjU != true)
					{
						lookedAtObjU = true;
						progress = progress + 25.0f;
					}
				}

				// after checking that i am hitting something check if its EmptyObjDown
				
				if(hitinfo.collider.name == "EmptyObjDown")
				{
					print ("I am looking at empty object down");
					if(lookedAtObjD != true)
					{
						lookedAtObjD = true;
						progress = progress + 25.0f;
					}
				}



			}
			
			else
			{
				print ("I'm looking at nothing");
			}

			if(progress >= 100)
			{
				gameOver = true;
				print ("game over");
			}


				       
		}
	
	}

	void OnGUI()
	{

		if (progress < 100) 
		{
			GUI.Label (new Rect (80, 80, 200, 100), "Mission Progress " + (int)progress + "%");
		} 
		else 
		{
			GUI.Label(new Rect(80, 80, 100, 100), "Time's Up");
		}
	}
}
