using UnityEngine;
using UnityEngine.UI;
using Extras;
using System.Collections;

public class CoinShopManager : MonoBehaviour {

	// This is the coin shop panel
	public GameObject coinShopPanel;

	// This is the panel where ITEMWS are spawned only
	public GameObject coinShopItemPanel;

	// This is used to track if we can drop coins when this window is opened
	public CoinManager coinManager;

	// This is the array that is of items for sale in the coin shop
	public GameObject[] coinShopItems;

	public GameObject coinShopItemPrefab;

	public Text playerCashLabel;

	public PowerupManager powerupManager;

	void Update()
	{
		// Update the player cash label inside of the coin shop
		playerCashLabel.text = coinManager.playerCash.ToString();
	}

	public void showWindow()
	{
		// Go through each object in the list saved for coin shop items
		foreach( GameObject item in coinShopItems )
		{
			// If this object does not exist in the panel
			if (!Tools.doesExistInList (item.name, coinShopItemPanel.transform)) 
			{
				// Load up the item prefab into the panel, make a new instance
				GameObject go = Instantiate (coinShopItemPrefab) as GameObject;

				// Define the name and other bits of data
				go.GetComponent<CoinShopItemInformation> ().itemName = item.name;

				// Set up the item cost
				go.GetComponent<CoinShopItemInformation>().itemCost = item.GetComponent<CoinEffect>().coinShopItemPrice;

				// Show the sprite
				go.GetComponent<CoinShopItemInformation>().itemImage = item.GetComponent<CoinEffect>().prizeImage;

				// Set the item effect information
				go.GetComponent<CoinShopItemInformation>().effect = item.GetComponent<CoinEffect>().typeOfCoin;

				// Reparent this to the UI keeping the original prefab sizes
				go.transform.SetParent (coinShopItemPanel.transform, false);

				// Setup the name of the object
				go.name = item.name;
			}
		}
		
		coinShopPanel.SetActive(true);

		coinManager.coinSpawnerReady = false;
	}

	public void closeWindow()
	{
		// Refresh the button amounts in on the power up panel so it picks up the new values
		powerupManager.refreshButtonAmounts();
		
		coinShopPanel.SetActive(false);

		coinManager.coinSpawnerReady = true;
	}
}