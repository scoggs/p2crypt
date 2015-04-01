#region Header

/*
 * The delivery package. Signature required.
 * This class is the class that will be seralized before it get sent through the net.
 *
 * Can use properties if  you want, can change it around if like you but it must contain a string and a byte[]
 *
 * This class is required to be in the same directory as the Server class.
 */

#endregion Header

using System;
using P2CCommon;

namespace P2CNetwork
{
    [Serializable]
    public class Package
    {
        #region Fields

        public byte[] data;
        public IPublicProfile user;

        #endregion Fields

        #region Constructors

        public Package(IPublicProfile user, byte[] data)
        {
            this.user = user;
            this.data = data;
        }

        #endregion Constructors
    }
}