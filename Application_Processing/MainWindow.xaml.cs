using System;
using System.Globalization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;
using System.Diagnostics;
using System.Collections.ObjectModel;
namespace Application_Processing
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public long ID_User = 0;
        public long Role_User = 0;
        public long ID_Client____ = 0;

        public string FIO_Client = string.Empty;
        Request req = new Request();



        public DataTable _dataTable_Client_Orders = null;
        public DataTable _dataTable_All_Orders = null;
        public DataTable _dataTable_All_Client = null;
        public DataTable _dataTable_All_Specialist = null;

        public static MainWindow SelfRefMain_frm
        {
            get;
            set;
        }
        public MainWindow()
        {
            InitializeComponent();
            SelfRefMain_frm = this;

            _dataTable_Client_Orders = new DataTable();
            _dataTable_All_Orders = new DataTable();
            _dataTable_All_Client = new DataTable();
            _dataTable_All_Specialist = new DataTable();
        }

        private void menu_Change_User_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        public void Fill_DGV_Client_Orders(long ID_Client)
        {
            _dataTable_Client_Orders.Clear();
            string sql_command_ = "SELECT " +
                "[ID] as 'ID', " +
                "[Type_of_request] as 'Тип заявки', " +
                "[Description] as 'Комментарий', " +
                "[Number_Audience] as 'Номер аудитории', " +
                "[Date_Add] as 'Дата добавления', " +
                "[Status] as 'Статус' " +
                "FROM [dbo].[Client_Order] CL_O " +
                "where CL_O.ID_Client=" + ID_Client + "";

            SqlDataAdapter dataAdapter = new SqlDataAdapter(sql_command_, req.con);
            dataAdapter.Fill(_dataTable_Client_Orders);
            DGV_Client_Orders.ItemsSource = _dataTable_Client_Orders.DefaultView;
            DGV_Client_Orders.Columns[0].Visibility = Visibility.Hidden;


            _dataTable_Client_Orders.Dispose();
            req.con.Close();

        }

        public void Fill_DGV_All_Client()
        {
            _dataTable_All_Client.Clear();
            string sql_command_ = "SELECT " +
                "[ID] as 'Уникальный номер', " +
                "[FIO] as 'ФИО', " +
                "[Login] as 'Логин', " +
                "[Password] as 'Пароль' " +
                "FROM [dbo].[Users] " +
                "Where ID_Role=1";

            SqlDataAdapter dataAdapter2 = new SqlDataAdapter(sql_command_, req.con);
            dataAdapter2.Fill(_dataTable_All_Client);
            DGV_All_Client.ItemsSource = _dataTable_All_Client.DefaultView;
        //    DGV_All_Client.Columns[0].Visibility = Visibility.Hidden;
            _dataTable_All_Client.Dispose();
            req.con.Close();
        }

        public void Fill_DGV_All_Specialist()
        {
        _dataTable_All_Specialist.Clear();
            string sql_command_ = "SELECT " +
                "[ID] as 'Уникальный номер', " +
                "[FIO] as 'ФИО', [Audience] as 'Аудитория', " +
                "[Email] as 'EMAIL', " +
                "[Phone] as 'Телефон' " +
                "FROM [dbo].[Specialist]";
        SqlDataAdapter dataAdapter3 = new SqlDataAdapter(sql_command_, req.con);
        dataAdapter3.Fill(_dataTable_All_Specialist);
            DGV_All_Specialist.ItemsSource = _dataTable_All_Specialist.DefaultView;
            _dataTable_All_Specialist.Dispose();
            req.con.Close();
    }

        public void Fill_DGV_All_Orders()
        {
            //_dataTable_All_Orders.Clear();

            //string sql_command_ = "";
            //SqlDataAdapter dataAdapter3 = new SqlDataAdapter(sql_command_, req.con);
            //dataAdapter3.Fill(_dataTable_All_Orders);
            //DGV_All_Orders.ItemsSource = _dataTable_All_Orders.DefaultView;
            //_dataTable_All_Orders.Dispose();
            //req.con.Close();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string sql_command = "SELECT US.ID_Role FROM [dbo].[Users] US where US.ID=" + ID_User + "";
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
                while (dr.Read())
                {
                    Role_User = Convert.ToInt64(dr[0].ToString());
                }
                dr.Close();
                req.con.Close();
            }

            dr.Close();
            req.con.Close();

            switch (Role_User.ToString())
            {
                case "1":
                    TAB1.Items.Remove(TAB1_Orders);
                    TAB1.Items.Remove(TAB1_Clients);
                    TAB1.Items.Remove(TAB1_Specialists);


                    string sql_command2 = "SELECT " +
                      "[ID], " +
                      "[FIO] " +
                      "FROM [dbo].[Users] " +
                      "where (ID=" + ID_User + ")";
                    int row_count2 = 0;
                    req.con.Open();
                    SqlCommand exeSql2 = new SqlCommand(sql_command2, req.con);
                    SqlDataReader dr2 = exeSql2.ExecuteReader();
                    while (dr2.Read())
                    {
                        row_count2++;
                    }
                    dr2.Close();
                    if (row_count2 > 0)
                    {
                        dr2 = exeSql2.ExecuteReader(CommandBehavior.CloseConnection);
                        while (dr2.Read())
                        {
                            ID_Client____ = Convert.ToInt64(dr2[0].ToString());
                            FIO_Client = dr2[1].ToString();
                        }
                        dr2.Close();
                        req.con.Close();
                    }

                    dr2.Close();
                    req.con.Close();
                    label_Hello.Content = "Здравствуйте, " + FIO_Client + "";
                    Fill_DGV_Client_Orders(ID_Client____);

                    break;
                case "2":
                    TAB1.Items.Remove(TAB1_Client_Orders);

                    label_Hello.Content = "Здравствуйте, Admin";

                    Fill_DGV_All_Client();
                    Fill_DGV_All_Specialist();
                    Fill_DGV_All_Orders();
                    break;

            }
        }

        private void menu_Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }



        private void btn_Edit_Order_Click(object sender, RoutedEventArgs e)
        {
            btn_Edit_Order.IsEnabled = false;
            btn_Delete_Order.IsEnabled = false;
            DGV_Client_Orders.UnselectAllCells();
        }

        private void btn_Delete_Order_Click(object sender, RoutedEventArgs e)
        {
            btn_Edit_Order.IsEnabled = false;
            btn_Delete_Order.IsEnabled = false;
            DGV_Client_Orders.UnselectAllCells();

            //            const string message =
            //"Вы действительно хотите удалить выбранную заявку? После удаления ее невозможно будет восстановить. Подтвердить удаление?";
            //            const string caption = "Удаление";
            //            var result = MessageBox.Show(message, caption,
            //                                         MessageBoxButton.YesNo,
            //                                         MessageBoxImage.Question);

            //            if (result == MessageBoxResult.Yes)
            //            {
            //                string sql_delete = "DELETE FROM [dbo].[Users] WHERE Users.ID=" + ID_Worker + "";
            //                int res = req.insert_del_update(sql_delete);
            //                if (res > 0)
            //                {
            //                    MessageBox.Show("Обліковий запис успішно видалений!", "Успіх!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                }

            //            }
            //            this.Add_new_Client_Load(sender, e);
        }

        private void btn_Add_Order_Click(object sender, RoutedEventArgs e)
        {
            Edit_Order edit_Order = new Edit_Order();
            edit_Order.ID_Client = this.ID_Client____;
            edit_Order.Show();


        }

        private void btn_Add_Client_Click(object sender, RoutedEventArgs e)
        {
            Edit_Client edit_Client = new Edit_Client();
            edit_Client.Show();
        }

        private void btn_Edit_Client_Click(object sender, RoutedEventArgs e)
        {
            DataRowView rowView = DGV_All_Client.SelectedValue as DataRowView;

            if (DGV_All_Client.SelectedItem != null)
            {
                Edit_Client edit_Client = new Edit_Client();
                edit_Client.ID_Client = Convert.ToInt64(rowView[0].ToString());
                edit_Client.Show();

            }
        }

        private void btn_Delete_Client_Click(object sender, RoutedEventArgs e)
        {
            DataRowView rowView = DGV_All_Client.SelectedValue as DataRowView;
            long ID_CLient_Del= Convert.ToInt64(rowView[0].ToString());

            if (DGV_All_Client.SelectedItem != null)
            {
                const string message =
      "Вы действительно хотите удалить выбранного клиента? После удаления его невозможно будет восстановить. Прежде чем удалить клиента - удалите все его заявки. Подтвердить удаление?";
                const string caption = "Удаление";
                var result = MessageBox.Show(message, caption,
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {

                    string sql_command = "SELECT [ID] FROM [dbo].[Client_Order] where (Client_Order.ID_Client=" + ID_CLient_Del + ")";
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
                        MessageBox.Show("Невозможно удалить клиента. У клиента есть заявки. Для того, чтобы удалить клиента удалите заявки клиента и повторите попытку!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    else
                    {
                        string sql_delete = "DELETE FROM [dbo].[Users] WHERE Users.ID=" + ID_CLient_Del + "";
                        int res = req.insert_del_update(sql_delete);
                        if (res > 0)
                        {
                            MessageBox.Show("Клиент успешно удален!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                            Fill_DGV_All_Client();
                        }
                    }
                }
            }
        }

        private void DGV_All_Client_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DGV_All_Client.SelectedIndex > -1)
            {
                btn_Edit_Client.IsEnabled = true;
                btn_Delete_Client.IsEnabled = true;
            }
            else
            {
                btn_Edit_Client.IsEnabled = false;
                btn_Delete_Client.IsEnabled = false;

            }
        }

        private void btn_Add_Specialist_Click(object sender, RoutedEventArgs e)
        {
            Edit_Specialist edit_Specialist = new Edit_Specialist();
            edit_Specialist.Show();
        }

        private void DGV_All_Specialist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DGV_All_Specialist.SelectedIndex > -1)
            {
                btn_Edit_Specialist.IsEnabled = true;
                btn_Delete_Specialist.IsEnabled = true;
            }
            else
            {
                btn_Edit_Specialist.IsEnabled = false;
                btn_Delete_Specialist.IsEnabled = false;

            }
        }

        private void btn_Edit_Specialist_Click(object sender, RoutedEventArgs e)
        {
            DataRowView rowView = DGV_All_Specialist.SelectedValue as DataRowView;

            if (DGV_All_Specialist.SelectedItem != null)
            {
                Edit_Specialist edit_Specialist = new Edit_Specialist();
                edit_Specialist.ID_Specialist = Convert.ToInt64(rowView[0].ToString());
                edit_Specialist.Show();
            }
        }
        
        private void btn_Delete_Specialist_Click(object sender, RoutedEventArgs e)
        {
            DataRowView rowView = DGV_All_Specialist.SelectedValue as DataRowView;
            long ID_Specialist_Del = Convert.ToInt64(rowView[0].ToString());

            if (DGV_All_Specialist.SelectedItem != null)
            {
                const string message =
      "Вы действительно хотите удалить выбранного специалиста? После удаления его невозможно будет восстановить. Прежде чем удалить специалиста - удалите все его талоны,в которых он числиться. Подтвердить удаление?";
                const string caption = "Удаление";
                var result = MessageBox.Show(message, caption,
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {

                    string sql_command = "SELECT [ID] FROM [dbo].[Ticket_of_order] where (Ticket_of_order.ID_Specialist=" + ID_Specialist_Del + ")";
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
                        MessageBox.Show("Невозможно удалить специалиста. Специалист имеет талоны в работе. Для того, чтобы удалить специалиста, удалите талоны специалиста и повторите попытку!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    else
                    {
                        string sql_delete = "DELETE FROM [dbo].[Specialist] WHERE Specialist.ID=" + ID_Specialist_Del + "";
                        int res = req.insert_del_update(sql_delete);
                        if (res > 0)
                        {
                            MessageBox.Show("Специалист успешно удален!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                            Fill_DGV_All_Specialist();
                        }
                    }
                }
            }
        }
    }
}