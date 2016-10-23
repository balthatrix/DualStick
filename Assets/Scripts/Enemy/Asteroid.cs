using UnityEngine;
using System.Collections;

public class Asteroid : BaseEnemy {


	public Vector3 startVelocity;

	public override void Start() {
		base.Start ();
		Debug.Log ("starting for " + gameObject.name);
		rb2d.velocity = startVelocity;
		SoundManager.instance.SetVolumeFor (tag + "Die"+id, 2.5f);
	}

	public void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("Collide with " + gameObject.name + "  :" + other.tag);
	}

	public override IEnumerator Die() {
		Debug.Log ("Asteroid dying!");
		StartCoroutine(base.Die ());
		yield return new WaitForSeconds (1);
	}
}
