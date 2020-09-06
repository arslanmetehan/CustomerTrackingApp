using CustomerTrackingApp.Entities;
using CustomerTrackingApp.Enums;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTrackingApp.Persistence.Dapper
{
	public class LogRepository : BaseSqliteRepository, ILogRepository
	{
		public void Log(LogType type, string message)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				dbConnection.Execute("INSERT INTO Log (Type, Message, Timestamp) VALUES(@Type, @Message, @Timestamp)", new { Type = type, Message = message, Timestamp = DateTime.UtcNow.Ticks });
			}
		}

		public void Log(Log LogType)
		{
			throw new NotImplementedException();
		}
	}
}
