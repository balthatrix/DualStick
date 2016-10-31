using UnityEngine;
using System.Collections;

public class EnemyRayShooter : MonoBehaviour {
	public float cooldown;
	bool rayCooling = false;
	private GameObject player;
	private Rigidbody2D playerRb;
	private StandOffNavigator nav;
	public int raySpeed;
	public GameObject ray;


	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		playerRb = player.GetComponent<Rigidbody2D> ();
		nav = GetComponent<StandOffNavigator> ();

	}
	
	// Update is called once per frame
	void Update () {
		if (rayCooling)
			return;
		if ((player.transform.position - transform.position).sqrMagnitude > (nav.keptDistance * nav.keptDistance))
			return;
		
		Vector3 dir = ((player.transform.position + (Vector3) (playerRb.velocity * 0.5f)) - transform.position).normalized;
		LaunchRay (dir * raySpeed);
	}

	IEnumerator CooldownRay() {
		rayCooling = true;
		yield return new WaitForSeconds (cooldown + Random.value * 2.0f);
		rayCooling = false;
	}

	private float ZRotationFromVect2(Vector2 vect) {
		return Mathf.Atan2(vect.y, vect.x) * Mathf.Rad2Deg + 180f;
	}

	private void LaunchRay(Vector2 dir) {
		GameObject newRay = Instantiate (ray);
		Rigidbody2D rayRb = newRay.GetComponent<Rigidbody2D> ();
		rayRb.velocity = dir;
		newRay.transform.position = transform.position;
		newRay.transform.Rotate (0,0,ZRotationFromVect2(dir));

		StartCoroutine (CooldownRay());
	}
}
