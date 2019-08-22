// IAP Manager
// Make sure that you have Unity services enabled for In App Purchasing. Enabling that also enables the Unity Analytics. Also, you need to import the files Unity
// includes under the In App Purchase portion of the Unity Services.
// For more information, reference this link: http://unity3d.com/learn/tutorials/topics/analytics/integrating-unity-iap-your-game-beta?playlist=17123

#if UNITY_ANALYTICS
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;
using System;
using System.Collections;
using System.Collections.Generic;

public class IAPManager : MonoBehaviour, IStoreListener {

	public GameObject storeWindowPanel;
	public CoinManager coinManager;
	public Text playerCashLabel;
	public GameObject restorePurchasesButton;

	private IStoreController controller;
	private IExtensionProvider extensions;

	[Header("Consumable Products")]
	public IAPProduct[] products;

	// Use this for initialization
	void Start () {
	
		// Make sure we do not have the system setup yet
		if( controller == null )
		{
			// Setup everything
			InitPurchasing();
		}

		// Only show the button on Apple platforms to restore purchases, other platforms handle this automatically
#if UNITY_IOS || UNITY_EDITOR
	restorePurchasesButton.SetActive(true);
#else
	restorePurchasesButton.SetActive(false);
#endif
	}

	void Update()
	{
		// Update the cash label
		playerCashLabel.text = coinManager.playerCash.ToString();
	}

	/// <summary>
	/// This buy button will allow you to drag and drop the actual Product asset (in IAP/Products) to link the button to this item
	/// </summary>
	/// <param name="product">Product ID which is pulled from the IAPProduct</param>
	public void buyButton(IAPProduct product)
	{
		buyProduct(product.internalID);
	}

	/// <summary>
	/// This version allows you to use a string for the internal ID
	/// </summary>
	/// <param name="productID">Product ID which is provided through the command line in the form of a string</param>
	public void buyButton(string productID)
	{
		buyProduct(productID);
	}

	/// <summary>
	/// Buy the specified product by using the IAPProduct
	/// </summary>
	/// <param name="product">Product.</param>
	public void buy(IAPProduct product)
	{
		buyProduct(product.internalID);
	}

	/// <summary>
	/// Buy the specified product by using a string ID
	/// </summary>
	/// <param name="productID">Product I.</param>
	public void buy(string productID)
	{
		buyProduct(productID);
	}

	/// <summary>
	/// Setup the purchasing, add our products we have and set up callbacks
	/// </summary>
	public void InitPurchasing()
	{
		// If for some reason we already are initalized
		if( isInitalized() )
		{
			// ... then just return since we are already configured
			return;
		}
		
		// Define the products that we can sell / restore
		var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

		// Go through the products above and add them
		foreach( IAPProduct product in products )
		{
			// Add each one to the builder
			builder.AddProduct(product.internalID, product.productType, new IDs
				{
					{product.appleID, AppleAppStore.Name},
					{product.googlePlayID, GooglePlay.Name}
				});
		}
		
		// Initalize the purchasing system in unity using our new products above, assign all call backs to our script here
		UnityPurchasing.Initialize(this, builder);
	}

	/// <summary>
	/// This purchases the product using the product ID
	/// </summary>
	/// <param name="productID">Product ID of what to purchase</param>
	void buyProduct(string productID)
	{
		try
		{
			// Make sure our system is initialized
			if( isInitalized() )
			{
				// Look up the product
				Product product = controller.products.WithID(productID);

				// Make sure our product isn't null and that it is available for purchase
				if( product != null && product.availableToPurchase )
				{
					//Debug.Log (string.Format("!! Purchasing product asychronously: '{0}'", product.definition.id));

					// Let's begin to purchase the product
					controller.InitiatePurchase(product);
				}

			}
			else
			{
				Debug.LogError("!! Purchasing system not initialized");
			}
		}
		catch( Exception e )
		{
			Debug.LogError("!! Product purchase failed : " + e);
		}
	}

