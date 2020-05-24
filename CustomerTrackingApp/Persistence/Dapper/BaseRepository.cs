using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTrackingApp.Persistence.Dapper
{
	public class BaseSqliteRepository
	{
		private static string _dbFilePath = "C:\\Workspace\\github\\CustomerTrackingApp\\CustomerTrackingApp.sqlite";

		private static bool IsInitialized = false;

		protected SQLiteConnection OpenConnection()
		{
			if (!IsInitialized)
			{
				if (!File.Exists(_dbFilePath))
				{
					SQLiteConnection.CreateFile(_dbFilePath);
				}
			}

			var conn = new SQLiteConnection($"Data Source={_dbFilePath};Version=3;Read Only=False;");
			conn.Open();

			if (!IsInitialized)
			{
				this.EnsureDatabaseIsInitialized(conn);
				IsInitialized = true;
			}

			return conn;
		}
		private void EnsureDatabaseIsInitialized(SQLiteConnection conn)
		{
			string sql = null;
			SQLiteCommand command = null;
			int count = 0;

			sql = "SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name = 'User'";
			command = new SQLiteCommand(sql, conn);
			count = Convert.ToInt32(command.ExecuteScalar());
			if (count == 0)
			{
				sql = "create table User (" +
							"Id INTEGER PRIMARY KEY, " +
							"Username TEXT NOT NULL, " +
							"FullName TEXT NOT NULL, " +
							"Password TEXT NOT NULL, " +
							"Email TEXT NOT NULL," +
							"BirthYear INT NOT NULL, " +
							"Gender INT NOT NULL, " +
							"Type INT NOT NULL, " +
							"IsActive INT NOT NULL, " +
							"ManagerId INT NOT NULL, " +
							"Phone INT NOT NULL" +
						")";

				command = new SQLiteCommand(sql, conn);
				command.ExecuteNonQuery();
			}
			sql = "SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name = 'Log'";
			command = new SQLiteCommand(sql, conn);
			count = Convert.ToInt32(command.ExecuteScalar());
			if (count == 0)
			{
				sql = "create table Log (" +
							"Id INTEGER PRIMARY KEY, " +
							"Type INTEGER NOT NULL, " +
							"Message TEXT, " +
							"Timestamp INTEGER" +
						")";

				command = new SQLiteCommand(sql, conn);
				command.ExecuteNonQuery();
			}
		}

	}
}
