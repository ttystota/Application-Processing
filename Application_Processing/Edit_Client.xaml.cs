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
    /// Логика взаимодействия для Edit_Client.xaml
    /// </summary>
    public partial class Edit_Client : Window
    {
        Request req = new Request();

        public long ID_Client = 0;
        public Edit_Client()
        {
            InitializeComponent();
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txt_FIO.Text.ToString()))
            {
                MessageBox.Show("Введите ФИО клиента", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (String.IsNullOrEmpty(txt_Login.Text.ToString()))
            {
                MessageBox.Show("Введите ЛОГИН клиента", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (String.IsNullOrEmpty(txt_Password.Text.ToString()))
            {
                MessageBox.Show("Введите ПАРОЛЬ клиента", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            string str_FIO = txt_FIO.Text.ToString();
            string str_LOGIN = txt_Login.Text.ToString();
            string str_PASSWORD = txt_Password.Text.ToString();



            if (ID_Client > 0)
            {
                string Sql_Command_Update_Client = "UPDATE [dbo].[Users] SET " +
                    "[FIO] = '" + str_FIO + "', " +
                    "[Password] = '" + str_PASSWORD + "' " +
                    "WHERE ID=" + ID_Client + "";
                long rez_Update_Client = req.insert_del_update(Sql_Command_Update_Client);

                if (rez_Update_Client > 0)
                {
                    MessageBox.Show("Учетная запись клиента успешно обновлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainWindow.SelfRefMain_frm.Fill_DGV_All_Client();
                    this.Close();
                }
            }
            else
            {
                string sql_command = "SELECT US.ID " +
       "FROM Users US " +
       "WHERE " +
       "(US.Login='" + str_LOGIN + "')";
                int row_count = 0;
                req.con.Open();
                SqlCommand exeSql = new SqlCommand(sql_command, req.con);
                SqlDataReader dr = exeSql.ExecuteReader();
                while (dr.Read())
                {
                    row_count++;
                }
                dr.Close();
                req.con.Close();
                if (row_count > 0)
                {
                    MessageBox.Show("Учетная запись с таким логином уже существует! Выберите другой логин!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    string Sql_Command_Insert_Client = "INSERT INTO [dbo].[Users] (" +
                                    "[FIO], " +
                                    "[Login], " +
                                    "[Password], " +
                                    "[ID_Role]) " +
                                    "VALUES (" +
                                    "'" + str_FIO + "', " +
                                    "'" + str_LOGIN + "', " +
                                    "'" + str_PASSWORD + "', " +
                                    "1)";
                    long rez_insert_Client = req.insert_del_update(Sql_Command_Insert_Client);

                    if (rez_insert_Client > 0)
                    {
                        MessageBox.Show("Учетная запись клиента успешно создана!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        MainWindow.SelfRefMain_frm.Fill_DGV_All_Client();
                        this.Close();
                    }
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (ID_Client > 0)
            {
                string sql_command = "SELECT [FIO],[Login],[Password] FROM [dbo].[Users] where ID=" + ID_Client + "";
                int row_count = 0;
                req.con.Open();
                SqlCommand exeSql = new SqlCommand(sql_command, req.con);
                SqlDataReader dr = exeSql.ExecuteReader();
                while (dr.Read())
                {
                    row_count++;
                }
                dr.Close();
                if (row_count > 0)
                {
                    dr = exeSql.ExecuteReader(CommandBehavior.CloseConnection);
                    int i = 0;
                    while (dr.Read())
                    {
                        txt_FIO.Text = (dr[0].ToString());
                        txt_Login.Text = (dr[1].ToString());
                        txt_Password.Text = (dr[2].ToString());
                    }
                    dr.Close();
                    req.con.Close();
                }

                dr.Close();
                req.con.Close();
                txt_Login.IsEnabled = false;
            }
        }
    }
}
