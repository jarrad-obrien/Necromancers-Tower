using UnityEngine;
using System.Collections;

/*
 * This makes the sprite that it's attached to render in front or behind other sprites based on its
 * y value at its feet. This makes it so a sprite that is below another one appears in front, and a 
 * sprite that is above another one appears behind.
 * 
 */
 [RequireComponent(typeof(SpriteRenderer))]
public class SpriteSortingLayer : MonoBehaviour {

	SpriteRenderer sprRend;
	private float halfHeight;

	void Awake()
	{
		sprRend = GetComponent<SpriteRenderer>();
		halfHeight = sprRend.bounds.size.y / 2;
	}

	// Use this for initialization
	void Start () {
			
	}
	
	// Update is called once per frame
	void Update () {
		sprRend.sortingOrder = Mathf.RoundToInt((transform.position.y - halfHeight) * 10f) * -1;
	}
}
