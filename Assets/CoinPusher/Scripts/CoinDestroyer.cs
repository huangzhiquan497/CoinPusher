using UnityEngine;
using System.Collections;

public class CoinDestroyer : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		// Make sure we do not trip up the Bumpers since they nest in the destroyer
		if( !other.gameObject.CompareTag("Bumpers") )
		{
			// Play the sound effect
			other.gameObject.GetComponent<CoinEffect>().playDestroyedSFX();
			other.gameObject.GetComponent<CoinEffect>().removeCoin();
		}
	}

	// For drawing inside of the Unity Editor
	void OnDrawGizmos() 
	{
		Gizmos.color = new Color(1, 0, 0, 0.5F);
		Gizmos.DrawCube(transform.position, new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z));
	}
}