using UnityEngine;
using System.Collections;

public class Asteroid : BaseEnemy {


	public Vector3 startVelocity;

	public override void Start() {
		base.Start ();
		rb2d.velocity = startVelocity;

		Debug.Log ("start velocity: " + rb2d.velocity.ToString ());
		SoundManager.instance.SetVolumeFor (tag + "Die" + id, 2.5f);
	}

	void OnCollisionEnter2D (Collision2D other) {

		foreach(ContactPoint2D contact in other.contacts)
		{	

			Vector2 otherV = new Vector2 (other.gameObject.transform.position.x, other.gameObject.transform.position.y);
			Vector2 suggestionOffset = otherV - contact.point;

			float xCom = Mathf.Round (suggestionOffset.x);
			float yCom = Mathf.Round (suggestionOffset.y);
			Debug.Log ("current: " + rb2d.velocity.ToString ());
			if (Mathf.Abs (xCom) > Mathf.Abs (yCom)) {
				Debug.Log ("collision was with a side");
				rb2d.velocity = new Vector2 (-rb2d.velocity.x, rb2d.velocity.y);
			} else {
				Debug.Log ("collision was with a top/botom");
				rb2d.velocity = new Vector2 (rb2d.velocity.x, -rb2d.velocity.y);
			}
			Debug.Log ("after: " + rb2d.velocity.ToString ());

			//rb2d.velocity = startVelocity;
//		
//					newSugg.transform.SetParent (camera.transform);
//					newSugg.transform.localPosition = Vector3.zero + new Vector3(xCom, yCom, 1) * 10f;// + new Vector3(suggestionOffset.x, suggestionOffset.y, 0.0f);
//		
//					newSugg.transform.Rotate(0,0, Mathf.Atan2(yCom, xCom) * Mathf.Rad2Deg + 180f);
			//Debug.Log("new sugg t pos: "+ newSugg.transform.position);
		}
	}


	public void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("Background")) {
			float xCom = other.offset.x;
			float yCom = other.offset.y;
			if (Mathf.Abs (xCom) > Mathf.Abs (yCom)) {
				Debug.Log ("collision was with a side");
				rb2d.velocity = new Vector2 (-rb2d.velocity.x, rb2d.velocity.y);
			} else {
				Debug.Log ("collision was with a top/botom");
				rb2d.velocity = new Vector2 (rb2d.velocity.x, -rb2d.velocity.y);
			}
			Debug.Log ("Collide with " + gameObject.name + "  : " + other.tag + ": " + other.offset);
		}
	}

	public override IEnumerator Die() {
		Debug.Log ("Asteroid dying!");
		StartCoroutine(base.Die ());
		yield return new WaitForSeconds (1);
	}
}
