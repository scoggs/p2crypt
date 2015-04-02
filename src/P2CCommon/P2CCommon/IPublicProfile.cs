using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace P2CCommon
{
    public interface IPublicProfile
    {
        
        Guid GlobalId { get; set; }

        RSAParameters RsaParameters { get; set; }

        string UserNick { get; set; }

        byte[] Encrypt(byte[] data);
    }
}
