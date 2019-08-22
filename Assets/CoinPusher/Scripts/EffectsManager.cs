using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EffectsManager : MonoBehaviour {

	// This is our player manager
	private CoinManager coinManager;

	// This is our play field to loop through for the quake effect
	private Transform playFieldArea;

	// This is the manager for collectable items
	private CollectableManager collectableManager;

	// This is our coin spawner
	private CoinSpawner coinSpawner;

	[Header("Global Effects Settings")]
	// This is the sound effect we play if we need to play an audio clip for rewarding the player
	public AudioClip rewardSound;

	[Header("Stop Coin Settings")]		
	// The length of time we pause
	public float timeToPausePusher = 3.0f;

	// The push bar under the machine
	private Pusher pushBar;

	[Header("Bullseye Coin Attack Settings")]
	// How many coins drop for coin attack which is the bullseye coin
	public int coinAttackDropAmount = 5;

	[Header("Bumper Coin Settings")]
	// The length of time for our effect
	public float bumpersEffectLength = 10.0f;

	// The public reference for the animator which is on the bumpers game object under the machine
	public Animator bumpersAnimator;

	// Our bumpers, we enable these only when they come up and disable when it falls down
	public MeshCollider leftBumperCollider;
	public MeshCollider rightBumperCollider;

	// Make sure we cannot trigger the bumper when it's already running
	private bool isBumpersRunning = false;

	[Header("Earth Quake Coin Settings")]
	// This is our earth shake function, if true we shake all
	public bool shakeEverything;

	// The length of time the camera and field will shake for
	public float shakeLength = 1.0f;
	private float internalShakeLength;

	// The amount of force we shake with
	public float shakeAmount = 2.0f;

	// Save the cameras original position so we can restore it when done
	private Vector3 cameraOriginalPosition;

	[Header("Popup Text Settings")]
	// This is where we parent our effect popup text images
	public Transform effectPopupImageUIPanel;

	// This is what we created to display a message to the user about something
	public Object popupTextPrefab;

	// These are the images that can be displayed as popup text
	public Sprite niceJobPopup;
	public Sprite coinAttackPopup;
	public Sprite coinShowerPopup;
	public Sprite sprinklesPopup;

	void Awake()
	{
		// Save the original camera position
		cameraOriginalPosition = Camera.main.transform.position;

		// Make a copy of this for internal tracking
		internalShakeLength = shakeLength;

		// Find our coin manager
		coinManager = GameObject.FindWithTag("CoinManager").GetComponent<CoinManager>();

		// Find our play field
		playFieldArea = GameObject.FindWithTag("CoinsPlayField").GetComponent<Transform>();

		// Find our coin spawner
		coinSpawner = GameObject.FindWithTag("CoinSpawnerArea").GetComponent<CoinSpawner>();

		// Find the collectableManager
		collectableManager = GameObject.FindWithTag("CollectableManager").GetComponent<CollectableManager>();

		// Find the pusher
		pushBar = GameObject.FindWithTag("PushBar").GetComponent<Pusher>();
	}

	void Update()
	{
		// Shake it off!
		if (shakeEverything)
		{
			// If we have time to shake still
			if (internalShakeLength > 0) 
			{
				Camera.main.transform.localPosition = cameraOriginalPosition + Random.insideUnitSphere * shakeAmount;

				internalShakeLength -= Time.deltaTime * 1.0f;

				shakeItemsOnPlayField();
			} 
			else 
			{
				// Done shaking
				shakeEverything = false;

				// Reset the camera position
				Camera.main.transform.localPosition = cameraOriginalPosition;

				// Reset our shake length timer
				internalShakeLength = shakeLength; 
			}
		}
	}

	/// <summary>
	/// This is our master function which will run all effects for the passed type of coin
	/// </summary>
	/// <param name="typeOfCoin">Type of coin.</param>
	/// <param name="coinValue">Coin value.</param>
	public void runEffect(CoinEffect.Effect typeOfCoin, int coinValue = 0)
	{
		// Which effect are we?
		switch( typeOfCoin )
		{
		case CoinEffect.Effect.RegularCoin:
			playRewardSFX();
			break;

		case CoinEffect.Effect.BumperWallCoin:
			this.triggerBumperEffect();
			playRewardSFX();
			break;

		case CoinEffect.Effect.BullseyeCoin:
			this.triggerBullsEyeEffect();
			playRewardSFX();
			break;

		case CoinEffect.Effect.CashCoin:
			// Trigger the cash effect in the effects manager while passing over the value we earned from this
			this.triggerCashEffect(coinValue);
			playRewardSFX();
			break;

		case CoinEffect.Effect.GiftCoin:
			this.triggerGiftCoin();
			playRewardSFX();
			break;

		case CoinEffect.Effect.QuakeShakeCoin:
			this.triggerEarthShakeEffect ();
			playRewardSFX();
			break;

		case CoinEffect.Effect.StopCoin:
			this.triggerStopCoin();
			playRewardSFX();
			break;

		case CoinEffect.Effect.CollectableDonut:
			this.triggerCollectableDonut(coinValue, CoinEffect.Effect.CollectableDonut);
			break;

		case CoinEffect.Effect.CollectableCocoDonut:
			this.triggerCollectableDonut(coinValue, CoinEffect.Effect.CollectableCocoDonut);
			break;

		case CoinEffect.Effect.CollectableDice:
			this.triggerCollectableDice(coinValue, CoinEffect.Effect.CollectableDice);
			break;

		case CoinEffect.Effect.CollectableOrangeDice:
			this.triggerCollectableDice(coinValue, CoinEffect.Effect.CollectableOrangeDice);
			break;

		case CoinEffect.Effect.CollectableGoldBar:
			this.triggerCollectableGoldBar(coinValue, CoinEffect.Effect.CollectableGoldBar);
			break;
		}
	}

	/// <summary>
	/// Play the sound effect we have configured for a reward of this coin
	/// </summary>
	void playRewardSFX()
	{
		// Make sure the variable is not null, if not, play it
		if( rewardSound != null )
			Camera.main.GetComponent<AudioSource>().PlayOneShot(rewardSound);
	}

	public void triggerBumperEffect()
	{
		if( !isBumpersRunning )
		{
			// Call the effect routine
			StartCoroutine(beginBumperEffect());
		}
	}

	IEnumerator beginBumperEffect()
	{
		isBumpersRunning = true;
		bumpersAnimator.SetBool("isActive", true);

		// Enable the bumper colliders
		leftBumperCollider.enabled = true;
		rightBumperCollider.enabled = true;

		// Wait some time defined above
		yield return new WaitForSeconds(bumpersEffectLength);

		bumpersAnimator.SetBool("isActive", false);
		isBumpersRunning = false;
	
		// Disable the bumper colliders
		leftBumperCollider.enabled = false;
		rightBumperCollider.enabled = false;
	}

	public void triggerBullsEyeEffect()
	{
		// This triggers the coin attack, drop X coins from the coin spawner area
		coinSpawner.coinAttackSpawner(coinAttackDropAmount);

		// Show the popup about the coin attack
		showPopupText(coinAttackPopup);
	}

	public void triggerCashEffect(int amount)
	{
		// Add the cash amount
		coinManager.addCash(amount);

		showPopupText(niceJobPopup);
	}

	public void triggerCollectableDonut(int amount, CoinEffect.Effect effect)
	{
		showPopupText(sprinklesPopup);

		collectableManager.addItem (effect, 1);
	}

	public void triggerCollectableDice(int amount, CoinEffect.Effect effect)
	{
		showPopupText(niceJobPopup);

		collectableManager.addItem (effect, 1);
	}

	public void triggerCollectableGoldBar(int amount, CoinEffect.Effect effect)
	{
		showPopupText(niceJobPopup);

		collectableManager.addItem (effect, 1);
	}
		
	public void triggerEarthShakeEffect()
	{
		// Make it shake!
		shakeEverything = true;

		#if UNITY_IOS || UNITY_ANDROID || UNITY_EDITOR
		// Shake the device if it's a phone
		Handheld.Vibrate();
		#endif
	}

	void shakeItemsOnPlayField()
	{
		// Go through each child
		foreach (Transform coin in playFieldArea) 
		{
			// Make sure they have a rigidboy so we can use it to shake things up
			if( coin.GetComponent<Rigidbody>() )
			{
				// Make some movement with add explosion
				coin.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(6f, 12f), coin.position, Random.Range(3f, 5f));

				// Add a random Flip
				coin.transform.Rotate(Random.Range(-10,10f), Random.Range(-10,10f), Random.Range(-10,10f));
			}
		}
	}

	/// <summary>
	/// This effect drops a random gift / collectable on the play field
	/// </summary>
	public void triggerGiftCoin()
	{
		// Trigger the gift
		coinSpawner.giftCoinSpawner();
	}

	/// <summary>
	/// This effect stops the pusher for a X seconds
	/// </summary>
	public void triggerStopCoin()
	{
		// Find the pusher
		pushBar = GameObject.FindWithTag("PushBar").GetComponent<Pusher>();

		// Fire off the coroutine
		StartCoroutine(stopPusherBar());
	}

	/// <summary>
	/// Stops the pusher bar, called from the triggerStopCoin
	/// </summary>
	/// <returns>The pusher bar.</returns>
	IEnumerator stopPusherBar()
	{
		// Pause the bar
		pushBar.paused = true;

		// Pause for the length of time we defined
		yield return new WaitForSeconds(timeToPausePusher);

		// Un pause now
		pushBar.paused = false;;
	}

	/// <summary>
	/// This is our private function that is used for effects to show, at random spots on the screen, the popup text for the effect
	/// </summary>
	/// <param name="popupToShow">Popup to show.</param>
	void showPopupText(Sprite popupToShow)
	{
		// Draw the cash image
		GameObject go = GameObject.Instantiate(popupTextPrefab) as GameObject;

		// Define which popup image we use
		go.GetComponent<Image> ().sprite = popupToShow;
	
		// Set the parent to the UI canvas, keep the world position stays to false so we reuse what we had in the prefab which is correct
		go.transform.SetParent(effectPopupImageUIPanel, false);

		// Set a random Y position to space them apart
		go.transform.localPosition = new Vector3(0, Random.Range(-300, 300), 0);
	}
}