using UnityEngine;
using System.Collections;

/*
 * Spawns cached enemies.
 * 
 */
public class EnemySpawner : MonoBehaviour {

	//Position where the enemy will spawn. The x position is fixed, but the y position will
	//be randomized between the two y values;
	[SerializeField]
	private float xPos;
	[SerializeField]
	private float yPosTop;
	[SerializeField]
	private float yPosBottom;

	//Time that the spawner can spawn another enemy.
	private float nextSpawnTime = 0f;

	//How long it takes to spawn another enemy.
	[SerializeField]
	private float timeBetweenSpawns = 2f;

	//public bool startSpawn;

	EnemyCache cache;

	void Awake()
	{
		cache = GetComponent<EnemyCache>();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SpawnEnemy()
	{
		GameObject enemy = cache.getEnemy();

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

		//Resets the enemy's health so it is full again.
		enemyHealthInstance.ResetHealth();

		//Gives the enemy a new position.
		enemy.transform.position = new Vector3(xPos, Random.Range(yPosBottom, yPosTop), 0);

		//Sets the enemy active so it will approach the target.
		enemy.SetActive(true);
	}

	public void BeginSpawning()
	{
		for(int i = 0; i < 10; i++)
		{
			SpawnEnemy();
		}
	}
}
