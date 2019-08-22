using UnityEngine;
using System.Collections;

public class HideAfter : MonoBehaviour {

	// Our public value of how long until we hide this
	public float hideAfterTime = 1.5f;

	void Awake() {
		
		// Fire up the coroutine
		StartCoroutine(hideAfter());
	}

	IEnumerator hideAfter()
	{
		//GetComponent<Animator>().enabled = true;
		
		// Wait until the timer is up
		yield return new WaitForSeconds(hideAfterTime);

		// Hide now
		//this.gameObject.SetActive(false);
		Destroy(gameObject);	// Kill this one
	}
}