using UnityEngine;
using System.Collections;

public class TitleController : MonoBehaviour {

	public Rigidbody2D rb2d;
	float prevX;
	float prevY;

	public void Start() {
		rb2d = GetComponent<Rigidbody2D> ();
		rb2d.velocity = new Vector2(0f, 10f);
		rb2d.velocity = Quaternion.Euler (0f, 0f, Random.Range (0f, 360f)) * rb2d.velocity;
		prevX = rb2d.velocity.x;
		prevY = rb2d.velocity.y;
	}



	public IEnumerator OnCollisionEnter2D(Collision2D other) {
		float xCom = other.collider.offset.x;
		float yCom = other.collider.offset.y;
		Vector2 newVel;
		if (Mathf.Abs (xCom) > Mathf.Abs (yCom)) {
			newVel = new Vector2 (-prevX, rb2d.velocity.y);
			prevX = -prevX;
		} else {
			newVel = new Vector2 (rb2d.velocity.x, -prevY);
			prevY = -prevY;
		}
	 
		yield return new WaitForEndOfFrame ();
		rb2d.velocity = newVel;


	}
}
