using UnityEngine;
using UnityEngine.UI;
using Extras;
using System.Collections;
using System.Collections.Generic;

public class CollectableManager : MonoBehaviour {

	// Our save manager
	public SaveManager saveManager;

	// Our inventory
	public Dictionary<CoinEffect.Effect, int> inventory;

	// This is the collectable PlayerPrefs name
	public string collectableSaveName = "collectables";

	[Header("Collectable UI Settings")]
	// This is the panel UI (top level)
	public GameObject collectablesPanelUIParent;

	// This is the panel in the UI we display all the collectables under
	public GameObject collectablePanel;

	// This is used for showing all collectables in the UI
	public GameObject[] collectableObjects;

	// This is the item prefab we have that is used for all items
	public GameObject itemPrefab;

	// Our coin manager
	private CoinManager coinManager;

	// Use this for initialization
	void Start () {

		// Find our coin manager
		coinManager = GameObject.FindWithTag("CoinManager").GetComponent<CoinManager>();

		// Load any saved data
		saveManager.loadData ();
	}

	/// <summary>
	/// This will load the collecatbles in the UI Panel
	/// </summary>
	public void loadCollectablesUI()
	{
		// Go through each object in the list saved for collectables
		foreach( GameObject collectable in collectableObjects )
		{
			// If this object does not exist in the panel
			if (!Tools.doesExistInList (collectable.name, collectablePanel.transform)) 
			{
				// Load up the item prefab into the panel, make a new instance
				GameObject go = Instantiate (itemPrefab) as GameObject;

				// Define the name and other bits of data
				go.GetComponent<CollectableItemInformation> ().itemName = collectable.name;

				// Get the amount saved, in the inventory, of this type of coin/collectable
				int amount = 0;

				// Try to get the value
				inventory.TryGetValue (collectable.GetComponent<CoinEffect> ().typeOfCoin, out amount);

				// Save the CoinEffect type to the object
				go.GetComponent<CollectableItemInformation> ().itemEffectType = collectable.GetComponent<CoinEffect> ().typeOfCoin;

				// Load the sprite 
				go.GetComponent<CollectableItemInformation> ().itemSprite = collectable.GetComponent<CoinEffect> ().prizeImage;

				// Save it into the information
				go.GetComponent<CollectableItemInformation> ().itemAmount = amount;

				// Reparent this to the UI keeping the original prefab sizes
				go.transform.SetParent (collectablePanel.transform, false);

				// Setup the name of the object
				go.name = collectable.name;
			}
			else
			{
				// We already exist so update the value/amount
				foreach(Transform obj in collectablePanel.transform)
				{
					// If we're on the object we need to update
					if( collectable.name == obj.gameObject.name )
					{
						// Update the item amount of what is on the list
						int amount = 0;
						inventory.TryGetValue (obj.gameObject.GetComponent<CollectableItemInformation>().itemEffectType, out amount);
						obj.gameObject.GetComponent<CollectableItemInformation>().itemAmount = amount;
					}
				}
			}
		}

		// Disable the spawning of coins in the coin manager
		coinManager.coinSpawnerReady = false;

		// Show the panel
		collectablesPanelUIParent.SetActive (true);
	}

	/// <summary>
	/// This will close the UI 
	/// </summary>
	public void exitCollectableUI()
	{
		// Hide the panel now
		collectablesPanelUIParent.SetActive (false);

		// Enable coin spawning again since the window is closed
		coinManager.coinSpawnerReady = true;
	}

	/// <summary>
	/// This function will add a new item to the inventory
	/// </summary>
	/// <param name="">.</param>
	/// <param name="amount">Amount.</param>
	public void addItem(CoinEffect.Effect item, int amount)
	{
		int invAmount = 0;
		
		// We need to only update the same CoinEffect.Effect by incrementing the amount they have of it by the amount above
		// Check to see if we have any of the CoinEffect.Effect
		if (inventory.TryGetValue(item, out invAmount)) 
		{				
			// Add up the new amount
			amount += invAmount;

			// Save it now
			inventory[item] = amount;
		} 
		else
		{
			// They did not have this item yet, so add it as a new one
			inventory.Add(item, amount);
		}

		// Save the data
		saveManager.saveData();
	}

	/// <summary>
	/// This is used to remove an item from the inventory
	/// </summary>
	/// <param name="item">Item to work on</param>
	/// <param name="amount">Amount to remove</param>
	public void removeItem(CoinEffect.Effect item, int amount)
	{
		int invAmount = 0;

		// We need to only update the same CoinEffect.Effect by incrementing the amount they have of it by the amount above
		// Check to see if we have any of the CoinEffect.Effect
		if (inventory.TryGetValue(item, out invAmount)) 
		{				
			// Add up the new amount
			invAmount = invAmount - amount;

			// Save it now
			inventory[item] = invAmount;
		} 

		// Save the data
		saveManager.saveData();
	}
}