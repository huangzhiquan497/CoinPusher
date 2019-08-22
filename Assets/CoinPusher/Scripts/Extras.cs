using UnityEngine;
using System.Collections;

namespace Extras
{
public class Tools 
	{
		/// <summary>
		/// This function is used to see if the string name exists in the coin shop item panel
		/// </summary>
		/// <returns><c>true</c>, if exist in list was in there, <c>false</c> otherwise.</returns>
		/// <param name="name">Name.</param>
		public static bool doesExistInList( string name, Transform transform ) 
		{
			// Go through the children, if any, and return true if found
			foreach (Transform child in transform) 
			{
				if (child.name == name)
					return true;
			}

			// If we hit here, it was never found
			return false;
		}
	}
}