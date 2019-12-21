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

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using System.Security.Cryptography;


namespace Application_Processing
{
    public class Request
    {
        //основное соединение
        public SqlConnection con = new SqlConnection(@"Data Source=TTYSTOTA-ПК;Initial Catalog=BD_Application_Processing;Integrated Security=False;User ID=admin;Password=123; MultipleActiveResultSets=True");

        //основное соединение

        public int insert_del_update(string command)
        {
            SqlCommand exeSql = new SqlCommand(command, con);
            exeSql.CommandType = CommandType.Text;
            con.Open();
            int res = (int)exeSql.ExecuteNonQuery();
            con.Close();
            return res;
        }

        public long insert_del_update_Scalar(string command)
        {
            SqlCommand cmd = new SqlCommand(command, con);
            cmd.CommandType = CommandType.Text;
            con.Open();
            cmd.ExecuteNonQuery();
            cmd.CommandText = "SELECT SCOPE_IDENTITY();";
            long lastId = Convert.ToInt64(cmd.ExecuteScalar());
            con.Close();
            return lastId;
        }

        public DataTable Get_Data_FromDB(string sql_command)
        {
            con.Open();
            DataTable Data_Result = new DataTable();
            string sql = sql_command;
            using (SqlCommand cmd = new SqlCommand(sql, this.con))
            {
                SqlDataReader dr = cmd.ExecuteReader();
                Data_Result.Load(dr);
                dr.Close();
                con.Close();
            }
            return Data_Result;

        }
    }
}