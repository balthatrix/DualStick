using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour {


	public int toYMapEdge = 30;
	public int toXMapEdge = 30;

	public GameObject[] enemies;

	public int waveLevel = 1;

	public static WaveManager instance = null;

	// Use this for initialization
	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
		
		DontDestroyOnLoad (gameObject);
	}

	// Use this for initialization
	void Start () {
		waveLevel = 1;
		//NextWave ();
	}

	private void NextWave() {
		Debug.Log ("Next wave spawning in.....");
		List<Vector3> spawnLocs = WaveManager.squareSpawnPattern (60, 20, 10, new Vector3 (0,0,0));
		foreach (Vector3 loc in spawnLocs) {
			GameObject newGo = Instantiate (enemies [0]);
			newGo.transform.position = loc;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static List<Vector3> squareSpawnPattern(int squareSize,  int count, int eachOffset, Vector3 centerOffset) {
		List<Vector3> ret = new List<Vector3>();
		//starts from the bottom left:
		int total = 0;
		int perSide = (int) Mathf.Floor (squareSize / eachOffset);
		int totalSides = (int) Mathf.Ceil (count / perSide);

		//start the spawn locations on bottom left...
		string dir = "right";
		Vector3 bottomLeftOffset = new Vector3 ( -(squareSize / 2), -(squareSize / 2), 0f);
		Vector3 runningLocation = Vector3.zero + centerOffset + bottomLeftOffset;

		Vector3 toRight = new Vector3 ( eachOffset, 0f, 0f);
		Vector3 toLeft  = new Vector3 (-eachOffset, 0f, 0f);
		Vector3 toUp    = new Vector3 (0f,  eachOffset, 0f);
		Vector3 toDown  = new Vector3 (0f, -eachOffset, 0f);

		for (int i = 0; i < totalSides; i++) {
			for (int k = 0; k < perSide; k++) {
				ret.Add (new Vector3(runningLocation.x, runningLocation.y, runningLocation.z));
				total++;
				if (total >= count)
					return ret;
				else {
					//add offset to the running location
					switch(dir) {
					case "right":
						runningLocation += toRight;
						break;
					case "left": 
						runningLocation += toLeft;
						break;
					case "up":
						runningLocation += toUp;
						break;
					case "down":
						runningLocation += toDown;
						break;
					}
				}
			}
			//toggle direction: 
			switch(dir) {
			case "right":
				dir = "up";
				break;
			case "left": 
				dir = "down";
				break;
			case "up":
				dir = "left";
				break;
			case "down":
				dir = "right";
				break;
			}
		}


		return ret;
	}

	public class SpawnChunk {

		public float timeOffset;

		public List<Spawn> spawnList;

		public virtual IEnumerator StartSpawning () {
			return null;
		}


		public void addSpawn(GameObject spawn, Vector3 cameraOffset, float delayAfter) {
			spawnList.Add (new Spawn (spawn, cameraOffset, delayAfter));
		}



		public class Spawn {
			GameObject enemy;
			Vector3 cameraOffset;
			float delayAfter;
			public Spawn(GameObject enemy, Vector3 cameraOffset, float delayAfter) {
				this.enemy = enemy;
				this.cameraOffset = cameraOffset;
				this.delayAfter = delayAfter;
			}
		}
	}
}
