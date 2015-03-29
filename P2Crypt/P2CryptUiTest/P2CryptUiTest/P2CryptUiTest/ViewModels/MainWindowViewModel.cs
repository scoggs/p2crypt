using P2CryptUiTest.Annotations;
using P2CryptUiTest.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace P2CryptUiTest.ViewModels
{
	public class MainWindowViewModel : INotifyPropertyChanged
	{
		private string _newMessage;
		private ObservableCollection<Message> _messages;

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

		private ICommand _sendCommand;
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

		public MainWindowViewModel()
		{
			Messages = new ObservableCollection<Message>
			{
				new Message
				{
					Content = "Test from viewmodel",
					Timestamp = DateTime.Now,
					UserName = "ViewModel"
				}
			};
		}

		public void SendMessage(string message)
		{
			if (string.IsNullOrEmpty(message))
				throw new ArgumentException("message");

			Messages.Add(new Message
			{
				Content = message,
				Timestamp = DateTime.Now,
				UserName = "You"
			});
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}