using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace service
{
    public class DataAccess
    {
        private SqlConnection connection;
        private SqlCommand command;
        private SqlDataReader reader;

        public SqlDataReader Reader
        {
            get { return reader; }
        }

        public DataAccess()
        {
            connection = new SqlConnection("server=.\\SQLEXPRESS; database=CATALOGO_DB; integrated security=true");
            command = new SqlCommand();
        }

        public void setQuery(string query)
        {
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = query;
        }

        public void executeRead()
        {
            command.Connection = connection;
            try
            {
                connection.Open();
                reader = command.ExecuteReader();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void executeAction()
        {
            command.Connection = connection;
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void setParameter(string name, object value)
        {
            command.Parameters.AddWithValue(name, value);
        }

        public void closeConnection()
        {
            if (reader != null)
                reader.Close();
            connection.Close();
        }
    }
}
