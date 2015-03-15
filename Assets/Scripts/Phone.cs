using UnityEngine;
using System.Collections;

public class Phone : Zombie {

	// Use this for initialization
	void Start () {
		base.Start ();
		base.setEasy (false);
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();

	}
}
