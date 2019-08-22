// This is the AD Manager for the game assset.
//
// NOTE: Uncomment this #define below to enable ADs based
//
//#define USING_ADS

using UnityEngine;

#if UNITY_ADS
using UnityEngine.Advertisements;
#endif

using System.Collections;

public class AdManager : MonoBehaviour {

	// This is the string of the reward zone
	public string rewardZoneName = "rewardedVideo";

	// This is the amount of coins we give the user for watching the video
	[Header("Reward Information")]
	public int videoRewardAmount = 5;

	// Our coin manager
	private CoinManager coinManager;

	void Start()
	{
		// Get our coin manager
		coinManager = GameObject.FindGameObjectWithTag("CoinManager").GetComponent<CoinManager>();
	}

	/// <summary>
	/// This is called from the UI to give the user a free coin if they do something, watch a video, etc
	/// </summary>
	public void freeCoinButton()
	{
		#if USING_ADS // Using ADs
		
		// If we are in the Unity Editor, let's have the game pause
		#if UNITY_EDITOR
			StartCoroutine(waitForAdToFinish());
		#endif

		// If we're on a mobile platform, we can run ads
		#if UNITY_IOS || UNITY_ANDROID || UNITY_EDITOR
		// If the ad is ready, which is a reward based on, show it and make sure to send the options
		if (Advertisement.IsReady(rewardZoneName))
		{
			var options = new ShowOptions { resultCallback = AdCallBackHandler };
			Advertisement.Show(rewardZoneName, options);
		}
		#endif

		// If WEBGL build give them the free coins
		#if UNITY_WEBGL
		coinManager.addCoin(videoRewardAmount, true);
		#endif

		#else	// else from Unity ADs above
			Debug.LogWarning("You need to enable Unity ADs this to work. Reference the README for details.");	// Warn the user that this is not running ads
		#endif
	}

	#if USING_ADS
	#if UNITY_IOS || UNITY_ANDROID || UNITY_EDITOR 
	/// <summary>
	/// This is our call back handler which is used when showing an AD
	/// </summary>
	/// <param name="result">Result.</param>
	void AdCallBackHandler(ShowResult result)
	{
		// Depending on what happens
		switch(result)
		{
		// They finished the AD, so let's give them something for free
		case ShowResult.Finished:
			
			// Since the user finished watching the ad, give them something!
			coinManager.addCoin(videoRewardAmount, true);
			break;
		
		// This will not be called 
		case ShowResult.Skipped:
			break;

		// Just in case of a failutre
		case ShowResult.Failed:
			break;
		}
	}

	/// <summary>
	/// This is a private ienumerator that runs only in the editor to simulate pausing on mobile
	/// </summary>
	/// <returns>The for ad to finish.</returns>
	IEnumerator waitForAdToFinish()
	{
		// Save the current time scale
		float currentTimeScale = Time.timeScale;

		// Basically pause the game
		Time.timeScale = 0.0f;

		// Do nothing
		yield return null;

		while (Advertisement.isShowing)
			yield return null;	// Continue to do nothing

		// Now that we are out of the ad showing, resume time operations
		Time.timeScale = currentTimeScale;
	}
	#endif
	#endif // From Unity ADs
}