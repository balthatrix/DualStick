﻿using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour {

	public GameObject prefab;
	public float delay;
	public Vector3 startVelocity;
	public int velModifierLow;
	public int velModifierHigh;

	private bool spawned = false;

	private int spawnIncrements = 5;

	private Wave parentWave;

	public void SetParentWave(Wave w) {
		parentWave = w;
	}

	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds (delay);
		if (prefab != null) {
				StartCoroutine(SpawnIn ());


		} else {
			ChildrenSpawnsActivate ();
		}
	}

	public IEnumerator SpawnIn() {
		if (!spawned && prefab != null) {
			spawned = true;

			GameObject spawn = Instantiate (prefab);
			spawn.transform.position = transform.position;

			BaseEnemy en = spawn.GetComponent<BaseEnemy> ();

			spawn.SetActive (false);
			GameObject spawnParticle = Instantiate (en.spawn);
			spawnParticle.transform.position = transform.position;
			yield return new WaitForSeconds (en.spawnTime);







			spawn.SetActive (true);
			//spawn the thing in by scaling it up....
			float scaleIncrX = spawn.transform.localScale.x / spawnIncrements;
			float scaleIncrY = spawn.transform.localScale.y / spawnIncrements;
			spawn.transform.localScale = Vector2.zero;
			for (int i = 0; i < spawnIncrements; i++) {
				yield return new WaitForSeconds (.03f);
				spawn.transform.localScale = new Vector3 (spawn.transform.localScale.x + scaleIncrX, spawn.transform.localScale.y + scaleIncrY, 1.0f);
				//spawn.transform.localScale.x += scaleIncrX;
				//spawn.transform.localScale.y += scaleIncrY;
			}

			Rigidbody2D rb = spawn.GetComponent<Rigidbody2D> ();
			rb.velocity = startVelocity;
			rb.velocity = Quaternion.Euler (0f, 0f, Random.Range (velModifierLow, velModifierHigh)) * rb.velocity;

			en.AddDeathSubscription (new Enemy.OnEnemyDied (PrefabDied));
		}

		spawned = true;
	}

	public void PrefabDied(GameObject enemy) {
		//ignore this callback if the spawn instance is null (next wave began)
		if (this == null || parentWave == null)
			return;
		ChildrenSpawnsActivate ();
		parentWave.CheckOffList (enemy);
	}


	public void ForceInSpawns() {
		StartCoroutine(SpawnIn ());
		int children = transform.childCount;
		for (int i = 0; i < children; ++i) {
			GameObject ch = transform.GetChild (i).gameObject;
			if (ch != null) {
				ch.SetActive (true);
				ch.GetComponent<Spawn> ().ForceInSpawns();
			}
		}
	}




	public void ChildrenSpawnsActivate() {
		int children = transform.childCount;
		for (int i = 0; i < children; ++i) {
			GameObject ch = transform.GetChild (i).gameObject;
			if (ch != null) {
				ch.GetComponent<Spawn> ().SetParentWave(this.parentWave);
				ch.SetActive (true);
			}
		}
	}


}
