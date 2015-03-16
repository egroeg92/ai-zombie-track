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
		
		speed = 2 * game.speed;
		avoidCollision ();

	}
	bool shiftLane(){
		bool shifted;
		if (base.lane == Lane.INNER) {
			shifted = base.shiftLaneUp ();
		} else if (base.lane == Lane.OUTTER) {
			shifted = base.shiftLaneDown ();
		} else if (Random.Range (0, 1f) < 0.5f) {
			shifted = base.shiftLaneUp ();
		} else {
			shifted = base.shiftLaneDown ();
		}
		return shifted;
	}
	protected override void avoidCollision(){
		foreach (Zombie z in zombies) {
			if(z == this)
				continue;

			float dist = Vector3.Distance(transform.position, z.transform.position);

			if(dist < 2 && lane == z.lane && (transform.position + (dist*forward) == z.transform.position) )
			{
				if(!shiftLane())
				{
					if( z.dir == dir){
						if( z.speed <= 2 * game.speed)
							speed = z.speed;
					}
				}
			}
		}
	}
}
