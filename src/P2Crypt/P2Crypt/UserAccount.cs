using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace P2Crypt
{
	//The public profile is something to share with other users. It is a way to identify and verify a user.
	[Serializable]
	public class PublicProfile
	{
		#region Properties

		[XmlElement]
		public Guid GlobalID { get; set; }

		[XmlElement]
		public RSAParameters RsaParameters { get; set; }

		[XmlElement]
		public string UserNick { get; set; }

		#endregion Properties

		#region Methods

		public byte[] Encrypt(byte[] data)
		{
			RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
			provider.ImportParameters(RsaParameters);

			return provider.Encrypt(data, true);
		}

		#endregion Methods
	}

	[Serializable]
	public class UserAccount
	{
		#region Fields

		private Guid _globalId;

		#endregion Fields

		#region Constructors

		public UserAccount()
		{
			GlobalId = Guid.NewGuid();
			RsaProvider = new RSACryptoServiceProvider();
			UserNick = "Anon";
		}

		#endregion Constructors

		#region Properties

		[XmlElement]
		public Guid GlobalId
		{
			get { return _globalId; } set { _globalId = value; }
		}

		//The public profile is something to share with other users. It is a way to identify and verify a user.
		[XmlIgnore]
		public PublicProfile PublicProfile
		{
			get
			{
				return new PublicProfile()
				{
					UserNick = UserNick,
					GlobalID = new Guid(GlobalId.ToByteArray()),
					RsaParameters = RsaProvider.ExportParameters(false)
				};
			}
		}

		[XmlElement]
		public String RsaParameters
		{
			get { return RsaProvider.ToXmlString(true); } //TODO: handle this in some other way. Very bad to expose this data as public.
			set { RsaProvider.FromXmlString(value); }
		}

		[XmlElement]
		public String UserNick { get; set; }

		private RSACryptoServiceProvider RsaProvider { get; set; }

		#endregion Properties

		#region Methods

		public static UserAccount Deserialize(string password, string FileName)
		{
			SymmetricAlgorithm alg = StandardAlgorithms.SymmetricAlgorithmFromPassword(password);

			// Decrypt the text from a file using the file name, key, and IV.
			return CryptoUtils.DeserializeFromFile<UserAccount>(FileName, alg);
		}

		public static void Serialize(UserAccount account, string password, string FileName)
		{
			SymmetricAlgorithm alg = StandardAlgorithms.SymmetricAlgorithmFromPassword(password);

			// Encrypt text to a file using the file name, key, and IV.
			CryptoUtils.SerializeToFile(account, FileName, alg);
		}

		public byte[] Decrypt(byte[] data)
		{
			return RsaProvider.Decrypt(data, true);
		}

		#endregion Methods
	}
}