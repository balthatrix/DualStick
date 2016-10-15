using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ray : MonoBehaviour {
	

	private RayManager rays;


	// Use this for initialization
	void Awake () {
	
		rays = RayManager.instance.GetComponent<RayManager> ();
	}


	private void OnTriggerEnter2D(Collider2D other) {

		if(other.CompareTag("Player"))
			return;
		
		//Debug.Log ("ray hit something other than player: " + other.tag);
		DestroyRay (this.gameObject);
	}

	void DestroyRay(GameObject ray) {
		rays.ReplenishAvailable (ray);
	}
}
