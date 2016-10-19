using UnityEngine;
using System.Collections;

public class Enemy1 : BaseEnemy {


	// Use this for initialization
	void Start () {
		base.Start ();

	}

	public override Vector3 FindTarget() {
		//for now just resort to base, but for future, this is how you can override this behavior
		return base.FindTarget ();
	}



}
