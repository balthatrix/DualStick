using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour {


	public float speed;
	public float rayCooldown;
	public float raySpeed;
	private bool alternateRayShot;
	private bool rayCooling;
	public bool dead;

	private int hitPoints;

	private Vector3 lastVelocity;

	public Camera cam;

	public float dieTime;


	public GameObject explosion;

	private RayManager rays;

	private Rigidbody2D rb2d;
	public AudioSource rayAudioSrc;



	private List<KeyCode> shootDirStack;

	public AudioClip deathClip;

	//public AudioSource deathSource;

	public SpriteRenderer sprite;




	void Start()
	{
		dead = false;
		rayCooling = false;

		shootDirStack = new List<KeyCode> ();
		rb2d = GetComponent<Rigidbody2D> ();
		alternateRayShot = false;
		rb2d.angularVelocity = 360f;
		rays = RayManager.instance.GetComponent<RayManager> ();
		hitPoints = 3;
		SoundManager.instance.Register (tag + "Die");
	} 

	private void ShootKeyPressed(KeyCode key) {
		if(!shootDirStack.Contains(key))
			shootDirStack.Add (key);
	}

	private void ShootKeyUnpressed(KeyCode key) {
		shootDirStack.Remove (key);
	}

	private void DebugShootStack() {
		string msg = "";
		for (int i = 0; i < shootDirStack.Count; i++) {
			msg += shootDirStack[i].ToString() + ", ";
		}
		Debug.Log (msg);
	}



	void Update() {
		//This is how to launch ray using arrow keys:
		UpdateDirStack();
		if (shootDirStack.Count > 0 && !rayCooling) {
			LaunchRay (GetShootUnitVector () * raySpeed);
		}

		//This is how to launch using mouse pos:
		if (Input.GetMouseButton(0) && !rayCooling) {
			Vector2 dir = (cam.ScreenToWorldPoint (Input.mousePosition) - transform.position);
			LaunchRay (dir.normalized * raySpeed);
		}
	}

	private void LaunchRay(Vector2 dir) {
		LaunchSingleRay (dir);

		rayAudioSrc.pitch = SoundManager.instance.RandomPitch (1.5f, 2.5f);
		rayAudioSrc.Play ();

		StartCoroutine (CooldownRay());
	}

	private void LaunchSingleRay(Vector2 dir) {
		Vector3 cross = new Vector3 (dir.x, dir.y, 1.0f);
		Vector3 perp = Vector3.Cross (dir, cross).normalized * .35f;
		GameObject newRay1 = rays.PopAvailable();
		Rigidbody2D rayRb1 = newRay1.GetComponent<Rigidbody2D> ();
		rayRb1.velocity = dir;
		if (alternateRayShot) {
			newRay1.transform.position = transform.position + perp;
		} else {
			newRay1.transform.position = transform.position - perp;
		}
		alternateRayShot = !alternateRayShot;
		newRay1.transform.Rotate (0,0,ZRotationFromVect2(dir));
	}

	private void LaunchDoubleRay(Vector2 dir) {
		Vector3 cross = new Vector3 (dir.x, dir.y, 1.0f);
		Vector3 perp = Vector3.Cross (dir, cross).normalized * .5f;
		GameObject newRay1 = rays.PopAvailable();
		GameObject newRay2 = rays.PopAvailable();
		Rigidbody2D rayRb1 = newRay1.GetComponent<Rigidbody2D> ();
		Rigidbody2D rayRb2 = newRay2.GetComponent<Rigidbody2D> ();
		rayRb1.velocity = dir;
		rayRb2.velocity = dir;

		newRay1.transform.position = transform.position + perp;
		newRay2.transform.position = transform.position - perp;
		newRay1.transform.Rotate (0,0,ZRotationFromVect2(dir));
		newRay2.transform.Rotate (0,0,ZRotationFromVect2(dir));
	}

	private float ZRotationFromVect2(Vector2 vect) {
		return Mathf.Atan2(vect.y, vect.x) * Mathf.Rad2Deg + 180f;
	}

	IEnumerator CooldownRay() {
		rayCooling = true;
		yield return new WaitForSeconds (rayCooldown);
		rayCooling = false;
	}



	void FixedUpdate()
	{
		if (dead)
			return;
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		float x = moveHorizontal * speed;
		float y = moveVertical * speed;
		Vector2 dir = new Vector2 (x,y);
		dir = Vector2.ClampMagnitude (dir, speed);

		lastVelocity = dir;
		rb2d.velocity = dir;
		if (rb2d.angularVelocity < 360f) {
			rb2d.angularVelocity = 360f;
		}
	}

	private void UpdateDirStack() {
		//just to debug when updates happen
		bool updateHappened = false;
		if (Input.GetKeyUp (KeyCode.RightArrow)) {
			ShootKeyUnpressed (KeyCode.RightArrow);
			updateHappened = true;
		}
		if (Input.GetKeyUp (KeyCode.LeftArrow)) {
			ShootKeyUnpressed (KeyCode.LeftArrow);
			updateHappened = true;
		}
		if (Input.GetKeyUp (KeyCode.UpArrow)) {
			ShootKeyUnpressed (KeyCode.UpArrow);
			updateHappened = true;
		}
		if (Input.GetKeyUp (KeyCode.DownArrow)) {
			ShootKeyUnpressed (KeyCode.DownArrow);
			updateHappened = true;
		}

		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			ShootKeyPressed (KeyCode.RightArrow);
			updateHappened = true;
		}
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			ShootKeyPressed (KeyCode.LeftArrow);
			updateHappened = true;
		}
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			ShootKeyPressed (KeyCode.UpArrow);
			updateHappened = true;
		}
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			ShootKeyPressed (KeyCode.DownArrow);
			updateHappened = true;
		}
		//if(updateHappened)
			//DebugShootStack ();
	}

	private Vector2 GetShootUnitVector() {
		Vector2 ret;
		if (shootDirStack.Count == 1) {
			ret = VectFromSingleDir (shootDirStack[0]);
		} else {
			KeyCode a = shootDirStack [shootDirStack.Count - 1];
			KeyCode b = shootDirStack [shootDirStack.Count - 2];

			if (ShootDirsConflict (a, b)) {
				ret = VectFromSingleDir (a);
			} else {
				ret = VectFromTwoDir (shootDirStack);
			}

		}
		return Vector2.ClampMagnitude (ret, 1f);
	}

	private Vector2 VectFromSingleDir(KeyCode dir) {
		switch (dir) {
		case KeyCode.UpArrow:
			return new Vector2 (0, 1);
		case KeyCode.DownArrow:
			return new Vector2 (0, -1);
		case KeyCode.LeftArrow:
			return new Vector2 (-1, 0);
		case KeyCode.RightArrow:
			return new Vector2 (1, 0);
		default:
			return new Vector2 (1, 0);
		}
	}

	private Vector2 VectFromTwoDir(List<KeyCode> dirStack) {
		if (dirStack.Contains(KeyCode.UpArrow) && dirStack.Contains(KeyCode.RightArrow)) {
			return new Vector2 (1, 1);
		} else if (dirStack.Contains(KeyCode.DownArrow) && dirStack.Contains(KeyCode.RightArrow)) {
			return new Vector2 (1, -1);
		} else if (dirStack.Contains(KeyCode.DownArrow) && dirStack.Contains(KeyCode.LeftArrow)) {
			return new Vector2 (-1, -1);
		} else if (dirStack.Contains(KeyCode.LeftArrow) && dirStack.Contains(KeyCode.UpArrow)) {
			return new Vector2 (-1, 1);
		} else {
			return new Vector2 (1, 1);
		}
	}

	private bool ShootDirsConflict(KeyCode a, KeyCode b) {
		if (a == KeyCode.UpArrow && b == KeyCode.DownArrow)
			return true;
		if (a == KeyCode.RightArrow && b == KeyCode.LeftArrow)
			return true;
		return false;
	}

	public IEnumerator Die() {
		if (!dead) {
			dead = true;
			SoundManager.instance.PlaySingleFor(tag+"Die", deathClip, .5f, .75f);

			GameObject explode = Instantiate (explosion);

			explode.transform.position = transform.position;


			//Debug.Log ("rigidbody in Nav: " + GetComponent<Rigidbody2D>().ToString ());
			rb2d.angularVelocity = -75f;

			rb2d.velocity = new Vector3 (0,0,0);

			CircleCollider2D coll = GetComponent<CircleCollider2D> ();
			coll.enabled = false;

			float remaining = dieTime;
			while (remaining > 0.0f) {
				remaining -= Time.deltaTime;
				sprite.color = new Color (sprite.color.r, sprite.color.g, sprite.color.b, (remaining / dieTime));

				yield return null;
			}
			Debug.Log ("Dying: ");
			gameObject.SetActive (false);
		}
	}

	public void TakeDamage(int amount) {
		Debug.Log ("Takin damsey: " + amount);
		hitPoints -= amount;
		if (hitPoints <= 0) {
			UIManager.instance.SetHealthColor ("red");
			StartCoroutine (Die ());
		} else {
			if (hitPoints == 1) {
				UIManager.instance.SetHealthColor ("red");
			} else {
				UIManager.instance.SetHealthColor ("yellow");
			}
		}

	}


}
