using UnityEngine;
using System.Collections;

public class OutOfCoinsManager : MonoBehaviour {

	// The popup panel itself
	public GameObject outOfCoinPopup;
	public CoinManager coinManager;
	public AdManager adManager;

	// Is our window open
	public bool isWindowOpen = false;

	/// <summary>
	/// This is called when they click the button to earn free coins
	/// </summary>
	public void freeCoinButton()
	{
		// Close the window
		closeWindow();

		// Fire off free coins
		adManager.freeCoinButton ();
	}

	/// <summary>
	/// This is where IAP would be handled
	/// </summary>
	public void buyCoinButton()
	{
		// Close the window
		closeWindow();
	}

	/// <summary>
	/// Closes the popup
	/// </summary>
	public void cancelButton()
	{
		// Close the window
		closeWindow();
	}

	/// <summary>
	/// Shows the window that they are out of coins
	/// </summary>
	public void showWindow()
	{
		outOfCoinPopup.SetActive (true);

		// Flag it open
		isWindowOpen = true;
	}

	/// <summary>
	/// Closes the window and resets the variables needed
	/// </summary>
	void closeWindow()
	{
		outOfCoinPopup.SetActive (false);

		// We are closed
		isWindowOpen = false;
	}
}