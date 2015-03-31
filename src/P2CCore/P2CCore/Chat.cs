using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace P2CCore
{
	/// <summary>
	/// A chat is a collection of exchanged data packages between multiple users.
	/// </summary>
	[Serializable]
	public class Chat
	{
		#region Properties

		/// <summary>
		/// This list contains the set of public profiles of the users taking part in the chat.
		/// </summary>
		[XmlArray]
		public List<PublicProfile> ChatMembers { get; set; }

		/// <summary>
		/// The chat history.
		/// TODO: this should be timestamped collection of object (must support text, emoji, images, etc.)
		/// TODO: will be storing image/video data in here as well? This will drastically increase the filesize of a chatlog..
		/// </summary>
		public List<string> Messages { get; set; }

		#endregion Properties
	}
}