using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {
	public GameObject rayManager;
	// Use this for initialization
	void Awake () 
	{
		if (RayManager.instance == null) 
		{
			Instantiate (rayManager);
		}
	}

}