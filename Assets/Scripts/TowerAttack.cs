using UnityEngine;
using System.Collections;

/*
 * This manages the shooting of projectiles from the tower.
 * 
 */
 [RequireComponent(typeof(Cache))]
public class TowerAttack : MonoBehaviour {

	private Cache cache;

	//Time that the tower can attack again.
	private float nextAttackTime = 0.0f;

	//How long the tower has to wait before they can attack again.
	[SerializeField]
	private float attackDelay = 1f;

	//Used to pause the ability to attack when an attack is occuring. This makes it so only 
	//one attack occurs at any time.
	private bool canAttack = true;

	void Awake()
	{
		cache = GetComponent<Cache>();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		CheckIfCanAttack();
	}

	/*
	 * Checks if the tower is able to attack.
	 * 
	 */
	void CheckIfCanAttack()
	{
		if(Input.GetMouseButton(0) && canAttack && Time.time > nextAttackTime)
		{
			canAttack = false;
			Attack();
		}
	}

	/*
	 * Fires a projectile towards the mouse's location. The projectile "explodes" on impact
	 * and damages all enemies caught in its range.
	 * 
	 */
	void Attack()
	{
		GameObject projectile = cache.GetCachedObject();

		Vector3 mouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 destination = new Vector2(mouseLocation.x, mouseLocation.y);

		projectile.SetActive(true);

		projectile.GetComponent<Projectile>().SetCanLaunch(destination);

		UpdateNextAttack();
	}

	/*
	 * Establishes when the tower can attack again and allows the tower to attack again.
	 * 
	 */
	void UpdateNextAttack()
	{
		nextAttackTime = Time.time + attackDelay;
		canAttack = true;
	}
}
