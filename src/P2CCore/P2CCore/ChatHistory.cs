using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace P2CCore
{
	/// <summary>
	/// A chat history is a collection of exchanged data packages between multiple users.
	/// </summary>
	[Serializable]
	public class ChatHistory
	{
		#region Properties

		/// <summary>
		/// This list contains the set of public profiles of the users taking part in the chat.
		/// </summary>
		[XmlArray]
		public List<PublicProfile> ChatMembers { get; set; }

		/// <summary>
		/// The chat history between the members of the chat.
		/// </summary>
		[XmlArray]
		public List<ChatEntry> Messages { get; set; }

		#endregion Properties
	}
}