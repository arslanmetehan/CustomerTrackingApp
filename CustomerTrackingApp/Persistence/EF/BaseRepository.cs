using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTrackingApp.Persistence.EF
{
	public class BaseEFRepository
	{
		protected SQLiteDBContext OpenConnection()
		{
			return new SQLiteDBContext();
		}
	}
}
