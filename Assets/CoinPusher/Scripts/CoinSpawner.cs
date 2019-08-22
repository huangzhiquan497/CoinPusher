using UnityEngine;
using System.Collections;

public class CoinSpawner : MonoBehaviour {

	// This is the play field of coins
	private Transform coinPlayField;

	// This is a play setting that can be configured in the Unity editor. This allows coins to be spawned randomly in the
	// spawn area versus being spawned at point of click/touch
	public bool spawnRandomLocations = false;

	[Header("Regular Coins")]
	// Our public array of coins we should spawn
	public Transform[] coins;

	// This is how we do items, we set up three arrays: common, rare, epic. The epic are the most rare of them all.
	[Header("Common Coins")]
	public Transform[] common;
	[Header("Rare Coins")]
	public Transform[] rare;
	[Header("Epic Coins")]
	public Transform[] epic;

	void Awake()
	{
		// Find the play field
		coinPlayField = GameObject.FindWithTag("CoinsPlayField").GetComponent<Transform>();
	}

	public void spawnCoin(Vector3 position)
	{
		Vector3 spawnLocation;

		if( spawnRandomLocations )
		{
			spawnLocation = new Vector3(transform.position.x,
			                                    transform.position.y,
			                                    Random.Range (transform.position.z - (transform.localScale.z / 2), transform.position.z + (transform.localScale.z / 2)));
		}
		else
		{
			// Spawn at click/tap location
			// x = z, y = y, z = x
			spawnLocation = new Vector3(position.x, transform.position.y, position.z);
		}

		// Here is where we decide on what item to spawn, based on rarity.
		// To get a common item, it will be between: 1 - 5
		// To get a rare item, it wil be between: 1 - 15
		// To get an epic item, it will be between: 1 - 30
		int findItem = 4;
		int findCommon = Random.Range (1, 5);
		int findRare = Random.Range (1, 30);
		int findEpic = Random.Range (1, 45);
	
		if( findItem == findCommon )
		{
			if( common.Length != 0 )
			{
				GameObject.Instantiate(common[Random.Range (0, common.Length)], spawnLocation, common[0].rotation);
			}
		}
		
		if( findItem == findRare )
		{
			if( rare.Length != 0 )
			{
				GameObject.Instantiate(rare[Random.Range (0, rare.Length)], spawnLocation, rare[0].rotation);
			}
		}

		if( findItem == findEpic )
		{
			if( epic.Length != 0 )
			{
				GameObject.Instantiate(epic[Random.Range (0, epic.Length)], spawnLocation, epic[0].rotation);
			}
		}

		if( findItem != findCommon && findItem != findRare && findItem != findEpic )
		{
			GameObject.Instantiate(coins[Random.Range (0, coins.Length)], spawnLocation, coins[0].rotation);
	    }
	}

	public void coinAttackSpawner(int amount)
	{
		for(int i = 1; i <= amount; i++ )
		{
			float newX = Random.Range (coinPlayField.position.x - 1, coinPlayField.position.x + 1);
			float newZ = Random.Range (coinPlayField.position.z - 1, coinPlayField.position.z + 1);

			Vector3 coinAttackSpawnLoc = new Vector3 (newX, 3.0f, newZ);

			Instantiate(coins[Random.Range (0, coins.Length)], coinAttackSpawnLoc, coins[0].rotation);
		}
	}

	public void giftCoinSpawner()
	{
		float newX = Random.Range (coinPlayField.position.x - 1, coinPlayField.position.x + 1);
		float newZ = Random.Range (coinPlayField.position.z - 1, coinPlayField.position.z + 1);

		Vector3 coinAttackSpawnLoc = new Vector3 (newX, 3.0f, newZ);

		Instantiate(rare[Random.Range (0, rare.Length)], coinAttackSpawnLoc, rare[0].rotation);
	}

	void OnDrawGizmos() 
	{
		Gizmos.color = new Color(0, 1, 0, 0.5F);
		Gizmos.DrawCube(transform.position, new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z));
	}
}