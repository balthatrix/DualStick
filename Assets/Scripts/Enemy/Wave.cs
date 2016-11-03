using UnityEngine;
using System.Collections;

public class Wave : MonoBehaviour {

	private int numToKill;
	public int timeAlotted;
	public float startedAt;
	private bool finished = false;

	// Use this for initialization
	void Start () {
		startedAt = Time.time;
		if (timeAlotted == null || timeAlotted == 0f) {
			timeAlotted = 40;
		}
		TallyChildrenSpawns ();
		ChildrenSpawnsActivate ();
	}
	void Update() {
		if (TimeForNext()) {
			Debug.Log ("Force completing the wave");
			StartCoroutine(FinishWave ());
		}
	}

	bool TimeForNext() {
		return Time.time - startedAt > timeAlotted;
	}



	public void ChildrenSpawnsActivate() {
		int children = transform.childCount;
		for (int i = 0; i < children; ++i) {
			GameObject ch = transform.GetChild (i).gameObject;
			if (ch != null) {
				ch.GetComponent<Spawn> ().SetParentWave(this);
				ch.SetActive (true);
			}
		}
	}

	public void TallyChildrenSpawns() {
		int children = transform.childCount;
		for (int i = 0; i < children; ++i) {
			TallyNumToKill (transform.GetChild(i).gameObject);
		}

		Debug.Log ("Done with tally: " + numToKill);
	}

	public void TallyNumToKill(GameObject childSpawn) {
		Spawn sp = childSpawn.GetComponent<Spawn> ();
		if (sp.prefab != null)
			numToKill++;
		int children = childSpawn.transform.childCount;
		for (int i = 0; i < children; ++i) {
			TallyNumToKill(childSpawn.transform.GetChild (i).gameObject);
		}
	}

	public void CheckOffList(GameObject died) {
		numToKill--;

		//Debug.Log ("Tally update: " + numToKill);
		if (numToKill <= 0) {
			StartCoroutine(FinishWave ());
		}
	}

	//nessecary for clearing out small asteroids..... but is it really nessecary?
	//used to wait for all to be destroyed, but now more exciting
	public IEnumerator FinishWave() {
		if(!finished) {
		finished = true;
		int children = transform.childCount;
		for (int i = 0; i < children; ++i) {
			transform.GetChild (i).gameObject.GetComponent<Spawn> ().ForceInSpawns ();
		}
		yield return new WaitForEndOfFrame ();
		GameObject[] enemiesLeft = GameObject.FindGameObjectsWithTag ("Enemy");
		while (enemiesLeft.Length > 0) {
			yield return new WaitForSeconds (1);
			if (TimeForNext ())
				break;
			enemiesLeft = GameObject.FindGameObjectsWithTag ("Enemy");
			//Debug.Log ("Wave trying to finish...: " + enemiesLeft.Length);
		}

		WaveManager.instance.WaveComplete(this);
		StartCoroutine (selfDestruct ());
//		//Debug.Log ("Wave finito!");
//		WaveManager.instance.WaveComplete(this);
		}
	}

	IEnumerator selfDestruct() {
		//wait for the rest of the spawn to get in if they are there...
		yield return new WaitForSeconds (3.0f);
		Destroy (gameObject);
	}
}
