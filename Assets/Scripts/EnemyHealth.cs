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

	//The scale of the green health bar.
	private float initialXScale;

	//Maintains the total damage taken so that it is known when the unit is dead.
	private float totalDamageTaken = 0f;
	private bool isDead = false;

	void Awake()
	{
		healthBarGreen = this.transform.FindChild("HealthBarGreen").gameObject;
		initialXScale = healthBarGreen.transform.localScale.x;
	}

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (isDead)
		{
			//TODO: make a death anim?
			Transform parent = this.transform.parent;

			parent.GetComponent<EnemyAttack>().DeathAnimation();
			parent.gameObject.SetActive(false);
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
	}
}