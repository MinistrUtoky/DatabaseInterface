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
            currentTableName = (sender as Button).Content.ToString();
            clsDB.FillDataGrid(tableGrid, currentTableName);
        }

        private void DB_Add_Record(string sName, string sDescription)
        {
            string sSQL = "SELECT * FROM " + currentTableName + " WHERE [name] = '" + sName + "'";
            DataTable tbl = clsDB.Get_DataTable(sSQL);
            if (tbl.Rows.Count < 1)
            {
                string sql_Add = "INSERT INTO " + currentTableName + " ([name],[description]) VALUES('" + sName + "','" + sDescription + "')";
                clsDB.Execute_SQL(sql_Add);
            }
            else
            {
                //make a popup that says you're gaily gay
            }
        }
        // argsList = id, name. description eg
        private void DB_Update_Record(string sName, string sDescription)
        {
            string sSQL = "SELECT * FROM " + currentTableName + " WHERE [name] = '" + sName + "'";
            DataTable tbl = clsDB.Get_DataTable(sSQL);
            if (tbl.Rows.Count < 1)
            {
                //make a popup that says you're gay
            }
            else
            {
                string ID = tbl.Rows[0]["id"].ToString();
                string sql_Update = "UPDATE " + currentTableName + " SET [name] = \'GAY\' WHERE id = '" + ID + "'";
                clsDB.Execute_SQL(sql_Update);
            }
        }
        
        private void DB_Remove_Record(string id)
        {             
        }

        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            Add_Popup.IsOpen = true;
            DB_Add_Record("gay?", "REALLY GAY");
            clsDB.FillDataGrid(tableGrid, currentTableName);
        }

        private void Update_Button_Click(object sender, RoutedEventArgs e)
        {
            Update_Popup.IsOpen = true;
            DB_Update_Record("gay?", "REALLY GAY");
            clsDB.FillDataGrid(tableGrid, currentTableName);
        }


        private void Remove_Button_Click(object sender, RoutedEventArgs e)
        {
            Remove_Popup.IsOpen = true;
            DB_Remove_Record("0");
        }

        private void Close_Add_Popup(object sender, RoutedEventArgs e)
        {
            Add_Popup.IsOpen = false;
        }
        private void Close_Update_Popup(object sender, RoutedEventArgs e)
        {
            Update_Popup.IsOpen = false;
        }
        private void Close_Remove_Popup(object sender, RoutedEventArgs e)
        {             
            Remove_Popup.IsOpen = false;
        }       
    }
}
