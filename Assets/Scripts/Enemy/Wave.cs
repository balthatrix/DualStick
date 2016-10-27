using UnityEngine;
using System.Collections;

public class Wave : MonoBehaviour {

	private int numToKill;

	// Use this for initialization
	void Start () {
		TallyChildrenSpawns ();
		ChildrenSpawnsActivate ();
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
	public IEnumerator FinishWave() {

		yield return new WaitForEndOfFrame ();
		GameObject[] enemiesLeft = GameObject.FindGameObjectsWithTag ("Enemy");
		while (enemiesLeft.Length > 0) {
			yield return new WaitForSeconds (1);
			enemiesLeft = GameObject.FindGameObjectsWithTag ("Enemy");
			//Debug.Log ("Wave trying to finish...: " + enemiesLeft.Length);
		}
		//Debug.Log ("Wave finito!");
		WaveManager.instance.WaveComplete(this);
	}
}
