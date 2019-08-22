using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		
		if( Input.GetMouseButtonDown(0) )
			SceneManager.LoadScene("Level01");
	}
}