#region Header

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
 *			- Find a better way for user to disconnect the program from the net.
 *			  Maybe instead of creating the Server class is a singleton just let
 *			  MainWindow create a Server object.
 *
 *			- gracefully prevent user from connecting to localhost
 */

#endregion Header



using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using P2CCommon;
using P2CNetwork;
using P2CCore;

namespace NetworkTest
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, IUserInteractor
	{
		#region Fields

		public readonly int defaultPort = 12345; // not the best way but it'll do for now. This number is access within Server class

		CancellationTokenSource tokenSource; // to stop the Server gracefully if user x out of the app.
		IUserAccount userAccount;

		#endregion Fields

		#region Constructors

		public MainWindow()
		{
			InitializeComponent();

			for (int i = 0; i <= 255; ++i)
			{							// populate each of the ip address section's drop down box
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

		#endregion Constructors

		#region Methods

		private void btnDisconnect_Click(object sender, RoutedEventArgs e)
		{
			tokenSource.Cancel();

			Task.Factory.StartNew(() =>
			{
				Server.Instance.Disconnect();
			});
		}

		private void btnSend_Click_1(object sender, RoutedEventArgs e)
		{
			if (String.IsNullOrEmpty(txtMessage.Text) || String.IsNullOrWhiteSpace(txtMessage.Text))
				return;

			string msg = txtMessage.Text;
			Task.Factory.StartNew(() => { Server.Instance.SendMessage(msg); });
		}

		/// <summary>
		/// This is how user will be able to connect to a destination ip.
		/// Once a connection has been established user can start communicating with that ip address.
		/// The ipaddress only need to connect once.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnShakeHand_Click(object sender, RoutedEventArgs e)
		{
			string str = ipFirstByte.SelectedItem.ToString() + "." +
						 ipSecondByte.SelectedItem.ToString() + "." +
						 ipThirdByte.SelectedItem.ToString() + "." +
						 ipFourthByte.SelectedItem.ToString();

			Task.Factory.StartNew(() =>
			{
				Server.Instance.ShakeHand(str);
			});
		}

		// the start button event
		private void Button_Click(object sender, RoutedEventArgs e)
		{
            
			#region Start Server
			tokenSource = new CancellationTokenSource();

			// create a user account for the current user
			userAccount = new UserAccount() { UserNick = userNickTxtBox.Text };

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

		private void userNickTxtBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			// The user have to enter in their user name before they can do anything
			if (!(String.IsNullOrEmpty(userNickTxtBox.Text) || String.IsNullOrWhiteSpace(userNickTxtBox.Text)))
			{
				btnStart.IsEnabled = true;
			}
			else
			{
				btnStart.IsEnabled = false;
			}
		}

		#endregion Methods

        #region IUserInteractor
        public void Notify(string information)
        {
            MessageBox.Show(information);
        }
        public void Notify(string text, string caption)
        {
            MessageBox.Show(text, caption);
        }
        public ITextList TextMessage { get { return new TextList(txtMessage); } }
        public ITextList TextChatWindow { get { return new TextList(txtChatWindow); } }
        public int DefaultPort { get { return defaultPort; } }
        public ITextList TextFriendsList { get { return new TextList(txtFriendsList); } }

        #endregion

        public class TextList : ITextList
        {
            private TextBox textBox;

            public TextList(TextBox tb)
            {
                textBox = tb;
            }

            public void AppendText(string text)
            {
                textBox.InvokedIfRequired(() => textBox.AppendText(text));
            }
            public void Clear()
            {
                textBox.InvokedIfRequired(() => textBox.Clear());
            }
            public string Text
            {
                get
                {
                    string buff = "";
                    textBox.InvokedIfRequired(() => buff = textBox.Text);
                    return buff;
                }
            }
        }
    }
}