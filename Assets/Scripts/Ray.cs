using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ray : MonoBehaviour {
	

	private RayManager rays;
	private Rigidbody2D rgbd;
	public GameObject hitParticle;
	public bool alreadyHit = false;


	// Use this for initialization
	void Awake () {
		rays = RayManager.instance.GetComponent<RayManager> ();
		rgbd = GetComponent<Rigidbody2D> ();
	}

	IEnumerator ResetAlreadyHit() {
		yield return new WaitForEndOfFrame ();
		alreadyHit = false;
	}


	private void OnTriggerEnter2D(Collider2D other) {
		if (alreadyHit)
			return;

		Debug.Log ("ray hit something: " + other.tag);


		if(other.CompareTag("Player") || other.CompareTag("PlayerMissile") || other.CompareTag("Background"))
			return;

		if (other.CompareTag ("Enemy")) {
			alreadyHit = true;
			BaseEnemy en = other.GetComponent<BaseEnemy> ();
			en.TakeDamage (1);
		}




		//Debug.Log ("ray hit something other than player: " + other.tag);
		GameObject newParticle = Instantiate(hitParticle);
		Transform tip = gameObject.transform.GetChild (0);
		newParticle.transform.position = tip.position;
		DestroyRay (this.gameObject);
	}

	void DestroyRay(GameObject ray) {
		StartCoroutine (ResetAlreadyHit ());
		rays.ReplenishAvailable (ray);
	}
}
