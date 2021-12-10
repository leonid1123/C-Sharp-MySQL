using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace exam
{
    public partial class Form1 : Form
    {
        public class DbWork
        {
            public MySqlConnection conn;
            public bool DBConnect()
            {
                String host = "127.0.0.1";
                String database = "exam";
                int port = 3306;
                String username = "examuser";
                String password = "123456";
                bool connected = false;
                String connString = "Server=" + host + ";Database=" + database
                    + ";port=" + port + ";User Id=" + username + ";password=" + password;
                try
                {
                    conn = new MySqlConnection(connString);
                    conn.Open();
                    connected = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                //label1.Text = connected.ToString();
                return connected;
            }
            public void DBClose()
            {
                conn.Close();   
            }
        }
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DbWork work = new DbWork();
            work.DBConnect();
            String selectSQL = "SELECT firstname,secondname,sex FROM students";
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = work.conn;
            cmd.CommandText = selectSQL;
            
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                listBox1.Items.Add(reader.GetString(0) + " " + reader.GetString(1) +
                    " " + reader.GetString(2));
            }
            reader.Close();
            
            work.DBClose();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DbWork work = new DbWork();
            work.DBConnect();
            String insertSQL = "INSERT INTO students(id,firstname,lastname,sex) " +
                "VALUES (null,@param1,@param2,@param3)";

        }
    }
}
