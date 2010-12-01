using System;
using System.Collections.Generic;
using System.Web;
using System.Data.OleDb;
using System.Web.SessionState;
using System.IO;

/// <summary>
/// Summary description for DBHelper
/// </summary>
//================================================================================
public class DBHelper
{
	//--------------------------------------------------------------------------------
	public static bool IsDevelopment (Uri u)
	{
		return u.PathAndQuery.StartsWith("/MTBScout/", StringComparison.InvariantCultureIgnoreCase);
	}

	//--------------------------------------------------------------------------------
	public static string GetDBPath (HttpRequest req)
	{
		return IsDevelopment(req.Url)
				? HttpContext.Current.Server.MapPath("~/mdb-database/DebugMtbScout.mdb")
				: HttpContext.Current.Server.MapPath("~/mdb-database/MtbScout.mdb");
	}

	//--------------------------------------------------------------------------------
	public static OleDbConnection OpenConnection (HttpRequest req)
	{
		OleDbConnection conn = new OleDbConnection(
					string.Format("Provider=Microsoft.Jet.OLEDB.4.0; Data Source={0}; User Id=admin; Password=", GetDBPath(req))
					);
		conn.Open();
		return conn;
	}

	//--------------------------------------------------------------------------------
	public static void CountVisitor (HttpRequest request, HttpSessionState session, string mdbPath)
	{
		lock (typeof(DBHelper))
		{
			string host = request["REMOTE_HOST"];

			OleDbConnection conn = null;
			try
			{
				conn = OpenConnection(request);

				int visitorSessionCount;
				using (OleDbCommand command = new OleDbCommand(
					string.Format("select visits from visitors where host=\"{0}\"", host),
					conn))
				{
					object o = command.ExecuteScalar();
					visitorSessionCount = o == null ? 0 : (int)o;
				}

				visitorSessionCount++;
				if (visitorSessionCount == 1)
				{
					using (OleDbCommand command = new OleDbCommand(
						string.Format("insert into visitors (host, visits) values(\"{0}\", {1})", host, visitorSessionCount),
						conn))
					{
						command.ExecuteNonQuery();
					}
				}
				else
				{
					using (OleDbCommand command = new OleDbCommand(
						string.Format("update visitors set visits={0} where host=\"{1}\"", visitorSessionCount, host),
						conn))
					{
						command.ExecuteNonQuery();
					}
				}

				using (OleDbCommand command = new OleDbCommand(
					string.Format("select count(*) as HostCount, sum(visits) as SessionCount from visitors"),
					conn))
				{
					using (OleDbDataReader reader = command.ExecuteReader())
					{
						if (reader.Read())
						{
							session["HostCount"] = Convert.ToInt32(reader["HostCount"]);
							session["SessionCount"] = Convert.ToInt32(reader["SessionCount"]);
						}
					}
				}

				session["VisitorSessionCount"] = visitorSessionCount;

			}
			finally
			{
				if (conn != null)
					conn.Close();
			}
		}
	}

	//--------------------------------------------------------------------------------
	public static void BackupDB (string mdbPath)
	{
		lock (typeof(DBHelper))
		{
			string newPath = Path.Combine(Path.GetDirectoryName(mdbPath), "Backup.mdb");
			try
			{
				File.Copy(mdbPath, newPath, true);
			}
			catch
			{
				throw;
			}
		}
	}
}
