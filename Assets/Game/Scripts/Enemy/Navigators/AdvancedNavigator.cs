using UnityEngine;
using System.Collections;

public class AdvancedNavigator : NavigatorComponent {
	public Rigidbody2D playerRb;
	public override void Start() {
		base.Start ();

		playerRb = player.GetComponent<Rigidbody2D> ();
	}
	// Update is called once per frame
	public override Vector3 FindTarget() {
		if(playerRb == null)
			return player.transform.position;
		if ((player.transform.position - transform.position).sqrMagnitude < 36) {//if the distance is less than 4 units...
		//	Debug.Log("going normzies");
		//	return player.transform.position;

			return player.transform.position + (((Vector3)playerRb.velocity) * 0.5f);
		} else {
			return player.transform.position + ((Vector3)playerRb.velocity);
		}
	}
}
