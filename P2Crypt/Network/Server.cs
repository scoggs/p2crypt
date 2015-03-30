using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;

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

			socket.Bind(
			
			while(!cancelToken.IsCancellationRequested){
				

			}
		}

	}
}
