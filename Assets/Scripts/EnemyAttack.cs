using UnityEngine;
using System.Collections;

/*
 * Manages the attacking of the enemies.
 * 
 * This requires the target to have a HealthBar prefab as a child GameObject.
 * 
 */
[RequireComponent(typeof(EnemyFollow))]
public class EnemyAttack : MonoBehaviour {

	Animator anim;
	EnemyHealth enemyHealth;
	BoxCollider2D boxCol2D;

	//The health of the target.
	Health targetHealth;

	private float damage = 5f;

	//Time that the player can attack again.
	private float nextAttackTime = 0f;

	//How long the attack animation takes. From when the attack begins to when it deals damage.
	[SerializeField]
	private float attackAnimDuration = 0.4f;

	//How long the unit has to wait before it can attack again.
	[SerializeField]
	private float attackDelay = 1f;

	//If this unit has reached the point where it can attack the target, this becomes true and 
	//the unit starts attacking.
	private bool atTower = false;

	//This is used so that the coroutine ExecuteAfterTime() is only called once, instead of 
	//multiple times while the if statement that calls the method is true.
	private bool canAttack = true;

	//Keeps track of whether or not the unit is alive. This is used to control the animations.
	private bool hasDied = false;

	void Awake()
	{
		anim = GetComponent<Animator>();
		GetThisHealth();
		boxCol2D = GetComponent<BoxCollider2D>();
	}

	// Use this for initialization
	void Start ()
	{
		GetTargetHealth();
	}
	
	// Update is called once per frame
	void Update ()
	{
		CheckIfCanAttack();
		SetIdleAnimation();
		DeathAnimation();
	}

	/*
	 * Gets the reference to the health bar. Using GetTarget() isn't totally necessary, 
	 * but it's been done to save Find() calls.
	 * 
	 */
	void GetTargetHealth()
	{
		GameObject target = GetComponent<EnemyFollow>().GetTarget();

		for (int i = 0; i < target.transform.childCount; i++)
		{
			if (target.transform.GetChild(i).transform.name == "HealthBar")
			{
				targetHealth = target.transform.GetChild(i).GetComponent<Health>();
				break;
			}
		}
	}

	/*
	 * Get the health of this unit.
	 * 
	 */
	void GetThisHealth()
	{
		for(int i = 0; i < this.transform.childCount; i++)
		{
			if(this.transform.GetChild(i).transform.name == "HealthBar")
			{
				enemyHealth = this.transform.GetChild(i).GetComponent<EnemyHealth>();
			}
		}
	}

	/*
	 * Checks if the unit is able to attack.
	 * 
	 */
	void CheckIfCanAttack()
	{
		if (atTower && canAttack && Time.time > nextAttackTime && !enemyHealth.CheckIfDead())
		{
			canAttack = false;
			nextAttackTime = Time.time + attackDelay + attackAnimDuration;
			StartCoroutine(ExecuteAfterTime(attackAnimDuration));
		}
	}

	/*
	 * When this unit can attack the target, this puts a delay on when the damage takes
	 * place. This is essentially the time of the attack animation.
	 * 
	 */
	IEnumerator ExecuteAfterTime(float time)
	{
		AnimationAttack();
		yield return new WaitForSeconds(time);
		Attack();
	}

	/*
	 * Attacks the target. It also maintains a timer such that there is a delay between 
	 * attacks (like a reload).
	 * 
	 */
	void Attack()
	{
		targetHealth.TakeDamage(damage);
		
		canAttack = true;
	}

	/*
	 * When the unit is in attacking range of the target, it sets a bool so that it can
	 * start attacking.
	 * 
	 */
	public void SetCanAttack()
	{
		atTower = true;
	}

	/*
	 * Sets the unit into an idle state when it reaches the tower.
	 * 
	 */
	void SetIdleAnimation()
	{
		if (atTower && !enemyHealth.CheckIfDead())
		{
			anim.SetBool("IsAtTower", true);
		}
	}

	/*
	 * Plays an attack animation.
	 * 
	 */
	void AnimationAttack()
	{
		if (Random.Range(0, 2) == 0)
		{
			anim.Play("sword_swing_0");
		}
		else
		{
			anim.Play("Sword_thrust");
		}
	}

	/*
	 * Plays the death animation.
	 * 
	 */
	public void DeathAnimation()
	{
		if(enemyHealth.CheckIfDead() && !hasDied)
		{
			hasDied = true;
			anim.Play("hit_0");

			boxCol2D.enabled = false;
		}
	}

	/*
	 * Sets the hasDied bool.
	 * 
	 */
	 public void SetIsDead(bool exp)
	{
		hasDied = exp;
	}

	/*
	 * Sets the atTower bool.
	 * 
	 */
	 public void SetAtTower(bool exp)
	{
		atTower = exp;
	}
}