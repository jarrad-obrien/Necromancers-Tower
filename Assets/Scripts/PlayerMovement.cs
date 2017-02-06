using UnityEngine;
using System.Collections;

/*
 * Manages the player's movement.
 *
 */
public class PlayerMovement : MonoBehaviour {

	SpriteRenderer sprRend;
	BoxCollider2D boxCol2D;

	private bool moveUp = false;
	private bool moveDown = false;
	private bool moveLeft = false;
	private bool moveRight = false;
	private bool freezeX = false;

	private bool facingRight = true;

	[SerializeField]
	private float moveSpeed;
	
	void Awake()
	{
		sprRend = GetComponent<SpriteRenderer>();
		boxCol2D = GetComponent<BoxCollider2D>();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Movement();
	}

	void Movement()
	{
		if (Input.GetKeyDown("w") || Input.GetKeyDown(KeyCode.UpArrow))
		{
			moveUp = true;
		}
		if (Input.GetKeyUp("w") || Input.GetKeyUp(KeyCode.UpArrow))
		{
			moveUp = false;
		}

		if (Input.GetKeyDown("a") || Input.GetKeyDown(KeyCode.LeftArrow))
		{
			moveLeft = true;
		}
		if (Input.GetKeyUp("a") || Input.GetKeyUp(KeyCode.LeftArrow))
		{
			moveLeft = false;
		}

		if (Input.GetKeyDown("s") || Input.GetKeyDown(KeyCode.DownArrow))
		{
			moveDown = true;
		}
		if (Input.GetKeyUp("s") || Input.GetKeyUp(KeyCode.DownArrow))
		{
			moveDown = false;
		}

		if (Input.GetKeyDown("d") || Input.GetKeyDown(KeyCode.RightArrow))
		{
			moveRight = true;
		}
		if (Input.GetKeyUp("d") || Input.GetKeyUp(KeyCode.RightArrow))
		{
			moveRight = false;
		}

		//This is done so that the player can stay facing one direction and attack while moving 
		//backwards.
		if(Input.GetKeyDown(KeyCode.LeftShift))
		{
			freezeX = true;
		}
		if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			freezeX = false;
		}

		if (moveUp)
		{
			this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y + moveSpeed * Time.deltaTime, this.transform.localPosition.z);
		}

		if (moveLeft)
		{
			
			this.transform.localPosition = new Vector3(this.transform.localPosition.x - moveSpeed * Time.deltaTime, this.transform.localPosition.y, this.transform.localPosition.z);

			//If we're not freezing thex x direction, then make sure the sprite is facing the right direction.
			if (!freezeX)
			{
				//Make it face left.
				sprRend.flipX = true;

				//If we were facing right, then flip the box collider so it also faces left.
				if (facingRight)
				{
					flipBoxCollider();
				}
			}
		}

		if (moveDown)
		{
			this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y - moveSpeed * Time.deltaTime, this.transform.localPosition.z);
		}

		if (moveRight)
		{
			this.transform.localPosition = new Vector3(this.transform.localPosition.x + moveSpeed * Time.deltaTime, this.transform.localPosition.y, this.transform.localPosition.z);

			//If we're not freezing thex x direction, then make sure the sprite is facing the right direction.
			if (!freezeX)
			{
				//Make it face right.
				sprRend.flipX = false;

				//If we were facing left, then flip the box collider so it also faces right.
				if (!facingRight)
				{
					flipBoxCollider();
				}
			}
		}
	}

	//Flips the box colliders and toggles the facingRight boolean.
	void flipBoxCollider()
	{
		boxCol2D.offset = new Vector2(boxCol2D.offset.x * -1, boxCol2D.offset.y);
		facingRight = !facingRight;
	}
}
