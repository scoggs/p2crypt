using System;
using System.IO;
using System.Security.Cryptography;
using System.Xml.Serialization;

namespace P2Crypt
{
	/// <summary>
	/// This class contains utility functions for encryption and decryption in various situations
	/// 
	/// No specific crypto implementations should be used here.
	/// </summary>
	public class CryptoUtils
	{
		#region Methods

		public static TData DeserializeFromFile<TData>(String FileName, SymmetricAlgorithm crypto)
		{
			// Create or open the specified file.
			using (FileStream fStream = File.Open(FileName, FileMode.OpenOrCreate))
			{
				TData val = DeserializeFromStream<TData>(fStream, crypto);

				fStream.Close();
				return val;
			}
		}

		public static TData DeserializeFromStream<TData>(Stream stream, SymmetricAlgorithm crypto)
		{
			// Create a CryptoStream using the FileStream
			// and the passed key and initialization vector (IV).
			CryptoStream cStream = new CryptoStream(stream,
				crypto.CreateDecryptor(),
				CryptoStreamMode.Read);

			// Create a StreamReader using the CryptoStream.
			XmlSerializer sReader = new XmlSerializer(typeof(TData));

			// Read the data from the stream
			// to decrypt it.
			TData ret = (TData)sReader.Deserialize(cStream);

			// Close the streams and
			// close the file.
			cStream.Close();

			return ret;
		}

		public static void SerializeToFile<TData>(TData data, String FileName, SymmetricAlgorithm crypto)
		{
			#if DEBUG //This is a really dangerous piece of code if enabled...

			// Create or open the specified file.
			/*using (FileStream fStream = File.Open(FileName + ".debug", FileMode.Create))
			{
				XmlSerializer sWriter = new XmlSerializer(typeof(TData));
				sWriter.Serialize(fStream, data);
			}*/
			#endif
			// Create or open the specified file.
			using (FileStream fStream = File.Open(FileName, FileMode.OpenOrCreate))
			{

				SerializeToStream<TData>(fStream, data, crypto);

				fStream.Close();
			}
		}

		public static void SerializeToStream<TData>(Stream stream, TData data, SymmetricAlgorithm crypto)
		{
			// Create a CryptoStream using the FileStream
			// and the passed key and initialization vector (IV).
			CryptoStream cStream = new CryptoStream(stream,
				crypto.CreateEncryptor(),
				CryptoStreamMode.Write);

			// Create a StreamWriter using the CryptoStream.
			XmlSerializer sWriter = new XmlSerializer(typeof(TData));

			//Serialize to crypto stream
			sWriter.Serialize(cStream, data);
			cStream.Flush();
			stream.Flush();
			cStream.Close();
		}

		#endregion Methods
	}
}