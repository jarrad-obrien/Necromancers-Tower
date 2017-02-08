﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Projectile : MonoBehaviour {

	//The list of enemies that are in attack range.
	private List<GameObject> enemies = new List<GameObject>();

	//The list of enemies that have died after the player's attack.
	private List<GameObject> deadEnemies = new List<GameObject>();

	//How fast the projectile will travel.
	[SerializeField]
	float speed;

	[SerializeField]
	private float damage = 10f;

	//When the projectile hits an enemy, this is set to true so that it doesn't explode
	//multiple times while it collects enemies in its explosion radius.
	private bool aboutToExplode = false;

	//Delay used to collect enemies in the explosion radius.
	[SerializeField]
	private float explosionTimeDelay;

	//How large the explosion radius is.
	[SerializeField]
	private float explosionRadiusMultiplier;

	//The destination of the projectile.
	private Vector2 destination;

	//When the projectile is given a destination, this allows the projectile to fire.
	private bool canLaunch = false;

	void Awake()
	{
		StartingPosition();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Launch();
	}

	/*
	 * Makes the projectile move towards the destination.
	 * 
	 */
	void Launch()
	{
		if (canLaunch)
		{
			this.transform.position = Vector2.MoveTowards(this.transform.position, destination, speed * Time.deltaTime);

			if(Vector2.Distance(new Vector2(this.transform.position.x, this.transform.position.y), destination) < 0.05f)
			{
				this.gameObject.SetActive(false);

				StartingPosition();
				enemies.Clear();
				deadEnemies.Clear();
				aboutToExplode = false;
			}
		}
	}

	/*
	 * This sets the destination of the projectile and allows it to launch.
	 * 
	 */
	public void SetCanLaunch(Vector2 dest)
	{
		destination = dest;
		canLaunch = true;
	}

	/*
	 * Adds enemies in the explosion radius to the list of enemies to be damaged.
	 * 
	 */
	void OnTriggerEnter2D (Collider2D col)
	{		
		if(col.tag == "Enemy")
		{
			enemies.Add(col.gameObject);

			//Only start one explosion while more enemies are added to the explosion radius.
			if (!aboutToExplode)
			{
				aboutToExplode = true;
				StartCoroutine(ExecuteAfterTime(explosionTimeDelay));
			}
		}
	}

	/*
	 * Adds a delay so that the projectile can collect more enemies in its explosion
	 * radius.
	 * 
	 */
	IEnumerator ExecuteAfterTime(float time)
	{
		yield return new WaitForSeconds(time);
		Explode();
	}

	/*
	 * Damages all enemies caught in the explosion radius.
	 * 
	 */
	void Explode()
	{
		//For every enemy in explosion range.
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
				if (enemyHealthInstance.CheckIfDead())
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

		this.gameObject.SetActive(false);

		StartingPosition();
		enemies.Clear();
		deadEnemies.Clear();
		aboutToExplode = false;
	}

	/*
	 * Expands the explosion radius to catch more enemies on explosion.
	 * 
	 */
	void ExpandExplosionRadius()
	{
		GetComponent<CircleCollider2D>().radius *= explosionRadiusMultiplier;
	}

	/*
	 * Sets the starting position of the projectile.
	 * 
	 */
	void StartingPosition()
	{
		this.transform.position = new Vector2(this.transform.parent.transform.position.x, this.transform.parent.transform.position.y);
	}

}
