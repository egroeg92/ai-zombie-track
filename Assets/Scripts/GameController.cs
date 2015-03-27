using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public Casual casual;
	public Shambler shambler;
	public Modern modern;
	public Phone phone;
	public Survivor survivor;

	
	public Survivor2 s;


	public int numberOfZombies;
	public float hardToEasyRatio;
	public int respawnProbabilty;
	public float speed;

	public float easies, hards;
	public float forwardRange, sideRange,backRange;

	public GameObject  ilt,mlt,olt,
	irt,mrt,ort,
	ilb,mlb,olb,
	irb,mrb,orb;

	public GameObject[] golds;
	public GameObject goal;
	public GameObject start;


	public ArrayList zombies;
	public bool seen = false;
	ArrayList startPositions;

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

		golds = GameObject.FindGameObjectsWithTag ("gold");
		goal = GameObject.FindGameObjectWithTag ("goal");
		start = GameObject.FindGameObjectWithTag ("start");

		survivor = Instantiate (survivor, start.transform.position, Quaternion.identity) as Survivor;
		//s = Instantiate (s, start.transform.position, Quaternion.identity) as Survivor2;


		startPositions = new ArrayList();
		for (int i = 0; i < numberOfZombies; i++) {
			Zombie z;

			if (Random.Range (0, 1f) > 0.5f) {
				z = Instantiate (casual,Vector3.zero,Quaternion.identity) as Casual;
				while(!setStartPosition(z));
				//z.name = "cas";
			} else {
				z = Instantiate (shambler,Vector3.zero,Quaternion.identity) as Shambler;
				while(!setStartPosition(z));
				//z.name = "shamb";
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool setStartPosition(Zombie zombie){

		Lane lane = (Lane)Random.Range ((int)Lane.INNER, (int)Lane.OUTTER + 1);
		
		float x = ilb.transform.position.x;
		float y = ilb.transform.position.z;
		float xRange = Vector3.Distance(ilb.transform.position , irb.transform.position);
		float yRange = Vector3.Distance(ilb.transform.position, ilt.transform.position);
		
		if (lane == Lane.MID) {
			x = mlb.transform.position.x;
			y = mlb.transform.position.z;
			xRange = Vector3.Distance(mlb.transform.position , mrb.transform.position);
			yRange = Vector3.Distance(mlb.transform.position , mlt.transform.position);
		} else if (lane == Lane.OUTTER) {
			x = olb.transform.position.x;
			y = olb.transform.position.z;
			xRange = Vector3.Distance(olb.transform.position , orb.transform.position);
			yRange =Vector3.Distance (olb.transform.position , olt.transform.position);
			
		}
		
		Side side = (Side)Random.Range ((int)Side.LEFT, (int) (int)Side.TOP +1);
		while(side==Side.RIGHT)
			side = (Side)Random.Range ((int)Side.LEFT, (int) (int)Side.TOP +1);

		zombie.lane = lane;
		zombie.side = side;
		zombie.setGoal();

		switch (side) {
			//left
		case(Side.LEFT):
			y += Random.Range (0,yRange);
			break;
			//bottom
		case(Side.BOTTOM):
			x += Random.Range(0,xRange);
			break;
			//right
		case(Side.RIGHT):
			x+=xRange;
			y+= Random.Range(0,yRange);
			break;
			//top
		case(Side.TOP):
			y+=yRange;
			x+= Random.Range(0,xRange);
			break;
			
		default:
			break;
		}
		Vector3 pos = new Vector3 (x, 0, y);

		foreach (Vector3 p in startPositions) {
			if(Vector3.Distance(pos,p) < 1 )
			{
				return false;
			}
		}
		startPositions.Add (pos);
		zombie.transform.position = pos;
		return true;
	}


}
