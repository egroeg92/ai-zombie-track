using UnityEngine;
using System.Collections;

public class Casual : Zombie {

	// Use this for initialization
	void Start () {
		base.Start ();
		base.setEasy (true);
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
		
	}
}
