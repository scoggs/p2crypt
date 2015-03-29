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
		
		//P2Crypt.CryptoUtils cryptoUtils;
		P2Crypt.PublicProfile publicProfile;
		//P2Crypt.StandardAlgorithms standardAlgorithms;
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

				
		}


	}
}
