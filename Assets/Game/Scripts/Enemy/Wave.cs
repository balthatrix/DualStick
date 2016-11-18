using UnityEngine;
using System.Collections;
using System;

public class Wave : MonoBehaviour {

	private int numToKill;
	public int timeAlotted;
	public float startedAt;

	private bool ending;

	private int wholeLeft = -1;

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
		if (TimeForNext() && !finished) {
			Debug.Log ("Force completing the wave");
			StartCoroutine(FinishWave ());
		}
		SetUntilNext ();
	}

	void SetUntilNext() {
		float epoch = SecondsSinceStarted ();
		float exactLeft = timeAlotted - epoch;

		int left = (int)exactLeft;
		if (wholeLeft != left) {
			if (!ending) {
				wholeLeft = left;
				UpdateUntilNextText ();
				if (wholeLeft <= 5 && wholeLeft >= 0) {
					if (wholeLeft > 0) {
						UIManager.instance.DoFlash (wholeLeft.ToString (), .35f, .35f);
					} else {
						UIManager.instance.DoFlash ("WARNING!!!", .35f, .35f);

					}
				}
			}
		}
	}



	void UpdateUntilNextText() {
		TimeSpan timeSpan = TimeSpan.FromSeconds(wholeLeft);
		string timeText = timeSpan.Minutes.ToString ().PadLeft (2, '0') + ":" + timeSpan.Seconds.ToString ().PadLeft (2, '0');

		UIManager.instance.timeLeft.text = "Next Wave: " + timeText;
	}

	private float SecondsSinceStarted() {
		return Time.time - startedAt;
	}

	bool TimeForNext() {
		return SecondsSinceStarted() > timeAlotted;
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
		ending = true;
		//wait for the rest of the spawn to get in if they are there...
		yield return new WaitForSeconds (3.0f);
		Destroy (gameObject);
	}
}
