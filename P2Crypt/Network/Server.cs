/**
 * NOTE:
 *		- Need to make the port number resided in a configuration file. So this class can query the file
 *				for specified port from advance user.
 * 
 *      - From within NewMessage; In the future maybe use the Socket parameter for loggin purpose?
 * 
 *		- Need to implement an algorithm that removed disconnected user from our dictionary of socket
 *		  The best place for this algorithm could be in the SendMessage method
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Text;

using P2Crypt;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
		static readonly object processLock = new object();

		Socket socketListener;

		// need this to decryp the message from our buddy.
		// string is the user nick
		Dictionary<Guid, P2Crypt.PublicProfile> publicProfileDict;

		// store each connection socket information, string is the user nick
		Dictionary<Guid, Socket> socketDict;

		static UserAccount userAccount;

		// so Server can stop gracefully when user exit the application.
		static CancellationTokenSource token;

		// max number of connections in the connection queue
		readonly int maxConnection = 100;	

		// to be able to communciate back to the GUI
		static MainWindow mainWindowGUI;
		
		#endregion

		#region Important 
		Server(){
			publicProfileDict = new Dictionary<Guid,PublicProfile>();
			socketDict = new Dictionary<Guid,Socket>();
		}


		public static void Initialization(UserAccount user, CancellationTokenSource cToken, 
										  MainWindow GUI){
			if(!isInitialized){
				userAccount = user;

				token = cToken;

				mainWindowGUI = GUI;

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
		// how the public get the server to start running
		public void Start(){
			socketListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			try{
				socketListener.LingerState = new LingerOption(false, 0);
				socketListener.NoDelay = true;
				socketListener.Bind(new IPEndPoint(IPAddress.Any, mainWindowGUI.defaultPort ));
				socketListener.Listen(maxConnection);

				// send em off
				Task.Factory.StartNew(()=>{ Run(); }, token.Token);
			}
			catch(SocketException se) {
#region//// DEBUG
				Task.Factory.StartNew(()=>{
					MessageBox.Show("Socket Exception:" + Environment.NewLine + 
									 se.Message + Environment.NewLine +
									 "Winsock error code: " + se.NativeErrorCode, 
									 "DEBUG SOCKET EXCEPTION");
				});
#endregion
			}
		}
		/// <summary>
		/// Keep running until the parent thread send's a cancellation notice
		/// </summary>
		/// <param name="cancelToken"></param>
		async void Run(){
			while(!token.IsCancellationRequested){

#region//// DEBUG
				Task.Factory.StartNew(()=>{
					MessageBox.Show("Server Running.");
				});
#endregion
				Socket client = await socketListener.Accept_Async();

				ServiceClient sc = new ServiceClient(client, token);
				Task t1 = Task.Factory.StartNew(()=>{ sc.Start(ProcessData); }, token.Token);
			}

#region//// DEBUG
			// let the dev know server will excit
			Task.Factory.StartNew(()=>{
				MessageBox.Show("SERVER EXITING NOW");
			});
#endregion
			
			CleanUP();
		}
	
		void CleanUP(){
			try{
				lock(myLock){
					publicProfileDict.Clear();
				}
			}
			catch(Exception){/*we are done already???!!*/}

			try{
				lock(myLock){
					foreach(var item in socketDict){
						try{
							item.Value.Close(0);
							socketDict[item.Key] = null;
						}
						catch(Exception){/*oooooo welllll*/}
					}
					socketDict.Clear();
				}

				socketListener.Close(0);
				socketListener = null;
			}
			catch(Exception){
				// o well the program is exiting anyways
			}

#region//// DEBUG
			Task.Factory.StartNew(()=>{
				MessageBox.Show("Cancellation requsted. Inside Server.CleanUP(). Server exiting");
			});
