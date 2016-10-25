using UnityEngine;
using System.Collections;


public class BaseEnemy : MonoBehaviour {

	private static int idCount = 0;
	public int id;
	public int hitPoints;
	public Rigidbody2D rb2d;
	private SpriteRenderer sprite;
	public GameObject explosion;



	public AudioClip dieClip;
	public AudioSource hitSound;




	public bool movementPaused;

	//private AudioSource dieSource;

	public float dieTime;

	public bool IsDead() {
		return hitPoints <= 0;
	}

	// Use this for initialization
	public virtual void Start () {

		id = BaseEnemy.idCount;
		BaseEnemy.idCount++;

		rb2d = GetComponent<Rigidbody2D> ();
		rb2d.angularVelocity = -150f;

		//Debug.Log ("rigidbody: " + rb2d.ToString ());

		sprite = GetComponent<SpriteRenderer> ();



		//managed by the sound manager so when the game object is removed, the sound doesn't stop.
		SoundManager.instance.Register (tag+"Die"+id);
		SoundManager.instance.SetClipFor (tag + "Die"+id, dieClip);
		SoundManager.instance.SetVolumeFor (tag + "Die"+id, 0.45f);

	}



	void Update () {
	}




	public void TakeDamage(int amount) {
		hitPoints -= amount;
		if (hitPoints <= 0) {
			StartCoroutine (Die ());
		} else {
			try {
			hitSound.pitch = SoundManager.instance.RandomPitch (1.5f, 2.0f);
			hitSound.Play ();
			} catch (UnityException e) {
				Debug.Log ("Thing didn't have sound: " + this);
			}
		}
	} 

	public virtual IEnumerator Die() {
		SoundManager.instance.PlaySourceFor (tag+"Die"+id, .5f, .8f);
		
		GameObject explode = Instantiate (explosion);
		explode.transform.position = transform.position;
			



		//Debug.Log ("rigidbody in Nav: " + GetComponent<Rigidbody2D>().ToString ());
		rb2d.velocity = Vector3.zero;
		rb2d.angularVelocity = -75f;

		CircleCollider2D coll = GetComponent<CircleCollider2D> ();
		coll.enabled = false;

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
