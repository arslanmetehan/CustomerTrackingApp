using CustomerTrackingApp.Entities;
using CustomerTrackingApp.Enums;
using CustomerTrackingApp.Persistence.EF;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTrackingApp.Persistence.EF
{
	public class LogRepository : BaseEFRepository, ILogRepository
	{
		public void Log(LogType logType, string message)
		{
			using (SQLiteDBContext dbConnection = this.OpenConnection())
			{
				Log newLog = new Log();
				newLog.Type = logType;
				newLog.Message = message;
				newLog.Timestamp = DateTime.UtcNow.Ticks;
				dbConnection.Log.Add(newLog);
				dbConnection.SaveChanges();
			}
		}

		/*public void Log(LogType ınfo, string v)
		{
			throw new NotImplementedException();
		}*/
	}
}
