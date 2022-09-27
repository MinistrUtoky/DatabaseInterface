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

        public static void FillDataGrid(DataGrid tableGrid)
        {

            string cn_String = Properties.Settings.Default.connectionString;
            using (SqlConnection con = new SqlConnection(cn_String))
            {
                string cmdString = "SELECT id, name, description FROM tbl_gay";
                SqlCommand cmd = new SqlCommand(cmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("tbl_gay");
                sda.Fill(dt);
                tableGrid.ItemsSource = dt.DefaultView;
            }
        }

        public static SqlConnection Get_DB_Connection()
        {
            string cn_String = Properties.Settings.Default.connectionString;
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
            string cn_String = Properties.Settings.Default.connectionString;
            SqlConnection cn_connection = new SqlConnection(cn_String);
            if (cn_connection.State != ConnectionState.Closed) cn_connection.Close();
        }
    }
}
