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

namespace Network{
	/// <summary>
	/// Control and handles incoming/outgoing packet. 
	/// </summary>
	public sealed class Server {

		#region Fields
		// singleton 
		static Server instance = null;
		static readonly object myLock = new object();

		Socket socket;
		#endregion


		Server(){}

		#region Properties
		public static Server Instance{
			get{
				lock(myLock){
					if(instance == null)
						instance = new Server();
					return instance;
				}
			}
		}
		#endregion


		/// <summary>
		/// Keep running until the parent thread send's a cancellation notice
		/// </summary>
		/// <param name="cancelToken"></param>
		public void Run(CancellationToken cancelToken){
			socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			
			
			while(!cancelToken.IsCancellationRequested){
				

			}
		}

	}
}
