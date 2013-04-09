﻿namespace Elevated.Data
{
	using System;
	using System.Data;
	using System.Data.SqlClient;
	using System.Linq;

	public static class SqlClient
	{
		private static string ConnectionString(Enum name)
		{
			return System.Configuration.ConfigurationManager.ConnectionStrings[name.Description()].ConnectionString;
		}

		public static int ExecuteNonQuery(Enum name, string sql, params SqlParameter[] parameters)
		{
			return ExecuteNonQuery(ConnectionString(name), sql, parameters);
		}

		public static int ExecuteNonQuery(string connectionString, string sql, params SqlParameter[] parameters)
		{
			using (var connection = new SqlConnection(connectionString))
			{
				connection.Open();

				using (var command = new SqlCommand(sql, connection))
				{
					return command.ExecuteNonQuery();
				}
			}
		}

		public static void ExecuteReader(Enum name, string sql, Action<SqlDataReader> execute, params SqlParameter[] parameters)
		{
			ExecuteReader(ConnectionString(name), sql, execute, parameters);
		}

		public static void ExecuteReader(string connectionString, string sql, Action<SqlDataReader> execute, params SqlParameter[] parameters)
		{
			using (var connection = new SqlConnection(connectionString))
			{
				connection.Open();

				using (var command = new SqlCommand(sql, connection))
				{
					using (var reader = command.ExecuteReader(CommandBehavior.CloseConnection))
					{
						execute(reader);
					}
				}
			}
		}
	}
}
