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
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Data;

namespace Application_Processing
{
    /// <summary>
    /// Логика взаимодействия для Edit_Specialist.xaml
    /// </summary>
    public partial class Edit_Specialist : Window
    {
        Request req = new Request();

        public long ID_Specialist = 0;
        public Edit_Specialist()
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
                MessageBox.Show("Введите ФИО специалиста", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (String.IsNullOrEmpty(txt_Audience.Text.ToString()))
            {
                MessageBox.Show("Введите АУДИТОРЮ специалиста!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (txt_Email.Text.Length == 0)
            {
                txt_Email.Focus();
                return;
            }
            else if (!Regex.IsMatch(txt_Email.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
            {
                MessageBox.Show("Email адрес специалиста неверный. Введите валидный Email!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);

                txt_Email.Select(0, txt_Email.Text.Length);
                txt_Email.Focus();
                return;
            }


            if (String.IsNullOrEmpty(txt_Phone.Text.ToString()))
            {
                MessageBox.Show("Введите ТЕЛЕФОН специалиста!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            string str_FIO = txt_FIO.Text.ToString();
            string str_AUDIENCE = txt_Audience.Text.ToString();
            string str_EMAIL = txt_Email.Text.ToString();
            string str_PHONE = txt_Phone.Text.ToString();


            if (ID_Specialist > 0)
            {
                string Sql_Command_Update_Specialist = "UPDATE [dbo].[Specialist] SET " +
                    "[FIO] = '" + str_FIO + "', " +
                    "[Audience] = '" + str_AUDIENCE + "', " +
                    "[Email] = '" + str_EMAIL + "', " +
                    "[Phone] = '" + str_PHONE + "' " +
                    "WHERE ID=" + ID_Specialist + "";
                long rez_Update_Specialist = req.insert_del_update(Sql_Command_Update_Specialist);

                if (rez_Update_Specialist > 0)
                {
                    MessageBox.Show("Данные о специалисте успешно обновлены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainWindow.SelfRefMain_frm.Fill_DGV_All_Specialist();
                    this.Close();
                }
            }
            else
            {

                string Sql_Command_Insert_Specialist = "INSERT INTO [dbo].[Specialist] (" +
                "[FIO], " +
                "[Audience], " +
                "[Email], [" +
                "Phone]) " +
                "VALUES (" +
                "'" + str_FIO + "', " +
                "'" + str_AUDIENCE + "', " +
                "'" + str_EMAIL + "', " +
                "'" + str_PHONE + "')";
                long rez_insert_Specialist = req.insert_del_update(Sql_Command_Insert_Specialist);

                if (rez_insert_Specialist > 0)
                {
                    MessageBox.Show("Специалист успешно создан!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainWindow.SelfRefMain_frm.Fill_DGV_All_Specialist();
                    this.Close();
                }
            }
        }

        private void txt_Phone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (ID_Specialist > 0)
            {
                string sql_command = "SELECT " +
                    "[FIO], " +
                    "[Audience], " +
                    "[Email], " +
                    "[Phone] " +
                    "FROM " +
                    "[dbo].[Specialist] " +
                    "where ID="+ID_Specialist+"";
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
                        txt_Audience.Text = (dr[1].ToString());
                        txt_Email.Text = (dr[2].ToString());
                        txt_Phone.Text = (dr[3].ToString());
                    }
                    dr.Close();
                    req.con.Close();
                }

                dr.Close();
                req.con.Close();
            }
        }
    }
}
