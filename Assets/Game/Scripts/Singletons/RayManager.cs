﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RayManager : MonoBehaviour {

	public static RayManager instance = null;

	public List<GameObject> available;
	private Transform rayHolder;

	[HideInInspector] public int availableYOffset = 10000;
	[HideInInspector] public int availableXSpacing = 300;
	public int poolSize;
	public GameObject rayPrefab;

	public GameObject PopAvailable() {
		if (available.Count == 0) {
			return null;
		}
		GameObject ret = available [available.Count - 1];
		available.Remove (ret);
		ret.transform.SetParent (null);
		ret.transform.rotation = Quaternion.identity;
		return ret;
	}

	public void ReplenishAvailable(GameObject ray) {
		if(available.Contains(ray)) {
			//Debug.Log ("For some raisin, I already have this ray: ");
			return;
		}

		Rigidbody2D rgbd = ray.GetComponent<Rigidbody2D> ();
		ray.transform.SetParent (rayHolder);
		rgbd.velocity = Vector2.zero;
		ray.transform.position = LocationForNextAvailable ();
		available.Add (ray);
	}


	private void Awake() {
		if (instance == null) {
			instance = this;
		} else if(instance != this){
			Destroy (gameObject);
		}
		//DontDestroyOnLoad (gameObject);

	}

	private void Start() {
		InitPool ();
	}

	public void OnRestart() {
		InitPool ();
	}





	public void InitPool() {
		available = new List<GameObject> ();
		Destroy(GameObject.Find ("Rays"));
		rayHolder = new GameObject ("Rays").transform;

		for (int i = 0; i < poolSize; i++) {
			GameObject rayInst = Instantiate (rayPrefab,
				LocationForNextAvailable(),
				Quaternion.identity) as GameObject;

			rayInst.transform.SetParent (rayHolder);

			available.Add (rayInst);

			DontDestroyOnLoad (rayInst);
		}
	}


	private Vector3 LocationForNextAvailable() {
		return new Vector3 (available.Count * availableXSpacing, availableYOffset, 0f);
		//return new Vector3 (0, availableYOffset, 0f);
	}
}
