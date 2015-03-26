using UnityEngine;
using System.Collections;

public class Survivor2 : MonoBehaviour {

	public GameController game;
	
	GameObject goal;
	ArrayList golds;
	ArrayList zombies;
	ArrayList visableZombies;
	ArrayList dangerousZombies;

	float maxSpeed;
	float avoidanceFactor;

	public GameObject currentGoal;

	Vector3 lastPos;
	public Vector3 velocity;
	Vector3 steering;

	
	public Vector3 forward;

	Side side;
	NavMeshPath path;
	NavMeshAgent agent;
	// Use this for initialization
	void Start () {
		game = GameObject.Find ("Level").GetComponent<GameController> ();
		
		gameObject.AddComponent<Rigidbody> ();
		gameObject.rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		gameObject.rigidbody.freezeRotation = true;
		
		zombies = game.zombies;
		visableZombies = new ArrayList ();
		dangerousZombies = new ArrayList ();
		golds = new ArrayList ();
		golds.AddRange(game.golds);
		goal = game.goal;
		golds.Add (goal);

		
		maxSpeed = game.speed * 1.5f;
		avoidanceFactor = 3;

		forward = transform.forward;

		path = new NavMeshPath ();
		agent = gameObject.AddComponent<NavMeshAgent> ();
		

	}
	
	// Update is called once per frame
	void Update () {
		lastPos = transform.position;
		setGoal ();

		//Debug.Log (currentGoal);
		search ();



	}
	bool search(){
		Debug.Log (currentGoal.transform.position);
		agent.CalculatePath(currentGoal.transform.position, path);
		
		
		for (int i = 0; i < path.corners.Length-1; i++) {
			
			Debug.DrawLine (path.corners [i], path.corners [i + 1], Color.red);	
		}
		
		if(path.corners.Length >= 2){
			transform.position = Vector3.MoveTowards (transform.position, path.corners [1], maxSpeed * Time.deltaTime);
		}
		
		return true;
	}

	bool evade(){

		return true;
	}


	bool enemyInSight(){
		bool inSight = false;
		visableZombies.Clear ();
		
		RaycastHit hit;
		Vector3 dir;
		
		foreach (Zombie z in zombies) {
			z.seen = false;
			dir = z.transform.position - transform.position;
			Physics.Raycast(transform.position, dir ,out hit);
			if(hit.transform.gameObject == z.gameObject ){
				visableZombies.Add(z);
				inSight = true;
				z.seen = true;
			}
			z.isVisable();
		}
		
		return inSight;
	}
	void setGoal()
	{
	//	Debug.Log (golds.Count);
		if (golds.Count == 0)
			return;
		else if (golds.Count == 1)
			currentGoal = goal;
		else {
			
			currentGoal = ((GameObject)golds [0]);
			while(currentGoal == goal)
				currentGoal = (GameObject)golds[Random.Range (0,golds.Count)];
			
			float dist = Vector3.Distance (currentGoal.transform.position, transform.position);
			foreach (GameObject g in golds) {
				if(g == goal)
					continue;
				float d = Vector3.Distance (g.transform.position, transform.position);
				
				
				
				if (d < dist) {
					dist = d;
					currentGoal = g;
				}
			}
			
		}
	}
	bool collect(){
		foreach (GameObject g in golds) {
			if (Vector3.Distance (g.transform.position, transform.position) < .5f) {
				golds.Remove (g);
				Destroy (g);
				if (g == currentGoal)
					currentGoal = null;
				return true;
			}
		}
		
		
		return false;
	}
}
