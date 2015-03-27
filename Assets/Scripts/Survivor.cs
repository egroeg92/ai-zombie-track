using UnityEngine;
using System.Collections;

public class Survivor : MonoBehaviour {
	GameController game;

	GameObject goal;
	ArrayList golds;
	ArrayList zombies;
	ArrayList visableZombies;
	ArrayList dangerousZombies;

	float maxSpeed;

	public GameObject currentGoal;


	Vector3 lastPos;
	Vector3 velocity;

	bool inSight;
	bool hide;

	Dirrection dir;


	NavMeshPath path;
	NavMeshAgent agent;

	float avoidanceForce = 3;

	Side side;

	public LayerMask visual;


	// Use this for initialization
	void Start () {
		game = GameObject.Find ("Level").GetComponent<GameController> ();

		//gameObject.AddComponent<Rigidbody> ();
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

		path = new NavMeshPath ();
		hide = false;
		agent = gameObject.AddComponent<NavMeshAgent> ();

		velocity = Vector3.zero;
	
	}


	
	void Update () {
		setSide ();
		maxSpeed = game.speed * 1.5f;
		lastPos = transform.position;
		zombies = game.zombies;


		canSeeEnemies ();
		if (setGoal ())
			search ();
		else {
			evade ();
		}

		collect ();


		velocity = transform.position - lastPos;
			
	}


	bool search(){

		if (path == null) {
			Debug.Log ("no path");
			return false;
		}

		for (int i = 0; i < path.corners.Length-1; i++) {
			Debug.DrawLine (path.corners [i], path.corners [i + 1], Color.red);	
		}

		if(path.corners.Length >= 2){

			//Vector3 move = Vector3.MoveTowards (transform.position, path.corners [1], maxSpeed * Time.deltaTime);
			Vector3 move = path.corners[1] - transform.position;
			Vector3 avoid = getAvoidance();

			transform.position += (move + avoid).normalized * maxSpeed *Time.deltaTime ;
			return true;
		}

		return false;
	}

	bool canSeeEnemies(){
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


	bool evade(){

		Zombie mostDangerous;

		if (dangerousZombies.Count == 0) {
			mostDangerous = visableZombies [0] as Zombie;


			float dist = Vector3.Distance (transform.position, mostDangerous.transform.position);

			foreach (Zombie z in visableZombies) {
				float d = Vector3.Distance (transform.position, z.transform.position);
				if (d < dist) {
					dist = d;
					mostDangerous = z;
				}

			}
		} else {
			mostDangerous = dangerousZombies [0] as Zombie;
			
			
			float dist = Vector3.Distance (transform.position, mostDangerous.transform.position);
			
			foreach (Zombie z in dangerousZombies) {
				float d = Vector3.Distance (transform.position, z.transform.position);
				if (d < dist) {
					dist = d;
					mostDangerous = z;
				}
				
			}
		}

		Vector3 move = mostDangerous.transform.position  - transform.position;

		Vector3 avoid = getAvoidance();
		
		transform.position += (move + avoid).normalized * maxSpeed *Time.deltaTime ;

		return true;

	}

	Vector3 getAvoidance(){
		Vector3 avoidance = Vector3.zero;

		float distance;

		foreach (Zombie z in visableZombies) {
			Vector3 zPos = new Vector3(z.transform.position.x, z.transform.position.y - 0.5f , z.transform.position.z);
			Vector3 pPos = new Vector3(transform.position.x, transform.position.y - 0.5f , transform.position.z);

			Vector3 dir = z.transform.position - transform.position;

			RaycastHit hit;
			if(Physics.Raycast( pPos, dir, out hit,Vector3.Distance(z.transform.position, transform.position))){

				if(hit.transform.gameObject.name == "vision"){
					distance = hit.distance;
				
					Vector3 tAvoid = -dir.normalized * avoidanceForce / Mathf.Pow (distance , 2);
					Debug.Log (tAvoid);

					if(tAvoid.magnitude > avoidance.magnitude)
						avoidance = tAvoid;
					}
			}
		}

		Debug.Log (avoidance);
		return avoidance;


	}



	bool inverter(bool b){
		return !b;
	}

	bool collect(){
		foreach (GameObject g in golds) {
			if( g.collider.bounds.Intersects (collider.bounds)){
				golds.Remove (g);
				Destroy (g);
				if (g == currentGoal)
					currentGoal = null;
				path = null;

				return true;
			}
		}


		return false;
	}



	bool setGoal()
	{
		if (golds.Count == 0)
			return true;
		else if (path != null && currentGoal != null && isPathSafe (path)) {
			agent.CalculatePath(currentGoal.transform.position , path);
			return true;
		}
		else {
			Hashtable paths = new Hashtable();

			foreach (GameObject g in golds) {
				NavMeshPath p = new NavMeshPath ();
				if(g == goal && golds.Count > 1)
					continue;

				agent.CalculatePath(g.transform.position, p);

				float d = 0;
				bool safe = true;

				for (int i = 0; i < p.corners.Length-1; i++) {
					
					d += Vector3.Distance(p.corners[i],p.corners[i+1]);

					foreach (Zombie z in visableZombies) {
						RaycastHit hit;
						if(Physics.Raycast(p.corners[i], p.corners[ i + 1] - p.corners[i] , out hit)){
							if(hit.transform.gameObject == z.vision && hit.distance <= Vector3.Distance(p.corners[i], p.corners[i+1])){
								safe = false;
								break;
							}
						}
					}
				}
				if(safe){
					if(paths[d] == null)
						paths.Add(d,p);

				}
			}
			if(paths.Count > 0){
				float dist = float.MaxValue;

				foreach (DictionaryEntry entry in paths)
				{
					if((float)entry.Key < dist)
					{
						dist = (float)entry.Key;
						NavMeshPath pat = (NavMeshPath)entry.Value;

					}
				}
				path = (NavMeshPath)paths[dist];

				currentGoal = getGoal(path.corners[path.corners.Length-1]);
				return true;
			}
			else {
				path = null;
				return false;
			}
		}
	}


	GameObject getGoal(Vector3 pos){
		foreach (GameObject g in golds) {


			if (g.transform.position.x == pos.x && g.transform.position.z == pos.z)
			{
				return g;
			}
		}
		return null;
	}
	bool isPathSafe(NavMeshPath path){
		if (visableZombies.Count == 0){
			return true;
		}
		for (int i = 0; i < path.corners.Length-1; i++) {

			foreach (Zombie z in visableZombies) {
				RaycastHit hit;
				if(Physics.Raycast(path.corners[i], path.corners[ i + 1] - path.corners[i] , out hit)){
					if(hit.transform.gameObject == z.vision){
						return false;
					}
				}
			}
		}
		return true;
	}

	void setSide(){
		if (transform.position.x > 28) {
			side = Side.RIGHT;
		}else if( transform.position.y > 16 ){
			side = Side.TOP;
		}else if(transform.position.y < 5){
			side = Side.BOTTOM;
		}else{
			side = Side.LEFT;
		}
	}

}











