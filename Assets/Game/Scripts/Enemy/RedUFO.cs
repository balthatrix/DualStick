using UnityEngine;
using System.Collections;

public class RedUFO : BaseEnemy {



	public GameObject novaIn;
	public GameObject novaOut;
	public float supernovaTime;

	public bool playerColliding;

	// Use this for initialization
	public override void Start () {
		base.Start ();


		rb2d.angularVelocity = -150f;

		//Debug.Log ("rigidbody: " + rb2d.ToString ());





	}

	public void OnTriggerEnter2D (Collider2D other) {
		if (other.CompareTag ("Player")) {
			rb2d.velocity = new Vector3 (0,0,0);
			movementPaused = true;
			playerColliding = true;
			StartCoroutine (AttemptSupernova (other.gameObject));
		}
	}

	public void OnTriggerExit2D (Collider2D other) {
		if (other.CompareTag ("Player")) {
			playerColliding = false;
		}
	}

	public IEnumerator AttemptSupernova(GameObject player) {
		GameObject ni = Instantiate (novaIn);
		ni.transform.position = transform.position;
		yield return new WaitForSeconds (supernovaTime);
		GameObject no = Instantiate (novaOut);
		no.transform.position = transform.position;
		if (playerColliding) {
			player.GetComponent<PlayerController> ().TakeDamage (1);
			movementPaused = false;
			if (playerColliding && !player.GetComponent<PlayerController>().dead) {
				StartCoroutine (AttemptSupernova (player));
			}
		} else {
			movementPaused = false;
		}
	}






}
