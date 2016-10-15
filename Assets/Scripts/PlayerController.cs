using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour {


	public float speed;
	public float rayCooldown;
	private bool rayCooling;
	public RayManager rays;
	public float raySpeed;

	private Rigidbody2D rb2d;


	private List<KeyCode> shootDirStack;




	void Start()
	{
		rayCooling = false;

		shootDirStack = new List<KeyCode> ();
		rb2d = GetComponent<Rigidbody2D> ();
		rb2d.angularVelocity = 360f;
		rays = RayManager.instance.GetComponent<RayManager> ();
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
		UpdateDirStack();
		if (shootDirStack.Count > 0 && !rayCooling) {
			LaunchRay (GetShootUnitVector () * raySpeed);
		}
	}

	private void LaunchRay(Vector2 dir) {
		GameObject newRay = rays.PopAvailable();
		Rigidbody2D rayRb = newRay.GetComponent<Rigidbody2D> ();
		rayRb.velocity = dir;
		newRay.transform.position = transform.position;
		newRay.transform.Rotate (0,0,ZRotationFromVect2(dir));
		StartCoroutine (CooldownRay());
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
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		float x = moveHorizontal * speed;
		float y = moveVertical * speed;
		Vector2 dir = new Vector2 (x,y);
		dir = Vector2.ClampMagnitude (dir, speed);


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



}
