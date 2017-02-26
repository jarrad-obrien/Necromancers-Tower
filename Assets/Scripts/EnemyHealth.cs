using UnityEngine;
using System.Collections;

/*
 * Works with the HealthBar sprites to manage a health bar.
 * 
 */
public class EnemyHealth : MonoBehaviour
{
	private GameObject healthBarGreen;
	[SerializeField]
	private float health;

	private EnemyAttack enemyAttackInstance;
	private EnemyFollow enemyFollowInstance;
	private Animator anim;
	private BoxCollider2D boxCol2D;

	//The scale of the green health bar.
	private float initialXScale;

	//Maintains the total damage taken so that it is known when the unit is dead.
	private float totalDamageTaken = 0f;
	private bool isDead = false;

	//How long the enemy stays dead on the ground before becoming inactivated.
	private float deathTimeDelay = 2f;

	void Awake()
	{
		healthBarGreen = this.transform.FindChild("HealthBarGreen").gameObject;
		initialXScale = healthBarGreen.transform.localScale.x;
		enemyAttackInstance = this.transform.parent.GetComponent<EnemyAttack>();
		enemyFollowInstance = this.transform.parent.GetComponent<EnemyFollow>();
		anim = this.transform.parent.GetComponent<Animator>();
		boxCol2D = this.transform.parent.GetComponent<BoxCollider2D>();
	}

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		HasDied();
	}
	
	/*
	 * Checks if the enemy is dead. If it is, execute the methods relating to its death. 
	 * 
	 */
	 void HasDied()
	{
		if (isDead)
		{
			enemyAttackInstance.DeathAnimation();
			enemyFollowInstance.StopMoving();

			StartCoroutine(ExecuteAfterTime(deathTimeDelay));

		}
	}

	/*
	 * Takes damage and reduces the health bar. If the unit's health is depleted, the unit is marked
	 * dead with isDead and the green health bar's scale is reduced to 0. 
	 * 
	 */
	public void TakeDamage(float damage)
	{
		//Adds to the total damage taken by the unit.
		totalDamageTaken += damage;

		//If the unit is still alive.
		if (!isDead)
		{
			//If the total damage taken is greater than its health, the unit is marked as dead and
			//the green health bar is effectively removed.
			if (totalDamageTaken >= health)
			{
				healthBarGreen.transform.localScale = new Vector3(0, healthBarGreen.transform.localScale.y, 0);
				isDead = true;
			}
			//Take damage as normal and reduce the green health bar.
			else
			{
				healthBarGreen.transform.localScale -= new Vector3(1 - ((health - damage) / health * initialXScale), 0, 0);
			}
		}
	}

	//Checks if the unit is alive or not.
	public bool CheckIfDead()
	{
		return isDead;
	}

	/*
	 * Resets the unit's health and associated components.
	 * 
	 */
	public void ResetHealth()
	{
		isDead = false;
		totalDamageTaken = 0;
		healthBarGreen.transform.localScale = new Vector3(initialXScale, healthBarGreen.transform.localScale.y, healthBarGreen.transform.localScale.z);
		enemyAttackInstance.SetIsDead(false);
		enemyAttackInstance.SetAtTower(false);
		enemyFollowInstance.StartMoving();
		boxCol2D.enabled = true;

	}

	/*
	 * Sets the enemy's animation as walking.
	 * 
	 */
	 public void ResetAnimator()
	{
		anim.SetBool("IsAtTower", false);
	}

	/*
	 * Timer to delay when the enemy becomes inactivated.
	 * 
	 */
	IEnumerator ExecuteAfterTime(float time)
	{
		yield return new WaitForSeconds(time);
		OnDeath();
	}

	/*
	 * Sets this enemy to inactive when it dies.
	 * 
	 */
	void OnDeath()
	{
		this.transform.parent.gameObject.SetActive(false);
	}
}