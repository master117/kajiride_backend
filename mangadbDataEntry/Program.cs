using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mangadbDataEntry
{
	class Program
	{
		static void Main(string[] args)
		{
			int userid = 1;

			SqlConnection conn = new SqlConnection();
			conn.ConnectionString = @"Data Source=localhost;" +
			"Database=mangadb;" +
			"Integrated Security=true;";
			conn.Open();

			// Add Entries to MANGA and MANGAUSER
			using (TextFieldParser parser = new TextFieldParser("mangadb.csv"))
			{
				parser.TextFieldType = FieldType.Delimited;
				parser.SetDelimiters(";");
				while (!parser.EndOfData)
				{
					//Processing row
					// Add Manga
					string[] fields = parser.ReadFields();
					string sqlM = "INSERT INTO MANGA (" +
						(string.IsNullOrWhiteSpace(fields[0]) ? "" : "NAME") +
						(string.IsNullOrWhiteSpace(fields[1]) ? "" : ", AUTHOR") +
						(string.IsNullOrWhiteSpace(fields[2]) ? "" : ", ARTIST") +
						(string.IsNullOrWhiteSpace(fields[3]) ? "" : ", PUBLISHER") +
						(string.IsNullOrWhiteSpace(fields[4]) ? "" : ", STATUS") +
						(string.IsNullOrWhiteSpace(fields[5]) ? "" : ", VOLUMES") +
						(string.IsNullOrWhiteSpace(fields[7]) ? "" : ", LANGUAGE") +
						(string.IsNullOrWhiteSpace(fields[8]) ? "" : ", GENRE") +
						(string.IsNullOrWhiteSpace(fields[9]) ? "" : ", IMAGE") +
						(string.IsNullOrWhiteSpace(fields[10]) ? "" : ", DESCRIPTION") +
						") VALUES (" +
						(string.IsNullOrWhiteSpace(fields[0]) ? "" : "@NAME") +
						(string.IsNullOrWhiteSpace(fields[1]) ? "" : ", @AUTHOR") +
						(string.IsNullOrWhiteSpace(fields[2]) ? "" : ", @ARTIST") +
						(string.IsNullOrWhiteSpace(fields[3]) ? "" : ", @PUBLISHER") +
						(string.IsNullOrWhiteSpace(fields[4]) ? "" : ", @STATUS") +
						(string.IsNullOrWhiteSpace(fields[5]) ? "" : ", @VOLUMES") +
						(string.IsNullOrWhiteSpace(fields[7]) ? "" : ", @LANGUAGE") +
						(string.IsNullOrWhiteSpace(fields[8]) ? "" : ", @GENRE") +
						(string.IsNullOrWhiteSpace(fields[9]) ? "" : ", @IMAGE") +
						(string.IsNullOrWhiteSpace(fields[10]) ? "" : ", @DESCRIPTION") +
						"); SELECT SCOPE_IDENTITY()   ";

					SqlCommand sqlC = new SqlCommand(sqlM, conn);
					sqlC.Parameters.Add(new SqlParameter("@NAME", fields[0]));
					sqlC.Parameters.Add(new SqlParameter("@AUTHOR", fields[1]));
					sqlC.Parameters.Add(new SqlParameter("@ARTIST", fields[2]));
					sqlC.Parameters.Add(new SqlParameter("@PUBLISHER", fields[3]));
					sqlC.Parameters.Add(new SqlParameter("@STATUS", fields[4]));
					sqlC.Parameters.Add(new SqlParameter("@VOLUMES", fields[5]));
					sqlC.Parameters.Add(new SqlParameter("@LANGUAGE", fields[7]));
					sqlC.Parameters.Add(new SqlParameter("@GENRE", fields[8]));
					sqlC.Parameters.Add(new SqlParameter("@IMAGE", fields[9]));
					sqlC.Parameters.Add(new SqlParameter("@DESCRIPTION", fields[10]));

					//Get the inserted query
					long insertedMangaID = Convert.ToInt64(sqlC.ExecuteScalar());

					// Add UserManga
					string sqlUM = "INSERT INTO USERMANGA (" +
						"USERID" +
						", MANGAID" +
						(string.IsNullOrWhiteSpace(fields[6]) ? "" : ", OWNED") +
						(string.IsNullOrWhiteSpace(fields[11]) ? "" : ", SCORE") +
						") VALUES (" +
						"@USERID" +
						", @MANGAID" +
						(string.IsNullOrWhiteSpace(fields[6]) ? "" : ", @OWNED") +
						(string.IsNullOrWhiteSpace(fields[11]) ? "" : ", @SCORE") +
						");";

					SqlCommand sqlUMC = new SqlCommand(sqlUM, conn);
					sqlUMC.Parameters.Add(new SqlParameter("@USERID", userid));
					sqlUMC.Parameters.Add(new SqlParameter("@MANGAID", insertedMangaID));
					sqlUMC.Parameters.Add(new SqlParameter("@OWNED", fields[6]));
					sqlUMC.Parameters.Add(new SqlParameter("@SCORE", fields[11]));

					sqlUMC.ExecuteNonQuery();
				}
			}
		}
	}
}
