using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyRay : MonoBehaviour {


	private Rigidbody2D rgbd;
	public Vector2 lastVelocity;
	public GameObject hitParticle;
	public bool alreadyHit = false;


	// Use this for initialization
	void Awake () {
		rgbd = GetComponent<Rigidbody2D> ();
	}



	private void OnTriggerEnter2D(Collider2D other) {

		if (other.CompareTag ("Enemy") || other.CompareTag("EnemyMissile") ||  other.CompareTag("PlayerMissile") || other.CompareTag("Background")) {
			return;
		}

		if (other.CompareTag ("Player")) {
			Debug.Log ("you dead");
			PlayerController p = other.GetComponent<PlayerController> ();
			p.StartCoroutine (p.Die());

		}

		GameObject newParticle = Instantiate(hitParticle);
		Transform tip = gameObject.transform.GetChild (0);
		newParticle.transform.position = tip.position;
		Destroy (this.gameObject);
	}

}
