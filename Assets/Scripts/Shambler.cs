using UnityEngine;
using System.Collections;

public class Shambler : Zombie {

	float shiftFrequency = .5f;
	float nextShift = 0.5f;
	static float shiftProbability = 1f;


	float startTime;
	// Use this for initialization
	void Start () {
		startTime = Time.time;
		base.Start ();
		base.setEasy (true);
		speed = speed / 2;

		//InvokeRepeating("shiftLane",0, shiftFrequency);
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();

		if (Time.time > nextShift) {
			nextShift = Time.time + shiftFrequency;
			shiftLane ();
		}



	}
	void shiftLane(){
		if (base.lane == Lane.INNER)
			base.shiftLaneUp ();
		else if (base.lane == Lane.OUTTER)
			base.shiftLaneDown ();
		else if (Random.Range (0, 1f) < 0.5f)
			base.shiftLaneUp ();
		else
			base.shiftLaneDown ();
	}

	protected override void avoidCollision(){
		
	}
}
