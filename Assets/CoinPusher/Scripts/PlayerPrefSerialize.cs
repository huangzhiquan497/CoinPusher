using UnityEngine; 
using System.IO; 
using System.Runtime.Serialization; 
using System.Runtime.Serialization.Formatters.Binary; 

// This class helps us serialize our data so we can save it, as a string, into the PlayerPrefs and then pull it back out when needed
public class PlayerPrefsSerialize<T> where T : ISerializable 
{
	/// <summary>
	/// Our save function
	/// </summary>
	/// <param name="name">Name.</param>
	/// <param name="obj">Object.</param>
 	public static void Save( string name, T obj ) 
	{ 
 		// Serialize the object we pass into the save function
 		Stream stream = new MemoryStream(); 
 		BinaryFormatter bf = new BinaryFormatter(); 
 		bf.Serialize( stream, obj ); 

 		// Read the stream back into a byte array
 		stream.Seek( 0, SeekOrigin.Begin ); 
 		byte[] stateData = new byte[stream.Length]; 
 		stream.Read( stateData, 0, (int)stream.Length ); 
 		stream.Close(); 

 		// Convert to base64 and save the string to PlayerPrefs
 		PlayerPrefs.SetString( name, System.Convert.ToBase64String (stateData) ); 
 	} 

	/// <summary>
	/// Our loading function, it deserializes the data
	/// </summary>
	/// <param name="name">Name.</param>
 	public static T Load( string name ) 
 	{ 
 		T loaded = default (T); 

		// If we have the key we are trying to load
		if(PlayerPrefs.HasKey(name)) 
 		{ 
 			byte[] stateData = System.Convert.FromBase64String( PlayerPrefs.GetString(name) ); 
 			Stream stream = new MemoryStream( stateData ); 
 			BinaryFormatter bf = new BinaryFormatter(); 
 
 			try 
			{ 
				// Try to deserialize the stream
 				loaded = (T)bf.Deserialize( stream ); 
 			} 
			catch 
			{ 
				// If we cannot deserilize the key, warn and remove it
 				Debug.LogWarning("Unable to deserialize data for key, deleting: " + name); 
 				PlayerPrefs.DeleteKey(name); 
 			} 
			
 			stream.Close (); 
		} 
 
		return loaded; 
 	} 
 } 