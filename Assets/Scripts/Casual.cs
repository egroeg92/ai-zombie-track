using UnityEngine;
using System.Collections;

public class Casual : Zombie {

	// Use this for initialization
	new void Start () {
		base.Start ();
		base.setEasy (true);
		name = "casual";
	}
	
	// Update is called once per frame
	new void Update () {
		base.Update ();
		speed = game.speed;
		avoidCollision ();
	}

	protected override void avoidCollision(){
		
		foreach (Zombie z in zombies) {
			if(z == this)
				continue;
			float dist = Vector3.Distance(transform.position, z.transform.position);

			if(dist < 2 && z.lane == lane && z.dir == dir){
				if(transform.position + (dist*forward) == z.transform.position && z.speed <= game.speed)
				{	
					speed = z.speed;
					break;
				}
				speed = game.speed;
			}

		}
	}
}
