using UnityEngine;
using System.Collections;

/*
 * Makes a GameObject approach a target until it reaches a minimum distance.
 * 
 */
public class EnemyFollow : MonoBehaviour {

	//The root of the target.
	GameObject target;

	//This is the real target to be followed. This one is followed so that this unit can approach
	//a specific part of the target.
	GameObject realTarget;

	EnemyAttack enemyAttackInstance;

	[SerializeField]
	private float moveSpeed;

	//Distance at which this unit can start attacking its target.
	[SerializeField]
	private float minDistance;

	void Awake()
	{
		target = GameObject.Find("TowerContainer");
		realTarget = GameObject.Find("LowerTarget");
		enemyAttackInstance = this.GetComponent<EnemyAttack>();
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		Follow();		
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
				enemyAttackInstance.setCanAttack();
			}
		}
	}

	/*
	 * Returns the target to be followed.
	 * 
	 */
	public GameObject getTarget()
	{
		return target;
	}
}
