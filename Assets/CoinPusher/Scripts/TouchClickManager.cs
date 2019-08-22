using UnityEngine;
using System.Collections;

public class TouchClickManager : MonoBehaviour {

	// Our save manager
	public SaveManager saveManager;

	// Our reference to the CoinSpawner which is based on a tag
	public CoinSpawner coinSpawner;			

	// The reference to the CoinManager for use later
	public CoinManager coinManager;

	// Our out of coin manager
	public OutOfCoinsManager outOfCoinsManager;

	void Update()
	{
		if (Input.GetButtonDown ("Fire1"))
		{
			// Make sure we found our Coin Spawner
			if( coinSpawner != null )
			{
				RaycastHit hit;
				if( Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit) )
				{
					// Make sure they clicked on the spawn area
					if (hit.collider.CompareTag ("TouchClickArea")) 
					{
						// Make sure the user has coins to actually use
						if( coinManager.canSpawnCoin() )
						{
							// Spawn a coin
							coinSpawner.spawnCoin(hit.point);		

							// Remove the coin from the CoinManager
							coinManager.removeCoin();

							// Save the game data
							saveManager.saveData();
						}
						else
						{
							// We cannot spawn the coin which means we should check to see if they have enough coins, if not, prompt to get some
							if( coinManager.currentCoinTotal == 0 && !outOfCoinsManager.isWindowOpen )
							{
								// Bring the popup up that they need to get more coins
								outOfCoinsManager.showWindow();
							}
						}
					}
				}
			}
			else
				Debug.LogError ("You need to assign a coin spawner!");
		}
	}
}