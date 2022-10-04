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
using static System.Net.Mime.MediaTypeNames;

namespace Database_Interface
{
    public partial class MainWindow : Window
    {
        private string currentTableName;
        private List<string> columnNames;
        public MainWindow()
        {
            InitializeComponent();
            columnNames = new List<string>();
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
            Popups_Filling();
            Refresh_Table();
        }

        private void Refresh_Table()
        {
            clsDB.FillDataGrid(tableGrid, currentTableName);
        }

        private void TableButton_Click(object sender, RoutedEventArgs e)
        {

            currentTableName = (sender as Button).Content.ToString();
            Popups_Filling();
            Refresh_Table();
        }

        private void Add_Popup_Filling()
        {
            StackPanel sp = new StackPanel();
            Add_Popup.Child = sp;
            DataColumnCollection dcc = clsDB.Get_DataTable("SELECT * FROM " + currentTableName).Columns;
            dcc.RemoveAt(0);
            foreach (DataColumn c in dcc)
            {
                TextBlock tb = new TextBlock();
                TextBox tbx = new TextBox();
                tb.Text = c.ToString();
                tb.Background = Brushes.White;
                sp.Children.Add(tb);
                sp.Children.Add(tbx);
            }
            Button b = new Button();
            b.Click += Close_Add_Popup;
            b.Content = "Add";
            sp.Children.Add(b);
        }

        private void Update_Popup_Filling()
        {
            StackPanel sp = new StackPanel();
            Update_Popup.Child = sp;
            foreach (DataColumn c in clsDB.Get_DataTable("SELECT * FROM " + currentTableName).Columns)
            {
                columnNames.Add(c.ToString()); // just dont wanna run the whole process again ahah
                TextBlock tb = new TextBlock();
                TextBox tbx = new TextBox();
                tb.Text = c.ToString();
                tb.Background = Brushes.White;
                sp.Children.Add(tb);
                sp.Children.Add(tbx);
            }
            Button b = new Button();
            b.Click += Close_Update_Popup;
            b.Content = "Update";
            sp.Children.Add(b);
        }
        private void Remove_Popup_Filling()
        {
            StackPanel sp = new StackPanel();
            Remove_Popup.Child = sp;
            TextBlock tb = new TextBlock();
            TextBox tbx = new TextBox();
            tb.Text = clsDB.Get_DataTable("SELECT * FROM " + currentTableName).Columns[0].ToString();
            tb.Background = Brushes.White;
            Button b = new Button();
            b.Click += Close_Remove_Popup;
            b.Content = "Remove";
            sp.Children.Add(tb); sp.Children.Add(tbx); sp.Children.Add(b);
        }

        private void Popups_Filling()
        {
            Add_Popup_Filling();
            Update_Popup_Filling();
            Remove_Popup_Filling();
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
            //DB_Add_Record("gay?", "REALLY GAY");
            Refresh_Table();
        }

        private void Update_Button_Click(object sender, RoutedEventArgs e)
        {
            Update_Popup.IsOpen = true;
            //DB_Update_Record("gay?", "REALLY GAY");
            Refresh_Table();
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
