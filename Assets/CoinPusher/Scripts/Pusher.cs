using UnityEngine;
using System.Collections;

public class Pusher : MonoBehaviour {

	public Transform destination;		// Where the pusher will stop, this is an empty gameobject
	public float moveSpeed = 0.25f;		// How fast will the pusher move
	private Vector3 originalPosition;	// The original position of the pusher, basically this is the "home position" of where it will stop

	// This is used to pause from the Stop Coin
	public bool paused = false;

	// This is used to track our own "time" inside of the game for the pusher bar
	private float timeTracker = 0.0f;

	// This is just a limit we compare to, we need to reset this variable every once in a while
	private float timeLimit = 10f;
	
	void Start()
	{
		// Save the original position of the pusher, this is where it starts
		originalPosition = transform.position;
	}

	void Update()
	{
		// If the pusher bar is paused
		if( !paused )
		{
			// Move the push bar
			GetComponent<Rigidbody>().MovePosition(Vector3.Lerp (originalPosition, destination.position, Mathf.PingPong(timeTracker * moveSpeed, 1.0f)));

			// Bump up our own time tracker
			timeTracker += 0.01f;

			// If we're back at the home position, we then check to see if our time limit is exceeded - This prevents skipping of the bar
			if( GetComponent<Rigidbody>().position == originalPosition )
			{
				// If we're larger than time limit, reset back to 0
				if( timeTracker > timeLimit )
					timeTracker = 0.0f;	// Reset the tracker
			}
		}

		/*
		// Move using a rigidbody MovePosition so we can rely on physics
		GetComponent<Rigidbody>().MovePosition(Vector3.Lerp (originalPosition, destination.position, Mathf.PingPong(Time.time * moveSpeed, 1.0f)));
		*/
	}
}