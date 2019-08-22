using UnityEngine;
using System.Collections;

public class TouchClickArea : MonoBehaviour {

	void OnDrawGizmos() 
	{
		Gizmos.color = new Color(0, 0, 1, 0.5F);
		Gizmos.DrawCube(transform.position, 
			new Vector3(transform.localScale.x, 
			transform.localScale.y, 
			transform.localScale.z));
	}
}