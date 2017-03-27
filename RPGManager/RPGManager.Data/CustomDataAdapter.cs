using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace RPGManager.Data
{
    public class CustomDataAdapter : IDisposable
    {
        protected string _connectionString;

        private SqlConnection _sqlConnection;

        private SqlCommand _sqlCommand;

        private SqlDataAdapter _sqlDataAdapter;

        public CustomDataAdapter(string sqlCommand, CommandType commandType, SqlParameter[] sqlParameters = null)
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            _sqlConnection = new SqlConnection(_connectionString);
            _sqlConnection.Open();

            _sqlCommand = new SqlCommand(sqlCommand, _sqlConnection)
            {
                CommandType = commandType
            };

            if (sqlParameters != null && sqlParameters.Any())
            {
                _sqlCommand.Parameters.AddRange(sqlParameters);
            }

            _sqlDataAdapter = new SqlDataAdapter(_sqlCommand);
        }

        public void Dispose()
        {
            _sqlDataAdapter.Dispose();
            _sqlCommand.Dispose();
            _sqlConnection.Dispose();
        }

        public int Fill(DataTable dataTable)
        {
            return _sqlDataAdapter.Fill(dataTable);
        }

        public int ExecuteNonQuery()
        {
            return _sqlCommand.ExecuteNonQuery();
        }
    }
}