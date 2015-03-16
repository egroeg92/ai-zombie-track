using UnityEngine;
using System.Collections;

public class Modern : Zombie {

	// Use this for initialization
	void Start () {

		base.Start ();
		base.setEasy (false);
		speed = 2 * speed;
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
		avoidCollision ();

	}
	void shiftLane(){
		if (base.lane == Lane.INNER) {
			base.shiftLaneUp ();
		} else if (base.lane == Lane.OUTTER) {
			base.shiftLaneDown ();
		} else if (Random.Range (0, 1f) < 0.5f) {
			base.shiftLaneUp ();
		} else {
			base.shiftLaneDown ();
		}
	}
	protected override void avoidCollision(){
		foreach (Zombie z in zombies) {
			if(z == this)
				continue;
			if(Vector3.Distance(transform.position, z.transform.position) < 2.5f && lane == z.lane)
				shiftLane();
			
		}
	}
}
