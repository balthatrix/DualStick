using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;

public class MainRestart : MonoBehaviour {
	
	public void RestartMainScene() {
		SceneManager.LoadScene (1);
		//RayManager.instance.OnRestart ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
