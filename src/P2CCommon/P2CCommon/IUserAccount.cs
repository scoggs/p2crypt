using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2CCommon
{
    public interface IUserAccount
    {

        Guid GlobalId {get;set;}

        //The public profile is something to share with other users. It is a way to identify and verify a user.
        IPublicProfile PublicProfile {get;}
        
        String RsaParameters { get; set; }

        String UserNick { get; set; }

        byte[] Decrypt(byte[] data);

    }
}
