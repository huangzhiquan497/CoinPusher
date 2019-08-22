using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CollectableItemInformation : MonoBehaviour {

	[Header("Item Information")]
	public CoinEffect.Effect itemEffectType;
	public int itemAmount = 0;
	public int itemSellBackAmount = 1;
	public Sprite itemSprite;
	public string itemName = "";

	// This is the popup that shows they earned some coins from what they did
	public GameObject coinItemSellPopup;
	public AudioClip soldSFX;

	[Header("UI Settings")]
	public Text itemAmountLabel;
	public Image itemUIImage;
	public Button sellButtonUI;

	// Our coin manager
	private CoinManager coinManager;

	// Our collectable manager
	private CollectableManager collectableManager;

	// Our popup window
	private GameObject collectablePanelWindow;

	void Start()
	{
		// Grab our coin manager
		coinManager = GameObject.FindWithTag ("CoinManager").GetComponent<CoinManager> ();

		// Grab our collectable manager
		collectableManager = GameObject.FindWithTag("CollectableManager").GetComponent<CollectableManager>();

		collectablePanelWindow = GameObject.Find("CollectablePanelWindow");

		// Setup the sprite image in the UI
		itemUIImage.sprite = itemSprite;
	}

	void Update()
	{
		// Update the amount we have
		itemAmountLabel.text = itemAmount.ToString();

		// Check to see if we are at 0, if so, disable the sell button
		if (itemAmount <= 0) 
		{
			sellButtonUI.interactable = false;
		} 
		else 
		{
			sellButtonUI.interactable = true;
		}
	}

	public void sellButton()
	{
		// Make sure they have more than 0 of this item
		if (itemAmount > 0) 
		{
			// Spawn the item coin reward image showing how many coins they jsut earnd from selling this
			GameObject coinSoldPopup = Instantiate(coinItemSellPopup);
			coinSoldPopup.transform.SetParent(gameObject.transform, false);
			coinItemSellPopup.GetComponentInChildren<Text> ().text = itemSellBackAmount.ToString ();

			if( soldSFX != null)
				Camera.main.GetComponent<AudioSource> ().PlayOneShot (soldSFX);

			// Add the coin they got for it
			coinManager.addCoin (itemSellBackAmount, false);

			// Remove one item from their inventory
			collectableManager.removeItem (itemEffectType, 1);

			// Remove one
			itemAmount--;
		}
	}
}