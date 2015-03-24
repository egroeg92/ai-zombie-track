using UnityEngine;
using System.Collections;

public class Survivor : MonoBehaviour {
	GameController game;

	GameObject goal;
	ArrayList golds;
	ArrayList zombies;
	ArrayList visableZombies;
	ArrayList dangerousZombies;
	GameObject[] alcoves;


	float maxSpeed;

	public GameObject currentGoal;
	Tile subGoal;


	Vector3 lastPos;
	Vector3 velocity;

	public Vector3 forward;
	public Vector3 rForward;
	public Vector3 lForward;
	public Vector3 evadeDir;
	
	bool hide;

	Tile currentTile;
	int tileX,tileY;
	Tile[,] tiles;

	GameObject vision;
	public float visionRadius = 3;

	NavMeshPath path;
	NavMeshAgent agent;

	Side side;

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

		alcoves = GameObject.FindGameObjectsWithTag ("alcove");

		maxSpeed = game.speed * 1.5f;

		forward = transform.forward;
		rForward = (transform.right + transform.forward).normalized;
		lForward = (-transform.right + transform.forward).normalized;
		//discretizeMap ();


		vision = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		vision.transform.localScale = new Vector3 (visionRadius, visionRadius, visionRadius);
		vision.collider.isTrigger = true;
		vision.renderer.enabled = false;
		vision.name = "vision";

		path = new NavMeshPath ();
		hide = false;
		agent = gameObject.AddComponent<NavMeshAgent> ();

