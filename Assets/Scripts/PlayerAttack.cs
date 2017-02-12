using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Manages the attacking of the player.
 * 
 * This requires the target to have a HealthBar prefab as a child GameObject.
 * 
 */
public class PlayerAttack : MonoBehaviour
{
	Animator anim;

	//The list of enemies that are in attack range.
	private List<GameObject> enemies = new List<GameObject>();

	//The list of enemies that have died after the player's attack.
	private List<GameObject> deadEnemies = new List<GameObject>();

	[SerializeField]
	private float damage = 10f;

	//Time that the player can attack again.
	private float nextAttackTime = 0.0f;

	//How long the attack animation takes. From when the attack begins to when it deals damage.
	[SerializeField]
	private float attackAnimDuration = 0.4f;

	//How long the player has to wait before they can attack again.
	[SerializeField]
	private float attackDelay = 1f;

	//This is used so that the coroutine ExecuteAfterTime() is only called once, instead of multiple 
	//times while the if statement that calls the method is true.
	private bool canAttack = true;

	void Awake()
	{
		anim = GetComponent<Animator>();
	}

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		CheckIfCanAttack();
		AnimationController();
	}

	/*
	 * When an enemy enters attack range, add them to the list.
	 * 
	 */
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == "Enemy")
		{
			enemies.Add(col.gameObject);
		}
	}

	/*
	 * When an enemy exits attack range, remove them from the list.
	 * 
	 */
	void OnTriggerExit2D(Collider2D col)
	{
		if (col.tag == "Enemy")
		{
			enemies.Remove(col.gameObject);
		}
	}

	/*
	 * Checks if the unit is able to attack.
	 * 
	 */
	void CheckIfCanAttack()
	{
		if (canAttack && Time.time > nextAttackTime && enemies.Count > 0)
		{
			canAttack = false;
			StartCoroutine(ExecuteAfterTime(attackAnimDuration));
		}
	}

	/*
	 * When an enemy enters attack range and the player is able to attack, this puts a delay on when 
	 * they are damaged. Enemies can enter or leave the attack range before the damage occurs.
	 * This is essentially the time of the attack animation.
	 * 
	 */
	IEnumerator ExecuteAfterTime(float time)
	{
		yield return new WaitForSeconds(time);
		Attack();
	}

	/*
	 * Attacks all the enemies inside of the attack range. The list is populated by OnTriggerEnter2D()
	 * and depopulated by OnTriggerExit2D(). It also maintains a timer such that there is a delay
	 * between attacks (like a reload).
	 * 
	 */
	void Attack()
	{
		//For every enemy in attack range.
		foreach (GameObject enemy in enemies)
		{
			//Get the health bar of the enemy.
			Health enemyHealthInstance = null;
			for (int i = 0; i < enemy.transform.childCount; i++)
			{
				if (enemy.transform.GetChild(i).transform.name == "HealthBar")
				{
					enemyHealthInstance = enemy.transform.GetChild(i).GetComponent<Health>();
					break;
				}
			}

			//Deal damage to the enemy.
			if (enemyHealthInstance != null) //NOT SURE IF NECESSARY; ALREADY FOUND HEALTH
			{
				enemyHealthInstance.TakeDamage(damage);
				if(enemyHealthInstance.CheckIfDead())
				{
					deadEnemies.Add(enemy);
				}
			}
		}

		//Remove the dead enemies from the live enemy list.
		foreach (GameObject deadEnemy in deadEnemies)
		{
			enemies.Remove(deadEnemy);
		}
		deadEnemies.Clear();
		
		UpdateNextAttack();
	}

	/*
	 * Establishes when the player can attack again and allows the player to attack again.
	 * 
	 */
	void UpdateNextAttack()
	{
		nextAttackTime = Time.time + attackDelay;
		canAttack = true;
	}

	void AnimationController()
	{
		if(!canAttack)
		{
			anim.Play("attack_0");
		}
	}
}
