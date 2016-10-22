using UnityEngine;
using System.Collections;

public class BaseEnemy : MonoBehaviour {

	private static int idCount = 0;
	private int id;
	public int hitPoints;
	private Rigidbody2D rb2d;
	private SpriteRenderer sprite;
	public GameObject explosion;

	public GameObject player;
	public Vector3 currentNavTarget;


	public AudioClip dieClip;
	public AudioSource hitSound;

	public float moveSpeed;
	public float navUpdateFreq;

	public bool movementPaused;
	public bool playerColliding;


	public GameObject novaIn;
	public GameObject novaOut;


	//private AudioSource dieSource;

	public float dieTime;

	// Use this for initialization
	public void Start () {

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

		player = GameObject.FindGameObjectWithTag ("Player");

		//Start navigation coroutine
		StartCoroutine(Navigate());
	}

	public  IEnumerator Navigate() {
		while (true) {
			if(!movementPaused) {
				currentNavTarget = FindTarget ();
				Vector3 targetVel = (currentNavTarget - transform.position).normalized;
				targetVel *= moveSpeed;
//				if(targetVel.magnitude > moveSpeed) 
//					targetVel = Vector3.ClampMagnitude (targetVel, moveSpeed);
//				else
//					targetVel = targetVel 
				rb2d.velocity = targetVel;
			}
			yield return new WaitForSeconds (navUpdateFreq);
		}
	}
	
	// Update is called once per frame
	public virtual Vector3 FindTarget() {
		return player.transform.position;
	}


	void Update () {
	}


	public virtual void OnTriggerEnter2D (Collider2D other) {
		if (other.CompareTag ("Player")) {
			rb2d.velocity = new Vector3 (0,0,0);
			movementPaused = true;
			playerColliding = true;
			StartCoroutine (AttemptSupernova (other.gameObject));
		}
	}

	public virtual void OnTriggerExit2D (Collider2D other) {
		if (other.CompareTag ("Player")) {
			playerColliding = false;
		}
	}

	public IEnumerator AttemptSupernova(GameObject player) {
		GameObject ni = Instantiate (novaIn);
		ni.transform.position = transform.position;
		yield return new WaitForSeconds (0.25f);
		GameObject no = Instantiate (novaOut);
		no.transform.position = transform.position;
		if (playerColliding) {
			StartCoroutine(player.GetComponent<PlayerController> ().Die ());
		} else {
			movementPaused = false;
		}
	}


	public void TakeDamage(int amount) {
		hitPoints -= 1;



		if (hitPoints <= 0) {
			StartCoroutine (Die ());
		} else {
			hitSound.pitch = SoundManager.instance.RandomPitch (1.5f, 2.0f);
			hitSound.Play ();
		}
	} 

	public IEnumerator Die() {
		SoundManager.instance.PlaySourceFor (tag+"Die"+id, .50f, .80f);
		
		GameObject explode = Instantiate (explosion);
		explode.transform.position = transform.position;
			



		//Debug.Log ("rigidbody in Nav: " + GetComponent<Rigidbody2D>().ToString ());
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
