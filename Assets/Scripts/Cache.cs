using UnityEngine;
using System.Collections;

/*
 * This script manages the caching of an object so that they don't need to be constantly instantated.
 * 
 */
public class Cache : MonoBehaviour {

	//The object prefab to be instantiated and cached.
	public GameObject objectPrefab;

	//The cache.
	private GameObject[] objectArray;

	//The number of objects to be instantiated. There should never be more than this number of 
	//object active at any one time.
	[SerializeField]
	private int maxNumberOfObjects;

	//Tracks the next object to be pulled from the cache.
	private int currentObject = 0;

	// Use this for initialization
	void Start()
	{
		GenerateObjects();
	}

	// Update is called once per frame
	void Update()
	{

	}

	/*
	 * Create a cache of objects.
	 * 
	 */
	void GenerateObjects()
	{
		objectArray = new GameObject[maxNumberOfObjects];

		for (int i = 0; i < maxNumberOfObjects; i++)
		{
			objectArray[i] = (GameObject)Instantiate(objectPrefab, this.transform);
			objectArray[i].SetActive(false);
		}
	}

	/*
	 * Gets a cached object and moves the counter so that it's ready to get the next cached object.
	 * 
	 */
	public GameObject GetCachedObject()
	{
		currentObject++;

		//If the max number of cached objects is reached, reset the counter.
		if (currentObject == maxNumberOfObjects)
		{
			currentObject = 0;
			return objectArray[maxNumberOfObjects - 1];
		}

		return objectArray[currentObject - 1]; 
	}
}
