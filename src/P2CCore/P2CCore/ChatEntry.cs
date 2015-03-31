using System;
using System.Xml.Serialization;

namespace P2CCore
{
	/// <summary>
	/// A chat entry is a single entity that was exchanged between multiple users.
	/// It is stored in a <see cref="ChatHistory"/>.
	/// </summary>
	[Serializable]
	public class ChatEntry
	{
		/// <summary>
		/// The timestamp when the entry occured.
		/// TODO: should we use http://nodatime.org/ ?
		/// </summary>
		[XmlElement]
		public DateTime Timestamp { get; set; }

		/// <summary>
		/// This indicates the type of chat message (e.g. video, audio, text, image, ...).
		/// TODO: Should we use a stronger type here?
		/// </summary>
		[XmlElement]
		public string ContentType { get; set; }

		/// <summary>
		/// The transmitted data.
		/// </summary>
		[XmlElement]
		public byte[] Content { get; set; }
	}
}