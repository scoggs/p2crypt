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

namespace Network {
	public class Package{
		public string userNick;
		public byte[] data;

		public Package(string userNick, byte[] data){
			this.userNick = userNick;
			this.data = data;
		}
	}
}