	/// <summary>
	/// This checks to see if we are ready and initalized.
	/// </summary>
	bool isInitalized()
	{
		// Everything needs to be true for us to continue
		return this.controller != null && this.extensions != null;
	}

	/// <summary>
	/// Restores the purchases, only applicable for the Apple platforms. Other platforms restore automatically upon install.
	/// </summary>
	public void restorePurchases()
	{
		// Make sure that we are initialized
		if(!isInitalized())
		{
			// Warn that we cannot continue and jump out
			Debug.Log("!! Restore Puchases - Purchasing has not been initalized!");

			return;
		}

		// Make sure we only run this on the Apple platforms
		if( Application.platform == RuntimePlatform.IPhonePlayer || 
			Application.platform == RuntimePlatform.OSXPlayer )
		{
			// Load the apple extensions
			var applePlatformExtensions = extensions.GetExtension<IAppleExtensions>();

			// Restore everthing we can now
			applePlatformExtensions.RestoreTransactions((result) => 
				{
					Debug.Log("!! Restore Purchases - Restores continuing: " + result);
				});
		}
		else
		{
			// Our platform is not Apple, skip restoring
			Debug.Log("!! Restore Purchases - This platform does not require restoring purchases.");
		}

	}

	/// <summary>
	/// Called when Unity IAP is ready to make purchases.
	/// </summary>
	public void OnInitialized (IStoreController controller, IExtensionProvider extensions)
	{
		// Init our variables
		this.controller = controller;
		this.extensions = extensions;
	}

	/// <summary>
	/// Called when Unity IAP encounters an unrecoverable initialization error.
	///
	/// Note that this will not be called if Internet is unavailable; Unity IAP
	/// will attempt initialization until it becomes available.
	/// </summary>
	public void OnInitializeFailed (InitializationFailureReason error)
	{
		Debug.Log("!! INIT FAILED");
	}

	/// <summary>
	/// Called when a purchase completes. This is also called when the restore purchase happens as well so you can restore their purchases.
	///
	/// May be called at any time after OnInitialized().
	/// </summary>
	public PurchaseProcessingResult ProcessPurchase (PurchaseEventArgs args)
	{
		// We know need to give the user whatever they bought, we compare against the product.internalID on the asset in IAP/Products
		switch(args.purchasedProduct.definition.id)
		{
		case "10CoinStack":
			// Now provide the player the amount of coins they bought!
			coinManager.addCoin(10, true);
			break;

		case "50CoinStack":
			// Now provide the player the amount of coins they bought!
			coinManager.addCoin(50, true);
			break;

		case "100CoinStack":
			// Now provide the player the amount of coins they bought!
			coinManager.addCoin(100, true);
			break;

		case "10StackOfCash":
			// Now provide the player the amount of cash they bought!
			coinManager.addCash(10);
			break;
		}

		// Here is where we would set up any products that were non consumables 
		switch(args.purchasedProduct.definition.id)
		{
		case "ValentineLevel":
			Debug.Log("Just purchased or restored the valentine level. You would either give them the level now or restore it since they already bought it.");
			break;

		}

		/* If you want to know how to loop through the products, use the code below
		// Loop through the products we have to see what they bought
		foreach( IAPProduct product in products )
		{
			if( String.Equals(args.purchasedProduct.definition.id, product.internalID, StringComparison.Ordinal) )
			{
				Debug.Log("!! We bought: " + args.purchasedProduct.definition.id + " -- " + product.internalID);
			}
		}
		*/

		// Return that we completed the purchase
		return PurchaseProcessingResult.Complete;
	}

	/// <summary>
	/// Called when a purchase fails.
	/// </summary>
	public void OnPurchaseFailed (Product i, PurchaseFailureReason p)
	{
		Debug.Log("!! PURCHASE FAILED!");
	}


	// UI Functions only below

	/// <summary>
	/// Shows the window.
	/// </summary>
	public void showWindow()
	{
		storeWindowPanel.SetActive(true);
	}

	/// <summary>
	/// Closes the window.
	/// </summary>
	public void closeWindow()
	{
		storeWindowPanel.SetActive(false);
	}
}
#endif