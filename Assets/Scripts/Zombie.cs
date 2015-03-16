﻿using UnityEngine;
using System.Collections;

public abstract class Zombie : MonoBehaviour {

	Casual casual;
	Shambler shambler;
	Modern modern;
	Phone phone;

	bool destroyFlag = false;
	protected GameController game;
	GameObject  ilt,mlt,olt,
				irt,mrt,ort,
				ilb,mlb,olb,
				irb,mrb,orb;
	GameObject[] corners;
	protected ArrayList zombies;
	public GameObject goalCorner;

	public Dirrection dir = Dirrection.CCW;

	public float speed = 1.0f;
	int spawnProb;
	float ratio;
	bool easy;
	public Lane lane;
	public Side side;

	public Vector3 forward;
	// Use this for initialization
	protected void Start () {

		game = GameObject.Find("Level").GetComponent<GameController> ();


		game.zombies.Add (this);
		zombies = game.zombies;

		casual = game.casual;
		shambler = game.shambler;
		modern = game.modern;
		phone = game.phone;

		spawnProb = game.respawnProbabilty;
		ratio = game.easyToHardRatio;
		speed = game.speed;
		ilt = game.ilt;
		mlt = game.mlt;
		olt = game.olt;
		
		irt = game.irt;
		mrt = game.mrt;;
		ort = game.ort;
		
		ilb = game.ilb;
		mlb = game.mlb;
		olb = game.olb;
		
		irb = game.irb;
		mrb = game.mrb;
		orb = game.orb;
		corners = new GameObject[12];
		corners [0] = ilt;
		corners [1] = mlt;
		corners [2] = olt;
		corners [3] = irt;
		corners [4] = mrt;
		corners [5] = ort;
		corners [6] = ilb;
		corners [7] = mlb;
		corners [8] = olb;
		corners [9] = irb;
		corners [10] = mrb;
		corners [11] = orb;
		
		setSide ();
		setForward ();
		setGoal ();

		
	}
	public void setForward()
	{
		if (side == Side.LEFT) {
			if (dir == Dirrection.CCW)
				forward = new Vector3 (0, 0, -1);
			else
				forward = new Vector3 (0, 0, 1);
		} else if (side == Side.BOTTOM) {
			if (dir == Dirrection.CCW)
				forward = new Vector3 (1, 0, 0);
			else
				forward = new Vector3 (-1, 0, 0);
		} else if (side == Side.RIGHT) {
			if (dir == Dirrection.CCW)
				forward = new Vector3 (0, 0, 1);
			else
				forward = new Vector3 (0, 0, -1);
		} else {
			if (dir == Dirrection.CCW)
				forward = new Vector3 (-1, 0, 0);
			else 
				forward = new Vector3 (1, 0, 0);
		}
	}

	public void setSide(){

		if (transform.position == game.ilt.transform.position) {
			side = Side.LEFT;
			lane = Lane.INNER;
		}else if(transform.position == game.mlt.transform.position){
			side = Side.LEFT;
			lane = Lane.MID;
			
		}else if(transform.position == game.olt.transform.position){
			side = Side.LEFT;
			lane = Lane.OUTTER;
			
		}else if(transform.position == game.irt.transform.position){
			side = Side.TOP;
			lane = Lane.INNER;
			
		}else if(transform.position == game.mrt.transform.position){
			side = Side.TOP;
			lane = Lane.MID;
			
		}else if(transform.position == game.ort.transform.position){
			side = Side.TOP;
			lane = Lane.OUTTER;
			
			
		}else if(transform.position == game.ilb.transform.position){
			side = Side.BOTTOM;
			lane = Lane.INNER;
			
		}else if(transform.position == game.mlb.transform.position){
			side = Side.BOTTOM;
			lane = Lane.MID;
			
		}else if(transform.position == game.olb.transform.position){
			side = Side.BOTTOM;
			lane = Lane.OUTTER;
			
		}else if(transform.position == game.irb.transform.position){
			side = Side.RIGHT;
			lane = Lane.INNER;
			
		}else if(transform.position == game.mrb.transform.position){
			side = Side.RIGHT;
			lane = Lane.MID;
			
		}else if(transform.position == game.orb.transform.position){
			side = Side.RIGHT;
			lane = Lane.OUTTER;
			
			
		}
	}

	public void setGoal(){
		switch (side) {
			//left
		case(Side.LEFT):
			if(lane == Lane.INNER)
			{
				if(dir == Dirrection.CCW)
					goalCorner = ilb;
				
				else
					goalCorner = ilt;
			}else if(lane == Lane.MID)
			{
				if(dir == Dirrection.CCW)
					goalCorner =mlb;
				else
					goalCorner =mlt;
			}else{
				if(dir == Dirrection.CCW)
					goalCorner = olb;
				else
					goalCorner = olt;
			}

			break;
			//bottom
		case(Side.BOTTOM):
			if(lane == Lane.INNER)
			{
				if(dir == Dirrection.CCW)
					goalCorner = irb;
				else
					goalCorner = ilb;
			}else if(lane == Lane.MID)
			{
				if(dir == Dirrection.CCW)
					goalCorner =mrb;
				else
					goalCorner =mlb;
			}else{
				if(dir == Dirrection.CCW)
					goalCorner = orb;
				else
					goalCorner = olb;
			}
			break;
			//right
		case(Side.RIGHT):
			if(lane == Lane.INNER)
			{
				if(dir == Dirrection.CCW)
					goalCorner = irt;
				else
					goalCorner = irb;
			}else if(lane == Lane.MID)
			{
				if(dir == Dirrection.CCW)
					goalCorner = mrt;
				else
					goalCorner = mrb;
			}else{
				if(dir == Dirrection.CCW)
					goalCorner = ort;
				else
					goalCorner = orb;
			}
			break;
			//top
		case(Side.TOP):
			if(lane == Lane.INNER)
			{
				if(dir == Dirrection.CCW)
					goalCorner = ilt;
				else
					goalCorner = irt;
			}else if(lane == Lane.MID)
			{
				if(dir == Dirrection.CCW)
					goalCorner = mlt;
				else
					goalCorner = mrt;
			}else{
				if(dir == Dirrection.CCW)
					goalCorner = olt;
				else
					goalCorner = ort;
			}
			break;
			
		default:
			break;
		}
	}

