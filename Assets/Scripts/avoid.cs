using UnityEngine;
using System.Collections;

public class avoid : MonoBehaviour {

	public GameObject target;

	public float maxVelocity;

	Vector3 velocity;

	Vector3 lastPos,nextPos;

	// Use this for initialization
	void Start () {

		velocity = transform.forward * maxVelocity;
	}
	
	// Update is called once per frame
	void Update () {



		Vector3 desiredVelocity = (transform.position - target.transform.position).normalized * maxVelocity;
		Vector3 steering = desiredVelocity - velocity;

		velocity += steering;

		transform.position += velocity;
		
	}
}
