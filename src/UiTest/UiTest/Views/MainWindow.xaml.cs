
using System.Windows;

namespace UiTest.Views
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		public MainWindow()
		{
		    this.InitializeComponent();
		    this.Loaded += this.OnLoaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
		    this.NewMessage.Focus();
		}
	}
}
