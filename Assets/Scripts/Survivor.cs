using UnityEngine;
using System.Collections;

public class Survivor : MonoBehaviour {
	GameController game;

	GameObject goal;
	ArrayList golds;
	ArrayList zombies;
	ArrayList visableZombies;

	float speed;

	public GameObject currentGoal;
	Tile subGoal;



	public Vector3 forward;
	public Vector3 right;
	public Vector3 left;

	Vector3 lasPos;

	Tile currentTile;
	int tileX,tileY;
	Tile[,] tiles;

	Stack path;
	// Use this for initialization
	void Start () {
		game = GameObject.Find ("Level").GetComponent<GameController> ();
		gameObject.AddComponent<Rigidbody> ();
		gameObject.rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		gameObject.rigidbody.freezeRotation = true;

		zombies = game.zombies;
		visableZombies = new ArrayList ();
		golds = new ArrayList ();
		golds.AddRange(game.golds);
		goal = game.goal;

		speed = game.speed * 1.5f;

		forward = transform.forward;
		discretizeMap ();

		path = new Stack ();
		//setGoal();



		//Tile t = aStar (currentTile);
		//setPath (t);
		//subGoal = (Tile)path.Pop ();
	
	}

	void setPath (Tile t)
	{
		while (t.position != currentTile.position) {
			path.Push (t);
			t = t.parent;
		}
	}
	
	void Update () {
		lasPos = transform.position;
		zombies = game.zombies;

		evaluateTree ();


			
	}
	bool evaluateTree(){
		if (golds.Count != 0) {
			
			//selector
			
			//check for enemies
			if(!dangerEvalSequence()){
				
				if(!collectGoldSequence()){
					
					return search();
				}
			}
			
			
		}
		return true;
	}


	bool dangerEvalSequence()
	{
		if (enemyInSight ()) {
			if (inDanger ()) {
				if (avoid ()) {
					return true;
				}
			}
		}
		return false;
	}
	bool collectGoldSequence()
	{
		if (goalInSight ()) {
			if (moveTowards ()) {
				if (inverter (collect ())) {
					return true;
				}
			}
		}
		return false;
	}

	bool search(){
		Debug.Log ("SEARCH");


		forward = transform.forward;
		right = (transform.right + transform.forward).normalized;
		left = (-transform.right + transform.forward).normalized;
		Vector3 backwards = -forward;
		Vector3 bLeft = -left;
		Vector3 bRight = -right;


		Debug.DrawRay (transform.position , forward * 10, Color.red);
		
		Debug.DrawRay (transform.position , right * 2, Color.yellow);
		
		Debug.DrawRay (transform.position , left  * 2, Color.green);

		RaycastHit f, r, l,b,br,bl;


		Physics.Raycast(transform.position, forward ,out f);

		if (f.distance < 1) {

			Physics.Raycast (transform.position, right, out r);
			Physics.Raycast (transform.position, left, out l);

			if (r.distance > 1 && l.distance > 1) {
				if (Random.Range (0, 1f) > 0.5f)
					forward = left;
				else
					forward = right;
			} else if (r.distance < 1 && l.distance > 1) {
				forward = left;
			} else if (r.distance > 1 && l.distance < 1) {
				forward = right;
			} else {
				Physics.Raycast (transform.position, bRight, out br);
				Physics.Raycast (transform.position, bLeft, out bl);
				Physics.Raycast (transform.position, backwards, out b);

				if (Random.Range (0, 1f) > 0.5f) {
					if (bl.distance > 1)
						forward = bLeft;
					else
						forward = backwards;

				} else {
					if (br.distance > 1)
						forward = bRight;
					else
						forward = backwards;
			
				}
			}

		} else {

		//	Random.Range(-5,5);
		//	forward = Quaternion.AngleAxis(Random.Range (-30,30) ,Vector3.up) * forward;


		}

		Debug.Log (forward);

		transform.forward = forward;
		transform.position = Vector3.MoveTowards (transform.position, transform.position + forward, speed * Time.deltaTime);

		//transform.Translate(forward * speed * Time.deltaTime);


		return true;
	}

	bool enemyInSight(){
		bool inSight = false;
		visableZombies.Clear ();

		RaycastHit hit;
		Vector3 dir;

		foreach (Zombie z in zombies) {
			dir = z.transform.position - transform.position;

			Physics.Raycast(transform.position, dir ,out hit);
			//Debug.Log (hit.transform.gameObject.name);
			if(hit.transform.gameObject == z.gameObject ){
				visableZombies.Add(z);
				inSight = true;
			}
		}
		//Debug.Log ("enemies in sight" + (inSight));

		return inSight;
	}

	bool inDanger(){
		
		return true;
	}
	bool avoid(){

		return true;
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
	bool moveTowards(){
		transform.position = Vector3.MoveTowards (transform.position, currentGoal.transform.position, speed * Time.deltaTime);
		if (transform.position != currentGoal.transform.position) {
			transform.forward = (currentGoal.transform.position - transform.position).normalized;
			forward = transform.forward;
		}

		forward = transform.forward;
		right = transform.right + transform.forward;
		left = -transform.right + transform.forward;
		
		Debug.DrawRay (transform.position , forward * 10, Color.red);
		
		Debug.DrawRay (transform.position , right * 2, Color.yellow);
		
		Debug.DrawRay (transform.position , left  * 2, Color.green);

		return true;
	}
	bool collect(){
		if (transform.position == currentGoal.transform.position) {
			golds.Remove(currentGoal);
			Destroy(currentGoal);
			currentGoal = null;
			return true;
		}

		return false;
	}



	void setGoal()
	{
		if (golds.Count == 0)
			currentGoal = goal;
		else {
			currentGoal = ((GameObject)golds [0]);
			float dist = Vector3.Distance (currentGoal.transform.position, transform.position);
			foreach (GameObject g in golds) {
				float d = Vector3.Distance (g.transform.position, transform.position);
				if (d < dist) {
					dist = d;
					currentGoal = g;
				}
			}

		}
	}

	void preAStar(){
		/*
		float dist = Vector3.Distance(currentGoal.transform.position,transform.position);

		foreach (GameObject g in golds) {
			float dist1 = Vector3.Distance(g.transform.position,transform.position);
			if(dist1 < dist)
			{
				currentGoal = g;
				//might be far from subgoal
				currentTile = subGoal;

		
				Tile t = aStar (subGoal);
		
				setPath (t);
			}
		}
		if (transform.position != subGoal.position) {
			transform.position = Vector3.MoveTowards (transform.position, subGoal.position, speed * Time.deltaTime);
		} else {
			subGoal = (Tile)path.Pop ();
		}

		if (transform.position == currentGoal.transform.position) {
			golds.Remove(currentGoal);
			setGoal();

			currentTile = subGoal;

			Tile t = aStar (subGoal);
	

			setPath (t);
		}*/
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

class Tree{


}

class Selector{
	
}
class Inverter{

}
class Sequence{

}
class Leaf{
	
}



