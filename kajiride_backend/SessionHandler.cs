using kajiride_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kajiride_backend
{
	public class SessionHandler
	{
		public enum Roles
		{
			user = 0,
			admin = 1
		}

		private struct TokenConfig
		{
			public string Token;
			public User User;

			public TokenConfig(string tempToken, User tempUser)
			{
				Token = tempToken;
				User = tempUser;
			}
		}

		private static List<TokenConfig> tokens = new List<TokenConfig>();

		public static User TryLoggingIn(string username, string password)
		{
			// Get User from DB
			User user = DBHandler.TryLoggingIn(username, password);

			if (user == null)
				return null;

			// Generate Session Token
			string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
			user.token = token;
			AddToken(token, user);

			return user;
		}

		public static bool LogOut(string token)
		{
			if(tokens.Any(x => x.Token == token))
			{
				tokens.RemoveAll(x => x.Token == token);
				return true;
			}

			return false;
		}

		private static void AddToken(string token, User user)
		{
			tokens.RemoveAll(x => x.Token == token);

			tokens.Add(new TokenConfig(token, user));
		}

		public static bool isAllowed(string token, Roles reqRole)
		{
			if (!tokens.Any(x => x.Token == token))
				return false;

			return (int)reqRole <= tokens.First(x => x.Token == token).User.role;
		}

		public static bool isUser(string token, long id)
		{
			if (!tokens.Any(x => x.Token == token))
				return false;

			return tokens.First(x => x.Token == token).User.id == id;
		}
	}
}