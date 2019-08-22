using UnityEngine;
using System.Collections;

public class RemoveAfter : MonoBehaviour {

	// Our public value of how long until we remove this
	public float removeAfterTime = 1.5f;

	// Use this for initialization
	void Start () {

		// Fire up the coroutine
		StartCoroutine(removeAfter());
	}

	IEnumerator removeAfter()
	{
		// Wait until the timer is up
		yield return new WaitForSeconds(removeAfterTime);

		// If we have an animator, play the animation called "hide"
		if (this.GetComponent<Animator> ())
		{
			// Set this bool true so we can hide this off screen
			this.GetComponent<Animator> ().SetBool ("hide", true);
		}

		// Wait a bit longer again, just to remove
		yield return new WaitForSeconds(removeAfterTime);
		
		// Once this object is spawned, plan it's demise!
		DestroyObject(this.gameObject);
	}
}