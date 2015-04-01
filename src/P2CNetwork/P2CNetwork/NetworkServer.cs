#region Header

/**
 * 
 * Controls outgoing/incoming data for the program.
 * 
 **/

#endregion Header

using System;


namespace P2CNetwork 
{
	public class NetworkServer 
	{

		#region Fields
		//switch
		bool hasPackage;							// when NetworkServer received data and finish de-serializing it this turn to true
		bool hasServerStart;

		int port;
		Package package;

		#endregion Fields


		#region Constructors

		public NetworkServer(int port = 12345)
		{
			this.port = port;
			hasPackage = false;
			hasServerStart = false;
		}

		#endregion Constructors


		#region Properties
		public Package Package
		{
			get
			{
				if(hasPackage)
					return package;
				else{
					return null;						// should we return an empty package?
				}
			}

			private set
			{
				package = value;
			}
		}


		#endregion Properties


		#region Methods


		#endregion Methods

	}
}
