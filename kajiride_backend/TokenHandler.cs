using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kajiride_backend
{
	public class TokenHandler
	{
		public enum Roles
		{
			user = 0,
			admin = 1
		}

		private struct TokenConfig
		{
			public string Token;
			public int Role;

			public TokenConfig(string tempToken, int tempRole)
			{
				Token = tempToken;
				Role = tempRole;
			}
		}

		private static List<TokenConfig> tokens = new List<TokenConfig>();
		
		public static void AddToken(string token, int role)
		{
			tokens.RemoveAll(x => x.Token == token);

			tokens.Add(new TokenConfig(token, role));
		}

		public static bool isAllowed(string token, Roles reqRole)
		{
			if (!tokens.Any(x => x.Token == token))
				return false;

			return (int)reqRole <= tokens.First(x => x.Token == token).Role;
		}
	}
}