	// Update is called once per frame
	protected void Update () {

		zombies = game.zombies;
		FSM ();
	}
	void LateUpdate(){
		if (destroyFlag) {
			if(easy)
				game.easies -= 1;
			else
				game.hards -= 1;


			game.zombies.Remove (this);
			spawnNew();

			Destroy (this.gameObject);
			Destroy (this);


		}
	}
	void spawnNew(){
		int easies = game.easies;
		int hards = game.hards;
		ratio = game.easyToHardRatio;

		GameObject startCorner = (corners [Random.Range (0, corners.Length)]);
		//TODO
		while (startCorner == goalCorner)
			startCorner = (corners [Random.Range (0, corners.Length)]);


		Vector3 start = startCorner.transform.position;
		Zombie z;
		if (ratio == 0) {
			if(Random.Range (0, 2) == 1){
				z = Instantiate(casual,start,Quaternion.identity)as Casual;
			}else{
				z = Instantiate(shambler,start,Quaternion.identity)as Shambler;
			}
		}else if (hards == 0) {
			if(Random.Range (0, 2) == 1){
				z = Instantiate(modern,start,Quaternion.identity) as Modern;
			}else{
				z = Instantiate(shambler,start,Quaternion.identity)as Phone;
			}
		}else if( (easies/hards) >= ratio ){
			if(Random.Range (0, 2) == 1){
				z = Instantiate(modern,start,Quaternion.identity)as Modern;
			}else{
				z = Instantiate(phone,start,Quaternion.identity)as Phone;
			}
		}else {
			if(Random.Range (0, 2) == 1){
				z = Instantiate(casual,start,Quaternion.identity)as Casual;
			}else{
				z = Instantiate(shambler,start,Quaternion.identity)as Shambler;
			}
		}

	}

	protected void FSM(){

		transform.position = Vector3.MoveTowards (transform.position, goalCorner.transform.position, speed * Time.deltaTime);


		if (transform.position == goalCorner.transform.position) {
			if(Random.Range (0,100) > spawnProb){
				switch (side) {
				case(Side.LEFT):
					if (dir == Dirrection.CCW)
						side = Side.BOTTOM;
					else
						side = Side.TOP;
					break;
				
				case(Side.BOTTOM):
					if (dir == Dirrection.CCW)
						side = Side.RIGHT;
					else
						side = Side.LEFT;
					break;

				case(Side.RIGHT):
					if (dir == Dirrection.CCW)
						side = Side.TOP;
					else
						side = Side.BOTTOM;
					break;

				case(Side.TOP):
					if (dir == Dirrection.CCW)
						side = Side.LEFT;
					else
						side = Side.RIGHT;
					break;

				}
				setForward();
				setGoal ();
			}
			else{
				destroyFlag = true;

			}
		}

	}
	protected void setEasy(bool e){
		easy = e;
		if (easy)
			game.easies += 1;
		else
			game.hards += 1;
	}

	protected bool shiftLaneUp(){

		if ((int)lane == 2)
			return false;
		foreach (Zombie z in zombies) {
			if(z == this)
				continue;
			if( Vector3.Distance(transform.position, z.transform.position) < 2 && (int)z.lane == (int)lane+1)
				return false;
		}
		lane += 1;

		switch (side) {
		case(Side.LEFT):
			transform.position -= Vector3.right;
			break;
		case(Side.BOTTOM):
			transform.position -= Vector3.forward;
			break;
		case(Side.RIGHT):
			transform.position += Vector3.right;
			break;
		case(Side.TOP):
			transform.position += Vector3.forward;
			break;
		default:
			break;
		}
		setGoal ();
		
		return true;

		
	}
	protected bool shiftLaneDown(){

		if ((int)lane == 0)
			return false;
		foreach (Zombie z in zombies) {
			if(z == this)
				continue;
			if( Vector3.Distance(transform.position, z.transform.position) < 2 && (int)z.lane == (int)lane-1)
				return false;
		}

		lane -= 1;

		switch (side) {
		case(Side.LEFT):
			transform.position += Vector3.right;
			break;
		case(Side.BOTTOM):
			transform.position += Vector3.forward;
			break;
		case(Side.RIGHT):
			transform.position -= Vector3.right;
			break;
		case(Side.TOP):
			transform.position -= Vector3.forward;
			break;
		default:
			break;
		}
		setGoal ();
		return true;

		
	}
	protected abstract void avoidCollision();


}
