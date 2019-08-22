using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class CoinManager : MonoBehaviour {

	[Header("DEBUG ONLY")]
	public bool deleteDataOnStart = false;

	[Header("Coin Manager Settings")]

	// Our save manager
	public SaveManager saveManager;

	// Find our level manager
	public LevelManager levelManager;

	// This is the amount of coins that the player should start with
	public int startCoinTotal = 100;

	// This is the amount of coins the player currently has. This is pulled from the saved data.
	public int currentCoinTotal = 0;

	// This is how many coins the user can drop at once without hitting a cool down
	public int maxCoinDrop = 5;

	// This is used to track internally how many coins they have dropped against the maxCoinDrop
	[HideInInspector] public int currentCoinCount;

	[Header("Coin Regen and Hopper Settings")]

	// How often do they get a free coin (in seconds)
	public int nextAvailableCoinSeconds = 30;

	// This is used internally for tracking the time that has past
	private float timeLeftForNextCoin;

	// This is the label in the UI for the current available coins
	public Text currentCoinTotalLabel;

	// This is the label in the UI for the amount of time until next coin
	public Text nextCoinInSecondsLabel;

	// This is how fast the user will get a new coin back in seconds
	public float coinRefillRate = 5.0f;

	// This is the multiplier that they will get coins, it is multiplied against the level number
	public int refillMultiplier = 2;

	// This is used to track how many coins, equal to their level number, the user can regen over time
	public int currentLevelCountAmount = 1;

	// This is used internally to track the amount of time that has lapse so we can refill their coin hopper
	private float timeLeftRefill;

	// These images are used to update the coin refill hopper area on the UI
	public Image[] coinHopperImages;

	// This is the sprite that shows a coin is available
	public Sprite coinAvailableSprite;

	// This is the sprite for when a coin is not available
	public Sprite coinNotAvailableSprite;

	// Is this our first touch? If so, we disable the touch here area and enabler/disabler
	private bool isFirstTouch = true;

	// The internal timer to control flashing
	private float timeLeftFlasher;

	[Header("UI Settings")]

	// The rate at which we flash
	public float touchHereFlashRate = 1.0f;

	// The gameobject we want to enable/disable (flash). This is the touch here area label.
	public GameObject touchHereLabel;

	// How much cash the player has earned
	public int playerCash = 0;

	// The cash UI label
	public Text playerCashLabel;

	// This is the area that shows the current picked up coin value
	public Text coinCounterLabel;

	// This is used to clear out the coin counter label after a coin is caught
	[Range(0f, 3f)]
	public float coinCounterLabelTimeout = 1f;

	// This is set externally to see if we can spawn a coin, this is used for any popup windows to disable spawning
	public bool coinSpawnerReady = true;

	[Header("Particle Reward Systems")]
	public ParticleSystem coinRewardParticleSystem;

	// Use this for initialization
	void Start () 
	{
		// DEBUG ONLY when set in inspector
		if( deleteDataOnStart )
		{
			// This will delete all of the saved data, only use this for DEBUG!!
			saveManager.deleteAllSavedGameData();
		}
		
		//Check for a saved game, if so, load the last coin amount
		if( saveManager.checkForSavedGame() )
		{
			// Load the data
			saveManager.loadData();			
		}
		else
		{
			// Set this to the beginning amount since they have not played yet
			currentCoinTotal = startCoinTotal;	

			// On first play, just set up the defaults. Once we increase a level, this is handled in the update level function in LevelManager
			currentLevelCountAmount *= refillMultiplier;
		}

		// Set up the time left timer with the defined amount they set in the inspector
		timeLeftForNextCoin = nextAvailableCoinSeconds;

		// Set up the timer for later
		timeLeftRefill = coinRefillRate;

		// Set up the amount of coins they have at the beginning in their hopper
		currentCoinCount = maxCoinDrop;

		// Set up the timer for the touch here flasher
		timeLeftFlasher = touchHereFlashRate;
	}
	
	void Update ()
	{
		// Update the label with the amount of coins they have
		currentCoinTotalLabel.text = currentCoinTotal.ToString();

		// Update the label with how many seconds are left to next free coin, convert the timer to an int to string
		nextCoinInSecondsLabel.text = Mathf.FloorToInt(timeLeftForNextCoin).ToString();

		// Update the players cash label
		playerCashLabel.text = playerCash.ToString();

		// This is our timer that gives a free coin after X seconds
		nextAvailableCoinGenerator();

		// Update hopper UI bar
		updateCoinHopperImages();

		// Run the timer to refill the coin hopper
		refillCoinHopper();

		// Check to see if this is our first touch, if so, we display the touch here area
		checkFirstTouch();
	}

	/// <summary>
	/// Check to see if this is our first touch, if so, display the touch area. If not, disable the touch area and the timer that flashes it.
	/// </summary>
	void checkFirstTouch()
	{
		// If this is the first touch, set it inactive
		if( !isFirstTouch )
		{
			touchHereLabel.SetActive(false);
		}
		else if( isFirstTouch )
		{
			// Subtract some time
			timeLeftFlasher -= Time.deltaTime;

			// If time has run out
			if( timeLeftFlasher < 0.0f )
			{
				touchHereLabel.SetActive(!touchHereLabel.activeInHierarchy);

				// Set up the disable rate again, aka, reset the time
				timeLeftFlasher = touchHereFlashRate;
			}
		}
	}

	/// <summary>
	/// This updates the UI with the images for the coins they are using
	/// </summary>
	void updateCoinHopperImages()
	{
		// Draw the coins the user has
		for( int i = 0; i < currentCoinCount; i++)
			coinHopperImages[i].sprite = coinAvailableSprite;

		// Draw the empty coins for the rest of the spaces
		for( int i = maxCoinDrop - currentCoinCount; i >= 1; i-- )
		{
			coinHopperImages[maxCoinDrop - i].sprite = coinNotAvailableSprite;
		}
	}

	/// <summary>
	/// This is our timer that will refill their hopper for more coins
	/// </summary>
	void refillCoinHopper()
	{
		timeLeftRefill -= Time.deltaTime;

		if( timeLeftRefill < 0.0f )
		{
			// Add a coin since we passed our timer
			if( currentCoinCount < maxCoinDrop )
			{
				// Bump up the coin amount they have dropped
				currentCoinCount++;

				// Reset the timer back to the refill rate
				timeLeftRefill = coinRefillRate;
			}
		}
	}
	
	/// <summary>
	/// This gives the user a free coin after X seconds
	/// </summary>
	void nextAvailableCoinGenerator()
	{
		timeLeftForNextCoin -= Time.deltaTime;

		if( timeLeftForNextCoin < 0.0f )
		{
			// Do we still have coins to spawn?
			if( currentLevelCountAmount > 0 )
			{	
				// Spawn a coin
				currentCoinTotal++;	

				// Remove one coin from our level amount for regeneration of coins
				currentLevelCountAmount--;
			}

			// Reset the timer
			timeLeftForNextCoin = nextAvailableCoinSeconds;	
		}
	}

	/// <summary>
	/// This is called from the LevelManager whenever we go up an entire level. This resets the amount of coins the player
	/// gets per level for free.
	/// </summary>
	public void resetCoinLevelGenAmount()
	{
		// Save back the current level count amount, this is multiplied by the multplier to give users X coins for free per level
		currentLevelCountAmount = (int)levelManager.currentLevel * refillMultiplier;
	}

	/// <summary>
	/// Check to see if we can spawn a coin or not
	/// </summary>
	/// <returns><c>true</c>, if current total of coins is > 0<c>false</c> otherwise.</returns>
	public bool canSpawnCoin()
	{		
		// Make sure the user has enough coins and enough coins in the 'hopper' aka on deck aka the amount that ticks down each coin drop
		return (currentCoinTotal > 0) && (currentCoinCount > 0) && (coinSpawnerReady) ? true : false;
	}

	/// <summary>
	/// This function removes a coin from the total the player has. This is called from the CoinSpawner.
	/// </summary>
	public void removeCoin()
	{
		// Check to see if this is our first coin, if so, mark it now false since this is not our first coin
		if( isFirstTouch ) isFirstTouch = false;

		currentCoinCount--;			// Remove one coin from their hopper
		currentCoinTotal--;			// Remove one coin
	}

	/// <summary>
	/// This is the function that will add a coin to their total coins
	/// </summary>
	/// <param name="coinType">Coin type.</param>
	public void addCoin(int coinValue, bool isReward)
	{
		// Add the coin value they got to the score
		currentCoinTotal += coinValue;	

		// If this is a reward coin, show them some fancy particles
		if( isReward )
		{
			coinRewardParticleSystem.Play();
			coinRewardParticleSystem.Stop();
		}

		// Fire up the coroutine
		StartCoroutine (addCoinCounterLabelUpdate (coinValue));
	}

	IEnumerator addCoinCounterLabelUpdate(int coinValue)
	{
		// Show the value of the coin they picked up!
		coinCounterLabel.text = coinValue.ToString ();

		// Wait
		yield return new WaitForSeconds(coinCounterLabelTimeout);

		// Clear out label
		coinCounterLabel.text = "";
	}

	/// <summary>
	/// Called to add cash to the player for buying things later
	/// </summary>
	/// <param name="amount">Amount of cash to add</param>
	public void addCash( int amount )
	{
		// Add the player their cash
		playerCash += amount;

		saveManager.saveData();
	}

	/// <summary>
	/// This rewards the user some coins
	/// </summary>
	/// <param name="amount">Amount of coins to reward the user</param>
	public void addRewardCoin(int rewardAmount)
	{
		// Add the coin
		addCoin(rewardAmount, true);

		// Thank the user
	}

	/// <summary>
	/// This rewards the user some cash
	/// </summary>
	/// <param name="amount">Amount of cash to reward the user</param>
	public void addRewardCash(int rewardAmount)
	{
		// Add the cash
		addCash(rewardAmount);
	}

	/*
	public float pausedTime;
	public float unpausedTime;
	void OnApplicationPause(bool pauseStatus)
	{
		// If we're paused, log what time we paused
		if( pauseStatus )
		{
			// Save the time we paused
			Debug.Log("Pause Time = " + DateTime.Now.ToString());
		}
		else if( !pauseStatus )
		{
			Debug.Log("Unpause Time = " + DateTime.Now.ToString());
		}
	}
	*/
}