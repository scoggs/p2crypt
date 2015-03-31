using System;
using System.Net.Sockets;
using System.Windows.Controls;

namespace NetworkTest
{
	public static class ClassExtension
	{

		// allow code residing on non-GUI thread to call control's methods within the GUI-thread
		// asynchronous
		public static void InvokedIfRequired(this Control control, Action action)
		{
			if (control.Dispatcher.CheckAccess())
				action();
			else
				control.Dispatcher.Invoke(action);
		}


		// to check if socket connection is still open
		// SocketExcpetion uses Winsock error code . need to update for non-windows os
		public static bool IsConnected(this Socket socket)
		{
			bool blockingState = socket.Blocking;
			bool objectDisposedExcpetionThrown = false;
			try
			{
				socket.Blocking = true;
				byte[] testData = new byte[1];
				socket.Send(testData, 0, 0, SocketFlags.None);
				return true;
			}
			catch (SocketException se)
			{
				// WSAEWOULDBLOCK = 10035
				if (se.NativeErrorCode == 10035)
					return true;
				else
					return false;
			}
			catch (ObjectDisposedException)
			{
				objectDisposedExcpetionThrown = true;
				return false;
			}
			finally
			{
				if (!objectDisposedExcpetionThrown)
					socket.Blocking = blockingState;
			}
		}

	}
}
