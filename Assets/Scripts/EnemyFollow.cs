using UnityEngine;
using System.Collections;

/*
 * Makes a GameObject approach a target until it reaches a minimum distance.
 * 
 */
public class EnemyFollow : MonoBehaviour
{

	//The root of the target.
	GameObject target;

	//This is the real target to be followed. This one is followed so that this unit can approach
	//a specific part of the target.
	GameObject realTarget;

	EnemyAttack enemyAttackInstance;
	EnemyHealth enemyHealthInstance;

	[SerializeField]
	private float moveSpeed;
	private float savedMoveSpeed;
	[SerializeField]
	private float acceleration;

	//Distance at which this unit can start attacking its target.
	[SerializeField]
	private float minDistance;

	private bool canAccelerate = false;

	void Awake()
	{
		//HARDCODED
		target = GameObject.Find("TowerContainer");

		//HARDCODED
		realTarget = GameObject.Find("LowerTarget");

		enemyAttackInstance = this.GetComponent<EnemyAttack>();

		savedMoveSpeed = moveSpeed;

		GetThisHealth();
	}

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		Follow();
		SpeedUp();
	}

	/*
	 * Approaches the target until it has reached attacking distance, at which point it
	 * stops moving and starts attacking.
	 * 
	 */
	void Follow()
	{
		if (realTarget != null)
		{
			if (Vector2.Distance(this.transform.position, realTarget.transform.position) > minDistance)
			{
				this.transform.position = Vector2.MoveTowards(this.transform.position, realTarget.transform.position, moveSpeed * Time.deltaTime);
			}
			else
			{
				enemyAttackInstance.SetCanAttack();
			}
		}
	}

	void SpeedUp()
	{
		if(canAccelerate && moveSpeed < savedMoveSpeed && !enemyHealthInstance.CheckIfDead())
		{
			moveSpeed += acceleration;
		}
		else
		{
			canAccelerate = false;
		}
	}

	/*
	 * Returns the target to be followed.
	 * 
	 */
	public GameObject GetTarget()
	{
		return target;
	}

	/*
	 * Stops this enemy from moving.
	 * 
	 */
	public void StopMoving()
	{
		moveSpeed = 0f;
	}

	/*
	 * Gives this enemy a move speed so it can approach its target.
	 * 
	 */
	public void StartMoving()
	{
		moveSpeed = savedMoveSpeed;
	}

	/*
	 * Get the health of this unit.
	 * 
	 */
	void GetThisHealth()
	{
		for (int i = 0; i < this.transform.childCount; i++)
		{
			if (this.transform.GetChild(i).transform.name == "HealthBar")
			{
				enemyHealthInstance = this.transform.GetChild(i).GetComponent<EnemyHealth>();
			}

		}
	}

	/*
	 * Starts the enemy accelerating after stopping.
	 * 
	 */
	public void StartAccelerating()
	{
		canAccelerate = true;
	}

}
