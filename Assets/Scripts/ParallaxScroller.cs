using UnityEngine;
using System.Collections;

public class ParallaxScroller : MonoBehaviour {

	//this is really based on the pixels per unit import setting on the background image sprite
	public float tileOffset;

	//  1.0 means that the player scrolling occurs at double the player's movement
	//  0.0 means that the scrolling occurs exactly equal to the player's movement
	public float parallaxRatio;
	public Vector3 tilePosition;
	private GameObject player;


	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		//parallaxRatio = 0.5f;
		//tilePosition = new Vector3 (0,0,0);
		Sprite mySprite = GetComponent<SpriteRenderer>().sprite;
		float pixel2units = mySprite.rect.width / mySprite.bounds.size.x;
		tileOffset = mySprite.rect.width / pixel2units;
		SetPosition ();
	}

	public void SetTilePosition(Vector3 tp) {
		this.tilePosition = tp;
	}

	public void SetParallaxRatio(float ratio) {
		parallaxRatio = ratio;
	}

	// Update is called once per frame
	void Update () {
		//sets position based on player's position.
		SetPosition();
	}

	private void SetPosition() {
		//Vector3 playerComp = 
		//Vector3 (tilePosition + Vector3.one) * parallaxRatio;
		transform.position = tilePosition * tileOffset + ((player.transform.position * -1f) * parallaxRatio);

	}
}
