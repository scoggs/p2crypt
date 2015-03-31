using System.Security.Cryptography;

namespace P2Crypt
{
	/// <summary>
	/// This class provide standard cryptos. 
	/// 
	/// All decisions like "should we use AES or DES for symmetric encryption"
	/// are made here.
	/// 
	/// </summary>
	public class StandardAlgorithms
	{
		#region Fields

		/// <summary>
		/// 128-256/64
		/// </summary>
		private static readonly int SymetricKeySizeInBits = 256;

		#endregion Fields

		#region Properties

		private static int SymetricKeySizeInBytes
		{
			get { return SymetricKeySizeInBits >> 3; }
		}

		#endregion Properties

		#region Methods

		public static AsymmetricAlgorithm AsymetricAlgorithm()
		{
			RSACryptoServiceProvider rsaAlg = new RSACryptoServiceProvider();

			return rsaAlg;
		}

		public static RSAParameters GenerateRSAParameters()
		{
			RSACryptoServiceProvider rsaAlg = new RSACryptoServiceProvider();

			return rsaAlg.ExportParameters(true);
		}

		public static SymmetricAlgorithm SymmetricAlgorithmFromPassword(string password)
		{
			//Create key from password
			byte[] salt = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
			Rfc2898DeriveBytes keyGen = new Rfc2898DeriveBytes(password, salt);

			AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider()
			{
				Key = keyGen.GetBytes(SymetricKeySizeInBytes),
			};
			aesAlg.IV = keyGen.GetBytes(aesAlg.BlockSize >> 3);
			return aesAlg;
		}

		#endregion Methods
	}
}