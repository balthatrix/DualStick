using UnityEngine;
using System.Collections;

public class Asteroid : BaseEnemy {


	public GameObject smallerAsteroid;

	public override void Start() {
		base.Start ();
		//rb2d.velocity = startVelocity;
		SoundManager.instance.SetVolumeFor (tag + "Die" + id, 2.5f);
	}


	public void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("Background")) {
			float xCom = other.offset.x;
			float yCom = other.offset.y;
			if (Mathf.Abs (xCom) > Mathf.Abs (yCom)) {
				rb2d.velocity = new Vector2 (-rb2d.velocity.x, rb2d.velocity.y);
			} else {
				rb2d.velocity = new Vector2 (rb2d.velocity.x, -rb2d.velocity.y);
			}
		} else if (other.CompareTag ("PlayerMissile")) {
			Rigidbody2D otherVel = other.GetComponent<Rigidbody2D> ();


			if (name.Contains ("LargeAsteroid")) {
				Vector2 vect = other.GetComponent<Ray> ().lastVelocity.normalized * 3.0f;
				rb2d.velocity = rb2d.velocity + vect;
			}

		} else if (other.CompareTag ("Player")) {
			Vector2 vect = other.GetComponent<Rigidbody2D> ().velocity.normalized * 4.0f;
			StartCoroutine(other.GetComponent<PlayerController> ().Die ());
			rb2d.velocity = rb2d.velocity + vect;
			TakeDamage (2);
		}

			//else if(other.GetComponent<Asteroid>() != null) {
//			Vector3 diff = other.transform.position - transform.position;
//			float xCom = diff.x;
//			float yCom = diff.y;
//			if (Mathf.Abs (xCom) > Mathf.Abs (yCom)) {
//				Debug.Log ("collision was with a side");
//				rb2d.velocity = new Vector2 (-rb2d.velocity.x, rb2d.velocity.y);
//			} else {
//				Debug.Log ("collision was with a top/botom");
//				rb2d.velocity = new Vector2 (rb2d.velocity.x, -rb2d.velocity.y);
//			}
//		}
	}

	public override IEnumerator Die() {
		if (name.Contains ("LargeAsteroid")) {
			for (int i = 0; i < 3; i++) {
				GameObject smAsteroid = Instantiate (smallerAsteroid);
				smAsteroid.transform.position = transform.position;
				Rigidbody2D rb = smAsteroid.GetComponent<Rigidbody2D> ();

				rb.velocity = rb2d.velocity * 1.35f;
				rb.velocity = Quaternion.Euler(0f,0f, Random.Range(-45, 45)) * rb.velocity;
			}
		}
		StartCoroutine(base.Die ());
		yield return new WaitForSeconds(0);
	}
}
