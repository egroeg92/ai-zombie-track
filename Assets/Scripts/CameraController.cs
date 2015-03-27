using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey("up"))
			transform.position  += new Vector3(0,0,1) * Time.deltaTime;
		
		if (Input.GetKey("down"))
			transform.position  -= new Vector3(0,0,1) * Time.deltaTime;
		
		if (Input.GetKey("right"))
			transform.position  += new Vector3(1,0,0) * Time.deltaTime;
		
		if (Input.GetKey("left"))
			transform.position  -= new Vector3(1,0,0) * Time.deltaTime;
	}
}
