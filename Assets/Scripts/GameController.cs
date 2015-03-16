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

	public ArrayList zombies;
	// Use this for initialization
	void Start () {
		zombies = new ArrayList ();
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

			if (Random.Range (0, 1f) > 0.5f) {
				z = Instantiate (casual) as Casual;
				z.setStartPosition();
			} else {
				z = Instantiate (shambler) as Shambler;
				z.setStartPosition();

			}
			zombies.Add (z);
		}
		/*

		Casual c = Instantiate (casual , new Vector3 (3.5f, 0,3.5f),Quaternion.identity) as Casual;
		Casual c1 = Instantiate (casual , new Vector3 (2.5f, 0,2.5f),Quaternion.identity) as Casual;
		Casual c2 = Instantiate (casual , new Vector3 (1.5f, 0,1.5f),Quaternion.identity) as Casual;

		//Shambler s = Instantiate (shambler , new Vector3 (3.5f, 0,3.5f),Quaternion.identity) as Shambler;
		//Modern m = Instantiate (modern , new Vector3 (29.5f, 0,17.5f),Quaternion.identity) as Modern;

		Phone p = Instantiate (phone, new Vector3 (29.5f, 0, 3.5f), Quaternion.identity) as Phone;
		Phone p1 = Instantiate (phone, new Vector3 (30.5f, 0, 2.5f), Quaternion.identity) as Phone;
		Phone p2 = Instantiate (phone, new Vector3 (31.5f, 0, 1.5f), Quaternion.identity) as Phone;

		zombies.Add (p);
		zombies.Add (p1);
		zombies.Add (p2);
		zombies.Add (c);
		zombies.Add (c1);
		zombies.Add (c2);
*/

	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
