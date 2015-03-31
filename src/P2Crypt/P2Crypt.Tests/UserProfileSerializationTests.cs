using Microsoft.VisualStudio.TestTools.UnitTesting;
using P2CCore;
using System;
using System.IO;

namespace EncryptionTests
{
	[TestClass]
	public class UserProfileSerializationTests
	{
		#region Methods

		[TestMethod]
		public void WriteReadTest()
		{
			UserAccount account = new UserAccount() { UserNick = "Jonas" };
			Guid id = account.GlobalId;

			Directory.CreateDirectory(@".\TestTempFiles\");
			String filepath = @".\TestTempFiles\WriteReadTest.profile";
			String password = "Secret set of characters";

			UserAccount.Serialize(account, password, filepath);
			UserAccount result = UserAccount.Deserialize(password, filepath);
			Assert.AreEqual(id, result.GlobalId);
		}

		#endregion Methods
	}
}