using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AudioManager : MonoBehaviour {

	// These are our audio sources we need to mute
	public AudioSource[] audioSources;

	public Image muteButtonImage; 
	public Sprite muteSprite;
	public Sprite unmuteSprite;

	/// <summary>
	/// This is the mute button on the GUI
	/// </summary>
	public void muteButton()
	{
		// Loop through each audio source
		foreach( AudioSource audioSource in audioSources )
		{
			// Mute or umute the audio source
			audioSource.mute = !audioSource.mute;

			// Flip the sprite depending on what state we are in
			if( audioSource.mute )
				muteButtonImage.sprite = unmuteSprite;
			else
				muteButtonImage.sprite = muteSprite;
		}
	}
}