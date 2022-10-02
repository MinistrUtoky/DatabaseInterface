using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows.Controls;

namespace Database_Interface.classes
{
    public static class clsDB
    {
        private static string cn_String = Properties.Settings.Default.connectionString;
        public static List<string> tableNames = new List<string>();
        public static void Tables_Upload()
        {
            using (SqlConnection con = Get_DB_Connection())
                foreach (DataRow row in con.GetSchema("Tables").Rows) 
                    tableNames.Add((string)row["TABLE_NAME"]);
        }

        public static void FillDataGrid(DataGrid tableGrid, string tableName)
        {
            using (SqlConnection con = Get_DB_Connection())
            {
                string cmdString = "SELECT * FROM " + tableName;
                SqlCommand cmd = new SqlCommand(cmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable(tableName);         
                sda.Fill(dt);
                
                tableGrid.ItemsSource = dt.DefaultView;
            }
        }

        public static SqlConnection Get_DB_Connection()
        {
            SqlConnection cn_connection = new SqlConnection(cn_String);
            if (cn_connection.State != ConnectionState.Open) cn_connection.Open();
            return cn_connection;
        }



        public static DataTable Get_DataTable(string SQL_Text)
        {
            SqlConnection cn_connection = Get_DB_Connection();
            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SQL_Text, cn_connection);
            adapter.Fill(table);
            return table;
        }

        public static void Execute_SQL(string SQL_Text)
        {
            SqlConnection cn_connection = Get_DB_Connection();
            SqlCommand cmd_Command = new SqlCommand(SQL_Text, cn_connection);
            cmd_Command.ExecuteNonQuery();
        }

        public static void Close_DB_Connection()
        {
            SqlConnection cn_connection = new SqlConnection(cn_String);
            if (cn_connection.State != ConnectionState.Closed) cn_connection.Close();
        }
    }
}
