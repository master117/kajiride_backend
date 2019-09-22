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
			SqlConnection conn = new SqlConnection();
			conn.ConnectionString = "Data Source=localhost;" +
			"Initial Catalog=mangadb;" +
			"Integrated Security=SSPI;";
			conn.Open();

			using (TextFieldParser parser = new TextFieldParser("mangadb.csv"))
			{
				parser.TextFieldType = FieldType.Delimited;
				parser.SetDelimiters(",");
				while (!parser.EndOfData)
				{
					//Processing row
					string[] fields = parser.ReadFields();
					string sql = "INSERT INTO MANGA (" +
						(string.IsNullOrWhiteSpace(fields[0]) ? "" : "NAME") +
						(string.IsNullOrWhiteSpace(fields[1]) ? "" : ", AUTHOR") +
						(string.IsNullOrWhiteSpace(fields[2]) ? "" : ", ARTIST") +
						(string.IsNullOrWhiteSpace(fields[3]) ? "" : ", PUBLISHER") +
						(string.IsNullOrWhiteSpace(fields[4]) ? "" : ", STATUS") +
						(string.IsNullOrWhiteSpace(fields[5]) ? "" : ", TOTALVOLUMES") +
						(string.IsNullOrWhiteSpace(fields[6]) ? "" : ", OWNEDVOLUMES") +
						(string.IsNullOrWhiteSpace(fields[7]) ? "" : ", LANGUAGE") +
						(string.IsNullOrWhiteSpace(fields[8]) ? "" : ", GENRE") +
						(string.IsNullOrWhiteSpace(fields[9]) ? "" : ", IMAGE") +
						(string.IsNullOrWhiteSpace(fields[10]) ? "" : ", DESCRIPTION") +
						(string.IsNullOrWhiteSpace(fields[11]) ? "" : ", SCORE") +
						") VALUES (" +
						(string.IsNullOrWhiteSpace(fields[0]) ? "" : "@NAME") +
						(string.IsNullOrWhiteSpace(fields[1]) ? "" : ", @AUTHOR") +
						(string.IsNullOrWhiteSpace(fields[2]) ? "" : ", @ARTIST") +
						(string.IsNullOrWhiteSpace(fields[3]) ? "" : ", @PUBLISHER") +
						(string.IsNullOrWhiteSpace(fields[4]) ? "" : ", @STATUS") +
						(string.IsNullOrWhiteSpace(fields[5]) ? "" : ", @TOTALVOLUMES") +
						(string.IsNullOrWhiteSpace(fields[6]) ? "" : ", @OWNEDVOLUMES") +
						(string.IsNullOrWhiteSpace(fields[7]) ? "" : ", @LANGUAGE") +
						(string.IsNullOrWhiteSpace(fields[8]) ? "" : ", @GENRE") +
						(string.IsNullOrWhiteSpace(fields[9]) ? "" : ", @IMAGE") +
						(string.IsNullOrWhiteSpace(fields[10]) ? "" : ", @DESCRIPTION") +
						(string.IsNullOrWhiteSpace(fields[11]) ? "" : ", @SCORE") +
						")";

					SqlCommand sqlC = new SqlCommand(sql, conn);
					sqlC.Parameters.Add(new SqlParameter("@NAME", fields[0]));
					sqlC.Parameters.Add(new SqlParameter("@AUTHOR", fields[1]));
					sqlC.Parameters.Add(new SqlParameter("@ARTIST", fields[2]));
					sqlC.Parameters.Add(new SqlParameter("@PUBLISHER", fields[3]));
					sqlC.Parameters.Add(new SqlParameter("@STATUS", fields[4]));
					sqlC.Parameters.Add(new SqlParameter("@TOTALVOLUMES", fields[5]));
					sqlC.Parameters.Add(new SqlParameter("@OWNEDVOLUMES", fields[6]));
					sqlC.Parameters.Add(new SqlParameter("@LANGUAGE", fields[7]));
					sqlC.Parameters.Add(new SqlParameter("@GENRE", fields[8]));
					sqlC.Parameters.Add(new SqlParameter("@IMAGE", fields[9]));
					sqlC.Parameters.Add(new SqlParameter("@DESCRIPTION", fields[10]));
					sqlC.Parameters.Add(new SqlParameter("@SCORE", fields[11]));

					sqlC.ExecuteNonQuery();
				}
			}
		}
	}
}
