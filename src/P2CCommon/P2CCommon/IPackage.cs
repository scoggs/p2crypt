using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2CCommon 
{
	public interface IPackage 
	{
		IPublicProfile PublicProfile{ get; private set; }
 
		byte[] Data{ get; private set; }

	}
}
