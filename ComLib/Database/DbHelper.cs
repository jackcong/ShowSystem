using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ComLib.Database
{
    public class DbHelper
    {
        private string connectionstStrings = "";
        public DbHelper(string connectionstStrings)
        {
            this.connectionstStrings = connectionstStrings;
        }
        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionstStrings);
        }

        public DataTable GetData(string strSql)
        {
            SqlConnection connection = GetConnection();
            SqlDataAdapter sda = new SqlDataAdapter(strSql, connection);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            connection.Close();
            connection.Dispose();
            return ds.Tables[0];
        }

        public void Execute(string strSql)
        {
            SqlConnection connection = GetConnection();
            connection.Open();
            SqlCommand scm = new SqlCommand(strSql);
            scm.Connection = connection;

            scm.ExecuteNonQuery();
            connection.Close();
            connection.Dispose();
        }

        public int Execute(string[] array)
        {
            int i = 0;
            SqlConnection connection = GetConnection();
            connection.Open();
            SqlTransaction trans = connection.BeginTransaction();
            SqlCommand scm = new SqlCommand();
            scm.Connection = connection;
            scm.Transaction = trans;
            try
            {
                scm.CommandText = array[0];
                scm.ExecuteNonQuery();
                scm.CommandText = array[1];
                i = Convert.ToInt32(scm.ExecuteScalar());
                trans.Commit();
            }
            catch (System.Exception ex)
            {
                trans.Rollback();
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
            return i;
        }

        public object ExecuteScalar(string strSql)
        {
            SqlConnection connection = GetConnection();
            connection.Open();
            SqlCommand scm = new SqlCommand(strSql);
            scm.Connection = connection;

            object objScalar = scm.ExecuteScalar();

            connection.Close();
            return objScalar;
        }
    }
}
