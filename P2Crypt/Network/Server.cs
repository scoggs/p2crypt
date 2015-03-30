/**
 * NOTE:
 *		- Need to make the port number resided in a configuration file. So this class can query the file
 *				for specified port from advance user.
 * 
 * 
 * 
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Net;

using P2Crypt;

namespace Network{
	/// <summary>
	/// Control and handles incoming/outgoing packet. 
	/// </summary>
	public sealed class Server {

		#region Fields
		// to make sure the Server received all the important data it need before it does anything
		static bool isInitialized = false;

		// singleton 
		static Server instance = null;
		static readonly object myLock = new object();

		Socket socket;

		// need this to decryp the message from our buddy.
		// string is the user nick
		Dictionary<string, P2Crypt.PublicProfile> publicProfileDict;

		// store each connection socket information, string is the user nick
		Dictionary<string, Socket> socketDict;

		static UserAccount userAccount;

		// so Server can stop gracefully when user exit the application.
		static CancellationTokenSource token;
		#endregion

		#region Important 
		Server(){
			publicProfileDict = new Dictionary<string,PublicProfile>();
			socketDict = new Dictionary<string,Socket>();
		}


		public static void Initialization(UserAccount user, CancellationTokenSource cToken){
			if(!isInitialized){
				userAccount = user;

				token = cToken;

				lock(myLock){
					instance = new Server();
				}

				isInitialized = true;
			}
		}
		#endregion

		#region Properties
		public static Server Instance{ get{ return instance; } }
		#endregion



		#region Methods
		/// <summary>
		/// Keep running until the parent thread send's a cancellation notice
		/// </summary>
		/// <param name="cancelToken"></param>
		public void Run(CancellationToken cancelToken){
			socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			
			
			while(!cancelToken.IsCancellationRequested){
				

			}
		}
		#endregion


	}
}
