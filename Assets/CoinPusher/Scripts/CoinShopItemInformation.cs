using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CoinShopItemInformation : MonoBehaviour {

	private CoinManager coinManager;
	private SaveManager saveManager;

	[Header("Item Information")]

	public string itemName;

	// The cost this item costs
	public int itemCost = 1;
	public Sprite itemImage;
	public CoinEffect.Effect effect;

	[Header("UI Settings")]

	public Text itemCostLabel;
	public Image itemImageUI;
	public Button sellButton;

	void Start()
	{
		itemCostLabel.text = itemCost.ToString();
		itemImageUI.sprite = itemImage;

		// Find the coin manager
		coinManager = GameObject.FindGameObjectWithTag("CoinManager").GetComponent<CoinManager>();
		saveManager = GameObject.FindGameObjectWithTag("SaveManager").GetComponent<SaveManager>();
	}

	/// <summary>
	/// When the user clicks this button on the item prefab, we will sell them the item
	/// </summary>
	public void sellButtonClick()
	{
		// Make sure they have enough cash to buy this
		if( coinManager.playerCash >= itemCost )
		{
			// Remove the cost from their cash
			coinManager.playerCash -= itemCost;

			// Now give them the item they bought
			int amount = PlayerPrefs.GetInt(effect + "Amount");

			// So they have amount above, add it back into the player prefs now. Just added 1 each purchase.
			PlayerPrefs.SetInt(effect + "Amount", amount + 1);

			// Save the data
			saveManager.saveData();
		}
	}
}