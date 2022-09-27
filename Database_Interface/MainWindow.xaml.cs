using Database_Interface.classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Database_Interface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            clsDB.FillDataGrid(tableGrid);
        }

        private void DB_Update_Add_Record(string sName, string sDescription)
        { 
            string sSQL = "SELECT TOP 1 * FROM tbl_gay WHERE [name] Like '" + sName + "'";
            DataTable tbl = clsDB.Get_DataTable(sSQL);
            if (tbl.Rows.Count == 0)
            {
                string sql_Add = "INSERT INTO tbl_gay ([name],[description]) VALUES('" + sName + "','" + sDescription + "')";
                clsDB.Execute_SQL(sql_Add);
            }
            else
            {
                string ID = tbl.Rows[0]["id"].ToString();
                string sql_Update = "UPDATE tbl_gay SET [name] = \"GAY\" WHERE id = " + ID;
                clsDB.Execute_SQL(sql_Update);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DB_Update_Add_Record("gay", "really gay");
            clsDB.FillDataGrid(tableGrid);
        }
    }
}
