using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour {


	public int toYMapEdge = 30;
	public int toXMapEdge = 30;

	public GameObject[] enemies;

	private int waveLevel;

	// Use this for initialization
	void Start () {
		waveLevel = 1;
	}

	private void NextWave() {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static List<Vector3> squarePattern(int squareSize, int count, Vector3 centerOffset) {
		List<Vector3> ret = new List<Vector3>();

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
