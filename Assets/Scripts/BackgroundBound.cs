using UnityEngine;
using System.Collections;

public class BackgroundBound : MonoBehaviour {

	public GameObject suggestionObject;
	public GameObject camera;

	void OnCollisionEnter2D (Collision2D other) {
		foreach(ContactPoint2D contact in other.contacts)
		{	
			Debug.Log ("Point of contact: " + contact.point.ToString());
			Vector2 otherV = new Vector2 (other.gameObject.transform.position.x, other.gameObject.transform.position.y);

			Debug.Log ("Opposing dir: " + (otherV - contact.point).ToString ());
			Vector2 suggestionOffset = otherV - contact.point;
			GameObject newSugg = Instantiate (suggestionObject);

			newSugg.transform.SetParent (camera.transform);
			float xCom = Mathf.Round (suggestionOffset.x);
			float yCom = Mathf.Round (suggestionOffset.y);
			newSugg.transform.localPosition = Vector3.zero + new Vector3(xCom, yCom, 1) * 4f;// + new Vector3(suggestionOffset.x, suggestionOffset.y, 0.0f);

			newSugg.transform.Rotate(0,0, Mathf.Atan2(yCom, xCom) * Mathf.Rad2Deg + 180f);
			StartCoroutine (FlashSuggestion (newSugg));
			//Debug.Log("new sugg t pos: "+ newSugg.transform.position);
		}
	}

	private IEnumerator FlashSuggestion(GameObject sugg) {
		float fadeTime = 0.5f;
		for (int i = 0; i < 3; i++) {
			float left = fadeTime;
			while (left > 0f) {
				foreach (Transform child in sugg.transform)
				{
					SpriteRenderer sr = child.GetComponent<SpriteRenderer> ();
					sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, left / fadeTime);
				}
				left -= Time.deltaTime;
				yield return null;
			}
			yield return new WaitForSeconds (0.2f);
		}
		Destroy (sugg);
	}
}
