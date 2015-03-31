/*
 * The delivery package. Signature required.
 * This class is the class that will be seralized before it get sent through the net.
 * 
 * Can use properties if  you want, can change it around if like you but it must contain a string and a byte[]
 * 
 * This class is required to be in the same directory as the Server class.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using P2Crypt;

namespace Network {

	[Serializable]
	public class Package{
		public PublicProfile user;
		public byte[] data;

		public Package(PublicProfile user, byte[] data){
			this.user = user;
			this.data = data;
		}
	}
}
