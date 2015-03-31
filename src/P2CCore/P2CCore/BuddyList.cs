using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace P2CCore
{
	/// <summary>
	/// Holds the list of all trusted friends of the current user.
	/// </summary>
	[Serializable]
	public class BuddyList
	{
		#region Properties

		[XmlArray]
		public List<PublicProfile> Profiles { get; set; }

		#endregion Properties
	}
}