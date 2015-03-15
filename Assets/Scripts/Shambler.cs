using UnityEngine;
using System.Collections;

public class Shambler : Zombie {

	// Use this for initialization
	void Start () {
		base.Start ();
		base.setEasy (true);
		speed = speed / 2;
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
	}
}
