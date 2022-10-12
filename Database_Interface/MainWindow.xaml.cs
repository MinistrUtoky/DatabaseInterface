using Database_Interface.classes;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
            Fill_Table_Names_Tree();
            Popups_Filling();
            Refresh_Table();
        }

        private void Fill_Table_Names_Tree()
        {
            foreach (string s in clsDB.tableNames)
            {
                Button tableButton = new Button();
                tableButton.Content = s;
                tableButton.Background = Brushes.White;
                tableButton.Foreground = Brushes.Blue;
                tableButton.Click += TableButton_Click;
                tableNamesTree.Items.Add(tableButton);
            }
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

        private StackPanel Create_StackPanel_Popup(Popup popup)
        {
            StackPanel sp = new StackPanel();
            ScrollViewer sw = new ScrollViewer();
            popup.Child = sw;
            sw.Content = sp;
            Button cancel = new Button();
            if (popup == Add_Popup) cancel.Click += Clear_Addition_Click;
            else if (popup == New_Table_subPopup) cancel.Click += Clear_New_Table_subPopup_Click;
            else if (popup == New_Table_Popup) cancel.Click += Clear_New_Table_Popup_Click;
            else if (popup == Update_Popup) cancel.Click += Clear_Update_Click;
            else if (popup == Remove_Popup) cancel.Click += Clear_Removal_Click;
            cancel.Content = "Cancel";
            sp.Children.Add(cancel);
            return sp;
        }


        private void Add_Popup_Filling()
        {
            StackPanel sp = Create_StackPanel_Popup(Add_Popup);
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
            StackPanel sp = Create_StackPanel_Popup(New_Table_subPopup);
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
            StackPanel sp = Create_StackPanel_Popup(New_Table_Popup);
            TextBlock n = new TextBlock();
            TextBox name = new TextBox();
            n.Text = "Table name"; n.Background = Brushes.White;
            sp.Children.Add(n); sp.Children.Add(name);
            for (int i = 1; i < columns_count + 1; i++)
            {
                TextBlock tb = new TextBlock();
                TextBox tbx = new TextBox();
                if (i == 1) tb.Text = i.ToString() + " column" + " (PRIMARY KEY)";
                else tb.Text = i.ToString() + " column";
                tb.Background = Brushes.White;
                Expander ex = new Expander();
                ListBox lb = new ListBox();
                for (int h = 0; h < 4; h++) {
                    TextBlock tbb = new TextBlock(); tbb.Text = "text"; tbb.Background = Brushes.White;
                    lb.Items.Add(tbb);
                }
                ex.Content = lb;
                sp.Children.Add(tb);
                sp.Children.Add(tbx);
                sp.Children.Add(ex);
            }
            Button b = new Button();
            b.Content = "Create Table";
            sp.Children.Add(b);
            b.Click += Enact_New_Table_Popup;
            b.Click += Clear_New_Table_Popup_Click;
        }

        private void Update_Popup_Filling()
        {
            StackPanel sp = Create_StackPanel_Popup(Update_Popup);
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
            StackPanel sp = Create_StackPanel_Popup(Remove_Popup);
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


        private void Clear_Popup(Popup popup)
        {
            popup.IsOpen = false;
            if (popup.Child.GetType() == typeof(ScrollViewer))
            {
                ScrollViewer sw = (popup.Child as ScrollViewer);
                if (sw.Content.GetType() == typeof(StackPanel))
                {
                    StackPanel sp = (sw.Content as StackPanel);
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
            for (int i = 1; i < columnNames.Count; i++)
            {
                fields.Append("[");
                fields.Append(columnNames[i]);
                fields.Append("]");
                if (i != columnNames.Count - 1) fields.Append(",");
            }

            StringBuilder values = new StringBuilder();
            for (int i = 0; i < new_element.Count; i++)
            {
                values.Append("'"); values.Append(new_element[i]); values.Append("'");
                if (i != new_element.Count - 1) values.Append(",");
            }
            string sql_Add = "INSERT INTO " + currentTableName + " (" + fields + ") VALUES(" + values + ")";
            clsDB.Execute_SQL(sql_Add);
        }

        private void DB_Update_Record(List<string> element_to_update)
        {
            string sSQL = "SELECT * FROM " + currentTableName + " WHERE [" + columnNames[0] + "] = '" + element_to_update[0] + "'";
            DataTable tbl = clsDB.Get_DataTable(sSQL);
            if (tbl.Rows.Count > 0)
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
        }

        private void DB_New_Table(string name, List<string> column_names)
        {
            if (clsDB.tableNames.Contains(name)) throw new Exception("table with the name " + name + " already exists");
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < column_names.Count; i++)
            {
                sb.Append(column_names[i]);
                sb.Append(" ");
                if (i == 0)
                    sb.Append("INT NOT NULL PRIMARY KEY IDENTITY, ");
                else
                    if (i == column_names.Count - 1)
                    sb.Append("nvarchar(1000) NULL ");
                else
                    sb.Append("nvarchar(1000) NULL, ");
            }
            string sql_Add_New_Table = "CREATE TABLE [dbo].[" + name + "] ( " + sb.ToString() + ");";
            clsDB.Execute_SQL(sql_Add_New_Table);
            clsDB.tableNames.Add(name);
            tableNamesTree.Items.Clear();
            Fill_Table_Names_Tree();
        }


        private List<string> Enact_Popup(Popup popup)
        {
            List<string> l = new List<string>();
            if (popup.Child.GetType() == typeof(ScrollViewer))
            {
                ScrollViewer sw = (popup.Child as ScrollViewer);
                if (sw.Content.GetType() == typeof(StackPanel))
                {
                    StackPanel sp = (sw.Content as StackPanel);
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
            if (l[0] == "") throw new Exception("index for update hasn't been passed");
            DB_Update_Record(l);
            Refresh_Table();
        }

        private void Enact_Remove_Popup(object sender, RoutedEventArgs e)
        {
            List<string> l = Enact_Popup(Remove_Popup);
            if (l == null) throw new Exception("removallist was null");
            if (l.Count == 0) throw new Exception("no textboxes in popup");
            if (!int.TryParse(l[0], out int a)) throw new Exception("id have to be an integer");
            if (l[0] == "") throw new Exception("index for removal hasn't been passed");
            DB_Remove_Record(l[0]);
            Refresh_Table();
        }

        private static bool EmptyElements(string s) { return s == ""; }

        private void Enact_New_Table_subPopup(object sender, RoutedEventArgs e)
        {
            List<string> l = Enact_Popup(New_Table_subPopup);
            if (l == null) throw new Exception("newtable count list was null");
            if (l.Count == 0) throw new Exception("no textboxes in popup");
            if (!int.TryParse(l[0], out int a)) throw new Exception("number of columns have to be an integer");
            if (l[0] == "0") throw new Exception("you need at least 1 column");
            New_Table_Popup_Filling(int.Parse(l[0]));
            New_Table_Popup.IsOpen = true;
        }

        private void Enact_New_Table_Popup(object sender, RoutedEventArgs e)
        {
            List<string> l = Enact_Popup(New_Table_Popup);
            if (l == null) throw new Exception("newtable list was null");
            if (l.Any(EmptyElements)) throw new Exception("Some elements were not present");
            string name = l[0];
            l.RemoveAt(0);
            DB_New_Table(name, l);
        }

        private void Add_Button_Click(object sender, RoutedEventArgs e) => Add_Popup.IsOpen = true;
        
        private void Update_Button_Click(object sender, RoutedEventArgs e) => Update_Popup.IsOpen = true;

        private void Remove_Button_Click(object sender, RoutedEventArgs e) => Remove_Popup.IsOpen = true;

        private void Add_New_Table_Button_Click(object sender, RoutedEventArgs e) => New_Table_subPopup.IsOpen = true;
        private void Nope_Click(object sender, RoutedEventArgs e) => (sender as Button).Content = "Nope";
        private void Help_Click(object sender, RoutedEventArgs e) => Jss.IsOpen=true;
        private void Thanks_Click(object sender, RoutedEventArgs e) => Jss.IsOpen = false;

        private void Clear_Addition_Click(object sender, RoutedEventArgs e) => Clear_Popup(Add_Popup);

        private void Clear_Update_Click(object sender, RoutedEventArgs e) => Clear_Popup(Update_Popup);

        private void Clear_Removal_Click(object sender, RoutedEventArgs e) => Clear_Popup(Remove_Popup);
        
        private void Clear_New_Table_Popup_Click(object sender, RoutedEventArgs e) => Clear_Popup(New_Table_Popup);

        private void Clear_New_Table_subPopup_Click(object sender, RoutedEventArgs e) => Clear_Popup(New_Table_subPopup);

    }
}
