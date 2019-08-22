using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour {

	// This is the list of things that need data to be saved
	public CoinManager coinManager;
	public LevelManager levelManager;
	public CollectableManager collectableManager;

	/// <summary>
	/// Loads the data.
	/// </summary>
	public void loadData()
	{
		// Get data for the CoinManager
		if( PlayerPrefs.HasKey("currentCoinTotal") )
			coinManager.currentCoinTotal = PlayerPrefs.GetInt("currentCoinTotal");

		if( PlayerPrefs.HasKey("playerCash") )
			coinManager.playerCash = PlayerPrefs.GetInt("playerCash");

		// Get data for the LevelManager
		if( PlayerPrefs.HasKey("currentLevel") )
			levelManager.currentLevel = PlayerPrefs.GetFloat ("currentLevel");

		if( PlayerPrefs.HasKey("currentLevelAmount") )
			levelManager.currentLevelAmount = PlayerPrefs.GetFloat ("currentLevelAmount");

		// Get the data for the CollectableManager
		if (PlayerPrefs.HasKey ("collectables")) 
			collectableManager.inventory = PlayerPrefsSerialize<Dictionary<CoinEffect.Effect, int>>.Load ("collectables");
		else 
			collectableManager.inventory = new Dictionary<CoinEffect.Effect, int>();
	}

	/// <summary>
	/// Saves the data.
	/// </summary>
	public void saveData()
	{
		// Save data for the CoinManager
		PlayerPrefs.SetInt("currentCoinTotal", coinManager.currentCoinTotal);
		PlayerPrefs.SetInt("playerCash", coinManager.playerCash);

		// Save data for the LevelManager
		PlayerPrefs.SetFloat ("currentLevel", levelManager.currentLevel);
		PlayerPrefs.SetFloat ("currentLevelAmount", levelManager.currentLevelAmount);

		// Save data for the CollectableManager
		PlayerPrefsSerialize<Dictionary<CoinEffect.Effect, int>>.Save (collectableManager.collectableSaveName, collectableManager.inventory);
	}

	/// <summary>
	/// Checks for saved game.
	/// </summary>
	/// <returns><c>true</c>, if for saved game was checked, <c>false</c> otherwise.</returns>
	public bool checkForSavedGame()
	{
		// See what the return value is, if it is -1 we have no value
		return PlayerPrefs.GetInt("currentCoinTotal", -1) == -1 ? false : true;
	}

	/// <summary>
	/// Deletes all saved game data.
	/// </summary>
	public void deleteAllSavedGameData()
	{
		PlayerPrefs.DeleteAll ();
	}
}