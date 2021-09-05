using System.Collections.Generic;

namespace InTechWebApp.Hubs.Models
{
	public class UserModel
	{
		public int UserId { get; set; }
		public string UserName { get; set; }
		public string SessionId { get; set; }
		public HashSet<string> ConnectionIds { get; set; }
	}
}
