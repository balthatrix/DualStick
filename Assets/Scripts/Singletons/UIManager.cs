using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public static UIManager instance = null;
	public Text flagText;
	public Text currentWave;
	public Text timeLeft;
	public RawImage playerLife;

	private List<TextFlashParams> flashStack;
	private bool flagging;

	public class TextFlashParams {
		public string text;
		public float lerpTime;
		public float waitTime;
		public TextFlashParams(string text,float  lerpTime,float waitTime) {
			this.text = text;
			this.lerpTime = lerpTime;
			this.waitTime = waitTime;
		}
	}


	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
	}

	// Use this for initialization
	void Start () {
		flagging = false;
		flashStack = new List<TextFlashParams> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}



	public IEnumerator FlashText(TextFlashParams tf) {
		flagging = true;
		float startScale = 2.0f;
		float frames = 20;
		float waitIncr = tf.lerpTime / frames;
		float lerpIncr = 1.0f / frames;

		flagText.text = tf.text;
		RectTransform rect = flagText.GetComponent<RectTransform> ();

		for (int i = 0; i < frames; i++) {
			float newS = Mathf.Lerp (startScale, 1.0f, i * lerpIncr);

			rect.localScale = new Vector3 (newS, newS);

			yield return new WaitForSeconds(waitIncr);
		}

		rect.localScale = new Vector3 (1.0f, 1.0f);
		yield return new WaitForSeconds (tf.waitTime);
		flagText.text = "";
		if (flashStack.Count > 0) {
			TextFlashParams nextFlash = flashStack [0];
			flashStack.Remove (nextFlash);
			StartCoroutine (FlashText(nextFlash));
		} else {
			flagging = false;
		}
	}

	public void DoFlash(string text, float lerpTime, float waitTime) {
		if (!flagging) {
			Debug.Log ("flashing " + text + " " + lerpTime + " " + waitTime);
			StartCoroutine (FlashText (new TextFlashParams(text, lerpTime, waitTime)));
		} else {
			Debug.Log ("cueing " + text + " " + lerpTime + " " + waitTime);
			flashStack.Add (new TextFlashParams(text, lerpTime, waitTime));
		}
	}

}
