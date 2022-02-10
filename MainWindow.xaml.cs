using System;
using System.Collections.Generic;
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

namespace cs_todo_list
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Todo> todos = new List<Todo>();

        public MainWindow()
        {
            InitializeComponent();
            LoadToDoList();
        }
        private void LoadToDoList()
        {
            todos = SQLiteDataAccess.LoadToDo();
            WireUpToDoList();
        }
        private void BtnRefreshList_Click(object sender, RoutedEventArgs e)
        {
            LoadToDoList();
        }
        private void WireUpToDoList()
        {
            // attach the in-memory list to the ListBox
            listToDoListBox.ItemsSource = null; // important to first make null
            listToDoListBox.ItemsSource = todos;
        }
        private void BtnAddToDo_Click(object sender, RoutedEventArgs e)
        {
            Todo td = new Todo();
            td.Name = txtboxName.Text;
            td.Status = txtboxStatus.Text;

            if (!string.IsNullOrEmpty(txtboxName.Text.Trim()) &&
                  !string.IsNullOrEmpty(txtboxStatus.Text.Trim()))
            {
                SQLiteDataAccess.SaveToDo(td);
                txtboxName.Text = "";
                txtboxStatus.Text = "";
            }
        }
        private void ListToDoListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = sender as ListBox;
            foreach (object o in lb.SelectedItems)
            {
                txtboxEditId.Text = (o as Todo).Id.ToString();
                txtboxEditName.Text = (o as Todo).Name;
                txtboxEditStatus.Text = (o as Todo).Status;
            }
        }
        private void BtnSaveToDo_Click(object sender, RoutedEventArgs e)
        {
            var td = new Todo();
            td.Id = Convert.ToInt32(txtboxEditId.Text);
            td.Name = txtboxEditName.Text;
            td.Status = txtboxEditStatus.Text;
            SQLiteDataAccess.UpdateToDo(td);
        }
        private void BtnDeleteToDo_Click(object sender, RoutedEventArgs e)
        {
            var td = new Todo();
            td.Id = Convert.ToInt32(txtboxEditId.Text);
            td.Name = txtboxEditName.Text;
            MessageBoxResult r = MessageBox.Show("Are you sure you want to permanently delete " + td.Name + "?", "Delete To Do",
                MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (r == MessageBoxResult.OK)
            {
                SQLiteDataAccess.DeleteToDo(td.Id);
                txtboxEditId.Text = "";
                txtboxEditName.Text = "";
                txtboxEditStatus.Text = "";
            }
        }
    }
}
