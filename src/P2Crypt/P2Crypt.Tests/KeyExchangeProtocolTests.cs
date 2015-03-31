using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using P2Crypt;
namespace EncryptionTests
{
    /// <summary>
    /// Summary description for KeyExchangeProtocolTests
    /// </summary>
    [TestClass]
    public class KeyExchangeProtocolTests
    {
        

        [TestMethod]
        public void ProfileExchangeTest()
        {
            UserAccount userA = new UserAccount() { UserNick = "UserA"};
            UserAccount userB = new UserAccount() { UserNick = "UserB"};
            UserAccount userC = new UserAccount() { UserNick = "UserC" };

            PublicProfile publicB = userB.PublicProfile;

            byte[] data = new byte[] { 1, 2, 3, 4, 5 };

            //User A has the public profile of B, and thus can encrypt for B.
            byte[] dataToSend = publicB.Encrypt(data);

            byte[] received = userB.Decrypt(dataToSend);
            //byte[] eavesDropped = userC.Decrypt(dataToSend); //This throws exception. 
            CollectionAssert.AreEqual(data, received);
            //CollectionAssert.AreNotEqual(data, eavesDropped);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Security.Cryptography.CryptographicException))]
        public void ProfileExchangeEvesdroppingTest()
        {
            UserAccount userA = new UserAccount() { UserNick = "UserA" };
            UserAccount userB = new UserAccount() { UserNick = "UserB" };
            UserAccount userC = new UserAccount() { UserNick = "UserC" };

            PublicProfile publicB = userB.PublicProfile;

            byte[] data = new byte[] { 1, 2, 3, 4, 5 };

            //User A has the public profile of B, and thus can encrypt for B.
            byte[] dataToSend = publicB.Encrypt(data);

            //User C should not be able to decrypt this
            byte[] eavesDropped = userC.Decrypt(dataToSend); //This throws exception. 
            
        }

    }

    
}
