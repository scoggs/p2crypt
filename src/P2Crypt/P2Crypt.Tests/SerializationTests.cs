using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using P2Crypt;
using System.Xml.Serialization;
using System.IO.Pipes;

namespace EncryptionTests
{
    [TestClass]
    public class SerializationTests
    {
        public class TestData
        {
            [XmlElement]
            public string someData { get; set; }
        }

        /// <summary>
        /// This test shows how to save any (xml serializable) object to an encrypted file and open it again.
        /// 
        /// While this test use the same instance of the algorithm (var alg) this is not required.
        /// </summary>
        [TestMethod]
        public void WriteReadCustomClassTest()
        {
            using (TestUtils.TempFile tmp = new TestUtils.TempFile())
            {
                //create some data to store
                string expected = "expected";
                TestData testData = new TestData() { someData = expected };

                //configure encryption
                String password = "Secret set of characters";
                var alg = StandardAlgorithms.SymmetricAlgorithmFromPassword(password);

                //perform the encryption
                CryptoUtils.SerializeToFile(testData, tmp.FilePath, alg);

                //decrypt the data
                TestData decrypted = CryptoUtils.DeserializeFromFile<TestData>(tmp.FilePath, alg);

                //check that it's ok
                Assert.AreEqual(expected, decrypted.someData);
            }
        }

        /// <summary>
        /// This test shows that even primitives can be encrypted
        /// </summary>
        [TestMethod]
        public void WriteReadSomeStringTest()
        {
            using (TestUtils.TempFile tmp = new TestUtils.TempFile())
            {
                //Test data
                String expected = "expected";

                //configure encryption
                String password = "Secret set of characters";
                var alg = StandardAlgorithms.SymmetricAlgorithmFromPassword(password);

                //Perform encryption
                CryptoUtils.SerializeToFile(expected, tmp.FilePath, alg);

                //perform decryption
                String decrypted = CryptoUtils.DeserializeFromFile<String>(tmp.FilePath, alg);

                //Check that it's ok.
                Assert.AreEqual(expected, decrypted);
            }
        }

        [TestMethod]
        public void StreamSerializationTest()
        {
            using (TestUtils.TempFile tmp = new TestUtils.TempFile())
            {

                //Test data
                String expected = "expected";
                String decrypted = null;
                //configure encryption
                String password = "Secret set of characters";
                var alg = StandardAlgorithms.SymmetricAlgorithmFromPassword(password);

                //We create a simple output stream against a temp file, but this could be sockets or anything. 
                using (FileStream stream = File.Open(tmp.FilePath, FileMode.CreateNew))
                {
                    //Perform encryption
                    CryptoUtils.SerializeToStream(stream, expected, alg);
                }

                //We create a simple input stream against the temp file, but this could be sockets or anything. 
                using (FileStream stream = File.Open(tmp.FilePath, FileMode.Open))
                {
                    //Perform decryption
                    decrypted = CryptoUtils.DeserializeFromStream<String>(stream, alg);
                    stream.Close();
                }
                //perform decryption
                
                //Check that it's ok.
                Assert.AreEqual(expected, decrypted);
            }
        }
    }
}

