using UnityEngine;
using System.Collections;

public class MachineSwitcher : MonoBehaviour {

	// The active machine index
	public int activeMachine = 0;

	// The array of machines we have to switch through
	public GameObject[] machines;

	// This is to enable the switcher buttons on the GUI
	public bool isSwitcherGUIActive = false;

	// This is the panel in the GUI
	public GameObject machineSwitcherPanelGUI;

	// Use this for initialization
	void Start () {

		// Switch to the active machine
		switchMachine(activeMachine);
	}
	
	// Update is called once per frame
	void Update () {
	
		// If our switcher is active hide or show it
		if( isSwitcherGUIActive )
			machineSwitcherPanelGUI.SetActive(true);
		else
			machineSwitcherPanelGUI.SetActive(false);
	}

	/// <summary>
	/// This handles switching the active machine based on the index
	/// </summary>
	/// <param name="machineIndex">Machine index.</param>
	void switchMachine(int machineIndex)
	{
		for( int i = 0; i < machines.Length; i++ )
		{
			// If we are on the index we need to enable
			if( machineIndex == i )
			{
				machines[i].SetActive(true);
			}
			else
			{
				machines[i].SetActive(false);
			}
		}
	}

	/// <summary>
	/// This is called from the GUI to switch the active machine
	/// </summary>
	/// <param name="direction">Direction.</param>
	public void switchMachineButton(int direction)
	{
		// Find out which direction they are clicking and mod the number 
		if( direction > 0 )
			activeMachine++;
		else if( direction < 0 )
			activeMachine--;

		// If the index is negative, reset to the higher end of the index to loop through, otherwise, reset back to 0 which is the beginning
		if( activeMachine < 0 )
			activeMachine = machines.Length - 1;
		else if( activeMachine > machines.Length - 1 )
			activeMachine = 0;
			
		// Now switch the machine to the new one
		switchMachine(activeMachine);
	}
}