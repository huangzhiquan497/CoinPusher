using UnityEngine;
using System.Collections;

public class CoinEffect : MonoBehaviour {

	// This is the value this coin is worth
	public int coinValue = 1;

	// Our EffectsManager
	private EffectsManager effectsManager;

	// Let's define the type of effect this coin is
	public enum Effect {
		RegularCoin,
		BumperWallCoin,
		BullseyeCoin,
		CashCoin,
		GiftCoin,
		QuakeShakeCoin,
		StopCoin,
		CollectableDonut,
		CollectableCocoDonut,
		CollectableDice,
		CollectableOrangeDice,
		CollectableGoldBar
	}
	public Effect typeOfCoin;

	// This is used to prevent the coin SFX from playing when the coins start on the play field
	public bool alreadyOnPlayField;

	// This is the SFX of when the coin is dropped
	public AudioClip droppedSound;

	// Did it land, set this to true so we stop collisions later
	private bool didLand = false;

	// This is the sound effect when the coin is dropped and not collected
	public AudioClip destroyedSound;

	[Header("Collectable and Coin Shop Settings")]

	// Prize only related things, if it can be sold on the collectable screen and in the coin shop for buying
	public Sprite prizeImage;

	// The price of this item in the coin shop only
	public int coinShopItemPrice = 1;

	void Start()
	{
		// Grab the object
		effectsManager = GameObject.FindWithTag("EffectManager").GetComponent<EffectsManager>();

		// Reparent this new coin
		this.gameObject.transform.parent = GameObject.FindWithTag("CoinsPlayField").transform;
	}

	/// <summary>
	/// Gets the value.
	/// </summary>
	/// <returns>The value.</returns>
	public int getValue()
	{
		return coinValue;
	}

	/// <summary>
	/// This is called from any area that needs to trigger the effect which is passed on to the effects manager
	/// </summary>
	public void effect()
	{
		effectsManager.runEffect(typeOfCoin, coinValue);

		// Remove the coin
		removeCoin();
	}

	/// <summary>
	/// This is called externally, in the coin destroyer, when it drops and is not collected.
	/// </summary>
	public void playDestroyedSFX()
	{
		// Make sure the variable is not null, if not, play it
		if( destroyedSound != null )
			Camera.main.GetComponent<AudioSource>().PlayOneShot(destroyedSound);
	}

	// Remove this object when done
	public void removeCoin()
	{
		DestroyObject(this.gameObject, 1.0f);
	}

	void OnCollisionEnter (Collision col)
	{
		// If we did not land
		if( !didLand )
		{
			// If we were not already on the play field (aka a coin in the beginnning)
			if( !alreadyOnPlayField )
			{
				// If we hit the push bar, floor
				if( col.gameObject.CompareTag("PushBar") ||  col.gameObject.CompareTag("Floor") )
				{
					// Make sure there is a sound effect to play
					if( droppedSound != null )
						Camera.main.GetComponent<AudioSource>().PlayOneShot(droppedSound);

					// Mark that we landed already
					didLand = true;
				}
			}
		}
	}
}