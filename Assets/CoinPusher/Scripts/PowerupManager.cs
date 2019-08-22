using UnityEngine;
using UnityEngine.UI;
using Extras;
using System.Collections;

public class PowerupManager : MonoBehaviour {

	// This is the panel where the powerups exist
	public GameObject powerupPanel;

	// This is our default prefab for powerup buttons
	public GameObject powerupButtonPrefab;

	// Refernce to the coin shop manager for pulling out the items for sale and use
	public CoinShopManager coinShopManager;

	// Our reference to the effect manager
	public EffectsManager effectManager;

	public int earthQuakePowerUpAmount = 0;
	public int bumperPowerUpAmount = 0;

	void Update()
	{
		// This will add only new buttons not found in the master array, aka, new items they bought
		buttonMaker();
	}

	/// <summary>
	/// Refreshs the power up button amounts, including the saved amount and label amount
	/// </summary>
	public void refreshButtonAmounts()
	{
		// Find the items we have anything of to use
		foreach( Transform transform in powerupPanel.transform )
		{
			transform.gameObject.GetComponent<PowerupItemPrefabInformation>().amount = PlayerPrefs.GetInt(transform.gameObject.GetComponent<PowerupItemPrefabInformation>().effect + "Amount");
			transform.gameObject.GetComponentInChildren<Text>().text = transform.gameObject.GetComponent<PowerupItemPrefabInformation>().amount.ToString();
		}
	}

	void buttonMaker()
	{
		// Find the items we have anything of to use
		foreach( GameObject go in coinShopManager.coinShopItems )
		{
			// Make sure we do not have anything of these already listed
			if( !Tools.doesExistInList(go.name, powerupPanel.transform) )
			{
				// Check to see how many we have of this item
				int amount = PlayerPrefs.GetInt(go.GetComponent<CoinEffect>().typeOfCoin + "Amount", 0);

				// If we have more than 0
				if( amount > 0 )
				{
					// Create the button
					GameObject itemGo = Instantiate(powerupButtonPrefab) as GameObject;

					// Reparent the button
					itemGo.transform.SetParent(powerupPanel.transform, false);

					// Setup the image
					itemGo.GetComponent<Image>().sprite = go.GetComponent<CoinEffect>().prizeImage;

					// Write the amount on the label
					itemGo.GetComponentInChildren<Text>().text = amount.ToString();

					// Assign what type of coin this is
					itemGo.GetComponent<PowerupItemPrefabInformation>().effect = go.GetComponent<CoinEffect>().typeOfCoin;

					// Save the amount to the button
					itemGo.GetComponent<PowerupItemPrefabInformation>().amount = amount;

					// Setup a listener so we know which button is hit when they click on it
					itemGo.GetComponent<Button>().onClick.AddListener(() => useOnClickButton(itemGo.GetComponent<Button>()) );

					// Setup a name
					itemGo.name = go.name;

					// Enable it
					itemGo.SetActive(true);
				}
			}
		}
	}

	/// <summary>
	/// This is our listener for the onClick method for the powerup button
	/// </summary>
	/// <param name="button">Button.</param>
	void useOnClickButton(Button button)
	{
		// How much does the player have
		int playerAmountSaved = button.GetComponent<PowerupItemPrefabInformation>().amount;

		// Just a santiy check, they should alwys have enough at this point no matter what
		if( playerAmountSaved > 0 )
		{
			// Update the player saved cash value
			playerAmountSaved -= 1;

			// Remove one from their item
			PlayerPrefs.SetInt(button.GetComponent<PowerupItemPrefabInformation>().effect + "Amount", playerAmountSaved);

			// Update the amount on the button info
			button.GetComponent<PowerupItemPrefabInformation>().amount = playerAmountSaved;

			// Update the label to show how many they have
			button.GetComponentInChildren<Text>().text = playerAmountSaved.ToString();

			// Fire off the effect for this type of power up
			effectManager.runEffect(button.GetComponent<PowerupItemPrefabInformation>().effect);

			// Let's see if they depleted their inventory amount, if so, remove this button
			if( playerAmountSaved == 0 )
			{
				// If we get here, they have zero of this button, so remove it
				Destroy(button.gameObject);
			}
		}
	}
}