using UnityEngine;
using UnityEngine;
using System.Collections;

public class StandOffNavigator : NavigatorComponent {
	
	public float rotationPointDeg;
	public int keptDistance;
	public override void Start() {
		base.Start ();
		rotationPointDeg = RandomOutskirtWithQuad ();
	}

	private float RandomOutskirtWithQuad() {
		Vector3 offset = transform.position - player.transform.position;
		if (offset.x <= 0 && offset.y > 0) { //up left quad ~ (0-90)
			return Random.Range(0,90);
		} else if (offset.x <= 0 && offset.y <= 0) { //bottom left quad ~ (90-180)
			return Random.Range(90,180);
		} else if (offset.x > 0 && offset.y <= 0) { //bottom right quad  ~ (180-270)
			return Random.Range(180,270);
		} else { //up right quad  ~ (270-360)
			return Random.Range(270,360);
		}
	}

	// Update is called once per frame
	public override Vector3 FindTarget() {
		rotationPointDeg = RandomOutskirtWithQuad ();
		//return base.FindTarget ();
		Vector3 targetOffset = Vector3.up * keptDistance;
		targetOffset = Quaternion.Euler (0f, 0f, rotationPointDeg) * targetOffset;

		return player.transform.position + targetOffset;
	}
}