		velocity = Vector3.zero;

	
	}


	
	void Update () {
		setSide ();
		maxSpeed = game.speed * 1.5f;
		vision.transform.position = transform.position;
		lastPos = transform.position;
		zombies = game.zombies;

		evaluateTree ();
		collect ();

		velocity = transform.position - lastPos;

		//dangerEvalSequence ();
			
	}
	bool evaluateTree(){
		if (golds.Count != 0) {
			
			//selector
			
			//check for enemies
			if(!dangerEvalSequence() && !hide){
				Debug.Log ("search");
				collectGoldSequence();
			}
			
			
		}
		return true;
	}


	bool dangerEvalSequence()
	{

		if (enemyInSight ()) {
			if (inDanger ()) {
				Debug.Log ("evade");
				if (evade ()) {
					return true;
				}
			}
		}
		return false;
	}
	bool collectGoldSequence()
	{
		setGoal ();
		search ();
		return true;
	}

	bool search(){

		agent.CalculatePath(currentGoal.transform.position, path);
		

		for (int i = 0; i < path.corners.Length-1; i++) {

			Debug.DrawLine (path.corners [i], path.corners [i + 1], Color.red);	
		}

		if(path.corners.Length >= 2){
			transform.position = Vector3.MoveTowards (transform.position, path.corners [1], maxSpeed * Time.deltaTime);
		}

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

	bool inDanger(){
		dangerousZombies.Clear ();

		bool inDanger = false;

		foreach (Zombie z in visableZombies) {

			if (z.isFacing (transform.position) && Vector3.Distance (transform.position, z.transform.position) < 15) {
				Debug.Log ("in danger");
				dangerousZombies.Add(z);
				inDanger = true;
			
			} else {
				if (Vector3.Distance (transform.position, z.transform.position) < 3) {
					if(!z.isLeft(gameObject) || !z.isRight(gameObject)){
						dangerousZombies.Add(z);
						inDanger = true;
					}

				}
			}


		}
		return inDanger;

	}
	bool evade(){
//		hide = true;
		Zombie mostDangerous = dangerousZombies[0] as Zombie;
		float dist = Vector3.Distance (transform.position, mostDangerous.transform.position);

		foreach (Zombie z in dangerousZombies) {
			float d = Vector3.Distance (transform.position, z.transform.position);
			if(d < dist){
				dist = d;
				mostDangerous =z ;
			}

		}

		Vector3 moveDir;

		Vector3 desiredVelocity = (transform.position - mostDangerous.transform.position).normalized * maxSpeed;
		Vector3 steering = desiredVelocity - velocity;
		
		velocity += steering;
		
		moveDir = velocity;


		if (mostDangerous.name == "cas" || mostDangerous.name == "modern") {
			//not to right or left
			Debug.Log ("left "+mostDangerous.isLeft(gameObject));
			Debug.Log ("right "+mostDangerous.isRight(gameObject));

			if (!mostDangerous.isRight (gameObject) && !mostDangerous.isLeft (gameObject)) {
				Debug.Log ("not to side!");
				if (mostDangerous.lane == Lane.OUTTER) {
					if(mostDangerous.dir == Dirrection.CCW){
						moveDir = -mostDangerous.right;
					}else{
						moveDir = mostDangerous.right;
					}
				
				} else if (mostDangerous.lane == Lane.INNER) {
					if(mostDangerous.dir == Dirrection.CCW){
						moveDir = mostDangerous.right;
					}else{
						moveDir = -mostDangerous.right;
					}
				} else{
					if(mostDangerous.side == side){
						moveDir =mostDangerous.right;
						RaycastHit l,r;
						Physics.Raycast(transform.position, mostDangerous.right,out r);
						Physics.Raycast(transform.position, -mostDangerous.right,out l);
						if(l.distance<r.distance){
							moveDir = -mostDangerous.right;
						}
					}else{
						if(mostDangerous.side == Side.LEFT )
							moveDir = new Vector3(1,0,0);
						else if(mostDangerous.side == Side.BOTTOM)
							moveDir = new Vector3(0,0,1);
						else if(mostDangerous.side == Side.RIGHT)
							moveDir = new Vector3(-1,0,0);
						
						else
							moveDir = new Vector3(0,0,-1);

					}
				}
			}
		} else if (mostDangerous.name == "shamb") {
			Debug.Log (mostDangerous.side + " "+ side); 
			if(mostDangerous.isFacing(transform.position)){
				if(mostDangerous.side == side){
					moveDir = mostDangerous.forward;
				}else{
					if(mostDangerous.side == Side.LEFT )
						moveDir = new Vector3(1,0,0);
					else if(mostDangerous.side == Side.BOTTOM)
						moveDir = new Vector3(0,0,1);
					else if(mostDangerous.side == Side.RIGHT)
						moveDir = new Vector3(-1,0,0);
					
					else
						moveDir = new Vector3(0,0,-1);


				}
			}
			else{
				moveDir = - mostDangerous.forward;
			}
		}
		transform.position += (moveDir.normalized)*maxSpeed*Time.deltaTime;

		return true;

	}

	Vector3 getClosestAlcove(){
		Vector3 alcove = transform.position;
		float dist = float.MaxValue;

		foreach (GameObject a in alcoves) {

			float d = Vector3.Distance (a.transform.position, transform.position);
			if (d < dist) {
				dist = d;
				alcove = a.transform.position;
			}
		}
		return alcove;

	}

	bool inverter(bool b){
		return !b;
	}
	bool goalInSight(){
		ArrayList inSight = new ArrayList ();
		bool vis = false;

		RaycastHit hit;
		Vector3 dir;
		if (golds.Count != 0) {
			if(currentGoal!= null)
				if(currentGoal.collider.bounds.Intersects (collider.bounds))
					return true;
			foreach (GameObject g in golds) {
				if(g == goal && golds.Count != 1)
					continue;
				dir = (g.transform.position - transform.position).normalized;
				//Debug.Log (dir);
				Physics.Raycast (transform.position, dir, out hit);
				if (hit.transform.gameObject == g) {
					//Debug.Log (hit.transform.position);

					inSight.Add (g);
					vis = true;
				}
			}
			if(inSight.Count != 0)
			{
				float dist = float.MaxValue;
				float t;
				foreach(GameObject gold in inSight){
					t =Vector3.Distance(gold.transform.position, transform.position); 
					if( t < dist){
						dist = t;
						currentGoal = gold;
					}
				}
			}

		} else {
			dir = (goal.transform.position - transform.position).normalized;
			Physics.Raycast (transform.position, dir, out hit);
			if (hit.transform.gameObject == goal) {
				currentGoal = goal;
				vis = true;
			}
		}
		return vis;
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



	void setGoal()
	{
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


	Tile aStar(Tile tile){
		ArrayList openList = new ArrayList ();
		ArrayList closedList = new ArrayList ();
		ArrayList adj;
	
		Tile current = tile;

		current.G = 0;
		current.H = setH (current);


		float gScore = 0;
		float fScore = gScore + current.H;
		openList.Add (current);

		float tmpGScore;
		int i = 0;

		while (openList.Count != 0) {
			fScore = float.MaxValue;
			foreach( Tile t in openList){
				if((t.H + t.G) < fScore){
					fScore = (t.H  + t.G);
					current = t;
				}
			}
			if( current.position == currentGoal.transform.position)
			{
				return current;
			}
			openList.Remove(current);
			closedList.Add(current);
			adj = getAdjacentTiles(current);

			foreach(Tile neighbour in adj){
				if(closedList.Contains(neighbour))
					continue;

				tmpGScore = current.G + Vector3.Distance(current.position , neighbour.position);

				if(!openList.Contains(neighbour) || tmpGScore < neighbour.G ){
					neighbour.parent = current;
					neighbour.G = tmpGScore;
					setH(neighbour);
					if(!openList.Contains(neighbour))
						openList.Add(neighbour);
				}
			}

		}

	return null;


	}



	ArrayList getAdjacentTiles(Tile tile){

		ArrayList adj = new ArrayList ();
		Vector3 dir;
		RaycastHit hit;

		for (int x = -1; x <=1; x++) {
			if(tile.x + x >= 0 && tile.x + x < tiles.GetLength(0)){
				
				for (int y = -1; y <=1; y++) {
					
					if(y == 0 && x == 0)
						continue;
					if(tile.y + y >= 0 && tile.y + y < tiles.GetLength(1)){

						Tile t = tiles[tile.x + x, tile.y+y];
						dir = (t.position - tile.position).normalized;

						if(Physics.Raycast(tile.position, dir ,out hit)){
							if ( hit.distance > 1 || hit.transform.gameObject == currentGoal){
								adj.Add(t);
							}
						}
					}
				}
			}
		}
		return adj;


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
	Side getGoldSide(Vector3 position){
		Side s;
		if (position.x > 26) {
			s = Side.RIGHT;
		}else if(position.y > 14 ){
			s = Side.TOP;
		}else if(position.y < 7){
			s = Side.BOTTOM;
		}else{
			s = Side.LEFT;
		}
		return s;
	}

	float setH(Tile t){
		t.H =Vector3.Distance (t.position, currentGoal.transform.position);
		return t.H;
	}

	void discretizeMap (){
		tiles = new Tile[37,21];
		Vector3 basePos = new Vector3 (-1.5f, 0, 0.5f);
		Vector3 pos;
		for (int x = 0; x < 37; x++) {
			for (int y = 0; y < 21; y++) {
				pos = basePos + new Vector3(x,0,y);
				Tile t = new Tile(pos);
				tiles[x,y] = t;
				t.x = x;
				t.y = y;
				if(pos == transform.position)
				{
					tileX = x;
					tileY = y;
					currentTile = t;
				}
			}
		}



	}



}
class DangerList{
	Vector3[] pos;
	int[] score;

	public DangerList(ArrayList p){
		pos = new Vector3[p.Count];
		score = new int[p.Count];
		int i = 0;
		foreach (Vector3 v in p) {
			pos[i] = v;
			score[i] = 0;
			i++;
		}
	}
	public Vector3 getHigest(){

		int s = int.MinValue;
		int index = 0;
		for(int i = 0 ; i < score.Length; i++) {
			if(score[i] < s){
				s = score[i];
				index = i;
			}

		}
		return pos [index];
	}

	public void changeScore(Vector3 p, int s){

		for(int i = 0 ; i < pos.Length; i++) {
			if(pos[i] == p){
				score[i] += s;
				break;
			}
			
		}
	}
	public Vector3[] getPositions(){
		Vector3[] clone = new Vector3[pos.Length];
		for (int i = 0; i < pos.Length; i++)
			clone [i] = pos [i];
		return clone;
	}



}

class Tile{
	public Tile parent;
	public float H ;
	public float G ;

	public int x, y;
	public Vector3 position;
	public Tile(Vector3 pos){

		position = pos;
	}
}




