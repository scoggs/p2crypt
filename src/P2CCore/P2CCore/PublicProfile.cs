using System;
using System.Security.Cryptography;
using System.Xml.Serialization;
using P2CCommon;

namespace P2CCore
{
	/// <summary>
	/// The public profile is something to share with other users. It is a way to identify and verify a user.
	/// </summary>
	[Serializable]
	public class PublicProfile : P2CCommon.IPublicProfile
	{
		#region Properties

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