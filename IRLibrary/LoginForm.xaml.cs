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
using System.Windows.Shapes;
using System.Data.SQLite;
using System.Windows.Markup;

namespace IRLibrary {
    /// <summary>
    /// Логика взаимодействия для LoginForm.xaml
    /// </summary>
    public partial class LoginForm : Window
    {
        string connStr;
        MainWindow mainWindow;
        public LoginForm(MainWindow main)
        {
            InitializeComponent();
            mainWindow = main;
            connStr = "Data Source=" + Environment.CurrentDirectory + "\\data\\BD.db;Version=3;";
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTB.Text;
            string password = PasswordTB.Password;
            using (SQLiteConnection conn = new SQLiteConnection(connStr))
            {
                conn.Open();
                string sql = "SELECT * FROM Users WHERE name = @name AND password = @password";
                SQLiteCommand command = new SQLiteCommand(sql, conn);
                command.Parameters.AddWithValue("@name", login);
                command.Parameters.AddWithValue("@password", password);
                SQLiteDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    int id = 0;
                    while (reader.Read())
                    {
                        id = reader.GetInt32(0);
                    }
                    MessageBox.Show("Авторизация прошла успешно");
                    mainWindow.updateData(true, login);
                    this.Close();
                }
                else
                {
                    Fail.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
