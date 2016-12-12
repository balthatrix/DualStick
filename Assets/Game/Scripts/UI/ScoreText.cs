using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour {


	IEnumerator Start() {
		float t = .50f;
		float delay = .5f;
		float scaleTime = t;
		float growthSpd = 3.0f;
		Text text = GetTextComponent ();
		yield return new WaitForSeconds (delay);
		//Vector3 now = Vector3.zero;
//		while (scaleTime > 0f) {
//			now = transform.localScale;
//			float factor = Time.deltaTime * growthSpd;
//			transform.localScale = new Vector3 (now.x + factor, now.y + factor, now.z);
//			scaleTime -= Time.deltaTime;
//			yield return null;
//		}


		Vector3 now = transform.localScale;
		Color nowC = text.color;
		while (now.x > 0f) {
			now = transform.localScale;
			nowC = text.color;
			float factor = Time.deltaTime * growthSpd;
			transform.localScale = new Vector3 (now.x - factor, now.y - factor, now.z);
			text.color = new Color (nowC.r, nowC.g - factor, nowC.b);
			yield return null;
		}

		Destroy (gameObject);
	}

	public Text GetTextComponent() {
		return transform.GetChild (0).gameObject.transform //game object should be a canvas
				.GetChild (0).gameObject.GetComponent<Text> (); //second game object should be the text object, which we get the text component off of

	}

	public void SetText(string text) {
		Text t = GetTextComponent ();
		t.text = text;
	}

}
