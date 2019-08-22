// Make sure that you have Unity services enabled for In App Purchasing. Enabling that also enables the Unity Analytics. Also, you need to import the files Unity
// includes under the In App Purchase portion of the Unity Services.
// For more information, reference this link: http://unity3d.com/learn/tutorials/topics/analytics/integrating-unity-iap-your-game-beta?playlist=17123

#if UNITY_ANALYTICS
using UnityEngine;
using UnityEngine.Purchasing;
using System.Collections;

[System.Serializable]
public class IAPProduct : ScriptableObject {

	public ProductType productType;

	public string internalID = "";
	public string appleID = "";
	public string googlePlayID = "";
}
#endif