using UnityEngine;
using System.Collections;

/*
 * Manages the player's movement.
 *
 */
public class SpriterPlayerMovement : MonoBehaviour
{

	//BoxCollider2D boxCol2D;
	Animator anim;

	private bool moveUp = false;
	private bool moveDown = false;
	private bool moveLeft = false;
	private bool moveRight = false;
	private bool freezeX = false;

	private Vector3 faceLeft;
	private Vector3 faceRight;

	private float topBoundary = 2;

	//private bool facingRight = true;

	[SerializeField]
	private float moveSpeed;

	void Awake()
	{
		//boxCol2D = GetComponent<BoxCollider2D>();
		anim = GetComponent<Animator>();
		CreateFacingVectors();
	}

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		Movement();
		AnimationController();
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
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			freezeX = true;
		}
		if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			freezeX = false;
		}

		if (moveUp)
		{
			if (this.transform.localPosition.y <= topBoundary)
			{
				this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y + moveSpeed * Time.deltaTime, this.transform.localPosition.z);
			}
		}

		if (moveLeft)
		{

			this.transform.localPosition = new Vector3(this.transform.localPosition.x - moveSpeed * Time.deltaTime, this.transform.localPosition.y, this.transform.localPosition.z);

			//If we're not freezing thex x direction, then make sure the sprite is facing the correct direction.
			if (!freezeX)
			{
				//Make it face left.
				this.transform.localScale = faceLeft;
			}
		}

		if (moveDown)
		{
			this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y - moveSpeed * Time.deltaTime, this.transform.localPosition.z);
		}

		if (moveRight)
		{
			this.transform.localPosition = new Vector3(this.transform.localPosition.x + moveSpeed * Time.deltaTime, this.transform.localPosition.y, this.transform.localPosition.z);

			//If we're not freezing thex x direction, then make sure the sprite is facing the correct direction.
			if (!freezeX)
			{
				//Make it face right.
				this.transform.localScale = faceRight;
			}
		}
	}

	/*
	 * Creates the vector3s required for facing left and right.
	 * 
	 */
	void CreateFacingVectors()
	{
		float xScale = this.transform.localScale.x;

		faceLeft = new Vector3(-xScale, this.transform.localScale.y, this.transform.localScale.z);
		faceRight = new Vector3(xScale, this.transform.localScale.y, this.transform.localScale.z);
	}

	/*
	 * Controls the walking animation of the player. 
	 * 
	 */
	void AnimationController()
	{
		if(moveDown || moveUp || moveLeft || moveRight)
		{
			anim.SetBool("IsWalking", true);
		}
		else
		{
			anim.SetBool("IsWalking", false);
		}
	}
}
