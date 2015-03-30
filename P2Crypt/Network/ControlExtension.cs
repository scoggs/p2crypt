using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Network{
	public static class ControlExtension{

		// allow code residing on non-GUI thread to call control's methods within the GUI-thread
		// asynchronous
		public static void InvokedIfRequired(this Control control, Action action){
			if(control.Dispatcher.CheckAccess())
				action();
			else
				control.Dispatcher.Invoke(action);
		}
	}
}
