using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Serialization;

namespace P2CCore
{
	/// <summary>
	/// The public profile is something to share with other users. It is a way to identify and verify a user.
	/// </summary>
	[Serializable]
	public class PublicProfile
	{
		#region Properties

		/// <summary>
		/// All the chats of which the current user is part of.
		/// </summary>
		[XmlArray]
		public List<ChatHistory> Chats { get; set; }

		[XmlElement]
		public Guid GlobalId { get; set; }

		[XmlElement]
		public RSAParameters RsaParameters { get; set; }

		[XmlElement]
		public string UserNick { get; set; }

		#endregion Properties

		#region Methods

		public byte[] Encrypt(byte[] data)
		{
			var provider = new RSACryptoServiceProvider();
			provider.ImportParameters(RsaParameters);

			return provider.Encrypt(data, true);
		}

		#endregion Methods
	}
}