using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;
	private Vector3 offset;
	public float parallaxRatio;

	// Use this for initialization
	void Start () {
		offset = transform.position - player.transform.position;
	}
	
	// LateUpdate is called once per frame, after all other Updates, 
	// Before rendering
	void Update () {
		transform.position = ((player.transform.position * -1f) * parallaxRatio) + offset;
	}
}
