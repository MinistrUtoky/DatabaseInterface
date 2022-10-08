using Database_Interface.classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Xml.Serialization;
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
            columnNames.Clear();
            currentTableName = (sender as Button).Content.ToString();
            Popups_Filling();
            Refresh_Table();
        }

        private void Add_Popup_Filling()
        {
            StackPanel sp = new StackPanel();
            Add_Popup.Child = sp;
            Button cancel = new Button();
            cancel.Click += Clear_Addition_Click;
            cancel.Content = "Cancel";
            sp.Children.Add(cancel);
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
            b.Click += Enact_Add_Popup;
            b.Click += Clear_Addition_Click;
            b.Content = "Add";
            sp.Children.Add(b);
        }

        private void New_Table_subPopup_Filling()
        {
            
            StackPanel sp = new StackPanel();
            New_Table_subPopup.Child = sp;
            Button cancel = new Button();
            cancel.Click += Clear_New_Table_subPopup_Click;
            cancel.Content = "Cancel";
            sp.Children.Add(cancel);
            TextBlock tb = new TextBlock();
            TextBox tbx = new TextBox();
            tb.Text = "How many columns do you need?";
            tb.Background = Brushes.White;
            sp.Children.Add(tb);
            sp.Children.Add(tbx);
            Button b = new Button();
            b.Click += Enact_New_Table_subPopup;
            b.Click += Clear_New_Table_subPopup_Click;
            b.Content = "Continue";
            sp.Children.Add(b);
        }

        private void New_Table_Popup_Filling(int columns_count)
        {
            StackPanel sp = new StackPanel();
            New_Table_Popup.Child = sp;
            Button cancel = new Button();
            cancel.Click += Clear_New_Table_Popup_Click;
            cancel.Content = "Cancel";
            sp.Children.Add(cancel);
            for (int i = 0; i < columns_count; i++)
            {

            }
            /*foreach (DataColumn c in clsDB.Get_DataTable("SELECT * FROM " + currentTableName).Columns)
            {
                TextBlock tb = new TextBlock();
                TextBox tbx = new TextBox();
                tb.Text = c.ToString();
                tb.Background = Brushes.White;
                sp.Children.Add(tb);
                sp.Children.Add(tbx);
            }
            //b.Click += Enact_New_Table_Popup;*/
            Button b = new Button();
            b.Content = "Create Table";
            sp.Children.Add(b);
            b.Click += Clear_New_Table_Popup_Click;
        }

        private void Update_Popup_Filling()
        {
            StackPanel sp = new StackPanel();
            Update_Popup.Child = sp;
            Button cancel = new Button();
            cancel.Click += Clear_Update_Click;
            cancel.Content = "Cancel";
            sp.Children.Add(cancel);
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
            b.Click += Enact_Update_Popup;
            b.Click += Clear_Update_Click;
            b.Content = "Update";
            sp.Children.Add(b);
        }

        private void Remove_Popup_Filling()
        {
            StackPanel sp = new StackPanel();
            Remove_Popup.Child = sp;
            Button cancel = new Button();
            cancel.Click += Clear_Removal_Click;
            cancel.Content = "Cancel";
            sp.Children.Add(cancel);
            TextBlock tb = new TextBlock();
            TextBox tbx = new TextBox();
            tb.Text = clsDB.Get_DataTable("SELECT * FROM " + currentTableName).Columns[0].ToString();
            tb.Background = Brushes.White;
            Button b = new Button();
            b.Click += Enact_Remove_Popup;
            b.Click += Clear_Removal_Click;
            b.Content = "Remove";
            sp.Children.Add(tb); sp.Children.Add(tbx); sp.Children.Add(b);
        }

        // Redo popup fillers as one common filler

        private void Clear_Popup(Popup popup)
        {
            popup.IsOpen = false;
            if (popup.Child.GetType() == typeof(StackPanel))
            {
                StackPanel sp = (popup.Child as StackPanel);
                foreach (object ch in sp.Children)
                {
                    if (ch.GetType() == typeof(TextBox))
                    {
                        TextBox t = (ch as TextBox);
                        t.Text = "";
                    }
                }
            }
        }
        
        private void Popups_Filling()
        {
            New_Table_subPopup_Filling();
            Add_Popup_Filling();
            Update_Popup_Filling();
            Remove_Popup_Filling();
        }


        private void DB_Add_Record(List<string> new_element)
        {
            StringBuilder fields = new StringBuilder();
            for (int i = 1; i < columnNames.Count-1; i++)
            {
                fields.Append("[");
                fields.Append(columnNames[i]);
                fields.Append("]");
                fields.Append(",");
            }
            fields.Append("[");
            fields.Append(columnNames[columnNames.Count - 1]);
            fields.Append("]");

            StringBuilder values = new StringBuilder();
            for (int i = 0; i < new_element.Count - 1; i++)
            {
                values.Append("'");
                values.Append(new_element[i]);
                values.Append("'");
                values.Append(",");
            }
            values.Append("'");
            values.Append(new_element[new_element.Count - 1]);
            values.Append("'");

            string sql_Add = "INSERT INTO " + currentTableName + " (" + fields + ") VALUES(" + values + ")";
            clsDB.Execute_SQL(sql_Add);
        }

        // argsList = id, name. description eg
        private void DB_Update_Record(List<string> element_to_update)
        {
            string sSQL = "SELECT * FROM " + currentTableName + " WHERE [" + columnNames[0] + "] = '" + element_to_update[0] + "'";
            DataTable tbl = clsDB.Get_DataTable(sSQL);
            if (tbl.Rows.Count < 1)
            {
                //make a popup that says you're gay
            }
            else
            {
                for (int i = 1; i < element_to_update.Count; i++)
                {
                    string sql_Update = "UPDATE " + currentTableName + " SET [" + columnNames[i] + "] = '" + element_to_update[i] + "' WHERE [" + columnNames[0] + "] = '" + element_to_update[0] + "'";
                    clsDB.Execute_SQL(sql_Update);
                }
            }
        }

        private void DB_Remove_Record(string id)
        {
            string sSQL = "SELECT * FROM " + currentTableName + " WHERE [" + columnNames[0] + "] = '" + id + "'";
            DataTable tbl = clsDB.Get_DataTable(sSQL);
            if (tbl.Rows.Count > 0)
            {
                string sql_Remove = "DELETE FROM " + currentTableName + " WHERE [" + columnNames[0] + "] = '" + id + "'";
                clsDB.Execute_SQL(sql_Remove);
            }
            else
            {
                //a popup saying fk u
            }
        }



        private void DB_New_Table(string name, List<string> column_names)
        {

        }


        private List<string> Enact_Popup(Popup popup)
        {
            List<string> l = new List<string>();
            if (popup.Child.GetType() == typeof(StackPanel))
            {
                StackPanel sp = (popup.Child as StackPanel);
                foreach (object ch in sp.Children)
                {
                    if (ch.GetType() == typeof(TextBox))
                    {
                        TextBox t = (ch as TextBox);
                        l.Add(t.Text);
                    }
                }
                return l;
            }
            return null;
        }

        private void Enact_Add_Popup(object sender, RoutedEventArgs e)
        {
            List<string> l = Enact_Popup(Add_Popup);
            if (l == null) throw new Exception("addlist was null");
            if (l.Count == 0) throw new Exception("no textboxes in popup");
            DB_Add_Record(l);
            Refresh_Table();
        }

        private void Enact_Update_Popup(object sender, RoutedEventArgs e)
        {
            List<string> l = Enact_Popup(Update_Popup);
            if (l == null) throw new Exception("updatelist was null");
            if (l.Count == 0) throw new Exception("no textboxes in popup");
            if (!int.TryParse(l[0], out int a)) throw new Exception("id have to be an integer");
            DB_Update_Record(l);
            Refresh_Table();
        }

        private void Enact_Remove_Popup(object sender, RoutedEventArgs e)
        {
            List<string> l = Enact_Popup(Remove_Popup);
            if (l == null) throw new Exception("removallist was null");
            if (l.Count == 0) throw new Exception("no textboxes in popup");
            if (!int.TryParse(l[0], out int a)) throw new Exception("id have to be an integer");
            //probably better to forbid the empty element but whatever
            DB_Remove_Record(l[0]);
            Refresh_Table();
        }

        private void Enact_New_Table_subPopup(object sender, RoutedEventArgs e)
        {
            List<string> l = Enact_Popup(New_Table_subPopup);
            if (l == null) throw new Exception("newtable count list was null");
            if (l.Count == 0) throw new Exception("no textboxes in popup");
            if (!int.TryParse(l[0], out int a)) throw new Exception("number of columns have to be an integer");
            New_Table_Popup_Filling(int.Parse(l[0]));
            New_Table_Popup.IsOpen = true;
        }

        private void Enact_New_Table_Popup(object sender, RoutedEventArgs e)
        {
            List<string> l = Enact_Popup(New_Table_Popup);
            if (l == null) throw new Exception("newtable list was null");
            // also need to check if the table with the name exists
            string name = l[0];
            l.RemoveAt(0);
            DB_New_Table(name, l);
        }

        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            Add_Popup.IsOpen = true;
        }

        private void Update_Button_Click(object sender, RoutedEventArgs e)
        {
            Update_Popup.IsOpen = true;
        }

        private void Remove_Button_Click(object sender, RoutedEventArgs e)
        {
            Remove_Popup.IsOpen = true;
        }
        private void Add_New_Table_Button_Click(object sender, RoutedEventArgs e)
        {
            New_Table_subPopup.IsOpen = true;  
        }

        private void Clear_Addition_Click(object sender, RoutedEventArgs e)
        {
            Clear_Popup(Add_Popup);
        }

        private void Clear_Update_Click(object sender, RoutedEventArgs e)
        {
            Clear_Popup(Update_Popup);
        }

        private void Clear_Removal_Click(object sender, RoutedEventArgs e)
        {
            Clear_Popup(Remove_Popup);
        }

        private void Clear_New_Table_Popup_Click(object sender, RoutedEventArgs e)
        {
            Clear_Popup(New_Table_Popup);
        }

        private void Clear_New_Table_subPopup_Click(object sender, RoutedEventArgs e)
        {
            Clear_Popup(New_Table_subPopup);
        }
    }
}
