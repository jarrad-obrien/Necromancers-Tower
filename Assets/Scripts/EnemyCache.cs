using UnityEngine;
using System.Collections;

/*
 * This script manages the caching of enemies so that they don't need to be constantly instantated.
 * 
 */
public class EnemyCache : MonoBehaviour {

	//The enemy to be instantiated and cached.
	public GameObject enemyPrefab;

	//The cache.
	private GameObject[] enemyArray;

	//The number of enemies to be instantiated. There should never be more than this number of 
	//enemies active at any one time.
	[SerializeField]
	private int maxNumberOfEnemies;

	//Tracks the next enemy to be pulled from the cache.
	private int currentEnemy = 0;

	// Use this for initialization
	void Start () {
		GenerateEnemies();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/*
	 * Create a cache of enemies.
	 * 
	 */
	void GenerateEnemies()
	{
		enemyArray = new GameObject[maxNumberOfEnemies];

		for (int i = 0; i < maxNumberOfEnemies; i++)
		{
			enemyArray[i] = (GameObject)Instantiate(enemyPrefab, this.transform);
			enemyArray[i].SetActive(false);
		}

		GetComponent<EnemySpawner>().BeginSpawning();
	}

	/*
	 * Gets a cached enemy and moves the counter so that it's ready to get the next cached enemy.
	 * 
	 */
	public GameObject GetEnemy()
	{
		currentEnemy++;

		//If the max number of cached enemies is reached, reset the counter.
		if(currentEnemy == maxNumberOfEnemies)
		{
			currentEnemy = 0;
			return enemyArray[maxNumberOfEnemies - 1];
		}

		return enemyArray[currentEnemy - 1];
	}
}
