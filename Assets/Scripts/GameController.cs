using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public Casual casual;
	public Shambler shambler;
	public Modern modern;
	public Phone phone;


	public int numberOfZombies;
	public int easyToHardRatio;
	public int respawnProbabilty;
	public float speed;

	public int easies, hards;

	public GameObject  ilt,mlt,olt,
	irt,mrt,ort,
	ilb,mlb,olb,
	irb,mrb,orb;
	// Use this for initialization
	void Start () {
		ilt = GameObject.FindGameObjectWithTag ("innerLT");
		mlt = GameObject.FindGameObjectWithTag ("midLT");
		olt = GameObject.FindGameObjectWithTag ("outterLT");
		
		irt = GameObject.FindGameObjectWithTag ("innerRT");
		mrt = GameObject.FindGameObjectWithTag ("midRT");
		ort = GameObject.FindGameObjectWithTag ("outterRT");
		
		ilb = GameObject.FindGameObjectWithTag ("innerLB");
		mlb = GameObject.FindGameObjectWithTag ("midLB");
		olb = GameObject.FindGameObjectWithTag ("outterLB");
		
		irb = GameObject.FindGameObjectWithTag ("innerRB");
		mrb = GameObject.FindGameObjectWithTag ("midRB");
		orb = GameObject.FindGameObjectWithTag ("outterRB");


		for (int i = 0; i < numberOfZombies; i++) {
			Zombie z;

			if (Random.Range (-1, 1) > 0) {
				z = Instantiate (casual) as Casual;
				z.setStartPosition();
			} else {
				z = Instantiate (shambler) as Shambler;
				z.setStartPosition();

			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
