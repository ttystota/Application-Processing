using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace Application_Processing
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        Request req = new Request();

        public Login()
        {
            InitializeComponent();
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void btn_Login_Click(object sender, RoutedEventArgs e)
        {           
            if (txt_Login.Text != "" &&
              txt_Password.Password.ToString() != "")
            {
                long ID_User = checkAccount(txt_Login.Text.ToString(), txt_Password.Password.ToString());

                if (ID_User > 0)
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.ID_User = ID_User;
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Имя пользователя или пароль неверные! ", " Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

            }

            else
            {
                MessageBox.Show("Заполните поля логин и пароль!", "Ошибка",MessageBoxButton.OK,MessageBoxImage.Warning);
            }
        }

        private long checkAccount(string username, string password)
        {
            long ID_User = 0;
            string sql_command_Select_Users = "SELECT US.ID " +
           "FROM Users US " +
           "WHERE (" +
           "(US.Login='" + username + "') AND " +
           "(US.Password='" + password + "'))";
            int row_count = 0;
            req.con.Open();
            SqlCommand exeSql = new SqlCommand(sql_command_Select_Users, req.con);
            SqlDataReader dr = exeSql.ExecuteReader();
            while (dr.Read())
            {
                row_count++;
            }
            dr.Close();
            if (row_count > 0)
            {   
                dr = exeSql.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    ID_User = Convert.ToInt64(dr[0].ToString());
                }
                dr.Close();
                req.con.Close();
            }
            dr.Close();
            req.con.Close();
            return ID_User;
        }
    }
}
