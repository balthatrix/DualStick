using UnityEngine;
using System.Collections;

public class FitUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		RectTransform uiPane = UIManager.instance.GetComponent<RectTransform> ();
		RectTransform mine = GetComponent<RectTransform> ();
		mine.sizeDelta = new Vector2 (uiPane.rect.width, uiPane.rect.height);
		//mine.rect.height = uiPane.rect.height;
		//mine.rect.width = uiPane.rect.width;
	}

}
