#region Header

/*
 * The delivery package. Signature required.
 * This class is the class that will be seralized before it get sent through the net.
 *
 * Can use properties if  you want, can change it around if like you but it must contain a PublicProfile and a byte[]
 */

#endregion Header

using System;
using P2CCommon;

namespace P2CNetwork
{
    [Serializable]
    public class Package : IPackage {

		#region Fields

		IPublicProfile publicProfile;
		byte[] data;

		#endregion Fields


		#region Properties

		public IPublicProfile PublicProfile {
			get{ return publicProfile; }
			private set{ publicProfile = value; }
		}

		public byte[] Data {
			get { throw new NotImplementedException(); }
		}

        #endregion Properties

        #region Constructors

        public Package(IPublicProfile userProfile, byte[] data)
        {
            this.publicProfile = userProfile;
            this.data = data;
        }

        #endregion Constructors
		
	}
}