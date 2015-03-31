using System;
using System.IO;
using System.Xml.Serialization;

namespace EncryptionTests
{
	public class TestUtils
	{
		#region Nested Types

		/// <summary>
		/// Util class to handle temporary files. 
		/// </summary>
		public class TempFile : IDisposable
		{
			#region Constructors

			public TempFile()
			{
				String pat = @"{0}\P2CryptTemp{1}.tmp";
				int index = 1;
				string folder = Path.GetTempPath();
				folder = @".\TestTempFiles";

				while (File.Exists(FilePath = String.Format(pat, Path.GetTempPath(), index++))) ;
			}

			#endregion Constructors

			#region Properties

			public string FilePath { get; private set; }

			#endregion Properties

			#region Methods

			public void Dispose()
			{
				if (File.Exists(FilePath))
				{
					File.Delete(FilePath);
				}
			}

			#endregion Methods
		}

		[Serializable]
		public class TestData
		{
			#region Properties

			[XmlElement]
			public string someData { get; set; }

			#endregion Properties

			#region Methods

			public override bool Equals(object obj)
			{
				return obj is TestData ? (obj as TestData).someData == someData : false;
			}

			public override int GetHashCode()
			{
				return someData.GetHashCode();
			}

			#endregion Methods
		}

		#endregion Nested Types
	}
}