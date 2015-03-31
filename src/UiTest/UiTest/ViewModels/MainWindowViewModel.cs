using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using UiTest.Models;
using UiTest.Properties;

namespace UiTest.ViewModels
{
	//todo: Implement drag and drop of text files.
	public sealed class MainWindowViewModel : INotifyPropertyChanged
	{
		#region Fields

		private ObservableCollection<Message> _messages;
		private string _newMessage;
		private ICommand _sendCommand;

		#endregion Fields

		#region Constructors

		public MainWindowViewModel()
		{
			Messages = new ObservableCollection<Message>
			{
				new Message
				{
					Content = "Test from viewmodel",
					Timestamp = DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToShortTimeString(),
					UserName = "ViewModel"
				}
			};
		}

		#endregion Constructors

		#region Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion Events

		#region Properties

		public ObservableCollection<Message> Messages
		{
			get { return _messages; }
			set
			{
				if (Equals(value, _messages)) return;
				_messages = value;
				OnPropertyChanged();
			}
		}

		public string NewMessage
		{
			get { return _newMessage; }
			set
			{
				if (value == _newMessage) return;
				_newMessage = value;
				OnPropertyChanged();
			}
		}

		public ICommand SendCommand
		{
			get
			{
				return _sendCommand ?? (_sendCommand = new RelayCommand<string>(o =>
				{
					SendMessage(NewMessage);
					NewMessage = null;
				}, o => !string.IsNullOrEmpty(NewMessage)));
			}
		}

		#endregion Properties

		#region Methods

		[NotifyPropertyChangedInvocator]
		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}

		private void SendMessage(string message)
		{
			if (string.IsNullOrEmpty(message))
			{
				throw new ArgumentException("message");
			}

			Messages.Add(new Message
			{
				Content = message,
				Timestamp = DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToShortTimeString(),
				UserName = "You"
			});
		}

		#endregion Methods
	}
}