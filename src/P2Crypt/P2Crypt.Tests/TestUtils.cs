using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
namespace EncryptionTests
{
    public class TestUtils
    {
        /// <summary>
        /// Util class to handle temporary files. 
        /// </summary>
        public class TempFile : IDisposable
        {

            public string FilePath { get; private set; }

            public TempFile()
            {
                String pat = @"{0}\P2CryptTemp{1}.tmp";
                int index = 1;
                string folder = Path.GetTempPath();
                folder = @".\TestTempFiles";

                while (File.Exists(FilePath = String.Format(pat, Path.GetTempPath(), index++))) ;
            }

            public void Dispose()
            {
                if (File.Exists(FilePath))
                {
                    File.Delete(FilePath);
                }
            }
        }

        [Serializable]
        public class TestData
        {
            [XmlElement]
            public string someData { get; set; }

            public override bool Equals(object obj)
            {
                return obj is TestData? (obj as TestData).someData == someData:false;
            }

            public override int GetHashCode()
            {
                return someData.GetHashCode();
            }
        }
    }
}
