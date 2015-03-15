using UnityEngine;
using System.Collections;

public class Modern : Zombie {

	// Use this for initialization
	void Start () {

		base.Start ();
		base.setEasy (false);
	
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();

	}

	protected override void avoidCollision(){
		
	}
}
