using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour {

	// Our save manager
	public SaveManager saveManager;

	// This is our coin manager
	public CoinManager coinManager;

	// This is the level the player currently is
	public float currentLevel = 1;

	// This is the players current level amount until next level. When it is 1 it hits the next level
	public float currentLevelAmount = 0;

	// This is the amount of percentage each item / coin adds to their level
	public float incrementAmount = 0.10f;

	// This is the Image that shows the amount of fill for the level
	public Image levelFillBar;

	// This is the label that is updated to show the level amount
	public Text levelAmountLabel;

	void Start()
	{
		// Load any data we may have
		saveManager.loadData();
	}

	// Update is called once per frame
	void Update () {

		// Update the fill amount
		levelFillBar.fillAmount = currentLevelAmount;

		// Update the level amount
		levelAmountLabel.text = currentLevel.ToString();
	}

	/// <summary>
	/// This is used to update the players level information
	/// </summary>
	/// <param name="updateAmount">Update amount.</param>
	public void updateLevel(float updateAmount)
	{
		// Add in the update amount for the level
		currentLevelAmount += (updateAmount * incrementAmount);

		// If we're over 1, reset back to 0 and increment the players level
		if (currentLevelAmount > 1) 
		{
			// Reset this back to 0
			currentLevelAmount = 0;

			// Add another level on the player
			currentLevel++;

			// Setup the coin manager currentLevelCountAmount value, this is used to track available regen coins
			coinManager.currentLevelCountAmount = (int)currentLevel;

			// Reset the counter inside of the CoinManager
			coinManager.resetCoinLevelGenAmount();
		}

		// Save the data since we modified it
		saveManager.saveData();
	}
}