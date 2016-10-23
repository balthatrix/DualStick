using UnityEngine;
using System.Collections;


public class NavigatorComponent : MonoBehaviour {
	
	public Rigidbody2D rb2d;

	public GameObject player;
	public Vector3 currentNavTarget;

	public BaseEnemy enemyComponent;



	public float moveSpeed;
	public float navUpdateFreq;



	public bool movementPaused;

	//private AudioSource dieSource;

	public float dieTime;


	// Use this for initialization
	public virtual void Start () {



		player = GameObject.FindGameObjectWithTag ("Player");


		//Start navigation coroutine
		StartCoroutine(Navigate());
	}

	public  IEnumerator Navigate() {
		while (true) {
			if(!movementPaused && !enemyComponent.IsDead()) {
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


}
