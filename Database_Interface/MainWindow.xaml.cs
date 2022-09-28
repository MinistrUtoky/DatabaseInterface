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
using System.Xml.Linq;

namespace Database_Interface
{
    public partial class MainWindow : Window
    {
        private string currentTableName;
        public MainWindow()
        {
            InitializeComponent();
            clsDB.Tables_Upload();
            currentTableName = clsDB.tableNames[0];
            foreach (string s in clsDB.tableNames)
            {
                Button tableButton = new Button();
                tableButton.Content = s;
                tableButton.Background = Brushes.White;
                tableButton.Foreground = Brushes.Blue;
                tableButton.Click += TableButton_Click;
                tableNamesTree.Items.Add(tableButton);
            }
            clsDB.FillDataGrid(tableGrid, currentTableName);
        }

        private void TableButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void DB_Add_Record(string sName, string sDescription)
        {             
            string sSQL = "SELECT * FROM tbl1 WHERE [name] = '" + sName + "'";
            DataTable tbl = clsDB.Get_DataTable(sSQL);
            if (tbl.Rows.Count < 1)
            {
                string sql_Add = "INSERT INTO tbl1 ([name],[description]) VALUES('" + sName + "','" + sDescription + "')";
                clsDB.Execute_SQL(sql_Add);
            }
            else
            {
                //make a popup that says you're gaily gay
            }
        }

        private void DB_Update_Record(string sName, string sDescription)
        {
            string sSQL = "SELECT * FROM tbl1 WHERE [name] = '" + sName + "'";
            DataTable tbl = clsDB.Get_DataTable(sSQL);
            if (tbl.Rows.Count < 1)
            {
                //make a popup that says you're gay
            }
            else
            {
                string ID = tbl.Rows[0]["id"].ToString();
                string sql_Update = "UPDATE tbl1 SET [name] = \'GAY\' WHERE id = '" + ID + "'";
                clsDB.Execute_SQL(sql_Update);
            }
        }

        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            DB_Add_Record("gay?", "REALLY GAY");
            clsDB.FillDataGrid(tableGrid, currentTableName);
        }

        private void Update_Button_Click(object sender, RoutedEventArgs e)
        {
            DB_Update_Record("gay?", "REALLY GAY");
            clsDB.FillDataGrid(tableGrid, currentTableName);
        }
    }
}
