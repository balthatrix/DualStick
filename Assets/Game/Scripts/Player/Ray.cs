using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ray : MonoBehaviour {
	

	private RayManager rays;
	private Rigidbody2D rgbd;
	public Vector2 lastVelocity;
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



		if(other.CompareTag("Player") || other.CompareTag("PlayerMissile") ||  other.CompareTag("EnemyMissile") || other.CompareTag("Background"))
			return;



		if (other.CompareTag ("Enemy")) {
			alreadyHit = true;

			BaseEnemy en = other.GetComponent<BaseEnemy> ();
			en.TakeDamage (1);
		}




		GameObject newParticle = Instantiate(hitParticle);
		Transform tip = gameObject.transform.GetChild (0);
		newParticle.transform.position = tip.position;
		DestroyRay (this.gameObject);
	}

	void DestroyRay(GameObject ray) {
		StartCoroutine (ResetAlreadyHit ());
		lastVelocity = rgbd.velocity;
		rays.ReplenishAvailable (ray);
	}
}
