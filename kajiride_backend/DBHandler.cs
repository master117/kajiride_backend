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
				using (SqlConnection conn = new SqlConnection())
				{
					conn.ConnectionString = "Data Source=localhost;" +
						"Initial Catalog=mangadb;" +
						"Integrated Security=SSPI;";
					conn.Open();

					string sql = "SELECT USERID, USERNAME, PASSWORD, SALT, ROLE FROM USERS WHERE USERNAME=@USERNAME";

					SqlCommand sqlCommand = new SqlCommand(sql, conn);
					SqlParameter userSqlParameter = new SqlParameter("@USERNAME", username); ;
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
				using (SqlConnection conn = new SqlConnection())
				{
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
			}
			catch (Exception e)
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
				using (SqlConnection conn = new SqlConnection())
				{
					conn.ConnectionString = "Data Source=localhost;" +
						"Initial Catalog=mangadb;" +
						"Integrated Security=SSPI;";
					conn.Open();

					string sql = "SELECT MANGAID, NAME, AUTHOR, ARTIST, PUBLISHER, STATUS, VOLUMES, " +
						"LANGUAGE, GENRE, IMAGE, DESCRIPTION, ORIGINALNAME FROM MANGA";

					SqlCommand sqlCommand = new SqlCommand(sql, conn);

					SqlDataReader reader = sqlCommand.ExecuteReader();
					while (reader.Read())
					{
						Manga manga = new Manga();
						manga.mangaid = (long)reader.GetValue(0);
						manga.name = reader.IsDBNull(1) ? null : (string)reader.GetValue(1);
						manga.author = reader.IsDBNull(2) ? null : (string)reader.GetValue(2);
						manga.artist = reader.IsDBNull(3) ? null : (string)reader.GetValue(3);
						manga.publisher = reader.IsDBNull(4) ? null : (string)reader.GetValue(4);
						manga.status = reader.IsDBNull(5) ? null : (string)reader.GetValue(5);
						manga.volumes = reader.IsDBNull(6) ? null : (int?)reader.GetValue(6);
						manga.language = reader.IsDBNull(7) ? null : (string)reader.GetValue(7);
						manga.genre = reader.IsDBNull(8) ? null : (string)reader.GetValue(8);
						manga.image = reader.IsDBNull(9) ? null : (string)reader.GetValue(9);
						manga.description = reader.IsDBNull(10) ? null : (string)reader.GetValue(10);
						manga.originalname = reader.IsDBNull(11) ? null : (string)reader.GetValue(11);

						mangaList.Add(manga);
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			return mangaList;
		}

		internal static Manga GetManga(long mangaId)
		{
			try
			{
				using (SqlConnection conn = new SqlConnection())
				{
					conn.ConnectionString = "Data Source=localhost;" +
						"Initial Catalog=mangadb;" +
						"Integrated Security=SSPI;";
					conn.Open();

					string sql = "SELECT " +
						"MANGAID, " +
						"NAME, " +
						"AUTHOR, " +
						"ARTIST, " +
						"PUBLISHER, " +
						"STATUS, " +
						"VOLUMES, " +
						"LANGUAGE, " +
						"GENRE, " +
						"IMAGE, " +
						"DESCRIPTION, " +
						"ORIGINALNAME " +
						"FROM MANGA " +
						"WHERE Manga.MANGAID=@MANGAID";

					SqlCommand sqlCommand = new SqlCommand(sql, conn);
					sqlCommand.Parameters.Add(new SqlParameter("@MANGAID", mangaId));

					SqlDataReader reader = sqlCommand.ExecuteReader();
					if (reader.Read())
					{
						Manga manga = new Manga();
						manga.mangaid = (long)reader.GetValue(0);
						manga.name = reader.IsDBNull(1) ? null : (string)reader.GetValue(1);
						manga.author = reader.IsDBNull(2) ? null : (string)reader.GetValue(2);
						manga.artist = reader.IsDBNull(3) ? null : (string)reader.GetValue(3);
						manga.publisher = reader.IsDBNull(4) ? null : (string)reader.GetValue(4);
						manga.status = reader.IsDBNull(5) ? null : (string)reader.GetValue(5);
						manga.volumes = reader.IsDBNull(6) ? null : (int?)reader.GetValue(6);
						manga.language = reader.IsDBNull(7) ? null : (string)reader.GetValue(7);
						manga.genre = reader.IsDBNull(8) ? null : (string)reader.GetValue(8);
						manga.image = reader.IsDBNull(9) ? null : (string)reader.GetValue(9);
						manga.description = reader.IsDBNull(10) ? null : (string)reader.GetValue(10);
						manga.originalname = reader.IsDBNull(11) ? null : (string)reader.GetValue(11);

						return manga;
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			return null;
		}

		public static Manga InsertManga(Manga manga)
		{
			try
			{
				using (SqlConnection conn = new SqlConnection())
				{
					conn.ConnectionString = "Data Source=localhost;" +
						"Initial Catalog=mangadb;" +
						"Integrated Security=SSPI;";
					conn.Open();

					string sql = "INSERT INTO MANGA (" +
						"NAME, AUTHOR, ARTIST, PUBLISHER, STATUS, VOLUMES, LANGUAGE, GENRE, IMAGE, DESCRIPTION, ORIGINALNAME" +
						") output INSERTED.MANGAID VALUES (" +
						"@NAME, @AUTHOR, @ARTIST, @PUBLISHER, @STATUS, @VOLUMES, @LANGUAGE, @GENRE, @IMAGE, @DESCRIPTION, @ORIGINALNAME" +
						");";

					SqlCommand sqlC = new SqlCommand(sql, conn);
					sqlC.Parameters.Add(new SqlParameter("@NAME", manga.name ?? (object)DBNull.Value));
					sqlC.Parameters.Add(new SqlParameter("@AUTHOR", manga.author ?? (object)DBNull.Value));
					sqlC.Parameters.Add(new SqlParameter("@ARTIST", manga.artist ?? (object)DBNull.Value));
					sqlC.Parameters.Add(new SqlParameter("@PUBLISHER", manga.publisher ?? (object)DBNull.Value));
					sqlC.Parameters.Add(new SqlParameter("@STATUS", manga.status ?? (object)DBNull.Value));
					sqlC.Parameters.Add(new SqlParameter("@VOLUMES", manga.volumes ?? (object)DBNull.Value));
					sqlC.Parameters.Add(new SqlParameter("@LANGUAGE", manga.language ?? (object)DBNull.Value));
					sqlC.Parameters.Add(new SqlParameter("@GENRE", manga.genre ?? (object)DBNull.Value));
					sqlC.Parameters.Add(new SqlParameter("@IMAGE", manga.image ?? (object)DBNull.Value));
					sqlC.Parameters.Add(new SqlParameter("@DESCRIPTION", manga.description ?? (object)DBNull.Value));
					sqlC.Parameters.Add(new SqlParameter("@ORIGINALNAME", manga.originalname ?? (object)DBNull.Value));

					SqlDataReader reader = sqlC.ExecuteReader();

					if (reader.Read())
						manga.mangaid = (long)reader.GetValue(0);

					return manga;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			return null;
		}

		public static Manga EditManga(Manga manga)
		{
			try
			{
				using (SqlConnection conn = new SqlConnection())
				{
					conn.ConnectionString = "Data Source=localhost;" +
						"Initial Catalog=mangadb;" +
						"Integrated Security=SSPI;";
					conn.Open();

					string sql = "UPDATE MANGA SET " +
						"NAME=@NAME, AUTHOR=@AUTHOR, ARTIST=@ARTIST, PUBLISHER=@PUBLISHER, STATUS=@STATUS, " +
						"VOLUMES=@VOLUMES, LANGUAGE=@LANGUAGE, GENRE=@GENRE, " +
						"IMAGE=@IMAGE, DESCRIPTION=@DESCRIPTION, ORIGINALNAME=@ORIGINALNAME " +
						"WHERE MANGAID=@MANGAID";

					SqlCommand sqlC = new SqlCommand(sql, conn);
					sqlC.Parameters.Add(new SqlParameter("@NAME", manga.name ?? (object)DBNull.Value));
					sqlC.Parameters.Add(new SqlParameter("@AUTHOR", manga.author ?? (object)DBNull.Value));
					sqlC.Parameters.Add(new SqlParameter("@ARTIST", manga.artist ?? (object)DBNull.Value));
					sqlC.Parameters.Add(new SqlParameter("@PUBLISHER", manga.publisher ?? (object)DBNull.Value));
					sqlC.Parameters.Add(new SqlParameter("@STATUS", manga.status ?? (object)DBNull.Value));
					sqlC.Parameters.Add(new SqlParameter("@VOLUMES", manga.volumes ?? (object)DBNull.Value));
					sqlC.Parameters.Add(new SqlParameter("@LANGUAGE", manga.language ?? (object)DBNull.Value));
					sqlC.Parameters.Add(new SqlParameter("@GENRE", manga.genre ?? (object)DBNull.Value));
					sqlC.Parameters.Add(new SqlParameter("@IMAGE", manga.image ?? (object)DBNull.Value));
					sqlC.Parameters.Add(new SqlParameter("@DESCRIPTION", manga.description ?? (object)DBNull.Value));
					sqlC.Parameters.Add(new SqlParameter("@ORIGINALNAME", manga.originalname ?? (object)DBNull.Value));
					sqlC.Parameters.Add(new SqlParameter("@MANGAID", manga.mangaid));

					if (sqlC.ExecuteNonQuery() > 0)
						return manga;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			return null;
		}

		internal static UserManga GetUserManga(long mangaId, long userId)
		{
			try
			{
				using (SqlConnection conn = new SqlConnection())
				{
					conn.ConnectionString = "Data Source=localhost;" +
						"Initial Catalog=mangadb;" +
						"Integrated Security=SSPI;";
					conn.Open();

					string sql = "SELECT " +
						"MANGAID, " +
						"USERID, " +
						"OWNED, " +
						"COMMENT, " +
						"SCORE " +
						"FROM USERMANGA " +
						"WHERE MANGAID=@MANGAID AND UserID=@USERID";

					SqlCommand sqlCommand = new SqlCommand(sql, conn);
					sqlCommand.Parameters.Add(new SqlParameter("@MANGAID", mangaId));
					sqlCommand.Parameters.Add(new SqlParameter("@USERID", userId));

					SqlDataReader reader = sqlCommand.ExecuteReader();
					if (reader.Read())
					{
						UserManga userManga = new UserManga();
						userManga.mangaid = (long)reader.GetValue(0);
						userManga.userid = (long)reader.GetValue(1);
						userManga.owned = reader.IsDBNull(2) ? null : (int?)reader.GetValue(2);
						userManga.comment = reader.IsDBNull(3) ? null : (string)reader.GetValue(3);
						userManga.score = reader.IsDBNull(4) ? null : (int?)reader.GetValue(4);

						return userManga;
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			return null;
		}

		public static UserManga InsertUserManga(UserManga userManga, long mangaId, long userId)
		{
			try
			{
				using (SqlConnection conn = new SqlConnection())
				{
					conn.ConnectionString = "Data Source=localhost;" +
						"Initial Catalog=mangadb;" +
						"Integrated Security=SSPI;";
					conn.Open();

					string sql = "INSERT INTO USERMANGA (" +
						"MANGAID, USERID, OWNED, COMMENT, SCORE" +
						") output INSERTED.MANGAID VALUES (" +
						"@MANGAID, @USERID, @OWNED, @COMMENT, @SCORE" +
						");";

					SqlCommand sqlC = new SqlCommand(sql, conn);
					sqlC.Parameters.Add(new SqlParameter("@MANGAID", mangaId));
					sqlC.Parameters.Add(new SqlParameter("@USERID", userId));
					sqlC.Parameters.Add(new SqlParameter("@OWNED", userManga.owned ?? (object)DBNull.Value));
					sqlC.Parameters.Add(new SqlParameter("@COMMENT", userManga.comment ?? (object)DBNull.Value));
					sqlC.Parameters.Add(new SqlParameter("@SCORE", userManga.score ?? (object)DBNull.Value));

					SqlDataReader reader = sqlC.ExecuteReader();

					return GetUserManga(mangaId, userId);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			return null;
		}

		public static UserManga EditUserManga(UserManga userManga)
		{
			try
			{
				using (SqlConnection conn = new SqlConnection())
				{
					conn.ConnectionString = "Data Source=localhost;" +
						"Initial Catalog=mangadb;" +
						"Integrated Security=SSPI;";
					conn.Open();

					string sql = "UPDATE USERMANGA SET " +
						"OWNED=@OWNED, COMMENT=@COMMENT, SCORE=@SCORE " +
						"WHERE MANGAID=@MANGAID AND USERID=@USERID";

					SqlCommand sqlC = new SqlCommand(sql, conn);
					sqlC.Parameters.Add(new SqlParameter("@OWNED", userManga.owned ?? (object)DBNull.Value));
					sqlC.Parameters.Add(new SqlParameter("@COMMENT", userManga.comment ?? (object)DBNull.Value));
					sqlC.Parameters.Add(new SqlParameter("@MANGAID", userManga.mangaid));
					sqlC.Parameters.Add(new SqlParameter("@USERID", userManga.userid));
					sqlC.Parameters.Add(new SqlParameter("@SCORE", userManga.score));

					if (sqlC.ExecuteNonQuery() > 0)
						return userManga;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			return null;
		}

		#endregion

		#region Release
		public static List<Release> GetAllReleases()
		{
			List<Release> releaseList = new List<Release>();

			try
			{
				using (SqlConnection conn = new SqlConnection())
				{
					conn.ConnectionString = "Data Source=localhost;" +
						"Initial Catalog=mangadb;" +
						"Integrated Security=SSPI;";
					conn.Open();

					string sql = "SELECT RELEASEID, MANGAID, VOLUME, ACTIVE, RELEASEDATE FROM RELEASE";

					SqlCommand sqlCommand = new SqlCommand(sql, conn);

					SqlDataReader reader = sqlCommand.ExecuteReader();
					while (reader.Read())
					{
						Release release = new Release(
							(long)reader.GetValue(0),
							(long)reader.GetValue(1),
							(int)reader.GetValue(2),
							(bool)reader.GetValue(3),
							(DateTime)reader.GetValue(4)
						);

						releaseList.Add(release);
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			return releaseList;
		}

		public static Release InsertRelease(Release release)
		{
			try
			{
				using (SqlConnection conn = new SqlConnection())
				{
					conn.ConnectionString = "Data Source=localhost;" +
						"Initial Catalog=mangadb;" +
						"Integrated Security=SSPI;";
					conn.Open();

					string sql = "INSERT INTO RELEASE (" +
						"MANGAID, VOLUME, ACTIVE, RELEASEDATE" +
						") output INSERTED.RELEASEID VALUES (" +
						"@MANGAID, @VOLUME, @ACTIVE, @RELEASEDATE" +
						");";

					SqlCommand sqlC = new SqlCommand(sql, conn);
					sqlC.Parameters.Add(new SqlParameter("@MANGAID", release.mangaid));
					sqlC.Parameters.Add(new SqlParameter("@VOLUME", release.volume));
					sqlC.Parameters.Add(new SqlParameter("@ACTIVE", release.active));
					sqlC.Parameters.Add(new SqlParameter("@RELEASEDATE", release.releaseDate));

					SqlDataReader reader = sqlC.ExecuteReader();

					if (reader.Read())
						release.releaseid = (long)reader.GetValue(0);

					return release;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			return null;
		}

		public static Release EditRelease(Release release)
		{
			try
			{
				using (SqlConnection conn = new SqlConnection())
				{
					conn.ConnectionString = "Data Source=localhost;" +
						"Initial Catalog=mangadb;" +
						"Integrated Security=SSPI;";
					conn.Open();

					string sql = "UPDATE RELEASE SET " +
						"MANGAID=@MANGAID, VOLUME=@VOLUME, ACTIVE=@ACTIVE, RELEASEDATE=@RELEASEDATE " +
						"WHERE RELEASEID=@RELEASEID";

					SqlCommand sqlC = new SqlCommand(sql, conn);
					sqlC.Parameters.Add(new SqlParameter("@MANGAID", release.mangaid));
					sqlC.Parameters.Add(new SqlParameter("@VOLUME", release.volume));
					sqlC.Parameters.Add(new SqlParameter("@ACTIVE", release.active));
					sqlC.Parameters.Add(new SqlParameter("@RELEASEDATE", release.releaseDate));
					sqlC.Parameters.Add(new SqlParameter("@RELEASEID", release.releaseid));

					if (sqlC.ExecuteNonQuery() > 0)
						return release;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			return null;
		}

		public static bool DeleteRelease(long releaseId)
		{
			try
			{
				using (SqlConnection conn = new SqlConnection())
				{
					conn.ConnectionString = "Data Source=localhost;" +
						"Initial Catalog=mangadb;" +
						"Integrated Security=SSPI;";
					conn.Open();

					string sql = "DELETE FROM RELEASE WHERE RELEASEID=@RELEASEID";

					SqlCommand sqlC = new SqlCommand(sql, conn);
					sqlC.Parameters.Add(new SqlParameter("@RELEASEID", releaseId));
					int affectedRows = sqlC.ExecuteNonQuery();

					return affectedRows > 0;
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