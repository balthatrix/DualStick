using UnityEngine;
using System.Collections;
using System;
using System.IO;
using UnityEngine.UI;

using System.Runtime.Serialization.Formatters.Binary;

public class HighScoreOnMenu : MonoBehaviour {
	public GameObject highScore;
	// Use this for initialization
	void Start () {
		LoadHighScore ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void LoadHighScore() {
		if (File.Exists (Application.persistentDataPath + "/highScore.dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/highScore.dat", FileMode.Open);
			PlayerData pd = (PlayerData)bf.Deserialize (file);
			file.Close ();
			Text t = highScore.GetComponent<Text> ();
			t.text = "High Score: " + pd.highScore;

		} else {
			highScore.SetActive (false);
		}

	}
}
