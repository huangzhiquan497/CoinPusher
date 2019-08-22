// Make sure that you have Unity services enabled for In App Purchasing. Enabling that also enables the Unity Analytics. Also, you need to import the files Unity
// includes under the In App Purchase portion of the Unity Services.
// For more information, reference this link: http://unity3d.com/learn/tutorials/topics/analytics/integrating-unity-iap-your-game-beta?playlist=17123

#if UNITY_ANALYTICS
using UnityEngine;
using UnityEditor;
using System.Collections;

public class IAPEditor : MonoBehaviour {

	// This sets up our menu item so we can add a new product
	[MenuItem("Window/Coin Pusher Pro/IAP/Add New Product")]
	static void addNewIAPProduct()
	{
		IAPProduct product = ScriptableObject.CreateInstance<IAPProduct>();
		AssetDatabase.CreateAsset(product, "Assets/CoinPusher/IAP/Products/NewProduct.asset");
		AssetDatabase.SaveAssets();
		EditorUtility.FocusProjectWindow();
		Selection.activeObject = product;
	}
}
#endif