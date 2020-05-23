using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTrackingApp.Models
{
	public class ApiResponse
	{
		public bool Success { get; protected set; }

		public string ErrorMessage { get; protected set; }

		public static ApiResponse WithSuccess()
		{
			return new ApiResponse() { Success = true };
		}

		public static ApiResponse WithError(string message)
		{
			return new ApiResponse() { Success = false, ErrorMessage = message };
		}
	}

	public class ApiResponse<TData> : ApiResponse
	{
		public TData Data { get; set; }

		public static ApiResponse<TData> WithSuccess(TData data)
		{
			return new ApiResponse<TData>() { Success = true, Data = data };
		}
	}
}
