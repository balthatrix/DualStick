using UnityEngine;
using System.Collections;

public class BaseEnemy : MonoBehaviour {

	public int hitPoints;
	private Rigidbody2D rb2d;
	private SpriteRenderer sprite;
	public GameObject explosion;
	public float dieTime;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		rb2d.angularVelocity = -150f;

		sprite = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnTriggerEnter2D (Collider2D other) {
		Debug.Log ("Enemy collision: " + other.tag);
		if (other.CompareTag ("PlayerMissile")) {
			StartCoroutine (Die ());
		}
	}

	IEnumerator Die() {
		GameObject explode = Instantiate (explosion);
		explode.transform.position = transform.position;
		CircleCollider2D coll = GetComponent<CircleCollider2D> ();
		coll.enabled = false;

		ParticleSystem particle = explode.GetComponent<ParticleSystem> ();
		rb2d.angularVelocity = -75f;
		float remaining = dieTime;
		while (remaining > 0.0f) {
			remaining -= Time.deltaTime;
			sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, (remaining/dieTime));




			yield return null;
		}
		explode.SetActive (false);
		gameObject.SetActive (false);
	}
}
