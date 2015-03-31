/**
 * NOTE:
 *			- it is very important that the user supplied a user name.
 * 
 *			- What you see on the GUI are the most important parts that are needed.
 *				
 *			  After creating Server.ShakeHand methods , the program need to use the same port number across the board.
 *			- Suggestion: create a configurable setting for advance user to change the port number. 
 *						  The port number is access by the Server Class before it starts up. 
 * 
 *			- I don't know how to access the delegate that is called when the window close so this code need to run when the window is closing:
 *					tokenSource.Cancel() 
 *  
 *			-
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using P2Crypt;
using System.Threading;

namespace Network {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow:Window {

		#region Fields	
		P2Crypt.UserAccount userAccount;

		public readonly int defaultPort = 12345;					// not the best way but it'll do for now. This number is access within Server class
		
		CancellationTokenSource tokenSource;						// to stop the Server gracefully if user x out of the app.
		#endregion
		
		public MainWindow() {
			InitializeComponent();

			for(int i = 0; i <= 255; ++i){							// populate each of the ip address section's drop down box
				ipFirstByte.Items.Add(i);
				ipSecondByte.Items.Add(i);
				ipThirdByte.Items.Add(i);
				ipFourthByte.Items.Add(i);
			}

			// set the each of the ip address section to 0
 			ipFirstByte.SelectedIndex = 0;
			ipSecondByte.SelectedIndex = 0;
			ipThirdByte.SelectedIndex = 0;
			ipFourthByte.SelectedIndex = 0;
			
			txtChatWindow.IsReadOnly = true;
			txtFriendsList.IsReadOnly = true;
		}


		private void userNickTxtBox_TextChanged(object sender, TextChangedEventArgs e) {
			// The user have to enter in their user name before they can do anything
			if( !(String.IsNullOrEmpty(userNickTxtBox.Text) || String.IsNullOrWhiteSpace(userNickTxtBox.Text)) ){
				btnStart.IsEnabled = true;
			}
			else{
				btnStart.IsEnabled = false;
			}
		}



		/// <summary>
		/// This is how user will be able to connect to a destination ip.
		/// Once a connection has been established user can start communicating with that ip address.
		/// The ipaddress only need to connect once.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnShakeHand_Click(object sender, RoutedEventArgs e){
			Task.Factory.StartNew(()=>{
				Server.Instance.ShakeHand(ipFirstByte + "." + ipSecondByte + "." + ipThirdByte + "." + ipFourthByte);
			});
		}

		private void btnDisconnect_Click(object sender, RoutedEventArgs e) {
			tokenSource.Cancel();
		}


		// the start button event
		private void Button_Click(object sender, RoutedEventArgs e) {
			#region Start Server
			tokenSource = new CancellationTokenSource();

			// create a user account for the current user
			userAccount = new UserAccount(){ UserNick = userNickTxtBox.Text };				
			
			// feed server the data it want's and start it
			Server.Initialization(userAccount, tokenSource, this);
			Server.Instance.Start();
			#endregion

			// enable all the buttons
			btnDisconnect.IsEnabled = true;
			btnSend.IsEnabled = true;
			btnShakeHand.IsEnabled = true;

			// disable start button 
			btnStart.IsEnabled = false;
		}

		private void btnSend_Click_1(object sender, RoutedEventArgs e) {
			if(String.IsNullOrEmpty(txtMessage.Text) || String.IsNullOrWhiteSpace(txtMessage.Text))
				return;

			Task.Factory.StartNew(()=>{ Server.Instance.SendMessage(txtMessage.Text); });
		}

		

		

	}
}
