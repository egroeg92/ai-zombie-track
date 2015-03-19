using UnityEngine;
using System.Collections;

public class Shambler : Zombie {

	float shiftFrequency = .5f;
	float nextShift ;


	// Use this for initialization
	new void Start () {
		base.Start ();
		base.setEasy (true);
		speed = speed / 2;
		nextShift = Time.time + shiftFrequency;

		//InvokeRepeating("shiftLane",0, shiftFrequency);
	}
	
	// Update is called once per frame
	new void Update () {
		base.Update ();

		if (Time.time > nextShift) {
			nextShift = Time.time + shiftFrequency;
			shiftLane ();
		}
		avoidCollision ();

	}

	bool shiftLane(){
		if (base.lane == Lane.INNER) {
			return base.shiftLaneUp ();
		} else if (base.lane == Lane.OUTTER) {
			return base.shiftLaneDown ();
		} else if (Random.Range (0, 1f) < 0.5f) {
			return base.shiftLaneUp ();
		} else {
			return base.shiftLaneDown ();
		}
	}

	protected override void avoidCollision(){
		

		foreach (Zombie z in zombies) {
			if(z == this)
				continue;
			if(Vector3.Distance(transform.position, z.transform.position) < 2.5f && lane == z.lane){
				if(!shiftLane()){

				}
			}
		}
	}
}
