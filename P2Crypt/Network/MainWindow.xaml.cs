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

namespace Network {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow:Window {
		// need this to decryp the message from our buddy.
		// string is the user nick
		Dictionary<string, P2Crypt.PublicProfile> publicProfileDict;
		
		P2Crypt.UserAccount userAccount;

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
			
			// create a user account for the current user
			userAccount = new UserAccount(){ UserNick = userNickTxtBox.Text };	

			publicProfileDict = new Dictionary<string,PublicProfile>();

			txtChatWindow.IsReadOnly = true;
		}


		private void userNickTxtBox_TextChanged(object sender, TextChangedEventArgs e) {
			// The user have to enter in their user name before they can do anything
			if( !(String.IsNullOrEmpty(userNickTxtBox.Text) || String.IsNullOrWhiteSpace(userNickTxtBox.Text)) ){
				btnSend.IsEnabled = true;
				btnShakeHand.IsEnabled = true;
				txtMessage.IsEnabled = true;
			}
			else{
				btnSend.IsEnabled = false;
				btnShakeHand.IsEnabled = false;
				txtMessage.IsEnabled = false;
			}
		}



		/// <summary>
		/// This is how user will be able to connect to a destination ip.
		/// Once a connection has been established user can start communicating with that ip address.
		/// The ipaddress only need to connect once.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnShakeHand_Click(object sender, RoutedEventArgs e) {

		}




	}
}
