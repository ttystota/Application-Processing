using Microsoft.Win32;
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
    /// Логика взаимодействия для Edit_Order.xaml
    /// </summary>
    public partial class Edit_Order : Window
    {

        public long ID_Client_Order = 0;
        public long ID_Client = 0;
        Request req = new Request();
        public Edit_Order()
        {
            InitializeComponent();
        }

    

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

     

        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(combo_Vyd.Text.ToString()))
            {
                MessageBox.Show("Выберите вид заявки", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (String.IsNullOrEmpty(txt_Description.Text.ToString()))
            {
                MessageBox.Show("Заполните поле Комментарий", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (Rad_Vybrat.IsChecked == true)
            {
                if (String.IsNullOrEmpty(combo_Audience.Text.ToString()))
                {
                    MessageBox.Show("Выберите номер аудитории со списка", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }
            else
            {
                if (Rad_Vvesty.IsChecked == true)
                {
                    if (String.IsNullOrEmpty(txt_Audience_New.Text.ToString()))
                    {
                        MessageBox.Show("Заполните поле Аудитория", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                }
            }
            ComboBoxItem typeItem = (ComboBoxItem)combo_Vyd.SelectedItem;

            string Vyd_Zayavka = typeItem.Content.ToString();
            string Description = txt_Description.Text.ToString();
            string Number_Audience = string.Empty;
            string date_Now = DateTime.Now.ToString();
            if (Rad_Vybrat.IsChecked == true)
            {
                Number_Audience = combo_Audience.SelectedItem.ToString();
            }
            else
            {
                if (Rad_Vvesty.IsChecked == true)
                {
                    Number_Audience = txt_Audience_New.Text.ToString();
                }
            }

            if (ID_Client_Order > 0)
            {
                string Sql_Command_Update = "UPDATE [dbo].[Client_Order] SET " +
                    "[Type_of_request] = '" + Vyd_Zayavka + "', " +
                    "[Description] = '" + Description + "', " +
                    "[Number_Audience] = '" + Number_Audience + "', " +
                    "[Date_Add] = '" + date_Now + "', " +
                    "WHERE ID=" + ID_Client_Order + "";

                long rez_update_Document = req.insert_del_update(Sql_Command_Update);
                if (rez_update_Document > 0)
                {
                    MessageBox.Show("Заявка успешно обновлена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainWindow.SelfRefMain_frm.Fill_DGV_Client_Orders(ID_Client);
                    this.Close();
                }
            }
            else
            {
                string Sql_Command_Insert = "INSERT INTO [dbo].[Client_Order] (" +
                    "[Type_of_request], " +
                    "[Description], " +
                    "[Number_Audience], " +
                    "[Date_Add], " +
                    "[ID_Client], " +
                    "[Status]) " +
                    "VALUES (" +
                    "'" + Vyd_Zayavka + "', " +
                    "'" + Description + "', " +
                    "'" + Number_Audience + "', " +
                    "'" + date_Now + "', " +
                    "" + ID_Client + ", " +
                    "'В работе')";

                long rez_insert_Document = req.insert_del_update(Sql_Command_Insert);
                if (rez_insert_Document > 0)
                {
                    MessageBox.Show("Заявка успешно добавлена в систему!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainWindow.SelfRefMain_frm.Fill_DGV_Client_Orders(ID_Client);
                    this.Close();
                }
            }


        }

        public void Fill_Combo_Audience()
        {
            combo_Audience.Items.Clear();
            string sql_command = "SELECT DISTINCT ORD.Number_Audience FROM Client_Order ORD";
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
                    for (int j = 0; j < dr.FieldCount; j++)
                    {
                        combo_Audience.Items.Add(dr[j].ToString());
                    }
                    i += 1;
                }
                dr.Close();
                req.con.Close();
            }

            dr.Close();
            req.con.Close();
            if(combo_Audience.Items.Count>0)
            {
                combo_Audience.SelectedIndex = 0;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Fill_Combo_Audience();
        }

        private void Rad_Vybrat_Checked(object sender, RoutedEventArgs e)
        {
            combo_Audience.Visibility = Visibility.Visible;
            txt_Audience_New.Visibility = Visibility.Hidden;
        }

        private void Rad_Vvesty_Checked(object sender, RoutedEventArgs e)
        {

            combo_Audience.Visibility = Visibility.Hidden;
            txt_Audience_New.Visibility = Visibility.Visible;
        }
    }
}
