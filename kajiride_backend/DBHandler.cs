using kajiride_backend.Models;
using Konscious.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace kajiride_backend
{
	public class DBHandler
	{
		#region LoginOut
		public static User TryLoggingIn(string username, string password)
		{
			try
			{
				SqlConnection conn = new SqlConnection();
				conn.ConnectionString = "Data Source=localhost;" +
				"Initial Catalog=mangadb;" +
				"Integrated Security=SSPI;";
				conn.Open();

				string sql = "SELECT USERID, USERNAME, PASSWORD, SALT, ROLE FROM USERS WHERE USERNAME=@USERNAME";

				SqlCommand sqlCommand = new SqlCommand(sql, conn);
				SqlParameter userSqlParameter = new SqlParameter("@USERNAME", username);;
				sqlCommand.Parameters.Add(userSqlParameter);

				SqlDataReader reader = sqlCommand.ExecuteReader();
				if (reader.Read())
				{
					byte[] passwordHash = (byte[])reader.GetValue(2);
					byte[] salt = (byte[])reader.GetValue(3);

					if (VerifyHash(password, salt, passwordHash))
					{
						int role = (int)reader.GetValue(4);
						

						User user = new User();
						user.id = (long)reader.GetValue(0);
						user.name = (string)reader.GetValue(1);
						user.role = role;				

						return user;
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			return null;
		}

		public static KeyValuePair<bool, string> TryRegisterUser(string username, string password)
		{
			try
			{
				// Hash password
				byte[] salt = CreateSalt();
				byte[] passwordHash = HashPassword(password, salt);

				// Create Connection
				SqlConnection conn = new SqlConnection();
				conn.ConnectionString = "Data Source=localhost;" +
				"Initial Catalog=mangadb;" +
				"Integrated Security=SSPI;";
				conn.Open();

				// Check for existing User
				string ckeckForUserSQL = "SELECT COUNT(*) FROM USERS WHERE USERNAME = @USERNAME";
				SqlCommand checkSqlCommand = new SqlCommand(ckeckForUserSQL, conn);
				checkSqlCommand.Parameters.Add(new SqlParameter("@USERNAME", username));
				if ((int)checkSqlCommand.ExecuteScalar() != 0)
					return new KeyValuePair<bool, string>(false, "User already exists.");

				// Create new User
				string inserUserSQL = "INSERT INTO USERS (USERNAME, PASSWORD, SALT) " +
					"VALUES (@USERNAME, @PASSWORD, @SALT)";

				SqlCommand insertSqlCommand = new SqlCommand(inserUserSQL, conn);
				insertSqlCommand.Parameters.Add(new SqlParameter("@USERNAME", username));
				insertSqlCommand.Parameters.Add(new SqlParameter("@PASSWORD", passwordHash));
				insertSqlCommand.Parameters.Add(new SqlParameter("@SALT", salt));

				if (insertSqlCommand.ExecuteNonQuery() > 0)
					return new KeyValuePair<bool, string>(true, "User created.");
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
			}

			return new KeyValuePair<bool, string>(false, "unknown error"); ;
		}
		#endregion

		#region Manga
		public static List<Manga> GetAllManga()
		{
			List<Manga> mangaList = new List<Manga>();

			try
			{
				SqlConnection conn = new SqlConnection();
				conn.ConnectionString = "Data Source=localhost;" +
				"Initial Catalog=mangadb;" +
				"Integrated Security=SSPI;";
				conn.Open();

				string sql = "SELECT MANGAID, NAME, AUTHOR, ARTIST, PUBLISHER, STATUS, TOTALVOLUMES, OWNEDVOLUMES, " +
					"LANGUAGE, GENRE, IMAGE, DESCRIPTION, SCORE FROM MANGA";

				SqlCommand sqlCommand = new SqlCommand(sql, conn);

				SqlDataReader reader = sqlCommand.ExecuteReader();
				while (reader.Read())
				{
					Manga manga = new Manga(
						(long)reader.GetValue(0),
						reader.IsDBNull(1) ? null : (string)reader.GetValue(1),
						reader.IsDBNull(2) ? null : (string)reader.GetValue(2),
						reader.IsDBNull(3) ? null : (string)reader.GetValue(3),
						reader.IsDBNull(4) ? null : (string)reader.GetValue(4),
						reader.IsDBNull(5) ? null : (string)reader.GetValue(5),
						reader.IsDBNull(6) ? null : (int?)reader.GetValue(6),
						reader.IsDBNull(7) ? null : (int?)reader.GetValue(7),
						reader.IsDBNull(8) ? null : (string)reader.GetValue(8),
						reader.IsDBNull(9) ? null : (string)reader.GetValue(9),
						reader.IsDBNull(10) ? null : (string)reader.GetValue(10),
						reader.IsDBNull(11) ? null : (string)reader.GetValue(11),
						reader.IsDBNull(12) ? null : (int?)reader.GetValue(12)
					);

					mangaList.Add(manga);
				}
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
			}

			return mangaList;
		}

		public static bool InsertEditManga(Manga manga, string token)
		{
			if (!SessionHandler.isAllowed(token, SessionHandler.Roles.admin))
				return false;

			try
			{
				SqlConnection conn = new SqlConnection();
				conn.ConnectionString = "Data Source=localhost;" +
				"Initial Catalog=mangadb;" +
				"Integrated Security=SSPI;";
				conn.Open();

				if (manga.mangaid == null)
				{
					string sql = "INSERT INTO MANGA (" +
						"NAME, AUTHOR, ARTIST, PUBLISHER, STATUS, TOTALVOLUMES, OWNEDVOLUMES, LANGUAGE, GENRE, IMAGE, DESCRIPTION, SCORE" +
						") VALUES (" +
						"@NAME, @AUTHOR, @ARTIST, @PUBLISHER, @STATUS, @TOTALVOLUMES, @OWNEDVOLUMES, @LANGUAGE, @GENRE, @IMAGE, @DESCRIPTION, @SCORE" +
						")";

					SqlCommand sqlC = new SqlCommand(sql, conn);
					sqlC.Parameters.Add(new SqlParameter("@NAME", manga.name));
					sqlC.Parameters.Add(new SqlParameter("@AUTHOR", manga.author));
					sqlC.Parameters.Add(new SqlParameter("@ARTIST", manga.artist));
					sqlC.Parameters.Add(new SqlParameter("@PUBLISHER", manga.publisher));
					sqlC.Parameters.Add(new SqlParameter("@STATUS", manga.status));
					sqlC.Parameters.Add(new SqlParameter("@TOTALVOLUMES", manga.totalvolumes));
					sqlC.Parameters.Add(new SqlParameter("@OWNEDVOLUMES", manga.ownedvolumes));
					sqlC.Parameters.Add(new SqlParameter("@LANGUAGE", manga.language));
					sqlC.Parameters.Add(new SqlParameter("@GENRE", manga.genre));
					sqlC.Parameters.Add(new SqlParameter("@IMAGE", manga.image));
					sqlC.Parameters.Add(new SqlParameter("@DESCRIPTION", manga.description));
					sqlC.Parameters.Add(new SqlParameter("@SCORE", manga.score));

					return sqlC.ExecuteNonQuery() > 0;
				}
				else
				{
					string sql = "UPDATE MANGA SET " +
						"NAME=@NAME, AUTHOR=@AUTHOR, ARTIST=@ARTIST, PUBLISHER=@PUBLISHER, STATUS=@STATUS, " +
						"TOTALVOLUMES=@TOTALVOLUMES, OWNEDVOLUMES=@OWNEDVOLUMES, LANGUAGE=@LANGUAGE, GENRE=@GENRE, " +
						"IMAGE=@IMAGE, DESCRIPTION=@DESCRIPTION, SCORE=@SCORE " +
						"WHERE MANGAID=@MANGAID)";

					SqlCommand sqlC = new SqlCommand(sql, conn);
					sqlC.Parameters.Add(new SqlParameter("@NAME", manga.name));
					sqlC.Parameters.Add(new SqlParameter("@AUTHOR", manga.author));
					sqlC.Parameters.Add(new SqlParameter("@ARTIST", manga.artist));
					sqlC.Parameters.Add(new SqlParameter("@PUBLISHER", manga.publisher));
					sqlC.Parameters.Add(new SqlParameter("@STATUS", manga.status));
					sqlC.Parameters.Add(new SqlParameter("@TOTALVOLUMES", manga.totalvolumes));
					sqlC.Parameters.Add(new SqlParameter("@OWNEDVOLUMES", manga.ownedvolumes));
					sqlC.Parameters.Add(new SqlParameter("@LANGUAGE", manga.language));
					sqlC.Parameters.Add(new SqlParameter("@GENRE", manga.genre));
					sqlC.Parameters.Add(new SqlParameter("@IMAGE", manga.image));
					sqlC.Parameters.Add(new SqlParameter("@DESCRIPTION", manga.description));
					sqlC.Parameters.Add(new SqlParameter("@SCORE", manga.score));
					sqlC.Parameters.Add(new SqlParameter("@MANGAID", manga.mangaid));

					return sqlC.ExecuteNonQuery() > 0;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			return false;
		}

		#endregion

		#region Helper
		//
		// Helper
		//
		private static byte[] CreateSalt()
		{
			var buffer = new byte[16];
			var rng = new RNGCryptoServiceProvider();
			rng.GetBytes(buffer);
			return buffer;
		}

		private static byte[] HashPassword(string password, byte[] salt)
		{
			var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));

			argon2.Salt = salt;
			argon2.DegreeOfParallelism = 8; // four cores
			argon2.Iterations = 4;
			argon2.MemorySize = 1024 * 1024; // 1 GB

			return argon2.GetBytes(32);
		}

		private static bool VerifyHash(string password, byte[] salt, byte[] hash)
		{
			var newHash = HashPassword(password, salt);
			return hash.SequenceEqual(newHash);
		}
		#endregion
	}
}