#endregion
		}


		// need to provide this to ServiceClient.Start
		void ProcessData(Package package, Socket client){
			// only one task can process this at a time.
			if(!token.IsCancellationRequested){
				lock(processLock){
					if(publicProfileDict.ContainsKey(package.user.GlobalID)){
						// incoming packge is a message:
						NewMessage(package, client);
					}
					else{
						// new user
						NewFriend(package, client);
					}						
				}
			}
			else{
				package.data = null;
				client.Close(0);
				client = null;
			}
		}


		void NewMessage(Package messageData, Socket client){
			// Future: use the socket to log the ip address and the package to log who sent the message?

			byte[] decrypticData = userAccount.Decrypt(messageData.data);

			string message = Encoding.UTF8.GetString(decrypticData);
			mainWindowGUI.txtChatWindow.InvokedIfRequired(()=>{				
				mainWindowGUI.txtChatWindow.AppendText(messageData.user.UserNick + ":" + Environment.NewLine + message);
			});

			messageData.data = null;
			messageData = null;
		}


		// doesn't handle out of memory exception
		// exception may/my not bubble up into the ServiceClient object. I need to read how Exception work in Task again.
		void NewFriend(Package messageData, Socket client) {
			// Send public profile to our new friend
			// don't refactor this, this method will be running on different thread.
			using(MemoryStream ms = new MemoryStream()){
				Package package = new Package(userAccount.PublicProfile, null);
				
				BinaryFormatter bf = new BinaryFormatter();
				bf.Serialize(ms, package);
				ms.Seek(0, SeekOrigin.Begin);
				client.Send(ms.ToArray(), 0, (int)ms.Length, SocketFlags.None);
			}

#region WARNING, May cause spinlock
			lock(myLock){
				publicProfileDict.Add(messageData.user.GlobalID, messageData.user);
				socketDict.Add(messageData.user.GlobalID, client);
			}
#endregion

			mainWindowGUI.txtFriendsList.InvokedIfRequired(()=>{
				mainWindowGUI.txtFriendsList.AppendText(messageData.user.UserNick + Environment.NewLine);
			});
		}


		// if user entered the wrong ip, just return
		// upon any exeption just return
		// Once a connection has been established send the public profile.
		// NOTE: doesn't handle out of memory exception gracefully.
		public void ShakeHand(String ipStr){
			IPAddress ipAddress;
			
			if(IPAddress.TryParse(ipStr, out ipAddress)){
				Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				
				try{
					socket.Connect(ipAddress, mainWindowGUI.defaultPort);

					Package package = new Package(userAccount.PublicProfile, null);

					using(MemoryStream ms = new MemoryStream()){
						BinaryFormatter bf = new BinaryFormatter();
						bf.Serialize(ms, package);
						ms.Seek(0, SeekOrigin.Begin);
						socket.Send(ms.ToArray(), 0, (int)ms.Length, SocketFlags.None);
					}
				}
				catch(Exception){
					// do nothing
					socket = null;
				}
			}
		}


		public void SendMessage(string message){
			Package deliveryPackage = new Package(userAccount.PublicProfile, Encoding.UTF8.GetBytes(message));
			BinaryFormatter bf = new BinaryFormatter();
			foreach(var pair in socketDict){
				try{
					using(MemoryStream ms = new MemoryStream()){
						bf.Serialize(ms, deliveryPackage);
						ms.Seek(0, SeekOrigin.Begin);
						pair.Value.Send(ms.ToArray(), 0, (int)ms.Length, SocketFlags.None);
					}
				}
				catch(Exception ex) {
#region//// DEBUG
					Task.Factory.StartNew(()=>{ 
						MessageBox.Show("EXCEPTION IN Server.SendMessage" + Environment.NewLine +
										"Message: " + Environment.NewLine +
										ex.Message);
					});
#endregion
				}
			}

			
			mainWindowGUI.txtMessage.InvokedIfRequired(()=>{
				mainWindowGUI.txtMessage.Clear();
			});
		}

		#endregion


	}


	public class ServiceClient{
		Socket client;
		CancellationTokenSource token;
		readonly int maxBufferSize = 1024;

		public ServiceClient(Socket socket, CancellationTokenSource cts){
			client = socket;
			token = cts;
		}

		/// <summary>
		/// Capture the data in transmission and then call a method within Server to process it.
		/// This keep the Server as the only class that has access to public profile info.
		/// </summary>
		/// <param name="LoadingDock">The method within Server that will do the data processing</param>
		public void Start(Action<Package, Socket> LoadingDock){
			if(token.IsCancellationRequested){
				client.Close(0);
				client = null;
				return;
			}

			Package deliveredPackage;

			try{
				using(MemoryStream ms = new MemoryStream()){
					// get the data that is coming in
					BinaryFormatter bf = new BinaryFormatter();
					byte[] buffer = new byte[maxBufferSize];
					int bytesRead = 0;
					while( (bytesRead = client.Receive(buffer, 0, maxBufferSize, SocketFlags.None)) > 0 ){
						ms.Write(buffer, 0, bytesRead);
					}

					ms.Seek(0, SeekOrigin.Begin);

					deliveredPackage = (Package)bf.Deserialize(ms);
				}

				LoadingDock(deliveredPackage, client);
			}
			catch(Exception ex) {
#region////DEBUG
				Task.Factory.StartNew(()=>{
					MessageBox.Show("EXCEPTION WHILE SERVICING INCOMING CONNECTION: " + Environment.NewLine +
									"Remote address: " + client.RemoteEndPoint.ToString() + Environment.NewLine +
									"EXCEPTION: " + Environment.NewLine +
									ex.Message);
				});
#endregion
			}

		}
	}

}// end namespace
