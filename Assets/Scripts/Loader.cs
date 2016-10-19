using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {
	public GameObject rayManager;
	public GameObject soundManager;
	// Use this for initialization
	void Awake () 
	{
		if (RayManager.instance == null) 
		{
			Debug.Log ("Starting bullets");
			Instantiate (rayManager);
		}
		if (SoundManager.instance == null) 
		{
			Debug.Log ("Starting sound");
			Instantiate (soundManager);
		}
	}

}