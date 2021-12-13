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


        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
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
            String name = textBox1.Text.Trim();
            String lastname=textBox2.Text.Trim();
            String sex = comboBox1.Text;
            if (!name.Equals("") && !lastname.Equals("") && !sex.Equals(""))
            {
                DbWork work = new DbWork();
                work.DBConnect();
                String insertSQL = "INSERT INTO students(id,firstname,secondname,sex) VALUES (null,@param1,@param2,@param3)";
                MySqlCommand cmd = new MySqlCommand(insertSQL, work.conn);
                cmd.Parameters.AddWithValue("@param1", name);
                cmd.Parameters.AddWithValue("@param2", lastname);
                cmd.Parameters.AddWithValue("@param3", sex);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    label1.Text = "Запись в БД внесена";
                } else
                {
                    label1.Text = "Ошибка записи";
                }
                work.DBClose();
            } else
            {
                MessageBox.Show("А писать кто будет??", "нуб детектед!!!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex!=-1)
            {
                DbWork work = new DbWork();
                work.DBConnect();
                String selectSQL = "SELECT id FROM students WHERE firstname=@param1 AND secondname=@param2";
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = work.conn;
                cmd.CommandText = selectSQL;
                cmd.Parameters.AddWithValue("param1", textBox1.Text.Trim());
                cmd.Parameters.AddWithValue("param2", textBox2.Text.Trim());
                MySqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                int id = reader.GetInt32(0);
                reader.Close();
                String updSQL = "UPDATE students SET firstname=@param3, secondname=@param4, sex=@Param5 WHERE id=@param6";
                MySqlCommand updCmd = new MySqlCommand();
                updCmd.Connection = work.conn;
                updCmd.CommandText = updSQL;
                updCmd.Parameters.AddWithValue("param3", textBox1.Text.Trim());
                updCmd.Parameters.AddWithValue("param4", textBox2.Text.Trim());
                updCmd.Parameters.AddWithValue("param5", comboBox1.Text);
                updCmd.Parameters.AddWithValue("param6", id);
                updCmd.ExecuteNonQuery();
            }
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            String[] str = listBox1.Items[listBox1.SelectedIndex].ToString().Split(' ');
            textBox1.Text = str[0];
            textBox2.Text = str[1];
            comboBox1.Text = str[2];
        }


    }
}
