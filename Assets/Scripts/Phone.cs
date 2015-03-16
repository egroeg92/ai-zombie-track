using UnityEngine;
using System.Collections;

public class Phone : Zombie {
	float speedMin, speedMax;

	
	float pauseShiftMin = 1f;
	float pauseShiftMax = 2f;
	float pauseShiftFrequency;
	float nextpauseShift;
	float laneShiftMin = 2f;
	float laneShiftMax = 3f;
	float laneShiftFrequency;
	float nextLaneShift;
	float dirShiftMin = 1f;
	float dirShiftMax = 3f;
	float dirShiftFrequency;
	float nextDirShift;
	float speedShiftMin = .25f;
	float speedShiftMax = 1f;
	float speedShiftFrequency = .25f;
	float nextSpeedShift;

	// Use this for initialization
	void Start () {
		base.Start ();
		base.setEasy (false);
		speedMin = speed / 2;
		speedMax = speed * 2;

		speed = Random.Range (speedMin, speedMax);

		pauseShiftFrequency = Random.Range (pauseShiftMin, pauseShiftMax);
		laneShiftFrequency = Random.Range (laneShiftMin, laneShiftMax);
		dirShiftFrequency = Random.Range (dirShiftMin, dirShiftMax);
		speedShiftFrequency = Random.Range (speedShiftMin, speedShiftMax);

		nextpauseShift = Time.time + pauseShiftFrequency;
		nextLaneShift = Time.time + laneShiftFrequency;
		nextDirShift = Time.time + dirShiftFrequency;
		nextSpeedShift = Time.time + speedShiftFrequency;
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();

		if (Time.time > nextLaneShift) {
			laneShiftFrequency = Random.Range (laneShiftMin, laneShiftMax);
			nextLaneShift = Time.time + laneShiftFrequency;
			shiftLane ();
		}
		if (Time.time > nextDirShift) {
			dirShiftFrequency = Random.Range (dirShiftMin, dirShiftMax);
			nextDirShift = Time.time + dirShiftFrequency;
			if(dir == Dirrection.CCW)
				dir = Dirrection.CW;
			else
				dir = Dirrection.CCW;
			setGoal();
		}
		if (Time.time > nextSpeedShift) {
				speedShiftFrequency = Random.Range (speedShiftMin, speedShiftMax);
				nextSpeedShift = Time.time + speedShiftFrequency;
				speed = Random.Range (speedMin, speedMax);
			
		}
		if (Time.time > nextpauseShift) {
				pauseShiftFrequency = Random.Range (pauseShiftMin, pauseShiftMax);
				nextpauseShift = Time.time + pauseShiftFrequency;
				speed = 0;
			
		}
		avoidCollision ();

	}

	bool shiftLane(){
		bool avoided = false;
		if (base.lane == Lane.INNER) {
			avoided = base.shiftLaneUp ();
		} else if (base.lane == Lane.OUTTER) {
			avoided = base.shiftLaneDown ();
		} else if (Random.Range (0, 1f) < 0.5f) {
			avoided = base.shiftLaneUp ();
		} else {
			avoided = base.shiftLaneDown ();
		}
		return avoided;
	}
	protected override void avoidCollision(){
		foreach (Zombie z in zombies) {
			if(z == this)
				continue;
			if(Vector3.Distance(transform.position, z.transform.position) < 2.5f && lane == z.lane)
			{
				if(!shiftLane()){
					dir = z.dir;
					setGoal();
					speed = z.speed;
				}
				
			}
		}
	}
}
